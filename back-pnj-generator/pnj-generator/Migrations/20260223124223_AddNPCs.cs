using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pnj_generator.Migrations
{
    /// <inheritdoc />
    public partial class AddNPCs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "npcs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UniverseId = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentitySnapshot = table.Column<string>(type: "text", nullable: false),
                    CharacteristicsSnapshot = table.Column<string>(type: "text", nullable: false),
                    SkillsSnapshot = table.Column<string>(type: "text", nullable: false),
                    WeaponsSnapshot = table.Column<string>(type: "text", nullable: false),
                    ProtectionsSnapshot = table.Column<string>(type: "text", nullable: false),
                    EquipmentSnapshot = table.Column<string>(type: "text", nullable: false),
                    TraitsSnapshot = table.Column<string>(type: "text", nullable: false),
                    HP = table.Column<int>(type: "integer", nullable: true),
                    PhysicalDescription = table.Column<string>(type: "text", nullable: true),
                    GMNotes = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_npcs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_npcs_universes_UniverseId",
                        column: x => x.UniverseId,
                        principalTable: "universes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_npcs_UniverseId",
                table: "npcs",
                column: "UniverseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "npcs");
        }
    }
}
