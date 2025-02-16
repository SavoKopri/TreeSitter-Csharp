using System.Runtime.InteropServices;
using TreeSitter_Csharp.constants;
using TreeSitter_Csharp.models.treeSitterModels.structs;

namespace TreeSitter_Csharp.models.treeSitterModels.classes
{
    public sealed class TSTree : IDisposable
    {
        internal nint Ptr { get; private set; }
        // Lista de símbolos o tipos de nodos que queremos omitir
        private HashSet<string> simbolosAOmitir = new HashSet<string> { ";", "{", "}", "(", ")", "[", "]" };

        public TSTree(nint ptr)
        {
            Ptr = ptr;
        }

        public void Dispose()
        {
            if (Ptr != nint.Zero)
            {
                ts_tree_delete(Ptr);
                Ptr = nint.Zero;
            }
        }

        public TSTree Copy()
        {
            var ptr = ts_tree_copy(Ptr);
            return ptr != nint.Zero ? new TSTree(ptr) : null;
        }

        public TSNode RootNode() => ts_tree_root_node(Ptr);

        public TSNode RootNodeWithOffset(uint offsetBytes, TSPoint offsetPoint)
        {
            return ts_tree_root_node_with_offset(Ptr, offsetBytes, offsetPoint);
        }

        public TSLanguage Language()
        {
            var ptr = ts_tree_language(Ptr);
            return ptr != nint.Zero ? new TSLanguage(ptr) : null;
        }

        public void Edit(TSInputEdit edit)
        {
            ts_tree_edit(Ptr, ref edit);
        }

        public void ImprimirArbolSinTexto()
        {
            // Obtiene el nodo raíz
            var nodoRaiz = RootNode();
            // Llama a la función recursiva para imprimir el árbol desde el nodo raíz
            ImprimirNodoSinTexto(nodoRaiz, 0);
        }

        // Función auxiliar recursiva para imprimir un nodo y sus hijos
        private void ImprimirNodoSinTexto(TSNode nodo, int indentacion)
        {
            // Obtiene las posiciones de inicio y fin
            var startPoint = nodo.StartPoint();
            var endPoint = nodo.EndPoint();

            // Construye la cadena con el tipo de nodo y su rango de posición
            Console.WriteLine($"{new string(' ', indentacion * 2)}{nodo.Type()} [{startPoint.Row}, {startPoint.Column}] - [{endPoint.Row}, {endPoint.Column}]");

            // Recorre todos los hijos nombrados del nodo
            for (uint i = 0; i < nodo.NamedChildCount(); i++)
            {
                // Llama recursivamente para cada hijo
                ImprimirNodoSinTexto(nodo.NamedChild(i), indentacion + 1);
            }
        }

        public void ImprimirArbolConTexto(string codigoFuente)
        {
            ImprimirNodoConTexto(RootNode(), 0, codigoFuente);
        }

        private void ImprimirNodoConTexto(TSNode nodo, int indentacion, string codigoFuente)
        {
            // Obtener el tipo del nodo y su texto
            var tipoNodo = nodo.Type();
            var textoNodo = nodo.Text(codigoFuente);
            if (textoNodo.Length > 50)
            {
                textoNodo = textoNodo.Substring(0, 50) + " ...";
            }

            // Si el tipo del nodo o su texto está en la lista de símbolos a omitir, no lo imprimimos
            if (simbolosAOmitir.Contains(tipoNodo) || simbolosAOmitir.Contains(textoNodo))
            {
                return;
            }

            // Obtener las posiciones de inicio y fin
            var startPoint = nodo.StartPoint();
            var endPoint = nodo.EndPoint();

            // Imprimir la información del nodo
            Console.WriteLine($"{new string(' ', indentacion * 2)}{tipoNodo} " +
                              $"[{startPoint.Row}, {startPoint.Column}] - [{endPoint.Row}, {endPoint.Column}]: {textoNodo}");

            // Recorrer los hijos del nodo
            for (uint i = 0; i < nodo.ChildCount(); i++)
            {
                ImprimirNodoConTexto(nodo.Child(i), indentacion + 1, codigoFuente);
            }
        }

        public List<TSSymbol> AnalizarVariables(string codigoFuente)
        {
            var simbolos = new Dictionary<string, TSSymbol>();
            var nodoRaiz = RootNode();

            // Paso 1: Encontrar todas las declaraciones de variables con su ámbito
            EncontrarDeclaracionesVariables(nodoRaiz, codigoFuente, simbolos, "global");

            // Devolver la lista de símbolos
            return new List<TSSymbol>(simbolos.Values);
        }

