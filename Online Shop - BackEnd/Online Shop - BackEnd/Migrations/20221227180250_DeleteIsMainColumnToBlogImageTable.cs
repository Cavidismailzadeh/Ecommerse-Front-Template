using Microsoft.EntityFrameworkCore.Migrations;

namespace Online_Shop___BackEnd.Migrations
{
    public partial class DeleteIsMainColumnToBlogImageTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMain",
                table: "BlogImages");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMain",
                table: "BlogImages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
