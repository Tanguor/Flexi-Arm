using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Flexi_Arm.Areas.Recettes.Data;

// Add profile data for application users by adding properties to the ApplicationRecette class
public class ApplicationRecette : IdentityUser
{
    //ajoute un champ nom pour la connexion
    public string Recette { get; set; }

}

