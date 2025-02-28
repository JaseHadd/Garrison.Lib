using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garrison.Lib.Migrations
{
    /// <inheritdoc />
    public partial class Sessions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "session",
                columns: table => new
                {
                    session_number = table.Column<ushort>(type: "smallint unsigned", nullable: false),
                    adventure_id = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_session", x => new { x.adventure_id, x.session_number });
                    table.ForeignKey(
                        name: "fk_session_adventure_adventure_id",
                        column: x => x.adventure_id,
                        principalTable: "adventure",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "session");
        }
    }
}
