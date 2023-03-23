using System.ComponentModel.DataAnnotations;

namespace Flexi_Arm.Models;

public class Recette
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Display(Name = "Nom")]
    [StringLength(50)]
    public string Name { get; set; }
    public string Description { get; set; }

}
