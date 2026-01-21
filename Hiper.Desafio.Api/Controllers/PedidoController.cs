using Microsoft.AspNetCore.Mvc;
using Hiper.Desafio.Domain.Entities;
using Hiper.Desafio.Domain.Interfaces;
using Hiper.Desafio.Application.Services;

namespace Hiper.Desafio.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidosController : ControllerBase
{
    private readonly IPedidoRepository _repository;
    private readonly CalculadoraDesconto _calculadora;

    public PedidosController(IPedidoRepository repository, CalculadoraDesconto calculadora)
    {
        _repository = repository;
        _calculadora = calculadora;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var pedidos = await _repository.GetAllAsync();
        return Ok(pedidos);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Pedido pedido)
    {
        // Aplica a regra de negócio via Strategy
        pedido.ValorFinal = _calculadora.Aplicar(pedido.TipoCliente, pedido.Valor);
        pedido.Status = "Processando";

        // Persistência
        await _repository.AddAsync(pedido);

        return Ok(pedido);
    }
}