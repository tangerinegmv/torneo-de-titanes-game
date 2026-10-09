using TorneoDeTitanes.Core.Combate;
using TorneoDeTitanes.Core.Modelos;

namespace TorneoDeTitanes.Core.Habilidades;

public sealed class HabilidadUltimateBestia : HabilidadBase
{
    public override string Nombre => "Juicio de las Bestias";

    public override string Descripcion => "Un rugido infecta al rival (veneno, debilidad y quemadura) mientras regenera sus heridas.";

    public override int CostoEnergia => 0;

    public override TipoPersonaje Tipo => TipoPersonaje.Bestia;

    public override NivelHabilidad Nivel => NivelHabilidad.Ultimate;

    protected override void EjecutarHabilidad(ContextoAccion contexto, ResultadoAccion resultado)
    {
        resultado.Agregar(
            $"{contexto.Atacante.Nombre} ruge con la fuerza de todas las bestias primigenias!",
            TipoEvento.Habilidad);

        AplicarAtaque(contexto, resultado, multiplicador: 1.2);

        AplicarEstado(
            contexto.Defensor,
            resultado,
            new EfectoEstado { Tipo = EstadoAlterado.Veneno, Duracion = 3, Potencia = 7, Fuente = Nombre },
            $"{contexto.Defensor.Nombre} queda ENVENENADO y perderá 7 de salud por turno.");

        AplicarEstado(
            contexto.Defensor,
            resultado,
            new EfectoEstado { Tipo = EstadoAlterado.Debilitado, Duracion = 3, Potencia = 1, Fuente = Nombre },
            $"{contexto.Defensor.Nombre} se debilita por el rugido y causará menos daño.");

        AplicarEstado(
            contexto.Defensor,
            resultado,
            new EfectoEstado { Tipo = EstadoAlterado.Quemadura, Duracion = 2, Potencia = 5, Fuente = Nombre },
            $"{contexto.Defensor.Nombre} arde bajo la maldición y perderá 5 de salud por turno.");

        AplicarCuracion(
            contexto.Atacante,
            resultado,
            25,
            $"{contexto.Atacante.Nombre} regenera 25 puntos de salud con el poder de las bestias.");
    }
}
