using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocConnect.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddAccessCodeToHoSoBacSi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccessCode",
                table: "HoSoBacSi",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql("UPDATE HoSoBacSi SET AccessCode = 'AB' + RIGHT('0000000' + CAST(Id AS VARCHAR), 7) + '1'");

            migrationBuilder.CreateIndex(
                name: "IX_HoSoBacSi_AccessCode",
                table: "HoSoBacSi",
                column: "AccessCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HoSoBacSi_AccessCode",
                table: "HoSoBacSi");

            migrationBuilder.DropColumn(
                name: "AccessCode",
                table: "HoSoBacSi");
        }
    }
}
