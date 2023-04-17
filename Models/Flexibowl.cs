using System.ComponentModel.DataAnnotations;

namespace Flexi_Arm.Models;

public class Flexibowl //modèle du flexi, le mieux serai de le renommer.
{
    [Key] // Définit la clé primaire de la table
    public int Id_flexi { get; set; }

    [Required] // Indique que la propriété est obligatoire
    [Display(Name = "Nom Flexibowl")] // Définit le nom à afficher dans l'interface utilisateur
    [StringLength(50)] // Définit la longueur maximale de la chaîne
    public string? Name { get; set; }

    [Required] // Indique que la propriété est obligatoire
    [Display(Name = "Ip du Flexibowl")] // Définit le nom à afficher dans l'interface utilisateur
    public string? Ip { get; set; } // Description de la recette

    [Required] // Indique que la propriété est obligatoire
    [Display(Name = "Port du Flexibowl")] // Définit le nom à afficher dans l'interface utilisateur
    public int Port { get; set; } // Description de la recette


    [Required] // Indique que la propriété est obligatoire
    [Display(Name = "Numéro du modèle")] // Définit le nom à afficher dans l'interface utilisateur
    public string? Modele_flexibowl { get; set; } // Description de la recette

    //Partie Shake

    [Range(1, 130, ErrorMessage = "La vitesse doit être comprise entre 0 et 130.")]
    public int sh_speed { get; set; } // Description de la recette

    [Range(0, 180, ErrorMessage = "L'angle horraire doit être comprise entre 0 et 180.")]
    public int cw_angle { get; set; } // Description de la recette

    [Range(0, 180, ErrorMessage = "L'angle antihoraire doit être comprise entre 0 et 180.")]
    public int cww_angle { get; set; } // Description de la recette

    [Range(0, 12, ErrorMessage = "pas besoin d'autant ... entre 0 et 12 devrai suffire.")]
    public int sh_count { get; set; } // Description de la recette


}

public class Bras_Robot
{
    [Key] // Définit la clé primaire de la table
    public int Id_Robot { get; set; }

    [Required] // Indique que la propriété est obligatoire
    [Display(Name = "Nom du bras robot")] // Définit le nom à afficher dans l'interface utilisateur
    [StringLength(50)] // Définit la longueur maximale de la chaîne
    public string? Name { get; set; }

    [Required] // Indique que la propriété est obligatoire
    [Display(Name = "Ip du bras robot")] // Définit le nom à afficher dans l'interface utilisateur
    public string? Ip { get; set; } // Description de la recette

    [Required] // Indique que la propriété est obligatoire
    [Display(Name = "Port du bras robot")] // Définit le nom à afficher dans l'interface utilisateur
    public int Port { get; set; } // Description de la recette

    [Required] // Indique que la propriété est obligatoire
    [Display(Name = "Numéro du modèle")] // Définit le nom à afficher dans l'interface utilisateur
    public string? Modele_Robot { get; set; } // Description de la recette


    [Range(1, 10, ErrorMessage = "La vitesse doit être comprise entre 0 et 10.")]
    public int speedfree { get; set; } // Description de la recette

    [Range(0, 5, ErrorMessage = "L'angle horraire doit être comprise entre 0 et 5.")]
    public int speedapproach { get; set; } // Description de la recette

}

public class Camera
{
    [Key] // Définit la clé primaire de la table
    public int Id_Camera { get; set; }

    [Required] // Indique que la propriété est obligatoire
    [Display(Name = "Nom de la camera")] // Définit le nom à afficher dans l'interface utilisateur
    [StringLength(50)] // Définit la longueur maximale de la chaîne
    public string? Name { get; set; }

    [Required] // Indique que la propriété est obligatoire
    [Display(Name = "Ip de la camera")] // Définit le nom à afficher dans l'interface utilisateur
    public string? Ip { get; set; } // Description de la recette

    [Required] // Indique que la propriété est obligatoire
    [Display(Name = "Portde la camera")] // Définit le nom à afficher dans l'interface utilisateur
    public int Port { get; set; } // Description de la recette

    [Required] // Indique que la propriété est obligatoire
    [Display(Name = "Numéro de modèle de la camera")] // Définit le nom à afficher dans l'interface utilisateur
    public string? Modele_Camera { get; set; } // Description de la recette

}