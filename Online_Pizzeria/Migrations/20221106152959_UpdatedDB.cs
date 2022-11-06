using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Online_Pizzeria.Migrations
{
    public partial class UpdatedDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "PizzaOrders");

            migrationBuilder.AddColumn<int>(
                name: "PizzaId",
                table: "PizzaOrders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Pizzas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Ingredients = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pizzas", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pizzas");

            migrationBuilder.DropColumn(
                name: "PizzaId",
                table: "PizzaOrders");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "PizzaOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
