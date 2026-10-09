using TorneoDeTitanes.Core.Combate;
using TorneoDeTitanes.Core.Modelos;

namespace TorneoDeTitanes.Core.Habilidades;

public sealed class HabilidadUltimateColosal : HabilidadBase
{
    public override string Nombre => "Detonación Apocalíptica";

    public override string Descripcion => "Libera vapor a presión: 250% de daño y quema al rival durante 3 turnos.";

    public override int CostoEnergia => 0;

    public override TipoPersonaje Tipo => TipoPersonaje.Colosal;

    public override NivelHabilidad Nivel => NivelHabilidad.Ultimate;

    protected override void EjecutarHabilidad(ContextoAccion contexto, ResultadoAccion resultado)
    {
        resultado.Agregar(
            $"{contexto.Atacante.Nombre} libera una nube de vapor ardiente sobre el campo!",
            TipoEvento.Habilidad);

        AplicarAtaque(contexto, resultado, multiplicador: 2.5);

        if (contexto.Defensor.EstaVivo)
        {
            AplicarEstado(
                contexto.Defensor,
                resultado,
                new EfectoEstado
                {
                    Tipo = EstadoAlterado.Quemadura,
                    Duracion = 3,
                    Potencia = 8,
                    Fuente = Nombre,
                },
                $"{contexto.Defensor.Nombre} arde entre el vapor y perderá 8 de salud por turno.");
        }
    }
}
