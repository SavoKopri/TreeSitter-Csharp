using System.Text;

namespace TreeSitter_Csharp.models.treeSitterModels.classes
{
    public class SymbolReference
    {
        public int LineNumber { get; }
        public string Scope { get; }

        public SymbolReference(int lineNumber, string scope)
        {
            LineNumber = lineNumber;
            Scope = scope;
        }

        public override string ToString() => $"Línea {LineNumber} (Ámbito: {Scope})";
    }

    public class TSSymbol
    {
        public string Name { get; }
        public string Scope { get; }
        public List<SymbolReference> ReadReferences { get; } = new List<SymbolReference>();
        public List<SymbolReference> WriteReferences { get; } = new List<SymbolReference>();

        public TSSymbol(string name, string scope)
        {
            Name = name;
            Scope = scope;
        }

        public bool IsSameSymbol(string name, string scope) =>
            Name.Equals(name) && Scope.Equals(scope);

        public string SymbolTableName() => $"{Scope}#{Name}";

        public override string ToString() => $"Variable: {Name} (Ámbito: {Scope})\n";

        public void AddReadReference(int lineNumber, string scope, string filePath = null) =>
            ReadReferences.Add(new SymbolReference(lineNumber, scope));

        public void AddWriteReference(int lineNumber, string scope, string filePath = null) =>
            WriteReferences.Add(new SymbolReference(lineNumber, scope));

        public string LinesReadedToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine("Lecturas:");

            if (ReadReferences.Count == 0)
            {
                builder.AppendLine("    No hay lecturas");
            }
            else
            {
                foreach (var reference in ReadReferences.OrderBy(r => r.LineNumber))
                {
                    builder.AppendLine($"    {reference}");
                }
            }
            return builder.ToString();
        }

        public string LinesWritedToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine("Escrituras:");

            if (WriteReferences.Count == 0)
            {
                builder.AppendLine("    No hay escrituras");
            }
            else
            {
                foreach (var reference in WriteReferences.OrderBy(r => r.LineNumber))
                {
                    builder.AppendLine($"    {reference}");
                }
            }
            return builder.ToString();
        }
    }
}