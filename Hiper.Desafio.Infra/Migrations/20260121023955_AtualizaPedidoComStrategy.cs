using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hiper.Desafio.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AtualizaPedidoComStrategy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TipoCliente",
                table: "Pedidos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "ValorFinal",
                table: "Pedidos",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoCliente",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "ValorFinal",
                table: "Pedidos");
        }
    }
}
