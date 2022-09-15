using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Disfarm.Data.Migrations
{
    public partial class UpdateImageEntityAddKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_images",
                table: "images");

            migrationBuilder.AddColumn<Guid>(
                name: "id",
                table: "images",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "pk_images",
                table: "images",
                column: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_images",
                table: "images");

            migrationBuilder.DropColumn(
                name: "id",
                table: "images");

            migrationBuilder.AddPrimaryKey(
                name: "pk_images",
                table: "images",
                column: "type");
        }
    }
}
