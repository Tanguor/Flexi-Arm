using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flexi_Arm.Migrations
{
    public partial class ChangeIdName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommunicationModel");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Camera",
                newName: "Id_Camera");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Bras_Robot",
                newName: "Id_Robot");

            migrationBuilder.AddColumn<int>(
                name: "id_Flexi",
                table: "Recette",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "id_Robot",
                table: "Recette",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Flexibowl",
                columns: table => new
                {
                    Id_flexi = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Ip = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Port = table.Column<int>(type: "int", nullable: false),
                    Modele_flexibowl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flexibowl", x => x.Id_flexi);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flexibowl");

            migrationBuilder.DropColumn(
                name: "id_Flexi",
                table: "Recette");

            migrationBuilder.DropColumn(
                name: "id_Robot",
                table: "Recette");

            migrationBuilder.RenameColumn(
                name: "Id_Camera",
                table: "Camera",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Id_Robot",
                table: "Bras_Robot",
                newName: "Id");

            migrationBuilder.CreateTable(
                name: "CommunicationModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ip = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Modele_flexibowl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Port = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunicationModel", x => x.Id);
                });
        }
    }
}
