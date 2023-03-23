using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualBasic;

namespace Flexi_Arm.Controllers
{
    public class RoleController : Controller
    {
        //permission uniquement au employé
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "RequireMaintenance")]
        public IActionResult Maintenance()
        {
            return View();
        }

        [Authorize(Policy = "RequireAdmin")]
        public IActionResult Admin()
        {
            return View();
        }
    }
}

//POUR AJOUT DE ROLES VOIR https://www.youtube.com/watch?v=E8JaZdtXTBQ&ab_channel=ISeeSharp
