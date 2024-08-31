using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShopping.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class editingunitprice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Stock_BookId",
                table: "Stock");

            migrationBuilder.AlterColumn<double>(
                name: "UnitPrice",
                table: "OrderDetails",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "UnitPrice",
                table: "CartDetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stock_BookId",
                table: "Stock",
                column: "BookId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Stock_BookId",
                table: "Stock");

            migrationBuilder.AlterColumn<int>(
                name: "UnitPrice",
                table: "OrderDetails",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "UnitPrice",
                table: "CartDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_BookId",
                table: "Stock",
                column: "BookId");
        }
    }
}
