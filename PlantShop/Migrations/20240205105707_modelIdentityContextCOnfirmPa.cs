using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantShop.Migrations
{
    public partial class modelIdentityContextCOnfirmPa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "RememberPassword",
                table: "AspNetUsers",
                newName: "ConfirmPassword");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ConfirmPassword",
                table: "AspNetUsers",
                newName: "RememberPassword");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
