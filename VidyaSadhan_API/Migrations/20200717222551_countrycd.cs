using Microsoft.EntityFrameworkCore.Migrations;

namespace VidyaSadhan_API.Migrations
{
    public partial class countrycd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhoneCode",
                table: "Country",
                maxLength: 10,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneCode",
                table: "Country");
        }
    }
}
