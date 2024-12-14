
using TreeSitter_Csharp.models.enums;
using TreeSitter_Csharp.models.treeSitterModels.classes;

namespace AnalizadorDeCodigo.Queries
{
    public class QueryHandler
    {
        private TSLanguage _language;
        private TSQuery _query;

        public QueryHandler(TSLanguage language, string querySource)
        {
            _language = language;
            GenerateNewQuery(querySource);
        }

        public void GenerateNewQuery(string querySource)
        {
            uint errorOffset;
            TSQueryError errorType;

            _query = _language.QueryNew(querySource, out errorOffset, out errorType);
            if (_query == null)
            {
                throw new InvalidOperationException($"Error creating query at offset {errorOffset}: {errorType}");
            }
        }

        public void ExecuteQuery(TSTree tree, string codigo)
        {
            if (_query == null)
            {
                throw new InvalidOperationException("Query not created. Call CreateQuery first.");
            }

            using (var cursor = new TSQueryCursor())
            {
                cursor.Exec(_query, tree.RootNode());

                while (cursor.NextMatch(out var match, out var captures))
                {
                    Console.WriteLine($"Match found! ID: {match.Id}, Pattern Index: {match.PatternIndex}, Capture Count: {match.CaptureCount}");

                    if (captures != null)
                    {
                        foreach (var capture in captures)
                        {
                            // Obtener el nombre del método
                            string methodText = capture.Node.Text(codigo);
                            Console.WriteLine($"Capture: Node type: {capture.Node.Type()}, Method Text: {methodText}, Start: {capture.Node.StartOffset()}, End: {capture.Node.EndOffset()}");
                        }
                    }
                }

                if (cursor.DidExceedMatchLimit())
                {
                    Console.WriteLine("Match limit exceeded.");
                }
            }
        }

        public void Dispose()
        {
            _query?.Dispose();
        }
    }
}
