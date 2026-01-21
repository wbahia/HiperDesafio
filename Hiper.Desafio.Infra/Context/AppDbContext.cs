using Microsoft.EntityFrameworkCore;
using Hiper.Desafio.Domain.Entities;

namespace Hiper.Desafio.Infra.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Pedido> Pedidos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração Fluente (Melhor que Data Annotations para Sênior)
            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Descricao).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Valor).HasColumnType("decimal(18,2)");
                entity.Property(e => e.ValorFinal).HasColumnType("decimal(18,2)");
            });
        }
    }
}