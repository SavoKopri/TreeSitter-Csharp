using System.Runtime.InteropServices;
using TreeSitter_Csharp.constants;
using TreeSitter_Csharp.models.treeSitterModels.structs;

namespace TreeSitter_Csharp.models.treeSitterModels.classes
{
    public sealed class TSTree : IDisposable
    {
        internal nint Ptr { get; private set; }

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

        public void ImprimirArbol()
        {
            // Obtiene el nodo raíz
            var nodoRaiz = RootNode();
            // Llama a la función recursiva para imprimir el árbol desde el nodo raíz
            ImprimirNodo(nodoRaiz, 0);
        }

        // Función auxiliar recursiva para imprimir un nodo y sus hijos
        private void ImprimirNodo(TSNode nodo, int indentacion)
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
                ImprimirNodo(nodo.NamedChild(i), indentacion + 1);
            }
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
