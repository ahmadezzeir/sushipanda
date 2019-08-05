using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class DishModelChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_File_Dish_DishId",
                table: "File");

            migrationBuilder.DropIndex(
                name: "IX_File_DishId",
                table: "File");

            migrationBuilder.DropColumn(
                name: "DishId",
                table: "File");

            migrationBuilder.AlterColumn<int>(
                name: "Weight",
                table: "Dish",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "Calories",
                table: "Dish",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Dish",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FileId",
                table: "Dish",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Dish",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_File_Dish_Id",
                table: "File",
                column: "Id",
                principalTable: "Dish",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_File_Dish_Id",
                table: "File");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Dish");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "Dish");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Dish");

            migrationBuilder.AddColumn<Guid>(
                name: "DishId",
                table: "File",
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Weight",
                table: "Dish",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<double>(
                name: "Calories",
                table: "Dish",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_File_DishId",
                table: "File",
                column: "DishId");

            migrationBuilder.AddForeignKey(
                name: "FK_File_Dish_DishId",
                table: "File",
                column: "DishId",
                principalTable: "Dish",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
