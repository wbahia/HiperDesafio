namespace Hiper.Desafio.Domain.Interfaces;

using Hiper.Desafio.Domain.Entities;

public interface IPedidoRepository
{
    Task AddAsync(Pedido pedido);
    Task<IEnumerable<Pedido>> GetAllAsync();
}