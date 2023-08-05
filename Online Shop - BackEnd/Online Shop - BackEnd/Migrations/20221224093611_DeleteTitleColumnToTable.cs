using Microsoft.EntityFrameworkCore.Migrations;

namespace Online_Shop___BackEnd.Migrations
{
    public partial class DeleteTitleColumnToTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Brands");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Brands",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
