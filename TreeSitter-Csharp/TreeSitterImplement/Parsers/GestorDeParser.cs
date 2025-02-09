using TreeSitter_Csharp.models.treeSitterModels.classes;

namespace AnalizadorDeCodigo.Parsers
{
    public class GestorDeParser
    {
        private TSParser _parser;

        public GestorDeParser(TSLanguage language)
        {
            _parser = new TSParser();
            _parser.SetLanguage(language);
        }

        public TSTree ParseCode(string code)
        {
            using var oldTree = _parser.ParseString(null, code); // No hay árbol anterior, por lo que es null
            return oldTree.Copy(); // Devuelve una copia del árbol sintáctico
        }
    }

}
