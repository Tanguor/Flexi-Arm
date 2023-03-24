namespace Flexi_Arm.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}

// Ce fichier contient la d�finition de la classe ErrorViewModel utilis�e pour afficher des erreurs dans les vues.
// La classe a une propri�t� RequestId qui peut �tre d�finie pour afficher des informations sur une demande d'erreur sp�cifique,
// et une m�thode ShowRequestId pour d�terminer si une RequestId est d�finie.