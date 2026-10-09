namespace TorneoDeTitanes.Console.Infraestructura;

internal static class RutasDeArchivo
{
    public static string Resolver(string ruta)
    {
        if (Path.IsPathRooted(ruta))
        {
            return ruta;
        }

        return Path.Combine(AppContext.BaseDirectory, ruta);
    }
}