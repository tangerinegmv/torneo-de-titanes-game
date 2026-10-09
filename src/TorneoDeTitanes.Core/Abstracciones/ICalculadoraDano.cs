using TorneoDeTitanes.Core.Modelos;

namespace TorneoDeTitanes.Core.Abstracciones;

public readonly record struct ResultadoDano(int Dano, bool Critico, bool Esquivado, bool Bloqueado);

public interface ICalculadoraDano
{
    ResultadoDano Calcular(Personaje atacante, Personaje defensor, bool defensorEstaDefendiendo, IRandomProvider random);
}
