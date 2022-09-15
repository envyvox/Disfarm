using Microsoft.EntityFrameworkCore.Migrations;

namespace Disfarm.Data.Migrations
{
    public partial class UpdateImageEntityUpdateIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_images_type",
                table: "images");

            migrationBuilder.CreateIndex(
                name: "ix_images_type_language",
                table: "images",
                columns: new[] { "type", "language" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_images_type_language",
                table: "images");

            migrationBuilder.CreateIndex(
                name: "ix_images_type",
                table: "images",
                column: "type");
        }
    }
}
