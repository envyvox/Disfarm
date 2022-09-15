using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Disfarm.Data.Migrations
{
    public partial class UpdateUserStatistic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_all_time_statistics");

            migrationBuilder.DropIndex(
                name: "ix_user_statistics_user_id_type",
                table: "user_statistics");

            migrationBuilder.AddColumn<byte>(
                name: "period",
                table: "user_statistics",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.CreateIndex(
                name: "ix_user_statistics_user_id_period_type",
                table: "user_statistics",
                columns: new[] { "user_id", "period", "type" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_user_statistics_user_id_period_type",
                table: "user_statistics");

            migrationBuilder.DropColumn(
                name: "period",
                table: "user_statistics");

            migrationBuilder.CreateTable(
                name: "user_all_time_statistics",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    type = table.Column<byte>(type: "smallint", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_all_time_statistics", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_all_time_statistics_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_user_statistics_user_id_type",
                table: "user_statistics",
                columns: new[] { "user_id", "type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_all_time_statistics_user_id_type",
                table: "user_all_time_statistics",
                columns: new[] { "user_id", "type" },
                unique: true);
        }
    }
}
