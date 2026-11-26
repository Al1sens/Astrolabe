using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrolabe.Migrations
{
    /// <inheritdoc />
    public partial class AddStarStagesAndDifficulty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMastered",
                table: "Stars");

            migrationBuilder.AddColumn<int>(
                name: "Difficulty",
                table: "Stars",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Stage",
                table: "Stars",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Difficulty",
                table: "Stars");

            migrationBuilder.DropColumn(
                name: "Stage",
                table: "Stars");

            migrationBuilder.AddColumn<bool>(
                name: "IsMastered",
                table: "Stars",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
