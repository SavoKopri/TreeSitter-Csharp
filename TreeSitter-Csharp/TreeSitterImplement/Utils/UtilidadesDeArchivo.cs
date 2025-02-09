namespace AnalizadorDeCodigo.Utils
{
    public static class UtilidadesDeArchivo
    {
        public static string LeerArchivo(string ruta)
        {
            return File.ReadAllText("C:\\Users\\savo9\\source\\repos\\TreeSitter-Csharp\\TreeSitter-Csharp\\bin\\Debug\\net7.0\\" + ruta);
        }
    }

}
