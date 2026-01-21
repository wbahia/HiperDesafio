namespace Hiper.Desafio.Domain.Strategies;

public interface IDescontoStrategy
{
    decimal Calcular(decimal valorOriginal);
    string TipoDesconto { get; }
}