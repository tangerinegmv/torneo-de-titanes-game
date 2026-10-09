using TorneoDeTitanes.Core.Abstracciones;

namespace TorneoDeTitanes.Core.Tests.Dobles;

public sealed class RandomSecuencia : IRandomProvider
{
    private readonly Queue<double> _dobles;
    private readonly Queue<int> _enteros;

    public RandomSecuencia(IEnumerable<double> dobles, IEnumerable<int>? enteros = null)
    {
        _dobles = new Queue<double>(dobles);
        _enteros = enteros is null ? new Queue<int>() : new Queue<int>(enteros);
    }

    public int Siguiente(int minInclusive, int maxExclusive)
        => _enteros.Count > 0 ? Math.Clamp(_enteros.Dequeue(), minInclusive, maxExclusive - 1) : minInclusive;

    public double SiguienteDoble()
        => _dobles.Count > 0 ? Math.Clamp(_dobles.Dequeue(), 0d, 1d) : 0d;
}