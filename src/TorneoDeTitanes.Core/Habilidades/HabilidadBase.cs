using TorneoDeTitanes.Core.Combate;
using TorneoDeTitanes.Core.Modelos;

namespace TorneoDeTitanes.Core.Habilidades;

public abstract class HabilidadBase : IHabilidad
{
    public abstract string Nombre { get; }

    public abstract string Descripcion { get; }

    public abstract int CostoEnergia { get; }

    public virtual int TurnosRecarga => 0;

    public abstract TipoPersonaje Tipo { get; }

    public virtual NivelHabilidad Nivel => NivelHabilidad.Especial;

    public void Ejecutar(ContextoAccion contexto, ResultadoAccion resultado)
    {
        contexto.Atacante.Energia = Math.Max(0, contexto.Atacante.Energia - CostoEnergia);
        EjecutarHabilidad(contexto, resultado);
    }

    protected abstract void EjecutarHabilidad(ContextoAccion contexto, ResultadoAccion resultado);

    protected static int AplicarAtaque(
        ContextoAccion contexto,
        ResultadoAccion resultado,
        double multiplicador = 1.0)
    {
        var calculo = contexto.CalculadoraDano.Calcular(
            contexto.Atacante,
            contexto.Defensor,
            defensorEstaDefendiendo: false,
            contexto.Random);

        if (calculo.Esquivado)
        {
            resultado.Agregar(
                $"{contexto.Defensor.Nombre} esquiva el ataque de {contexto.Atacante.Nombre}.",
                TipoEvento.Esquiva);
            return 0;
        }

        int dano = Math.Max(1, (int)Math.Round(calculo.Dano * multiplicador));
        contexto.Defensor.RecibirDano(dano);
        resultado.DanoInfligido += dano;

        string prefijo = calculo.Critico ? "¡GOLPE CRÍTICO! " : string.Empty;
        resultado.Agregar(
            $"{prefijo}{contexto.Atacante.Nombre} causa {dano} de daño a {contexto.Defensor.Nombre}.",
            calculo.Critico ? TipoEvento.Critico : TipoEvento.Ataque);

        return dano;
    }

    protected static void AplicarEstado(
        Personaje objetivo,
        ResultadoAccion resultado,
        EfectoEstado efecto,
        string mensaje)
    {
        objetivo.AgregarEstado(efecto);
        resultado.Agregar(mensaje, TipoEvento.Estado);
    }

    protected static void AplicarCuracion(
        Personaje objetivo,
        ResultadoAccion resultado,
        int cantidad,
        string mensaje)
    {
        objetivo.Curar(cantidad);
        resultado.Agregar(mensaje, TipoEvento.Curacion);
    }
}
