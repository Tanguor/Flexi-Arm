namespace Flexi_Arm.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}

// Ce fichier contient la définition de la classe ErrorViewModel utilisée pour afficher des erreurs dans les vues.
// La classe a une propriété RequestId qui peut être définie pour afficher des informations sur une demande d'erreur spécifique,
// et une méthode ShowRequestId pour déterminer si une RequestId est définie.