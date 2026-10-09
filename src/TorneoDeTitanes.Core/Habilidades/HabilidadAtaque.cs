using TorneoDeTitanes.Core.Combate;
using TorneoDeTitanes.Core.Modelos;

namespace TorneoDeTitanes.Core.Habilidades;

public sealed class HabilidadAtaque : HabilidadBase
{
    public override string Nombre => "Asalto Relámpago";

    public override string Descripcion => "Encadena de 2 a 4 golpes rápidos y provoca sangrado durante 3 turnos.";

    public override int CostoEnergia => 35;

    public override int TurnosRecarga => 2;

    public override TipoPersonaje Tipo => TipoPersonaje.Ataque;

    protected override void EjecutarHabilidad(ContextoAccion contexto, ResultadoAccion resultado)
    {
        int golpes = contexto.Random.Siguiente(2, 5);
        resultado.Agregar(
            $"{contexto.Atacante.Nombre} desata un Asalto Relámpago con {golpes} golpes!",
            TipoEvento.Habilidad);

        for (int i = 0; i < golpes && contexto.Defensor.EstaVivo; i++)
        {
            AplicarAtaque(contexto, resultado, multiplicador: 0.6);
        }

        if (contexto.Defensor.EstaVivo)
        {
            AplicarEstado(
                contexto.Defensor,
                resultado,
                new EfectoEstado
                {
                    Tipo = EstadoAlterado.Sangrado,
                    Duracion = 3,
                    Potencia = 5,
                    Fuente = Nombre,
                },
                $"{contexto.Defensor.Nombre} sufre SANGRADO y perderá 5 de salud por turno.");
        }
    }
}
