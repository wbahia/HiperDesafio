using Hiper.Desafio.Application.Services;
using Hiper.Desafio.Domain.Strategies;

namespace Hiper.Desafio.Tests;

public class CalculadoraDescontoTests
{
    private readonly CalculadoraDesconto _calculadora;

    public CalculadoraDescontoTests()
    {
        // lista de estratégias para o teste
        var strategies = new List<IDescontoStrategy>
        {
            new DescontoVipStrategy(),
            new DescontoComumStrategy()
        };
        
        _calculadora = new CalculadoraDesconto(strategies);
    }

    [Theory]
    [InlineData(100.00, "VIP", 85.00)]   // 15% de desconto
    [InlineData(100.00, "Comum", 95.00)] // 5% de desconto
    [InlineData(0.00, "VIP", 0.00)]      // fronteira: valor 0
    [InlineData(250.50, "VIP", 212.92)]  // Valor quebrado (15% de 250.50)
    public void Aplicar_DeveCalcularValorFinalCorretamente(decimal valorOriginal, string tipoCliente, decimal valorEsperado)
    {
        // Act
        var resultado = _calculadora.Aplicar(tipoCliente, valorOriginal);

        // Assert
        Assert.Equal(valorEsperado, resultado, 2);
    }

    [Fact]
    public void Aplicar_TipoDesconhecido_DeveRetornarValorComDescontoPadrao()
    {
        // Arrange
        var valorOriginal = 100m;
        var valorEsperado = 95m; // Desconto Comum como fallback
        var tipoInexistente = "Premium_Nao_Mapeado";

        // Act
        var resultado = _calculadora.Aplicar(tipoInexistente, valorOriginal);

        // Assert
        Assert.Equal(valorEsperado, resultado, 2);
    }
}