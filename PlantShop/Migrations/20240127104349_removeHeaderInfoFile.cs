using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantShop.Migrations
{
    public partial class removeHeaderInfoFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "HeaderInfos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "HeaderInfos",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
