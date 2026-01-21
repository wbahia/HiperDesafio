using Hiper.Desafio.Application.Services;
using Hiper.Desafio.Domain.Entities;
using Hiper.Desafio.Domain.Interfaces;
using Hiper.Desafio.Infra.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace Hiper.Desafio.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidosController : ControllerBase
{
    private readonly IPedidoRepository _repository;
    private readonly CalculadoraDesconto _calculadora;
    private readonly MessageBusService _messageBus;

    public PedidosController(IPedidoRepository repository, 
                             CalculadoraDesconto calculadora, 
                             MessageBusService messageBus)
    {
        _repository = repository;
        _calculadora = calculadora;
        _messageBus = messageBus;
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
        // Lógica de negócio (Strategy)
        pedido.ValorFinal = _calculadora.Aplicar(pedido.TipoCliente, pedido.Valor);
        pedido.Status = "Processando";

        // Persistência
        await _repository.AddAsync(pedido);

        // Envio de mensagem
        _messageBus.PublicarPedido(pedido);

        return Ok(pedido);
    }
}