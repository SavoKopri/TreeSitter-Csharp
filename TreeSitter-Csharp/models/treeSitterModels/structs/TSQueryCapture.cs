using System.Runtime.InteropServices;
using TreeSitter_Csharp.models.treeSitterModels.classes;

namespace TreeSitter_Csharp.models.treeSitterModels.structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TSQueryCapture
    {
        public TSNode Node;
        public uint Index;
    }
}
