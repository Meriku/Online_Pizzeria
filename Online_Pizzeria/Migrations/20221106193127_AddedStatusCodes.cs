using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Online_Pizzeria.Migrations
{
    public partial class AddedStatusCodes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusCode",
                table: "PizzaOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusCode",
                table: "PizzaOrders");
        }
    }
}
