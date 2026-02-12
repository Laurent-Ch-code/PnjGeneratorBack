using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pnj_generator.Migrations
{
    /// <inheritdoc />
    public partial class AddAgeAndDescriptionToIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "identities",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "identities",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "identities");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "identities");
        }
    }
}
