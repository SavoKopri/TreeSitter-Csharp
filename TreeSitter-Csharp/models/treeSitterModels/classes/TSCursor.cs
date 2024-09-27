using System.Runtime.InteropServices;
using TreeSitter_Csharp.constants;
using TreeSitter_Csharp.models.treeSitterModels.structs;

namespace TreeSitter_Csharp.models.treeSitterModels.classes
{
    public sealed class TSCursor : IDisposable
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct TSTreeCursor
        {
            private nint Tree;
            private nint Id;
            private uint Context0;
            private uint Context1;
        }

        private nint Ptr;
        private TSTreeCursor cursor;
        public TSLanguage Lang { get; private set; }

        public TSCursor(TSTreeCursor cursor, TSLanguage lang)
        {
            this.cursor = cursor;
            Lang = lang;
            Ptr = new nint(1);
        }

        public TSCursor(TSNode node, TSLanguage lang)
        {
            cursor = ts_tree_cursor_new(node);
            Lang = lang;
            Ptr = new nint(1);
        }

        public void Dispose()
        {
            if (Ptr != nint.Zero)
            {
                ts_tree_cursor_delete(ref cursor);
                Ptr = nint.Zero;
            }
        }

        public void Reset(TSNode node) => ts_tree_cursor_reset(ref cursor, node);

        public TSNode CurrentNode() => ts_tree_cursor_current_node(ref cursor);

        public string CurrentField() => Lang.Fields[CurrentFieldId()];

        public string CurrentSymbol()
        {
            ushort symbol = CurrentNode().Symbol();
            return symbol != ushort.MaxValue ? Lang.Symbols[symbol] : "ERROR";
        }

        public ushort CurrentFieldId() => ts_tree_cursor_current_field_id(ref cursor);

        public bool GotoParent() => ts_tree_cursor_goto_parent(ref cursor);

        public bool GotoNextSibling() => ts_tree_cursor_goto_next_sibling(ref cursor);

        public bool GotoFirstChild() => ts_tree_cursor_goto_first_child(ref cursor);

        public long GotoFirstChildForOffset(uint offset) => ts_tree_cursor_goto_first_child_for_byte(ref cursor, offset * sizeof(ushort));

        public long GotoFirstChildForPoint(TSPoint point) => ts_tree_cursor_goto_first_child_for_point(ref cursor, point);

        public TSCursor Copy() => new TSCursor(ts_tree_cursor_copy(ref cursor), Lang);

        #region PInvoke
        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSTreeCursor ts_tree_cursor_new(TSNode node);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ts_tree_cursor_delete(ref TSTreeCursor cursor);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ts_tree_cursor_reset(ref TSTreeCursor cursor, TSNode node);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSNode ts_tree_cursor_current_node(ref TSTreeCursor cursor);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern nint ts_tree_cursor_current_field_name(ref TSTreeCursor cursor);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern ushort ts_tree_cursor_current_field_id(ref TSTreeCursor cursor);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool ts_tree_cursor_goto_parent(ref TSTreeCursor cursor);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool ts_tree_cursor_goto_next_sibling(ref TSTreeCursor cursor);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool ts_tree_cursor_goto_first_child(ref TSTreeCursor cursor);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern long ts_tree_cursor_goto_first_child_for_byte(ref TSTreeCursor cursor, uint byteOffset);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern long ts_tree_cursor_goto_first_child_for_point(ref TSTreeCursor cursor, TSPoint point);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSTreeCursor ts_tree_cursor_copy(ref TSTreeCursor cursor);
        #endregion
    }
}
