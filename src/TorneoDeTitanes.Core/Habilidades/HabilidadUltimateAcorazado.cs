using TorneoDeTitanes.Core.Combate;
using TorneoDeTitanes.Core.Modelos;

namespace TorneoDeTitanes.Core.Habilidades;

public sealed class HabilidadUltimateAcorazado : HabilidadBase
{
    public override string Nombre => "Fortaleza Impenetrable";

    public override string Descripcion => "Cristaliza su piel: escudo masivo, daño reducido y un contraataque brutal.";

    public override int CostoEnergia => 0;

    public override TipoPersonaje Tipo => TipoPersonaje.Acorazado;

    public override NivelHabilidad Nivel => NivelHabilidad.Ultimate;

    protected override void EjecutarHabilidad(ContextoAccion contexto, ResultadoAccion resultado)
    {
        int escudo = 25 + contexto.Atacante.Armadura * 3;
        contexto.Atacante.OtorgarEscudo(escudo);

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
            $"{contexto.Atacante.Nombre} se recubre de cristal y gana un escudo de {escudo} puntos.");

        AplicarAtaque(contexto, resultado, multiplicador: 1.4);
    }
}
