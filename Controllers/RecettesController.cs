using Flexi_Arm.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Flexi_Arm.Controllers
{
    public class RecettesController : Controller
    {
        private readonly Areas.Identity.Data.ApplicationDbContext _context;

        // Modifiez le constructeur pour inclure IRecetteService
        public RecettesController(Areas.Identity.Data.ApplicationDbContext context )
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Propriété pour stocker la recette sélectionnée
        [BindProperty]
        public Recette RecetteChoisie { get; set; } = default!;

        // Action pour récupérer la recette sélectionnée par son id et la stocker dans la propriété RecetteChoisie
        public async Task<IActionResult> RecetteSlct(int? itemid)
        {
            // Vérifie si l'id est null ou si le contexte est null
            if (itemid == null || _context.Recette == null)
            {
                return NotFound();
            }

            // Recherche la recette correspondante à l'id
            var recette = await _context.Recette.FirstOrDefaultAsync(m => m.Id == itemid);
            // Vérifie si la recette n'est pas null
            if (recette == null)
            {
                return NotFound();
            }
            var json = System.IO.File.ReadAllText("appsettings.json");
            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

            // Mettre à jour la valeur
            jsonObj["DefaultRecetteId"] = recette.Id;

            // Écrire le fichier de configuration
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            System.IO.File.WriteAllText("appsettings.json", output);
            // Définit cette recette comme étant la recette actuellement chargée
            TempData["MessageRecette"] = recette.Name;
            TempData["IdRecette"] = recette.Id;

            // Redirige vers l'Index
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