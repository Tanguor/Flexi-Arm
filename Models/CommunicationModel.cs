using System.ComponentModel.DataAnnotations;

namespace Flexi_Arm.Models;

public class CommunicationModel
{
    [Key] // Définit la clé primaire de la table
    public int Id { get; set; }

    [Required] // Indique que la propriété est obligatoire
    [Display(Name = "Nom Flexibowl")] // Définit le nom à afficher dans l'interface utilisateur
    [StringLength(50)] // Définit la longueur maximale de la chaîne
    public string Name { get; set; }

    [Required] // Indique que la propriété est obligatoire
    [Display(Name = "Ip du Flexibowl")] // Définit le nom à afficher dans l'interface utilisateur
    public string Ip { get; set; } // Description de la recette

    [Required] // Indique que la propriété est obligatoire
    [Display(Name = "Port du Flexibowl")] // Définit le nom à afficher dans l'interface utilisateur
    public int Port { get; set; } // Description de la recette

    [Required] // Indique que la propriété est obligatoire
    [Display(Name = "Numéro du modèle")] // Définit le nom à afficher dans l'interface utilisateur
    public string Modele_flexibowl { get; set; } // Description de la recette

}