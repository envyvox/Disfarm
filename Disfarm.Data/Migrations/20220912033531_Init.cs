using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Disfarm.Data.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "banners",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    rarity = table.Column<byte>(type: "smallint", nullable: false),
                    price = table.Column<long>(type: "bigint", nullable: false),
                    url = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_banners", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "guilds",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_guilds", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "localizations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    category = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    language = table.Column<byte>(type: "smallint", nullable: false),
                    single = table.Column<string>(type: "text", nullable: false),
                    @double = table.Column<string>(name: "double", type: "text", nullable: false),
                    multiply = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_localizations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    about = table.Column<string>(type: "text", nullable: true),
                    level = table.Column<long>(type: "bigint", nullable: false),
                    xp = table.Column<long>(type: "bigint", nullable: false),
                    title = table.Column<byte>(type: "smallint", nullable: false),
                    gender = table.Column<byte>(type: "smallint", nullable: false),
                    command_color = table.Column<string>(type: "text", nullable: false),
                    is_premium = table.Column<bool>(type: "boolean", nullable: false),
                    language = table.Column<byte>(type: "smallint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "world_properties",
                columns: table => new
                {
                    type = table.Column<byte>(type: "smallint", nullable: false),
                    value = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_world_properties", x => x.type);
                });

            migrationBuilder.CreateTable(
                name: "world_states",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    current_season = table.Column<byte>(type: "smallint", nullable: false),
                    weather_today = table.Column<byte>(type: "smallint", nullable: false),
                    weather_tomorrow = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_world_states", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_banners",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    banner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_banners", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_banners_banners_banner_id",
                        column: x => x.banner_id,
                        principalTable: "banners",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_banners_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_currencies",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    type = table.Column<byte>(type: "smallint", nullable: false),
                    amount = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_currencies", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_currencies_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_titles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    type = table.Column<byte>(type: "smallint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_titles", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_titles_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_banners_name",
                table: "banners",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_guilds_id",
                table: "guilds",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_localizations_category_name",
                table: "localizations",
                columns: new[] { "category", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_banners_banner_id",
                table: "user_banners",
                column: "banner_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_banners_user_id_banner_id",
                table: "user_banners",
                columns: new[] { "user_id", "banner_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_currencies_user_id_type",
                table: "user_currencies",
                columns: new[] { "user_id", "type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_titles_user_id_type",
                table: "user_titles",
                columns: new[] { "user_id", "type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_id",
                table: "users",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_world_properties_type",
                table: "world_properties",
                column: "type",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "guilds");

            migrationBuilder.DropTable(
                name: "localizations");

            migrationBuilder.DropTable(
                name: "user_banners");

            migrationBuilder.DropTable(
                name: "user_currencies");

            migrationBuilder.DropTable(
                name: "user_titles");

            migrationBuilder.DropTable(
                name: "world_properties");

            migrationBuilder.DropTable(
                name: "world_states");

            migrationBuilder.DropTable(
                name: "banners");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
