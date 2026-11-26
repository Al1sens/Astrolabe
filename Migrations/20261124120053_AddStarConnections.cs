using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Astrolabe.Migrations
{
    /// <inheritdoc />
    public partial class AddStarConnections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StarConnection",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FromStarId = table.Column<int>(type: "integer", nullable: false),
                    ToStarId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StarConnection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StarConnection_Stars_FromStarId",
                        column: x => x.FromStarId,
                        principalTable: "Stars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StarConnection_Stars_ToStarId",
                        column: x => x.ToStarId,
                        principalTable: "Stars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StarConnection_FromStarId",
                table: "StarConnection",
                column: "FromStarId");

            migrationBuilder.CreateIndex(
                name: "IX_StarConnection_ToStarId",
                table: "StarConnection",
                column: "ToStarId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StarConnection");
        }
    }
}
