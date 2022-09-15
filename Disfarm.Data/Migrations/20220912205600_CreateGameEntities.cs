using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Disfarm.Data.Migrations
{
    public partial class CreateGameEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "amount",
                table: "user_currencies",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)");

            migrationBuilder.CreateTable(
                name: "fishes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    rarity = table.Column<byte>(type: "smallint", nullable: false),
                    catch_weather = table.Column<byte>(type: "smallint", nullable: false),
                    catch_times_day = table.Column<byte>(type: "smallint", nullable: false),
                    catch_seasons = table.Column<string>(type: "text", nullable: false),
                    price = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_fishes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "seeds",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    season = table.Column<byte>(type: "smallint", nullable: false),
                    growth_days = table.Column<long>(type: "bigint", nullable: false),
                    re_growth_days = table.Column<long>(type: "bigint", nullable: false),
                    is_multiply = table.Column<bool>(type: "boolean", nullable: false),
                    price = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_seeds", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_collections",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    category = table.Column<byte>(type: "smallint", nullable: false),
                    item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_collections", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_collections_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_cooldowns",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    type = table.Column<byte>(type: "smallint", nullable: false),
                    expiration = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_cooldowns", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_cooldowns_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_daily_rewards",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    day_of_week = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_daily_rewards", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_daily_rewards_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_fishes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    fish_id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_fishes", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_fishes_fishes_fish_id",
                        column: x => x.fish_id,
                        principalTable: "fishes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_fishes_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "crops",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    price = table.Column<long>(type: "bigint", nullable: false),
                    seed_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_crops", x => x.id);
                    table.ForeignKey(
                        name: "fk_crops_seeds_seed_id",
                        column: x => x.seed_id,
                        principalTable: "seeds",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_seeds",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    seed_id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_seeds", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_seeds_seeds_seed_id",
                        column: x => x.seed_id,
                        principalTable: "seeds",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_seeds_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_crops",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    crop_id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_crops", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_crops_crops_crop_id",
                        column: x => x.crop_id,
                        principalTable: "crops",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_crops_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_crops_name",
                table: "crops",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_crops_seed_id",
                table: "crops",
                column: "seed_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_fishes_name",
                table: "fishes",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_seeds_name",
                table: "seeds",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_collections_user_id_category_item_id",
                table: "user_collections",
                columns: new[] { "user_id", "category", "item_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_cooldowns_user_id_type",
                table: "user_cooldowns",
                columns: new[] { "user_id", "type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_crops_crop_id",
                table: "user_crops",
                column: "crop_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_crops_user_id_crop_id",
                table: "user_crops",
                columns: new[] { "user_id", "crop_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_daily_rewards_user_id_day_of_week",
                table: "user_daily_rewards",
                columns: new[] { "user_id", "day_of_week" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_fishes_fish_id",
                table: "user_fishes",
                column: "fish_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_fishes_user_id_fish_id",
                table: "user_fishes",
                columns: new[] { "user_id", "fish_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_seeds_seed_id",
                table: "user_seeds",
                column: "seed_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_seeds_user_id_seed_id",
                table: "user_seeds",
                columns: new[] { "user_id", "seed_id" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_collections");

            migrationBuilder.DropTable(
                name: "user_cooldowns");

            migrationBuilder.DropTable(
                name: "user_crops");

            migrationBuilder.DropTable(
                name: "user_daily_rewards");

            migrationBuilder.DropTable(
                name: "user_fishes");

            migrationBuilder.DropTable(
                name: "user_seeds");

            migrationBuilder.DropTable(
                name: "crops");

            migrationBuilder.DropTable(
                name: "fishes");

            migrationBuilder.DropTable(
                name: "seeds");

            migrationBuilder.AlterColumn<decimal>(
                name: "amount",
                table: "user_currencies",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
