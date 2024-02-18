using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantShop.Migrations
{
    public partial class fileimage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Icon",
                table: "HeaderInfos",
                newName: "FilePath");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "HeaderInfos",
                newName: "Icon");
        }
    }
}
