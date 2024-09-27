using System.Diagnostics;
using TreeSitter_Csharp.models.treeSitterModels.classes;

class Program
{
    static void Main(string[] args)
    {
        // Create a parser.
        using var parser = new TSParser();

        // Set the parser's language (JSON in this case).
        var language = new TSLanguage(LanguageLoader.LoadLanguage(SupportedLanguages.Json));
        parser.SetLanguage(language);

        // Build a syntax tree based on source code stored in a string.
        string sourceCode = "[1, null]";
        TSTree tree = parser.ParseString(null, sourceCode);

        // Get the root node of the syntax tree.
        TSNode rootNode = tree.RootNode();

        // Get some child nodes.
        TSNode arrayNode = rootNode.NamedChild(0);
        TSNode numberNode = arrayNode.NamedChild(0);

        // Check that the nodes have the expected types.
        Debug.Assert(rootNode.Type() == "document", $"Expected 'document', got '{rootNode.Type()}'");
        Debug.Assert(arrayNode.Type() == "array", $"Expected 'array', got '{arrayNode.Type()}'");
        Debug.Assert(numberNode.Type() == "number", $"Expected 'number', got '{numberNode.Type()}'");

        // Check that the nodes have the expected child counts.
        Debug.Assert(rootNode.ChildCount() == 1, $"Expected child count 1, got {rootNode.ChildCount()}");
        //Debug.Assert(arrayNode.ChildCount() == 2, $"Expected child count 2, got {arrayNode.ChildCount()}");
        Debug.Assert(arrayNode.NamedChildCount() == 2, $"Expected named child count 2, got {arrayNode.NamedChildCount()}");
        Debug.Assert(numberNode.ChildCount() == 0, $"Expected child count 0, got {numberNode.ChildCount()}");

        // Print the syntax tree as an S-expression.
        string syntaxTree = rootNode.ToString();
        Console.WriteLine($"Syntax tree: {syntaxTree}");

        // Free all of the heap-allocated memory.
        tree.Dispose();
    }
}