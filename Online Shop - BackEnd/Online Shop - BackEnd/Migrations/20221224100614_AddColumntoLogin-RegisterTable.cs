using Microsoft.EntityFrameworkCore.Migrations;

namespace Online_Shop___BackEnd.Migrations
{
    public partial class AddColumntoLoginRegisterTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Login_Registers");

            migrationBuilder.AddColumn<string>(
                name: "LoginImg",
                table: "Login_Registers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MainImg",
                table: "Login_Registers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegisterImg",
                table: "Login_Registers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoginImg",
                table: "Login_Registers");

            migrationBuilder.DropColumn(
                name: "MainImg",
                table: "Login_Registers");

            migrationBuilder.DropColumn(
                name: "RegisterImg",
                table: "Login_Registers");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Login_Registers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
