using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pnj_generator.Migrations
{
    /// <inheritdoc />
    public partial class AddModifierRules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ModifierRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UniverseId = table.Column<Guid>(type: "uuid", nullable: false),
                    CharacteristicId = table.Column<Guid>(type: "uuid", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    RangeMin = table.Column<int>(type: "integer", nullable: true),
                    RangeMax = table.Column<int>(type: "integer", nullable: true),
                    Modifier = table.Column<int>(type: "integer", nullable: true),
                    AvailableValue = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModifierRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModifierRules_characteristics_CharacteristicId",
                        column: x => x.CharacteristicId,
                        principalTable: "characteristics",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ModifierRules_universes_UniverseId",
                        column: x => x.UniverseId,
                        principalTable: "universes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModifierRules_CharacteristicId",
                table: "ModifierRules",
                column: "CharacteristicId");

            migrationBuilder.CreateIndex(
                name: "IX_ModifierRules_UniverseId",
                table: "ModifierRules",
                column: "UniverseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModifierRules");
        }
    }
}
