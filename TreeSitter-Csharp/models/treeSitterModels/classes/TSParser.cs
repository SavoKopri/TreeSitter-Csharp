using System.Runtime.InteropServices;
using TreeSitter_Csharp.constants;
using TreeSitter_Csharp.models.enums;
using TreeSitter_Csharp.models.treeSitterModels.structs;

namespace TreeSitter_Csharp.models.treeSitterModels.classes
{
    public sealed class TSParser : IDisposable
    {
        private nint Ptr { get; set; }

        public TSParser()
        {
            Ptr = ts_parser_new();
        }

        public void Dispose()
        {
            if (Ptr != nint.Zero)
            {
                ts_parser_delete(Ptr);
                Ptr = nint.Zero;
            }
        }

        public bool SetLanguage(TSLanguage language)
        {
            return ts_parser_set_language(Ptr, language.Ptr);
        }

        public TSLanguage Language()
        {
            var ptr = ts_parser_language(Ptr);
            return ptr != nint.Zero ? new TSLanguage(ptr) : null;
        }

        public bool SetIncludedRanges(TSRange[] ranges)
        {
            return ts_parser_set_included_ranges(Ptr, ranges, (uint)ranges.Length);
        }

        public TSRange[] IncludedRanges()
        {
            uint length;
            return ts_parser_included_ranges(Ptr, out length);
        }

        public TSTree ParseString(TSTree oldTree, string input)
        {
            var ptr = ts_parser_parse_string_encoding(Ptr, oldTree != null ? oldTree.Ptr : nint.Zero,
                                                      input, (uint)input.Length * 2, TSInputEncoding.TSInputEncodingUTF16);
            return ptr != nint.Zero ? new TSTree(ptr) : null;
        }

        public void Reset() { ts_parser_reset(Ptr); }
        public void SetTimeoutMicros(ulong timeout) { ts_parser_set_timeout_micros(Ptr, timeout); }
        public ulong TimeoutMicros() { return ts_parser_timeout_micros(Ptr); }

        public void SetLogger(TSLogger logger)
        {
            var code = new _TSLoggerCode(logger);
            var data = new _TSLoggerData { Log = logger != null ? new TSLogCallback(code.LogCallback) : null };
            ts_parser_set_logger(Ptr, data);
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct _TSLoggerData
        {
            private nint Payload;
            internal TSLogCallback Log;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void TSLogCallback(nint payload, TSLogType logType, [MarshalAs(UnmanagedType.LPUTF8Str)] string message);

        private class _TSLoggerCode
        {
            private TSLogger logger;

            internal _TSLoggerCode(TSLogger logger)
            {
                this.logger = logger;
            }

            internal void LogCallback(nint payload, TSLogType logType, string message)
            {
                logger(logType, message);
            }
        }

        #region PInvoke
        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern nint ts_parser_new();

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ts_parser_delete(nint parser);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool ts_parser_set_language(nint parser, nint language);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern nint ts_parser_language(nint parser);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool ts_parser_set_included_ranges(nint parser, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] TSRange[] ranges, uint length);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]
        private static extern TSRange[] ts_parser_included_ranges(nint parser, out uint length);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern nint ts_parser_parse_string_encoding(nint parser, nint oldTree, [MarshalAs(UnmanagedType.LPWStr)] string input, uint length, TSInputEncoding encoding);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ts_parser_reset(nint parser);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ts_parser_set_timeout_micros(nint parser, ulong timeout);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern ulong ts_parser_timeout_micros(nint parser);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ts_parser_set_logger(nint parser, _TSLoggerData logger);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ts_parser_set_cancellation_flag(nint parser, ref nint flag);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern nint ts_parser_cancellation_flag(nint parser);
        #endregion
    }

    // El delegado TSLogger, usado en TSParser para logging
    public delegate void TSLogger(TSLogType logType, string message);
}
