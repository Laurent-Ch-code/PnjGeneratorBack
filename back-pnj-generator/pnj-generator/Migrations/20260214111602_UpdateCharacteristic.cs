using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pnj_generator.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCharacteristic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "characteristics");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "characteristics");

            migrationBuilder.AddColumn<string>(
                name: "DiceType",
                table: "characteristics",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "HasModifiers",
                table: "characteristics",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MaxDice",
                table: "characteristics",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinDice",
                table: "characteristics",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiceType",
                table: "characteristics");

            migrationBuilder.DropColumn(
                name: "HasModifiers",
                table: "characteristics");

            migrationBuilder.DropColumn(
                name: "MaxDice",
                table: "characteristics");

            migrationBuilder.DropColumn(
                name: "MinDice",
                table: "characteristics");

            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "characteristics",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "characteristics",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
