using System.ComponentModel.DataAnnotations;

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
}
