using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flexi_Arm.Migrations
{
    public partial class LinkId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id_Camera",
                table: "Recette",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id_Camera",
                table: "Recette");
        }
    }
}
