using Microsoft.EntityFrameworkCore.Migrations;

namespace Disfarm.Data.Migrations
{
    public partial class UpdateAchievementRemoveNameProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "name",
                table: "achievements");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "achievements",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
