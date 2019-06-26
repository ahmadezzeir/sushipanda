using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class DishAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Dish",
                table => new
                {
                    Id = table.Column<Guid>(),
                    Name = table.Column<string>(nullable: true),
                    Calories = table.Column<double>(),
                    Weight = table.Column<double>()
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dish", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "Dish");
        }
    }
}
