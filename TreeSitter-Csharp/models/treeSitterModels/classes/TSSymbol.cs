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

    }
}
