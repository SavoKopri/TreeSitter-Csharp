using System.Runtime.InteropServices;
using TreeSitter_Csharp.models.enums;

namespace TreeSitter_Csharp.models.treeSitterModels.structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TSQueryPredicateStep
    {
        public TSQueryPredicateStepType Type;
        public uint ValueId;
    }
}
