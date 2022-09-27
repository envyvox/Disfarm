using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Disfarm.Data.Migrations
{
    public partial class CreateAchievementsEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "guilds");

            migrationBuilder.CreateTable(
                name: "achievements",
                columns: table => new
                {
                    type = table.Column<byte>(type: "smallint", nullable: false),
                    category = table.Column<byte>(type: "smallint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    reward_type = table.Column<byte>(type: "smallint", nullable: false),
                    reward_number = table.Column<long>(type: "bigint", nullable: false),
                    points = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_achievements", x => x.type);
                });

            migrationBuilder.CreateTable(
                name: "user_achievements",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    type = table.Column<byte>(type: "smallint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_achievements", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_achievements_achievements_type",
                        column: x => x.type,
                        principalTable: "achievements",
                        principalColumn: "type",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_achievements_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_achievements_type",
                table: "achievements",
                column: "type",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_achievements_type",
                table: "user_achievements",
                column: "type");

            migrationBuilder.CreateIndex(
                name: "ix_user_achievements_user_id_type",
                table: "user_achievements",
                columns: new[] { "user_id", "type" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_achievements");

            migrationBuilder.DropTable(
                name: "achievements");

            migrationBuilder.CreateTable(
                name: "guilds",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_guilds", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_guilds_id",
                table: "guilds",
                column: "id",
                unique: true);
        }
    }
}
