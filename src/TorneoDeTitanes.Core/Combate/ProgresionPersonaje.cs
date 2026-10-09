using TorneoDeTitanes.Core.Abstracciones;
using TorneoDeTitanes.Core.Modelos;

namespace TorneoDeTitanes.Core.Combate;

public static class ProgresionPersonaje
{
    private const int ArmaduraMaxima = 10;
    private const int NivelMaximo = 5;

    public static void Mejorar(Personaje personaje, IRandomProvider random)
    {
        switch (personaje.Tipo)
        {
            case TipoPersonaje.Colosal:
                personaje.Fuerza += random.Siguiente(1, 3);
                personaje.Armadura = Math.Min(personaje.Armadura + random.Siguiente(1, 3), ArmaduraMaxima);
                break;

            case TipoPersonaje.Acorazado:
                personaje.Armadura = Math.Min(personaje.Armadura + random.Siguiente(2, 4), ArmaduraMaxima);
                personaje.Fuerza += random.Siguiente(1, 2);
                break;

            case TipoPersonaje.Bestia:
                personaje.Armadura = Math.Min(personaje.Armadura + random.Siguiente(2, 4), ArmaduraMaxima);
                personaje.Fuerza += random.Siguiente(1, 3);
                break;

            case TipoPersonaje.Ataque:
                personaje.Fuerza += random.Siguiente(1, 3);
                personaje.Velocidad += random.Siguiente(1, 3);
                personaje.Armadura = Math.Min(personaje.Armadura + random.Siguiente(1, 2), ArmaduraMaxima);
                break;
        }

        personaje.Nivel = Math.Min(personaje.Nivel + 1, NivelMaximo);
    }

    public static void Penalizar(Personaje personaje)
    {
        personaje.Fuerza = Math.Max(1, personaje.Fuerza - 1);
        personaje.Velocidad = Math.Max(1, personaje.Velocidad - 1);
        personaje.Destreza = Math.Max(1, personaje.Destreza - 1);
        personaje.Armadura = Math.Max(1, personaje.Armadura - 1);
        personaje.Nivel = Math.Max(1, personaje.Nivel - 1);
    }
}
