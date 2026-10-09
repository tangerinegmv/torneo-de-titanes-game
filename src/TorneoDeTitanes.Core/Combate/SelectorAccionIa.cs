using TorneoDeTitanes.Core.Abstracciones;

namespace TorneoDeTitanes.Core.Combate;

public sealed class SelectorAccionIa : ISelectorAccion
{
    private readonly IRandomProvider _random;

    public SelectorAccionIa(IRandomProvider random)
    {
        _random = random;
    }

    public AccionDisponible ElegirAccion(ContextoTurno contexto)
    {
        bool puedeUsarHabilidad = contexto.AccionesDisponibles.Contains(AccionDisponible.Habilidad);
        bool puedeUsarUltimate = contexto.AccionesDisponibles.Contains(AccionDisponible.Ultimate);
        double salud = contexto.Actor.PorcentajeSalud;
        double saludRival = contexto.Oponente.PorcentajeSalud;

        if (puedeUsarUltimate && (saludRival < 0.45 || _random.SiguienteDoble() < 0.6))
        {
            return AccionDisponible.Ultimate;
        }

        if (puedeUsarHabilidad && salud < 0.5 && _random.SiguienteDoble() < 0.75)
        {
            return AccionDisponible.Habilidad;
        }

        if (puedeUsarHabilidad && _random.SiguienteDoble() < 0.45)
        {
            return AccionDisponible.Habilidad;
        }

        if (salud < 0.3 && _random.SiguienteDoble() < 0.45)
        {
            return AccionDisponible.Defender;
        }

        return AccionDisponible.Atacar;
    }
}
