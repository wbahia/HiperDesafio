namespace Hiper.Desafio.Domain.Strategies;

public class DescontoVipStrategy : IDescontoStrategy
{
    public string TipoDesconto => "VIP";

    public decimal Calcular(decimal valorOriginal)
    {
        return valorOriginal * 0.85m; // 15%
    }
}