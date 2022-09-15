using Microsoft.EntityFrameworkCore.Migrations;

namespace Disfarm.Data.Migrations
{
    public partial class UpdateLocalizationEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_localizations_category_name",
                table: "localizations");

            migrationBuilder.CreateIndex(
                name: "ix_localizations_category_language_name",
                table: "localizations",
                columns: new[] { "category", "language", "name" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_localizations_category_language_name",
                table: "localizations");

            migrationBuilder.CreateIndex(
                name: "ix_localizations_category_name",
                table: "localizations",
                columns: new[] { "category", "name" },
                unique: true);
        }
    }
}
