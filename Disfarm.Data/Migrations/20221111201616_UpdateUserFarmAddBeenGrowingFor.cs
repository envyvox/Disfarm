using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Disfarm.Data.Migrations
{
    public partial class UpdateUserFarmAddBeenGrowingFor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "been_growing_for",
                table: "user_farms",
                type: "interval",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "been_growing_for",
                table: "user_farms");
        }
    }
}
