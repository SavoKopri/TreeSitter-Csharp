using System.Text;

namespace TreeSitter_Csharp.models.treeSitterModels.classes
{
    public class TSSymbol
    {
        public string Name { get; set; }
        public string Scope { get; set; }
        public List<string> LinesReaded { get; set; } = new List<string>();
        public List<string> LinesWrited { get; set; } = new List<string>();

        public TSSymbol(string name, string scope) 
        {
            Name = name;
            Scope = scope;
        }

        public bool IsSameSymbol(string name, string scope) => Name.Equals(name) && Scope.Equals(scope);
        public string SymbolTableName() => $"{Scope}#{Name}";
        public override string ToString() => ($"Variable: {Name} (Ámbito: {Scope})\n");
        public string LinesReadedToString()
        {
            var linesReadedBuilder = new StringBuilder();
            linesReadedBuilder.AppendLine("Lecturas:");
            if (LinesReaded == null || LinesReaded.Count == 0)
            {
                linesReadedBuilder.AppendLine("    No hay lecturas");
            }
            else
            {
                foreach (var linea in LinesReaded)
                {
                    linesReadedBuilder.AppendLine($"    {linea}");
                }
            }
            return linesReadedBuilder.ToString();
        }

        public string LinesWritedToString()
        {
            var linesReadedBuilder = new StringBuilder();
            linesReadedBuilder.AppendLine("Escrituras:");
            if (LinesWrited == null || LinesWrited.Count == 0)
            {
                linesReadedBuilder.AppendLine("    No hay escrituras");
            }
            else
            {
                foreach (var linea in LinesWrited)
                {
                    linesReadedBuilder.AppendLine($"    {linea}");
                }
            }
            return linesReadedBuilder.ToString();
        }

    }
}
