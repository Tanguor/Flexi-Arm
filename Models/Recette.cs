using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flexi_Arm.Models;

public class Recette
{
    [Key] // Définit la clé primaire de la table
    public int Id { get; set; }

    [Required] // Indique que la propriété est obligatoire
    [Display(Name = "Nom")] // Définit le nom à afficher dans l'interface utilisateur
    [StringLength(50)] // Définit la longueur maximale de la chaîne
    public string Name { get; set; }

    public string Description { get; set; } // Description de la recette

    [ForeignKey("Id_Robot")] // clé étrangère pour lier à la table Bras_Robot
    public int id_Robot { get; set; }

    [ForeignKey("Id_flexi")] // clé étrangère pour lier à la table Flexibowl
    public int id_Flexi { get; set; }

    [ForeignKey("Id_Camera")] // clé étrangère pour lier à la table Flexibowl
    public int Id_Camera { get; set; }
}
