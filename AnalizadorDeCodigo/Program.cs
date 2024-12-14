using AnalizadorDeCodigo.Parsers;
using AnalizadorDeCodigo.Queries; // Asegúrate de incluir este espacio de nombres
using AnalizadorDeCodigo.Utils;
using TreeSitter_Csharp.models.treeSitterModels.classes;

class Program
{
    static void Main(string[] args)
    {
        // Leer el código fuente de un archivo
        var codigo = UtilidadesDeArchivo.LeerArchivo("codigoFuente.cs");

        // Configurar el parser con el lenguaje (C# en este caso)
        var gestorDeParser = new GestorDeParser(new TSLanguage(LanguageLoader.LoadLanguage(SupportedLanguages.CSharp)));

        // Parsear el código y obtener el árbol de sintaxis
        using var arbolSintactico = gestorDeParser.ParseCode(codigo);

        // Crear el QueryHandler con una consulta
        string querySource = "(method_declaration name: (identifier) @methodName)";
        var queryHandler = new QueryHandler(new TSLanguage(LanguageLoader.LoadLanguage(SupportedLanguages.CSharp)), querySource);

        // Ejecutar la consulta en el árbol sintáctico
        //queryHandler.ExecuteQuery(arbolSintactico, codigo);

        // Limpieza
        queryHandler.Dispose();

        // Imprimir el árbol sintáctico (opcional)
        arbolSintactico.ImprimirArbol();
    }
}
