using System.Runtime.InteropServices;

namespace TreeSitter_Csharp.models.treeSitterModels.structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TSInputEdit
    {
        public uint StartByte;      // Byte de inicio de la edición
        public uint OldEndByte;     // Byte de finalización anterior
        public uint NewEndByte;     // Byte de nueva finalización
        public TSPoint StartPoint;   // Punto de inicio de la edición
        public TSPoint OldEndPoint;  // Punto de finalización anterior
        public TSPoint NewEndPoint;  // Punto de nueva finalización
    }
}
