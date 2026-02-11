using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pnj_generator.Migrations
{
    /// <inheritdoc />
    public partial class AddIdentitySystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdditionalInformation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    UniverseId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdditionalInformation_universes_UniverseId",
                        column: x => x.UniverseId,
                        principalTable: "universes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "fragment_identities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UniverseId = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Gender = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fragment_identities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_fragment_identities_universes_UniverseId",
                        column: x => x.UniverseId,
                        principalTable: "universes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "alignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_alignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_alignments_AdditionalInformation_Id",
                        column: x => x.Id,
                        principalTable: "AdditionalInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cultures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cultures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_cultures_AdditionalInformation_Id",
                        column: x => x.Id,
                        principalTable: "AdditionalInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "origins",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_origins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_origins_AdditionalInformation_Id",
                        column: x => x.Id,
                        principalTable: "AdditionalInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "species",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_species", x => x.Id);
                    table.ForeignKey(
                        name: "FK_species_AdditionalInformation_Id",
                        column: x => x.Id,
                        principalTable: "AdditionalInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UniverseId = table.Column<Guid>(type: "uuid", nullable: false),
                    CultureId = table.Column<Guid>(type: "uuid", nullable: true),
                    SpecieId = table.Column<Guid>(type: "uuid", nullable: true),
                    AlignmentId = table.Column<Guid>(type: "uuid", nullable: true),
                    OriginId = table.Column<Guid>(type: "uuid", nullable: true),
                    NameId = table.Column<Guid>(type: "uuid", nullable: true),
                    FirstNameId = table.Column<Guid>(type: "uuid", nullable: true),
                    AliasId = table.Column<Guid>(type: "uuid", nullable: true),
                    Gender = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_identities_alignments_AlignmentId",
                        column: x => x.AlignmentId,
                        principalTable: "alignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_identities_cultures_CultureId",
                        column: x => x.CultureId,
                        principalTable: "cultures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_identities_fragment_identities_AliasId",
                        column: x => x.AliasId,
                        principalTable: "fragment_identities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_identities_fragment_identities_FirstNameId",
                        column: x => x.FirstNameId,
                        principalTable: "fragment_identities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_identities_fragment_identities_NameId",
                        column: x => x.NameId,
                        principalTable: "fragment_identities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_identities_origins_OriginId",
                        column: x => x.OriginId,
                        principalTable: "origins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_identities_species_SpecieId",
                        column: x => x.SpecieId,
                        principalTable: "species",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_identities_universes_UniverseId",
                        column: x => x.UniverseId,
                        principalTable: "universes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalInformation_UniverseId",
                table: "AdditionalInformation",
                column: "UniverseId");

            migrationBuilder.CreateIndex(
                name: "IX_fragment_identities_UniverseId",
                table: "fragment_identities",
                column: "UniverseId");

            migrationBuilder.CreateIndex(
                name: "IX_identities_AliasId",
                table: "identities",
                column: "AliasId");

            migrationBuilder.CreateIndex(
                name: "IX_identities_AlignmentId",
                table: "identities",
                column: "AlignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_identities_CultureId",
                table: "identities",
                column: "CultureId");

            migrationBuilder.CreateIndex(
                name: "IX_identities_FirstNameId",
                table: "identities",
                column: "FirstNameId");

            migrationBuilder.CreateIndex(
                name: "IX_identities_NameId",
                table: "identities",
                column: "NameId");

            migrationBuilder.CreateIndex(
                name: "IX_identities_OriginId",
                table: "identities",
                column: "OriginId");

            migrationBuilder.CreateIndex(
                name: "IX_identities_SpecieId",
                table: "identities",
                column: "SpecieId");

            migrationBuilder.CreateIndex(
                name: "IX_identities_UniverseId",
                table: "identities",
                column: "UniverseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "identities");

            migrationBuilder.DropTable(
                name: "alignments");

            migrationBuilder.DropTable(
                name: "cultures");

            migrationBuilder.DropTable(
                name: "fragment_identities");

            migrationBuilder.DropTable(
                name: "origins");

            migrationBuilder.DropTable(
                name: "species");

            migrationBuilder.DropTable(
                name: "AdditionalInformation");

            migrationBuilder.RenameColumn(
                name: "WeaponFireMode",
                table: "weapons",
                newName: "weaponFireMode");

            migrationBuilder.RenameColumn(
                name: "Radius",
                table: "weapons",
                newName: "radius");
        }
    }
}
