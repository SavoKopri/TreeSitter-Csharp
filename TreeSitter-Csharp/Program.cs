using AnalizadorDeCodigo.Parsers;
using AnalizadorDeCodigo.Utils;
using System.Diagnostics;
using TreeSitter_Csharp.models.treeSitterModels.classes;

class Program
{
    static void Main(string[] args)
    {
        // Leer el código fuente de un archivo
        var codigo = UtilidadesDeArchivo.LeerArchivo("PruebaCodigoC.c");

        // Configurar el parser con el lenguaje (C# en este caso)
        var gestorDeParser = new GestorDeParser(new TSLanguage(LanguageLoader.LoadLanguage(SupportedLanguages.C)));

        // Parsear el código y obtener el árbol de sintaxis
        using var arbolSintactico = gestorDeParser.ParseCode(codigo);

        // Crear el QueryHandler con una consulta
        //string querySource = "(method_declaration name: (identifier) @methodName)";
        //var queryHandler = new QueryHandler(new TSLanguage(LanguageLoader.LoadLanguage(SupportedLanguages.C)), querySource);

        // Ejecutar la consulta en el árbol sintáctico
        //queryHandler.ExecuteQuery(arbolSintactico, codigo);

        // Limpieza
        //queryHandler.Dispose();

        // Imprimir el árbol sintáctico (opcional)
        //arbolSintactico.ImprimirArbolConTexto(codigo);
        var simbolos = arbolSintactico.AnalizarVariables(codigo);

        foreach (var simbolo in simbolos)
        {
            Console.WriteLine($"Variable: {simbolo.Name} (Ámbito: {simbolo.Scope})");
            Console.WriteLine("  Lecturas:");
            if (simbolo.LinesReaded.Count() == 0)
            {
                Console.WriteLine("    No se leyo");
            }
            foreach (var linea in simbolo.LinesReaded)
            {
                Console.WriteLine($"    {linea}");
            }
            Console.WriteLine("  Escrituras:");
            if (simbolo.LinesWrited.Count() == 0)
            {
                Console.WriteLine("    No se escribio");
            }
            foreach (var linea in simbolo.LinesWrited)
            {
                Console.WriteLine($"    {linea}");
            }
        }
    }
}