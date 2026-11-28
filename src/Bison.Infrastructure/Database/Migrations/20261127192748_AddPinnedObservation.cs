using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bison.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddPinnedObservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPinned",
                table: "Posts",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "License",
                table: "Posts",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPinned",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "License",
                table: "Posts");
        }
    }
}
