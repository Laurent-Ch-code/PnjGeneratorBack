using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pnj_generator.Migrations
{
    /// <inheritdoc />
    public partial class protections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "characteristics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UniverseId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Value = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Modifier = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_characteristics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_characteristics_universes_UniverseId",
                        column: x => x.UniverseId,
                        principalTable: "universes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "protections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UniverseId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ArmorRating = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Material = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Weight = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_protections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_protections_universes_UniverseId",
                        column: x => x.UniverseId,
                        principalTable: "universes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "skills",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UniverseId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    RelatedCharacteristic = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Bonus = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Malus = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Prerequisites = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_skills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_skills_universes_UniverseId",
                        column: x => x.UniverseId,
                        principalTable: "universes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "traits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UniverseId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Effect = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Prerequisites = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_traits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_traits_universes_UniverseId",
                        column: x => x.UniverseId,
                        principalTable: "universes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_characteristics_UniverseId",
                table: "characteristics",
                column: "UniverseId");

            migrationBuilder.CreateIndex(
                name: "IX_protections_UniverseId",
                table: "protections",
                column: "UniverseId");

            migrationBuilder.CreateIndex(
                name: "IX_skills_UniverseId",
                table: "skills",
                column: "UniverseId");

            migrationBuilder.CreateIndex(
                name: "IX_traits_UniverseId",
                table: "traits",
                column: "UniverseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "characteristics");

            migrationBuilder.DropTable(
                name: "protections");

            migrationBuilder.DropTable(
                name: "skills");

            migrationBuilder.DropTable(
                name: "traits");
        }
    }
}
