using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Disfarm.Data.Migrations
{
    public partial class UpdateSeedAndFarmUseTimeSpan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "progress",
                table: "user_farms");

            migrationBuilder.DropColumn(
                name: "growth_days",
                table: "seeds");

            migrationBuilder.DropColumn(
                name: "re_growth_days",
                table: "seeds");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "complete_at",
                table: "user_farms",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "growth",
                table: "seeds",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "re_growth",
                table: "seeds",
                type: "interval",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "complete_at",
                table: "user_farms");

            migrationBuilder.DropColumn(
                name: "growth",
                table: "seeds");

            migrationBuilder.DropColumn(
                name: "re_growth",
                table: "seeds");

            migrationBuilder.AddColumn<long>(
                name: "progress",
                table: "user_farms",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "growth_days",
                table: "seeds",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "re_growth_days",
                table: "seeds",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
