using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flexi_Arm.Migrations
{
    public partial class SimpleParameterRobotAndFlexi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "cw_angle",
                table: "Flexibowl",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "cww_angle",
                table: "Flexibowl",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "sh_count",
                table: "Flexibowl",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "sh_speed",
                table: "Flexibowl",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "speedapproach",
                table: "Bras_Robot",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "speedfree",
                table: "Bras_Robot",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cw_angle",
                table: "Flexibowl");

            migrationBuilder.DropColumn(
                name: "cww_angle",
                table: "Flexibowl");

            migrationBuilder.DropColumn(
                name: "sh_count",
                table: "Flexibowl");

            migrationBuilder.DropColumn(
                name: "sh_speed",
                table: "Flexibowl");

            migrationBuilder.DropColumn(
                name: "speedapproach",
                table: "Bras_Robot");

            migrationBuilder.DropColumn(
                name: "speedfree",
                table: "Bras_Robot");
        }
    }
}
