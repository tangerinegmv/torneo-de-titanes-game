using TorneoDeTitanes.Core.Combate;
using TorneoDeTitanes.Core.Modelos;

namespace TorneoDeTitanes.Core.Habilidades;

public sealed class HabilidadUltimateAtaque : HabilidadBase
{
    public override string Nombre => "Filo del Titán de Ataque";

    public override string Descripcion => "Una secuencia imparable de cortes enderezados que desgarra al rival.";

    public override int CostoEnergia => 0;

    public override TipoPersonaje Tipo => TipoPersonaje.Ataque;

    public override NivelHabilidad Nivel => NivelHabilidad.Ultimate;

    protected override void EjecutarHabilidad(ContextoAccion contexto, ResultadoAccion resultado)
    {
        resultado.Agregar(
            $"{contexto.Atacante.Nombre} canaliza su voluntad y desata una secuencia letal!",
            TipoEvento.Habilidad);

        for (int i = 0; i < 5 && contexto.Defensor.EstaVivo; i++)
        {
            AplicarAtaque(contexto, resultado, multiplicador: 0.85);
        }

        if (contexto.Defensor.EstaVivo)
        {
            AplicarEstado(
                contexto.Defensor,
                resultado,
                new EfectoEstado { Tipo = EstadoAlterado.Sangrado, Duracion = 3, Potencia = 6, Fuente = Nombre },
                $"{contexto.Defensor.Nombre} sufre SANGRADO profundo y perderá 6 de salud por turno.");
        }
    }
}
