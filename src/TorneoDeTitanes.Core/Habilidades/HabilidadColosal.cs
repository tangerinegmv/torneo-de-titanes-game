using TorneoDeTitanes.Core.Combate;
using TorneoDeTitanes.Core.Modelos;

namespace TorneoDeTitanes.Core.Habilidades;

public sealed class HabilidadColosal : HabilidadBase
{
    public override string Nombre => "Pisotón Sísmico";

    public override string Descripcion => "Un golpe devastador (180% de daño) con 40% de probabilidad de aturdir al rival.";

    public override int CostoEnergia => 40;

    public override int TurnosRecarga => 3;

    public override TipoPersonaje Tipo => TipoPersonaje.Colosal;

    protected override void EjecutarHabilidad(ContextoAccion contexto, ResultadoAccion resultado)
    {
        int dano = AplicarAtaque(contexto, resultado, multiplicador: 1.8);

        if (dano > 0 && contexto.Defensor.EstaVivo && contexto.Random.SiguienteDoble() < 0.40)
        {
            AplicarEstado(
                contexto.Defensor,
                resultado,
                new EfectoEstado
                {
                    Tipo = EstadoAlterado.Aturdido,
                    Duracion = 1,
                    Potencia = 1,
                    Fuente = Nombre,
                },
                $"{contexto.Defensor.Nombre} queda ATURDIDO por la onda sísmica y perderá su próximo turno.");
        }
    }
}
