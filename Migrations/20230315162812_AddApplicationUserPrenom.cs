using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flexi_Arm.Migrations
{
    public partial class AddApplicationUserPrenom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Prenom",
                table: "AspNetUsers",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true, //champ changé manuellement pour éviter les erreurs si l'on veut changer le élément du registering.
                                //mettre false si on ne veut utiliser le champ. Il est aussi possible de delete ce champs de la data base voir : https://learn.microsoft.com/fr-fr/ef/core/managing-schemas/migrations/?tabs=vs
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Prenom",
                table: "AspNetUsers");
        }
    }
}
