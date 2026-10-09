using TorneoDeTitanes.Core.Abstracciones;
using TorneoDeTitanes.Core.Modelos;

namespace TorneoDeTitanes.Core.Combate;

public sealed class CalculadoraDano : ICalculadoraDano
{
    private const double EsquivaBase = 0.05;
    private const double EsquivaPorDiferenciaVelocidad = 0.03;
    private const double EsquivaMaxima = 0.40;
    private const double CriticoBase = 0.05;
    private const double CriticoPorDestreza = 0.012;
    private const double MultiplicadorCritico = 1.75;
    private const double MitigacionPorFortalecido = 0.5;
    private const double PenalizacionPorDebilitado = 0.6;

    public ResultadoDano Calcular(
        Personaje atacante,
        Personaje defensor,
        bool defensorEstaDefendiendo,
        IRandomProvider random)
    {
        double probabilidadEsquiva = Math.Clamp(
            EsquivaBase + Math.Max(0, defensor.Velocidad - atacante.Velocidad) * EsquivaPorDiferenciaVelocidad,
            0,
            EsquivaMaxima);

        if (random.SiguienteDoble() < probabilidadEsquiva)
        {
            return new ResultadoDano(0, false, true, false);
        }

        double factorVariacion = 0.85 + random.SiguienteDoble() * 0.30;
        double poderAtaque = atacante.Fuerza * (1 + atacante.Destreza * 0.08) * atacante.Nivel;
        double mitigacion = defensor.Armadura * 1.8 + defensor.Velocidad * 0.5;
        double dano = (poderAtaque - mitigacion) * factorVariacion;

        bool critico = random.SiguienteDoble() < CriticoBase + atacante.Destreza * CriticoPorDestreza;
        if (critico)
        {
            dano *= MultiplicadorCritico;
        }

        if (atacante.TieneEstado(EstadoAlterado.Debilitado))
        {
            dano *= PenalizacionPorDebilitado;
        }

        if (defensor.TieneEstado(EstadoAlterado.Fortalecido))
        {
            dano *= MitigacionPorFortalecido;
        }

        bool bloqueado = false;
        if (defensorEstaDefendiendo)
        {
            dano *= 0.45;
            bloqueado = true;
        }

        int danoFinal = Math.Max(1, (int)Math.Round(dano));
        return new ResultadoDano(danoFinal, critico, false, bloqueado);
    }
}
