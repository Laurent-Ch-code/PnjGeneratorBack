using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pnj_generator.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "universes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Era = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DiceRule = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    HasModifiers = table.Column<bool>(type: "boolean", nullable: false),
                    ModifierType = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_universes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "additionalInformations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    UniverseId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_additionalInformations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_additionalInformations_universes_UniverseId",
                        column: x => x.UniverseId,
                        principalTable: "universes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "characteristics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UniverseId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    GenerationType = table.Column<int>(type: "integer", nullable: false),
                    DiceType = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    MinDice = table.Column<int>(type: "integer", nullable: true),
                    MaxDice = table.Column<int>(type: "integer", nullable: true),
                    MinValue = table.Column<int>(type: "integer", nullable: true),
                    MaxValue = table.Column<int>(type: "integer", nullable: true),
                    HasModifiers = table.Column<bool>(type: "boolean", nullable: false)
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
                name: "equipments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UniverseId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Bonus = table.Column<string>(type: "text", nullable: false),
                    Malus = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_equipments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_equipments_universes_UniverseId",
                        column: x => x.UniverseId,
                        principalTable: "universes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "fragmentIdentities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UniverseId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Gender = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fragmentIdentities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_fragmentIdentities_universes_UniverseId",
                        column: x => x.UniverseId,
                        principalTable: "universes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateTable(
                name: "weapons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UniverseId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Damage = table.Column<string>(type: "text", nullable: false),
                    Range = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Capacity = table.Column<int>(type: "integer", nullable: false),
                    Radius = table.Column<int>(type: "integer", nullable: false),
                    WeaponFireMode = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weapons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_weapons_universes_UniverseId",
                        column: x => x.UniverseId,
                        principalTable: "universes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                        name: "FK_alignments_additionalInformations_Id",
                        column: x => x.Id,
                        principalTable: "additionalInformations",
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
                        name: "FK_cultures_additionalInformations_Id",
                        column: x => x.Id,
                        principalTable: "additionalInformations",
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
                        name: "FK_origins_additionalInformations_Id",
                        column: x => x.Id,
                        principalTable: "additionalInformations",
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
                        name: "FK_species_additionalInformations_Id",
                        column: x => x.Id,
                        principalTable: "additionalInformations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "modifierRules",
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
                    table.PrimaryKey("PK_modifierRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_modifierRules_characteristics_CharacteristicId",
                        column: x => x.CharacteristicId,
                        principalTable: "characteristics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_modifierRules_universes_UniverseId",
                        column: x => x.UniverseId,
                        principalTable: "universes",
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
                    Gender = table.Column<int>(type: "integer", nullable: false),
                    Age = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
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
                        name: "FK_identities_fragmentIdentities_AliasId",
                        column: x => x.AliasId,
                        principalTable: "fragmentIdentities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_identities_fragmentIdentities_FirstNameId",
                        column: x => x.FirstNameId,
                        principalTable: "fragmentIdentities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_identities_fragmentIdentities_NameId",
                        column: x => x.NameId,
                        principalTable: "fragmentIdentities",
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
                name: "IX_additionalInformations_UniverseId",
                table: "additionalInformations",
                column: "UniverseId");

            migrationBuilder.CreateIndex(
                name: "IX_characteristics_UniverseId",
                table: "characteristics",
                column: "UniverseId");

            migrationBuilder.CreateIndex(
                name: "IX_equipments_UniverseId",
                table: "equipments",
                column: "UniverseId");

            migrationBuilder.CreateIndex(
                name: "IX_fragmentIdentities_UniverseId",
                table: "fragmentIdentities",
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

            migrationBuilder.CreateIndex(
                name: "IX_modifierRules_CharacteristicId",
                table: "modifierRules",
                column: "CharacteristicId");

            migrationBuilder.CreateIndex(
                name: "IX_modifierRules_UniverseId",
                table: "modifierRules",
                column: "UniverseId");

            migrationBuilder.CreateIndex(
                name: "IX_npcs_UniverseId",
                table: "npcs",
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

            migrationBuilder.CreateIndex(
                name: "IX_weapons_UniverseId",
                table: "weapons",
                column: "UniverseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "equipments");

            migrationBuilder.DropTable(
                name: "identities");

            migrationBuilder.DropTable(
                name: "modifierRules");

            migrationBuilder.DropTable(
                name: "npcs");

            migrationBuilder.DropTable(
                name: "protections");

            migrationBuilder.DropTable(
                name: "skills");

            migrationBuilder.DropTable(
                name: "traits");

            migrationBuilder.DropTable(
                name: "weapons");

            migrationBuilder.DropTable(
                name: "alignments");

            migrationBuilder.DropTable(
                name: "cultures");

            migrationBuilder.DropTable(
                name: "fragmentIdentities");

            migrationBuilder.DropTable(
                name: "origins");

            migrationBuilder.DropTable(
                name: "species");

            migrationBuilder.DropTable(
                name: "characteristics");

            migrationBuilder.DropTable(
                name: "additionalInformations");

            migrationBuilder.DropTable(
                name: "universes");
        }
    }
}
