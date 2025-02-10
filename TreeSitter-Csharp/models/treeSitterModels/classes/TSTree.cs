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

        public List<TSSimbol> AnalizarVariables(string codigoFuente)
        {
            var simbolos = new Dictionary<string, TSSimbol>();
            var nodoRaiz = RootNode();

            Console.WriteLine("Iniciando análisis del árbol...");
            Console.WriteLine("===============================");

            // Paso 1: Encontrar todas las declaraciones de variables con su ámbito
            EncontrarDeclaracionesVariables(nodoRaiz, codigoFuente, simbolos, "global");

            Console.WriteLine("Declaraciones de variables encontradas:");
            foreach (var clave in simbolos.Keys)
            {
                Console.WriteLine($"- {clave}");
            }
            Console.WriteLine("===============================");

            // Paso 2: Verificar lecturas y escrituras de variables
            VerificarLecturasYEscrituras(nodoRaiz, codigoFuente, simbolos);

            Console.WriteLine("Análisis completado.");
            Console.WriteLine("===============================");

            // Devolver la lista de símbolos
            return new List<TSSimbol>(simbolos.Values);
        }

        private void EncontrarDeclaracionesVariables(TSNode nodo, string codigoFuente, Dictionary<string, TSSimbol> simbolos, string ambitoActual)
        {
            Console.WriteLine($"Procesando nodo: Tipo={nodo.Type()}, Ámbito={ambitoActual}");

            // Si el nodo es una declaración de variable, crear un TSSimbol
            if (nodo.Type() == "declaration")
            {
                var nombreVariable = ObtenerNombreVariable(nodo, codigoFuente);
                if (!string.IsNullOrEmpty(nombreVariable))
                {
                    // Crear una clave única que combine el nombre y el ámbito
                    var clave = $"{nombreVariable}_{ambitoActual}";
                    if (!simbolos.ContainsKey(clave))
                    {
                        Console.WriteLine($"Encontrada declaración de variable: {nombreVariable} (Ámbito: {ambitoActual})");
                        simbolos[clave] = new TSSimbol(nombreVariable, ambitoActual);
                    }
                }
            }

            // Si el nodo es una función o un bloque, actualizar el ámbito actual
            string nuevoAmbito = ambitoActual;
            if (nodo.Type() == "function_definition")
            {
                // El ámbito ahora es el nombre de la función
                nuevoAmbito = ObtenerNombreFuncion(nodo, codigoFuente);
                Console.WriteLine($"Entrando en función: {nuevoAmbito}");
            }
            else if (nodo.Type() == "compound_statement")
            {
                // El ámbito ahora es el bloque actual (podría ser un bucle, una condición, etc.)
                nuevoAmbito = $"{ambitoActual}_block";
                Console.WriteLine($"Entrando en bloque: {nuevoAmbito}");
            }

            // Recorrer los hijos del nodo
            for (uint i = 0; i < nodo.ChildCount(); i++)
            {
                EncontrarDeclaracionesVariables(nodo.Child(i), codigoFuente, simbolos, nuevoAmbito);
            }
        }

        private string ObtenerNombreVariable(TSNode nodo, string codigoFuente)
        {
            // Buscar el identificador de la variable (el nombre)
            for (uint i = 0; i < nodo.ChildCount(); i++)
            {
                var hijo = nodo.Child(i);
                if (hijo.Type() == "identifier")
                {
                    var nombreVariable = hijo.Text(codigoFuente);
                    Console.WriteLine($"Identificador encontrado: {nombreVariable}");
                    return nombreVariable;
                } else if (hijo.Type() == "init_declarator")
                {
                    return ObtenerNombreVariable(hijo, codigoFuente);
                }
            }
            return null;
        }

        private string ObtenerNombreFuncion(TSNode nodo, string codigoFuente)
        {
            // Buscar el identificador de la función (el nombre)
            for (uint i = 0; i < nodo.ChildCount(); i++)
            {
                var hijo = nodo.Child(i);
                if (hijo.Type() == "identifier")
                {
                    var nombreFuncion = hijo.Text(codigoFuente);
                    Console.WriteLine($"Nombre de función encontrado: {nombreFuncion}");
                    return nombreFuncion;
                }
            }
            return "anonima";
        }

        private void VerificarLecturasYEscrituras(TSNode nodo, string codigoFuente, Dictionary<string, TSSimbol> simbolos)
        {
            // Verificar si el nodo es una lectura o escritura de una variable
            if (nodo.Type() == "identifier")
            {
                var nombreVariable = nodo.Text(codigoFuente);
                Console.WriteLine($"Identificador encontrado: {nombreVariable}");

                // Determinar el ámbito actual del nodo
                var ambitoActual = ObtenerAmbitoActual(nodo);
                Console.WriteLine($"Ámbito actual: {ambitoActual}");

                // Crear una clave única que combine el nombre y el ámbito
                var clave = $"{nombreVariable}_{ambitoActual}";

                if (simbolos.ContainsKey(clave))
                {
                    var simbolo = simbolos[clave];

                    // Determinar si es una lectura o escritura
                    if (EsEscritura(nodo, codigoFuente))
                    {
                        Console.WriteLine($"Escritura de variable: {nombreVariable} en línea {nodo.StartPoint().Row + 1}");
                        simbolo.LinesWrited.Add($"Línea {nodo.StartPoint().Row + 1}: {nodo.Text(codigoFuente)}");
                    }
                    else
                    {
                        Console.WriteLine($"Lectura de variable: {nombreVariable} en línea {nodo.StartPoint().Row + 1}");
                        simbolo.LinesReaded.Add($"Línea {nodo.StartPoint().Row + 1}: {nodo.Text(codigoFuente)}");
                    }
                }
                else
                {
                    Console.WriteLine($"Variable no declarada: {nombreVariable} (Ámbito: {ambitoActual})");
                }
            }

            // Recorrer los hijos del nodo
            for (uint i = 0; i < nodo.ChildCount(); i++)
            {
                VerificarLecturasYEscrituras(nodo.Child(i), codigoFuente, simbolos);
            }
        }

        private string ObtenerAmbitoActual(TSNode nodo)
        {
            var actual = nodo;
            while (!actual.IsNull())
            {
                if (actual.Type() == "function_definition")
                {
                    // Obtener el nombre de la función
                    var nombreFuncion = ObtenerNombreFuncion(actual, "");
                    Console.WriteLine($"Ámbito actual: función {nombreFuncion}");
                    return nombreFuncion;
                }
                else if (actual.Type() == "compound_statement")
                {
                    // Si está dentro de un bloque, devolver el ámbito del bloque
                    Console.WriteLine("Ámbito actual: bloque");
                    return "block";
                }
                actual = actual.Parent();
            }
            // Si no está dentro de una función o bloque, es global
            Console.WriteLine("Ámbito actual: global");
            return "global";
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
                    Console.WriteLine($"Identificador es parte de una asignación (escritura): {nodo.Text(codigoFuente)}");
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
