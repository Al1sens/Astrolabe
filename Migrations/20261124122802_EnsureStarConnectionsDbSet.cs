using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrolabe.Migrations
{
    /// <inheritdoc />
    public partial class EnsureStarConnectionsDbSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StarConnection_Stars_FromStarId",
                table: "StarConnection");

            migrationBuilder.DropForeignKey(
                name: "FK_StarConnection_Stars_ToStarId",
                table: "StarConnection");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StarConnection",
                table: "StarConnection");

            migrationBuilder.RenameTable(
                name: "StarConnection",
                newName: "StarConnections");

            migrationBuilder.RenameIndex(
                name: "IX_StarConnection_ToStarId",
                table: "StarConnections",
                newName: "IX_StarConnections_ToStarId");

            migrationBuilder.RenameIndex(
                name: "IX_StarConnection_FromStarId",
                table: "StarConnections",
                newName: "IX_StarConnections_FromStarId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StarConnections",
                table: "StarConnections",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StarConnections_Stars_FromStarId",
                table: "StarConnections",
                column: "FromStarId",
                principalTable: "Stars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StarConnections_Stars_ToStarId",
                table: "StarConnections",
                column: "ToStarId",
                principalTable: "Stars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StarConnections_Stars_FromStarId",
                table: "StarConnections");

            migrationBuilder.DropForeignKey(
                name: "FK_StarConnections_Stars_ToStarId",
                table: "StarConnections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StarConnections",
                table: "StarConnections");

            migrationBuilder.RenameTable(
                name: "StarConnections",
                newName: "StarConnection");

            migrationBuilder.RenameIndex(
                name: "IX_StarConnections_ToStarId",
                table: "StarConnection",
                newName: "IX_StarConnection_ToStarId");

            migrationBuilder.RenameIndex(
                name: "IX_StarConnections_FromStarId",
                table: "StarConnection",
                newName: "IX_StarConnection_FromStarId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StarConnection",
                table: "StarConnection",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StarConnection_Stars_FromStarId",
                table: "StarConnection",
                column: "FromStarId",
                principalTable: "Stars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StarConnection_Stars_ToStarId",
                table: "StarConnection",
                column: "ToStarId",
                principalTable: "Stars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
