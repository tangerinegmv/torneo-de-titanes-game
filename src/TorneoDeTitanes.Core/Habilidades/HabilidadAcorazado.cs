using TorneoDeTitanes.Core.Combate;
using TorneoDeTitanes.Core.Modelos;

namespace TorneoDeTitanes.Core.Habilidades;

public sealed class HabilidadAcorazado : HabilidadBase
{
    public override string Nombre => "Muro de Acero";

    public override string Descripcion => "Endurece su armadura (50% menos daño) durante 3 turnos y contraataca.";

    public override int CostoEnergia => 30;

    public override int TurnosRecarga => 3;

    public override TipoPersonaje Tipo => TipoPersonaje.Acorazado;

    protected override void EjecutarHabilidad(ContextoAccion contexto, ResultadoAccion resultado)
    {
        AplicarAtaque(contexto, resultado, multiplicador: 0.8);

        AplicarEstado(
            contexto.Atacante,
            resultado,
            new EfectoEstado
            {
                Tipo = EstadoAlterado.Fortalecido,
                Duracion = 3,
                Potencia = 1,
                Fuente = Nombre,
            },
            $"{contexto.Atacante.Nombre} endurece su armadura de cristal y reduce el daño recibido durante 3 turnos.");
    }
}
