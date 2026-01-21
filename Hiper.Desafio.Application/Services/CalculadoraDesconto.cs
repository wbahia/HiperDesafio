using Hiper.Desafio.Domain.Strategies;

namespace Hiper.Desafio.Application.Services;

public class CalculadoraDesconto
{
    // Recebe TODAS as estratégias registradas na injeção de dependência
    private readonly IEnumerable<IDescontoStrategy> _strategies;

    public CalculadoraDesconto(IEnumerable<IDescontoStrategy> strategies)
    {
        _strategies = strategies;
    }

    public decimal Aplicar(string tipoCliente, decimal valor)
    {
        // Procura na lista a estratégia que tem o nome igual ao tipo do cliente
        var strategy = _strategies.FirstOrDefault(s => s.TipoDesconto == tipoCliente);

        // Se não achar (ex: cliente mandou "XYZ"), usa a estratégia Comum como fallback
        if (strategy == null)
        {
            strategy = new DescontoComumStrategy();
        }

        return strategy.Calcular(valor);
    }
}