using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garrison.Lib.Migrations
{
    /// <inheritdoc />
    public partial class UserPasswords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "password",
                table: "user",
                type: "binary(49)",
                fixedLength: true,
                maxLength: 49,
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "password",
                table: "user");
        }
    }
}
