using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flexi_Arm.Models;
using Microsoft.AspNetCore.Identity;

namespace Flexi_Arm.Areas.Communication.Data;

// Add profile data for application users by adding properties to the ApplicationRecette class
public class ApplicationRecette : Flexibowl
{
    //ajoute un champ nom pour la connexion
    public string CommunicationModel { get; set; }

}
