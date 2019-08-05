using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class FileModelChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_File_Dish_Id",
                table: "File");

            migrationBuilder.CreateIndex(
                name: "IX_Dish_FileId",
                table: "Dish",
                column: "FileId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Dish_File_FileId",
                table: "Dish",
                column: "FileId",
                principalTable: "File",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dish_File_FileId",
                table: "Dish");

            migrationBuilder.DropIndex(
                name: "IX_Dish_FileId",
                table: "Dish");

            migrationBuilder.AddForeignKey(
                name: "FK_File_Dish_Id",
                table: "File",
                column: "Id",
                principalTable: "Dish",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
