namespace Hiper.Desafio.Domain.Entities;

public class Pedido
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Descricao { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public decimal ValorFinal { get; set; }
    public string TipoCliente { get; set; } = "Comum";
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "Pendente";
}