using System.Diagnostics;
using System.Runtime.InteropServices;
using TreeSitter_Csharp.constants;
using TreeSitter_Csharp.models.enums;

namespace TreeSitter_Csharp.models.treeSitterModels.classes
{
    public sealed class TSLanguage : IDisposable
    {
        internal nint Ptr { get; private set; }

        public string[] Symbols { get; private set; }
        public string[] Fields { get; private set; }
        public Dictionary<string, ushort> FieldIds { get; private set; }

        public TSLanguage(nint ptr)
        {
            Ptr = ptr;

            Symbols = new string[SymbolCount() + 1];
            for (ushort i = 0; i < Symbols.Length; i++)
            {
                Symbols[i] = Marshal.PtrToStringAnsi(ts_language_symbol_name(Ptr, i));
            }

            Fields = new string[FieldCount() + 1];
            FieldIds = new Dictionary<string, ushort>((int)FieldCount() + 1);

            for (ushort i = 0; i < Fields.Length; i++)
            {
                Fields[i] = Marshal.PtrToStringAnsi(ts_language_field_name_for_id(Ptr, i));
                if (Fields[i] != null)
                {
                    if (!FieldIds.ContainsKey(Fields[i]))
                    {
                        FieldIds.Add(Fields[i], i);
                    }
                    else
                    {
                        throw new InvalidOperationException($"Duplicate field name found: {Fields[i]}");
                    }
                }
            }

            //ValidateNoDuplicates(Symbols);
            //ValidateNoDuplicates(Fields);
        }

        public void Dispose()
        {
            if (Ptr != nint.Zero)
            {
                // Liberar recursos específicos si es necesario.
                // ts_query_cursor_delete(Ptr); 
                Ptr = nint.Zero;
            }
        }

        public TSQuery QueryNew(string source, out uint errorOffset, out TSQueryError errorType)
        {
            var ptr = ts_query_new(Ptr, source, (uint)source.Length, out errorOffset, out errorType);
            return ptr != nint.Zero ? new TSQuery(ptr) : null;
        }

        public uint SymbolCount() => ts_language_symbol_count(Ptr);
        public string SymbolName(ushort symbol) => symbol != ushort.MaxValue ? Symbols[symbol] : "ERROR";
        public ushort SymbolForName(string str, bool isNamed) => ts_language_symbol_for_name(Ptr, str, (uint)str.Length, isNamed);
        public uint FieldCount() => ts_language_field_count(Ptr);
        public string FieldNameForId(ushort fieldId) => Fields[fieldId];
        public ushort FieldIdForName(string str) => ts_language_field_id_for_name(Ptr, str, (uint)str.Length);
        public TSSymbolType SymbolType(ushort symbol) => ts_language_symbol_type(Ptr, symbol);

        #region PInvoke
        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern nint ts_query_new(nint language, [MarshalAs(UnmanagedType.LPUTF8Str)] string source, uint sourceLen, out uint errorOffset, out TSQueryError errorType);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern uint ts_language_symbol_count(nint language);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern nint ts_language_symbol_name(nint language, ushort symbol);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern ushort ts_language_symbol_for_name(nint language, [MarshalAs(UnmanagedType.LPUTF8Str)] string str, uint length, bool isNamed);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern uint ts_language_field_count(nint language);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern nint ts_language_field_name_for_id(nint language, ushort fieldId);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern ushort ts_language_field_id_for_name(nint language, [MarshalAs(UnmanagedType.LPUTF8Str)] string str, uint length);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSSymbolType ts_language_symbol_type(nint language, ushort symbol);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern uint ts_language_version(nint language);
        #endregion

        //private void ValidateNoDuplicates(string[] items)
        //{
        //    for (int i = 0; i < items.Length; i++)
        //    {
        //        for (int j = 0; j < i; j++)
        //        {
        //            Debug.Assert(items[i] != items[j], $"Duplicate item found: {items[i]}");
        //        }
        //    }
        //}
    }
}
