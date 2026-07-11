using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocConnect.Web.Migrations
{
    public partial class AddPhienTuVanIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BacSiId",
                table: "PhienTuVan",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BenhNhanId",
                table: "PhienTuVan",
                type: "nvarchar(450)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BacSiId",
                table: "PhienTuVan");

            migrationBuilder.DropColumn(
                name: "BenhNhanId",
                table: "PhienTuVan");
        }
    }
}