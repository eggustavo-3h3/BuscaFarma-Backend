using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarmaciaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddCodigoResetSenhaToUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChaveResetSenha",
                table: "TB_Usuario");

            migrationBuilder.AddColumn<string>(
                name: "codigo_reset_senha",
                table: "TB_Usuario",
                type: "varchar(6)",
                maxLength: 6,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "codigo_reset_senha",
                table: "TB_Usuario");

            migrationBuilder.AddColumn<Guid>(
                name: "ChaveResetSenha",
                table: "TB_Usuario",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");
        }
    }
}