        private void EncontrarDeclaracionesVariables(TSNode nodo, string codigoFuente, Dictionary<string, TSSymbol> simbolos, string ambitoActual)
        {
            if (nodo.Type() == "declaration")
            {
                ProcesarDeclaracion(nodo, codigoFuente, simbolos, ambitoActual);
            }

            if (nodo.Type() == "identifier")
            {
                ProcesarIdentificador(nodo, codigoFuente, simbolos, ambitoActual);
            }

            string nuevoAmbito = nodo.Type() == "function_definition"
                ? ActualizarAmbito(nodo, ambitoActual, codigoFuente)
                : ambitoActual;

            for (uint i = 0; i < nodo.ChildCount(); i++)
            {
                EncontrarDeclaracionesVariables(nodo.Child(i), codigoFuente, simbolos, nuevoAmbito);
            }
        }

        private void ProcesarDeclaracion(TSNode nodo, string codigoFuente, Dictionary<string, TSSymbol> simbolos, string ambitoActual)
        {
            var nombreVariable = ObtenerNombreVariable(nodo, codigoFuente);
            if (!string.IsNullOrEmpty(nombreVariable))
            {
                var clave = $"{ambitoActual}#{nombreVariable}";
                if (!simbolos.ContainsKey(clave))
                {
                    simbolos[clave] = new TSSymbol(nombreVariable, ambitoActual);
                }
            }
        }

        // Si el nodo es una función, actualizar el ámbito actual
        private string ActualizarAmbito(TSNode nodo, string ambitoActual, string codigoFuente)
        {
                // El ámbito ahora es el nombre de la función
                string nuevoAmbito = ObtenerNombreFuncion(nodo, codigoFuente);
                return $"{nuevoAmbito}#{ambitoActual}";
        }

        private void ProcesarIdentificador(TSNode nodo, string codigoFuente, Dictionary<string, TSSymbol> simbolos, string ambitoActual)
        {
            var nombreVariable = nodo.Text(codigoFuente);
            var clave = BuscarSimboloEnTabla(nombreVariable, ambitoActual, simbolos);

            if (simbolos.ContainsKey(clave))
            {
                var simbolo = simbolos[clave];
                if (EsEscritura(nodo, codigoFuente))
                {
                    simbolo.LinesWrited.Add($"Línea {nodo.StartPoint().Row + 1}");
                }
                else
                {
                    simbolo.LinesReaded.Add($"Línea {nodo.StartPoint().Row + 1}");
                }
            }
        }

        private string ObtenerNombreFuncion(TSNode nodo, string codigoFuente)
        {
            // Buscar el identificador de la función (el nombre)
            for (uint i = 0; i < nodo.ChildCount(); i++)
            {
                var hijo = nodo.Child(i);
                if (hijo.Type() == "identifier")
                {
                    return hijo.Text(codigoFuente);
                }
                else if (hijo.Type() == "function_declarator")
                {
                    return ObtenerNombreFuncion(hijo, codigoFuente);
                }
            }
            return "anonima";
        }

        private string BuscarSimboloEnTabla(string simbolName, string ambitoActual, Dictionary<string, TSSymbol> simbolos)
        {
            if (string.IsNullOrEmpty(ambitoActual))
            {
                return string.Empty;
            }

            string[] ambitos = ambitoActual.Split('#');

            // Recorrer desde el ámbito más específico hasta el más general
            for (int i = 0; i < ambitos.Length; i++)
            {
                // Construir el ámbito acumulado desde el nivel actual hasta el final
                string ambitoAcumulado = string.Join("#", ambitos.Skip(i));

                // Construir la clave
                string clave = $"{ambitoAcumulado}#{simbolName}";

                if (simbolos.ContainsKey(clave))
                {
                    return clave;
                }
            }

            return string.Empty;
        }



        private string ObtenerNombreVariable(TSNode nodo, string codigoFuente)
        {
            var pila = new Stack<TSNode>();
            pila.Push(nodo);

            while (pila.Count > 0)
            {
                var actual = pila.Pop();
                if (actual.Type() == "identifier")
                {
                    return actual.Text(codigoFuente);
                }

                for (uint i = 0; i < actual.ChildCount(); i++)
                {
                    pila.Push(actual.Child(i));
                }
            }

            return string.Empty;
        }

        

        private bool EsEscritura(TSNode nodo, string codigoFuente)
        {
            // Verificar si el nodo es parte de una asignación (escritura)
            var padre = nodo.Parent();
            if (padre.Type() == "assignment_expression")
            {
                // El identificador es el lado izquierdo de la asignación
                var ladoIzquierdo = padre.Child(0);
                if (ladoIzquierdo.Equals(nodo))
                {
                    return true;
                }
            }
            return false;
        }

        #region PInvoke
        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern nint ts_tree_copy(nint tree);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ts_tree_delete(nint tree);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSNode ts_tree_root_node(nint tree);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSNode ts_tree_root_node_with_offset(nint tree, uint offsetBytes, TSPoint offsetPoint);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern nint ts_tree_language(nint tree);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern nint ts_tree_included_ranges(nint tree, out uint length);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ts_tree_included_ranges_free(nint ranges);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ts_tree_edit(nint tree, ref TSInputEdit edit);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern nint ts_tree_get_changed_ranges(nint oldTree, nint newTree, out uint length);
        #endregion
    }
}
