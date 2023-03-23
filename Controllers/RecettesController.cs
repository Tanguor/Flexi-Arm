using Flexi_Arm.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Flexi_Arm.Controllers
{
    public class RecettesController : Controller
    {
        private readonly Areas.Identity.Data.ApplicationDbContext _context;

    public RecettesController(Areas.Identity.Data.ApplicationDbContext context)
    {
        _context = context;
    }
        public IActionResult Index()
        {

            return View();
        }

        [BindProperty]
        public Recette RecetteChoisie { get; set; } = default!;


        public async Task<IActionResult> RecetteSlct(int? itemid)
        {
            if (itemid == null || _context.Recette == null)
            {
                return NotFound();
            }

            var recette = await _context.Recette.FirstOrDefaultAsync(m => m.Id == itemid);
            if (recette == null)
            {
                return NotFound();
            }
            RecetteChoisie = recette;
            TempData["MessageRecette"] = RecetteChoisie.Name;
            return RedirectToAction("Index");
        }
    }
}

//Ce morceau de code est un contrôleur ASP.NET Core MVC nommé "RecettesController". Il est défini dans le namespace "Flexi_Arm.Controllers".
//Le contrôleur contient deux méthodes d'action publiques nommées "Index" et "RecetteSlct".
//La méthode "Index" retourne simplement une vue, mais ne prend aucun paramètre.
//La méthode "RecetteSlct" prend un paramètre nullable "itemid".

//Si cet élément est null ou si aucune recette n'est trouvée dans la base de données, la méthode retourne un résultat "NotFound".
//Sinon, elle récupère une recette à partir de la base de données en utilisant l'ID passé en paramètre, et stocke cette recette dans une propriété "RecetteChoisie".
//Elle stocke également le nom de la recette dans la propriété "TempData".
//Enfin, elle redirige vers la méthode "Index".

//Le contrôleur a également une propriété publique nommée "NomRecetteCharge", mais elle n'est pas utilisée dans ce code et peut être supprimée.
//Le constructeur prend en paramètre une instance de "ApplicationDbContext" qui est injectée dans le contrôleur.
//Cette instance est stockée dans une variable privée nommée "_context" pour une utilisation ultérieure.
//Enfin, le contrôleur a une propriété publique nommée "RecetteChoisie" qui est décorée avec l'attribut "BindProperty".
//Cela indique que cette propriété peut être liée à des données d'entrée du client.