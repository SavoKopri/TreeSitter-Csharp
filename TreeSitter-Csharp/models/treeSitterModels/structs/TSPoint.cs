using System.Runtime.InteropServices;

namespace TreeSitter_Csharp.models.treeSitterModels.structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TSPoint
    {
        public uint Row;
        public uint Column;

        public TSPoint(uint row, uint column)
        {
            Row = row;
            Column = column;
        }
    }
}
