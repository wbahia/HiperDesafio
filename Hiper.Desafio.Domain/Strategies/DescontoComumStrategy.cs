namespace Hiper.Desafio.Domain.Strategies;

public class DescontoComumStrategy : IDescontoStrategy
{
    public string TipoDesconto => "Comum";

    public decimal Calcular(decimal valorOriginal)
    {
        return valorOriginal * 0.95m; // 5%
    }
}