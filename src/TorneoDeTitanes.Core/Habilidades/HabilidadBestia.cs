using TorneoDeTitanes.Core.Combate;
using TorneoDeTitanes.Core.Modelos;

namespace TorneoDeTitanes.Core.Habilidades;

public sealed class HabilidadBestia : HabilidadBase
{
    public override string Nombre => "Rugido Bestial";

    public override string Descripcion => "Debilita al rival (40% menos daño) y lo envenena durante 3 turnos.";

    public override int CostoEnergia => 35;

    public override int TurnosRecarga => 3;

    public override TipoPersonaje Tipo => TipoPersonaje.Bestia;

    protected override void EjecutarHabilidad(ContextoAccion contexto, ResultadoAccion resultado)
    {
        AplicarAtaque(contexto, resultado, multiplicador: 0.5);

        AplicarEstado(
            contexto.Defensor,
            resultado,
            new EfectoEstado
            {
                Tipo = EstadoAlterado.Debilitado,
                Duracion = 3,
                Potencia = 1,
                Fuente = Nombre,
            },
            $"{contexto.Defensor.Nombre} se debilita por el rugido y causará menos daño durante 3 turnos.");

        AplicarEstado(
            contexto.Defensor,
            resultado,
            new EfectoEstado
            {
                Tipo = EstadoAlterado.Veneno,
                Duracion = 3,
                Potencia = 6,
                Fuente = Nombre,
            },
            $"{contexto.Defensor.Nombre} queda ENVENENADO y perderá 6 de salud por turno.");
    }
}
