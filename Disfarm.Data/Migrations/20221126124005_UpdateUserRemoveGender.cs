using Microsoft.EntityFrameworkCore.Migrations;

namespace Disfarm.Data.Migrations
{
    public partial class UpdateUserRemoveGender : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "gender",
                table: "users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "gender",
                table: "users",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0);
        }
    }
}
