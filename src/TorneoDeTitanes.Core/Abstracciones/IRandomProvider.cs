namespace TorneoDeTitanes.Core.Abstracciones;

public interface IRandomProvider
{
    int Siguiente(int minInclusive, int maxExclusive);

    double SiguienteDoble();
}

public sealed class RandomProvider : IRandomProvider
{
    private readonly Random _random;

    public RandomProvider(int? semilla = null)
    {
        _random = semilla.HasValue ? new Random(semilla.Value) : new Random();
    }

    public int Siguiente(int minInclusive, int maxExclusive) => _random.Next(minInclusive, maxExclusive);

    public double SiguienteDoble() => _random.NextDouble();
}
