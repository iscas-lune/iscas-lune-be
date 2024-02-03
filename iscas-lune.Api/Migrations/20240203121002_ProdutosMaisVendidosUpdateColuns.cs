using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iscaslune.Api.Migrations
{
    /// <inheritdoc />
    public partial class ProdutosMaisVendidosUpdateColuns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantidade",
                table: "ProdutosMaisVendidos",
                newName: "QuantidadeProdutos");

            migrationBuilder.AddColumn<decimal>(
                name: "QuantidadePedidos",
                table: "ProdutosMaisVendidos",
                type: "numeric(12,2)",
                precision: 12,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantidadePedidos",
                table: "ProdutosMaisVendidos");

            migrationBuilder.RenameColumn(
                name: "QuantidadeProdutos",
                table: "ProdutosMaisVendidos",
                newName: "Quantidade");
        }
    }
}
