using TorneoDeTitanes.Core.Modelos;

namespace TorneoDeTitanes.Core.Tests.Dobles;

public static class FabricaDePersonajesDePrueba
{
    public static Personaje Crear(
        TipoPersonaje tipo = TipoPersonaje.Ataque,
        string nombre = "Titán de prueba",
        int fuerza = 10,
        int velocidad = 10,
        int destreza = 5,
        int armadura = 5,
        int nivel = 5,
        int saludMaxima = 100)
    {
        return new Personaje
        {
            Nombre = nombre,
            Tipo = tipo,
            Fuerza = fuerza,
            Velocidad = velocidad,
            Destreza = destreza,
            Armadura = armadura,
            Nivel = nivel,
            SaludMaxima = saludMaxima,
            Salud = saludMaxima,
            EnergiaMaxima = Personaje.EnergiaInicial,
            Energia = Personaje.EnergiaInicial,
        };
    }
}