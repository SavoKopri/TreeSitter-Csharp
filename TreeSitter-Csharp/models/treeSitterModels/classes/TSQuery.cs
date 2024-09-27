using System.Runtime.InteropServices;
using TreeSitter_Csharp.constants;
using TreeSitter_Csharp.models.enums;

namespace TreeSitter_Csharp.models.treeSitterModels.classes
{
    public sealed class TSQuery : IDisposable
    {
        internal nint Ptr { get; private set; }

        public TSQuery(nint ptr)
        {
            Ptr = ptr;
        }

        public void Dispose()
        {
            if (Ptr != nint.Zero)
            {
                ts_query_delete(Ptr);
                Ptr = nint.Zero;
            }
        }

        public uint PatternCount() => ts_query_pattern_count(Ptr);
        public uint CaptureCount() => ts_query_capture_count(Ptr);
        public uint StringCount() => ts_query_string_count(Ptr);
        public uint StartOffsetForPattern(uint patternIndex) => ts_query_start_byte_for_pattern(Ptr, patternIndex) / sizeof(ushort);
        public nint PredicatesForPattern(uint patternIndex, out uint length) => ts_query_predicates_for_pattern(Ptr, patternIndex, out length);
        public bool IsPatternRooted(uint patternIndex) => ts_query_is_pattern_rooted(Ptr, patternIndex);
        public bool IsPatternNonLocal(uint patternIndex) => ts_query_is_pattern_non_local(Ptr, patternIndex);
        public bool IsPatternGuaranteedAtOffset(uint offset) => ts_query_is_pattern_guaranteed_at_step(Ptr, offset / sizeof(ushort));
        public string CaptureNameForId(uint id, out uint length) => Marshal.PtrToStringAnsi(ts_query_capture_name_for_id(Ptr, id, out length));
        public TSQuantifier CaptureQuantifierForId(uint patternId, uint captureId) => ts_query_capture_quantifier_for_id(Ptr, patternId, captureId);
        public string StringValueForId(uint id, out uint length) => Marshal.PtrToStringAnsi(ts_query_string_value_for_id(Ptr, id, out length));
        public void DisableCapture(string captureName) => ts_query_disable_capture(Ptr, captureName, (uint)captureName.Length);
        public void DisablePattern(uint patternIndex) => ts_query_disable_pattern(Ptr, patternIndex);

        #region PInvoke
        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ts_query_delete(nint query);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern uint ts_query_pattern_count(nint query);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern uint ts_query_capture_count(nint query);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern uint ts_query_string_count(nint query);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern uint ts_query_start_byte_for_pattern(nint query, uint patternIndex);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern nint ts_query_predicates_for_pattern(nint query, uint patternIndex, out uint length);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool ts_query_is_pattern_rooted(nint query, uint patternIndex);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool ts_query_is_pattern_non_local(nint query, uint patternIndex);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool ts_query_is_pattern_guaranteed_at_step(nint query, uint byteOffset);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern nint ts_query_capture_name_for_id(nint query, uint id, out uint length);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSQuantifier ts_query_capture_quantifier_for_id(nint query, uint patternId, uint captureId);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern nint ts_query_string_value_for_id(nint query, uint id, out uint length);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ts_query_disable_capture(nint query, [MarshalAs(UnmanagedType.LPUTF8Str)] string captureName, uint captureNameLength);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ts_query_disable_pattern(nint query, uint patternIndex);
        #endregion
    }
}
