using System.Runtime.InteropServices;

namespace TreeSitter_Csharp.models.treeSitterModels.structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TSQueryMatch
    {
        public uint Id;
        public ushort PatternIndex;
        public ushort CaptureCount;
        public nint Captures;
    }
}
