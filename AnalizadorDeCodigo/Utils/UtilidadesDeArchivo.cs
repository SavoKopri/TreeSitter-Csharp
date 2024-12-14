namespace AnalizadorDeCodigo.Utils
{
    public static class UtilidadesDeArchivo
    {
        public static string LeerArchivo(string ruta)
        {
            return File.ReadAllText(ruta);
        }
    }

}
