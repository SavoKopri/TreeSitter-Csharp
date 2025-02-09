namespace TreeSitter_Csharp.models.treeSitterModels.classes
{
    public class TSSimbol
    {
        public string Name { get; set; }
        public string Scope { get; set; }
        public List<string> LinesReaded { get; set; } = new List<string>();
        public List<string> LinesWrited { get; set; } = new List<string>();

        public TSSimbol(string name, string scope) 
        {
            Name = name;
            Scope = scope;
        }
    }
}
