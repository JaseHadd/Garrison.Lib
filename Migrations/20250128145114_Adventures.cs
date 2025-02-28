using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Garrison.Lib.Migrations
{
    /// <inheritdoc />
    public partial class Adventures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "adventure",
                columns: table => new
                {
                    id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    game_master_id = table.Column<uint>(type: "int unsigned", nullable: false),
                    name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_adventure", x => x.id);
                    table.ForeignKey(
                        name: "fk_adventure_user_game_master_id",
                        column: x => x.game_master_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "adventure_player",
                columns: table => new
                {
                    player_id = table.Column<uint>(type: "int unsigned", nullable: false),
                    adventure_id = table.Column<uint>(type: "int unsigned", nullable: false),
                    character_id = table.Column<uint>(type: "int unsigned", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_adventure_player", x => new { x.adventure_id, x.player_id });
                    table.ForeignKey(
                        name: "fk_adventure_player_adventure_adventure_id",
                        column: x => x.adventure_id,
                        principalTable: "adventure",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_adventure_player_character_character_id",
                        column: x => x.character_id,
                        principalTable: "character",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_adventure_player_user_player_id",
                        column: x => x.player_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "ix_adventure_game_master_id",
                table: "adventure",
                column: "game_master_id");

            migrationBuilder.CreateIndex(
                name: "ix_adventure_player_character_id",
                table: "adventure_player",
                column: "character_id");

            migrationBuilder.CreateIndex(
                name: "ix_adventure_player_player_id",
                table: "adventure_player",
                column: "player_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "adventure_player");

            migrationBuilder.DropTable(
                name: "adventure");
        }
    }
}
