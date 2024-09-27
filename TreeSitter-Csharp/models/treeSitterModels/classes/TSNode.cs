using System.Runtime.InteropServices;
using TreeSitter_Csharp.constants;
using TreeSitter_Csharp.models.treeSitterModels.structs;

namespace TreeSitter_Csharp.models.treeSitterModels.classes
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TSNode
    {
        private uint context0;
        private uint context1;
        private uint context2;
        private uint context3;
        public nint id;
        private nint tree;

        public void Clear()
        {
            id = nint.Zero;
            tree = nint.Zero;
        }
        public bool IsZero() => id == nint.Zero && tree == nint.Zero;
        public string Type() => Marshal.PtrToStringAnsi(ts_node_type(this));
        public string Type(TSLanguage lang) => lang.SymbolName(Symbol());
        public ushort Symbol() => ts_node_symbol(this);
        public uint StartOffset() => ts_node_start_byte(this) / sizeof(ushort);
        public TSPoint StartPoint() => new TSPoint(ts_node_start_point(this).Row, ts_node_start_point(this).Column / sizeof(ushort));
        public uint EndOffset() => ts_node_end_byte(this) / sizeof(ushort);
        public TSPoint EndPoint() => new TSPoint(ts_node_end_point(this).Row, ts_node_end_point(this).Column / sizeof(ushort));
        public string ToString()
        {
            var dat = ts_node_string(this);
            var str = Marshal.PtrToStringAnsi(dat);
            return str;
        }
        public bool IsNull() => ts_node_is_null(this);
        public bool IsNamed() => ts_node_is_named(this);
        public bool IsMissing() => ts_node_is_missing(this);
        public bool IsExtra() => ts_node_is_extra(this);
        public bool HasChanges() => ts_node_has_changes(this);
        public bool HasError() => ts_node_has_error(this);
        public TSNode Parent() => ts_node_parent(this);
        public TSNode Child(uint index) => ts_node_child(this, index);
        public nint FieldNameForChild(uint index) => ts_node_field_name_for_child(this, index);
        public uint ChildCount() => ts_node_child_count(this);
        public TSNode NamedChild(uint index) => ts_node_named_child(this, index);
        public uint NamedChildCount() => ts_node_named_child_count(this);
        public TSNode ChildByFieldName(string fieldName) => ts_node_child_by_field_name(this, fieldName, (uint)fieldName.Length);
        public TSNode ChildByFieldId(ushort fieldId) => ts_node_child_by_field_id(this, fieldId);
        public TSNode NextSibling() => ts_node_next_sibling(this);
        public TSNode PrevSibling() => ts_node_prev_sibling(this);
        public TSNode NextNamedSibling() => ts_node_next_named_sibling(this);
        public TSNode PrevNamedSibling() => ts_node_prev_named_sibling(this);
        public TSNode FirstChildForOffset(uint offset) => ts_node_first_child_for_byte(this, offset * sizeof(ushort));
        public TSNode FirstNamedChildForOffset(uint offset) => ts_node_first_named_child_for_byte(this, offset * sizeof(ushort));
        public TSNode DescendantForOffsetRange(uint start, uint end) => ts_node_descendant_for_byte_range(this, start * sizeof(ushort), end * sizeof(ushort));
        public TSNode DescendantForPointRange(TSPoint start, TSPoint end) => ts_node_descendant_for_point_range(this, start, end);
        public TSNode NamedDescendantForOffsetRange(uint start, uint end) => ts_node_named_descendant_for_byte_range(this, start * sizeof(ushort), end * sizeof(ushort));
        public TSNode NamedDescendantForPointRange(TSPoint start, TSPoint end) => ts_node_named_descendant_for_point_range(this, start, end);
        public bool Eq(TSNode other) => ts_node_eq(this, other);

        public string Text(string data)
        {
            uint beg = StartOffset();
            uint end = EndOffset();
            return data.Substring((int)beg, (int)(end - beg));
        }

        #region PInvoke
        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern nint ts_node_type(TSNode node);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern ushort ts_node_symbol(TSNode node);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern uint ts_node_start_byte(TSNode node);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSPoint ts_node_start_point(TSNode node);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern uint ts_node_end_byte(TSNode node);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSPoint ts_node_end_point(TSNode node);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern nint ts_node_string(TSNode node);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool ts_node_is_null(TSNode node);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool ts_node_is_named(TSNode node);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool ts_node_is_missing(TSNode node);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool ts_node_is_extra(TSNode node);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool ts_node_has_changes(TSNode node);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool ts_node_has_error(TSNode node);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSNode ts_node_parent(TSNode node);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSNode ts_node_child(TSNode node, uint index);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern nint ts_node_field_name_for_child(TSNode node, uint index);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern uint ts_node_child_count(TSNode node);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSNode ts_node_named_child(TSNode node, uint index);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern uint ts_node_named_child_count(TSNode node);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSNode ts_node_child_by_field_name(TSNode self, [MarshalAs(UnmanagedType.LPUTF8Str)] string fieldName, uint fieldNameLength);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSNode ts_node_child_by_field_id(TSNode self, ushort fieldId);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSNode ts_node_next_sibling(TSNode self);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSNode ts_node_prev_sibling(TSNode self);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSNode ts_node_next_named_sibling(TSNode self);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSNode ts_node_prev_named_sibling(TSNode self);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSNode ts_node_first_child_for_byte(TSNode self, uint byteOffset);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSNode ts_node_first_named_child_for_byte(TSNode self, uint byteOffset);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSNode ts_node_descendant_for_byte_range(TSNode self, uint startByte, uint endByte);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSNode ts_node_descendant_for_point_range(TSNode self, TSPoint startPoint, TSPoint endPoint);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSNode ts_node_named_descendant_for_byte_range(TSNode self, uint startByte, uint endByte);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSNode ts_node_named_descendant_for_point_range(TSNode self, TSPoint startPoint, TSPoint endPoint);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool ts_node_eq(TSNode left, TSNode right);
        #endregion
    }
}
