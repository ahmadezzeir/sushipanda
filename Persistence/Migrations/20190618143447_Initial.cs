using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "User",
                table => new
                {
                    Id = table.Column<Guid>(),
                    Name = table.Column<string>(),
                    NormalizedName = table.Column<string>(),
                    Email = table.Column<string>(),
                    NormalizedEmail = table.Column<string>(),
                    PasswordHash = table.Column<string>(),
                    EmailConfirmed = table.Column<bool>()
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "User");
        }
    }
}
