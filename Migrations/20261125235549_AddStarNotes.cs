using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrolabe.Migrations
{
    /// <inheritdoc />
    public partial class AddStarNotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Stars",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Stars");
        }
    }
}
