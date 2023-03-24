using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualBasic;

namespace Flexi_Arm.Controllers
{
    public class RoleController : Controller
    {
        //permission uniquement au employé
        // Définit une action qui retourne une vue
        public IActionResult Index()
        {
            return View();
        }

        // Définit une action accessible uniquement aux utilisateurs ayant l'autorisation "RequireMaintenance"
        [Authorize(Policy = "RequireMaintenance")]
        public IActionResult Maintenance()
        {
            return View();
        }

        // Définit une action accessible uniquement aux utilisateurs ayant l'autorisation "RequireAdmin"
        [Authorize(Policy = "RequireAdmin")]
        [Authorize(Policy = "RequireAdmin")]
        public IActionResult Admin()
        {
            return View();
        }
    }
}

//POUR AJOUT DE ROLES VOIR https://www.youtube.com/watch?v=E8JaZdtXTBQ&ab_channel=ISeeSharp
