using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShopping.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class ed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StockQuantity",
                table: "CartDetails",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StockQuantity",
                table: "CartDetails");
        }
    }
}
