using System.Runtime.InteropServices;

namespace TreeSitter_Csharp.models.treeSitterModels.structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TSRange
    {
        public TSPoint StartPoint;
        public TSPoint EndPoint;
        public uint StartByte;
        public uint EndByte;
    }
}
