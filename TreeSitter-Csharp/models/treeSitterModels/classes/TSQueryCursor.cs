using System.Runtime.InteropServices;
using TreeSitter_Csharp.constants;
using TreeSitter_Csharp.models.treeSitterModels.structs;

namespace TreeSitter_Csharp.models.treeSitterModels.classes
{
    public sealed class TSQueryCursor : IDisposable
    {
        private nint Ptr { get; set; }

        private TSQueryCursor(nint ptr)
        {
            Ptr = ptr;
        }

        public TSQueryCursor()
        {
            Ptr = ts_query_cursor_new();
        }

        public void Dispose()
        {
            if (Ptr != nint.Zero)
            {
                ts_query_cursor_delete(Ptr);
                Ptr = nint.Zero;
            }
        }

        public void Exec(TSQuery query, TSNode node)
        {
            ts_query_cursor_exec(Ptr, query.Ptr, node);
        }

        public bool DidExceedMatchLimit()
        {
            return ts_query_cursor_did_exceed_match_limit(Ptr);
        }

        public uint MatchLimit()
        {
            return ts_query_cursor_match_limit(Ptr);
        }

        public void SetMatchLimit(uint limit)
        {
            ts_query_cursor_set_match_limit(Ptr, limit);
        }

        public void SetRange(uint start, uint end)
        {
            ts_query_cursor_set_byte_range(Ptr, start * sizeof(ushort), end * sizeof(ushort));
        }

        public void SetPointRange(TSPoint start, TSPoint end)
        {
            ts_query_cursor_set_point_range(Ptr, start, end);
        }

        public bool NextMatch(out TSQueryMatch match, out TSQueryCapture[] captures)
        {
            captures = null;
            if (ts_query_cursor_next_match(Ptr, out match))
            {
                if (match.CaptureCount > 0)
                {
                    captures = new TSQueryCapture[match.CaptureCount];
                    for (ushort i = 0; i < match.CaptureCount; i++)
                    {
                        var intPtr = match.Captures + Marshal.SizeOf(typeof(TSQueryCapture)) * i;
                        captures[i] = Marshal.PtrToStructure<TSQueryCapture>(intPtr);
                    }
                }
                return true;
            }
            return false;
        }

        public void RemoveMatch(uint id)
        {
            ts_query_cursor_remove_match(Ptr, id);
        }

        public bool NextCapture(out TSQueryMatch match, out uint index)
        {
            return ts_query_cursor_next_capture(Ptr, out match, out index);
        }

        #region PInvoke
        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern nint ts_query_cursor_new();

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ts_query_cursor_delete(nint cursor);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ts_query_cursor_exec(nint cursor, nint query, TSNode node);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool ts_query_cursor_did_exceed_match_limit(nint cursor);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern uint ts_query_cursor_match_limit(nint cursor);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ts_query_cursor_set_match_limit(nint cursor, uint limit);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ts_query_cursor_set_byte_range(nint cursor, uint start_byte, uint end_byte);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ts_query_cursor_set_point_range(nint cursor, TSPoint start_point, TSPoint end_point);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool ts_query_cursor_next_match(nint cursor, out TSQueryMatch match);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ts_query_cursor_remove_match(nint cursor, uint id);

        [DllImport(DllConstants.TreeSitterDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool ts_query_cursor_next_capture(nint cursor, out TSQueryMatch match, out uint capture_index);
        #endregion
    }
}
