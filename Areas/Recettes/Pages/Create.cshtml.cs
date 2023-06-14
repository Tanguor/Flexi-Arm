using Flexi_Arm.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Flexi_Arm.Areas.Recettes.Pages
{
    [Authorize(Policy = "RequireAdmin")]
    public class CreateModel : PageModel
    {

        private readonly Identity.Data.ApplicationDbContext _context;

        public CreateModel(Identity.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Recette Recette { get; set; } = default!;
        

        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Recette == null || Recette == null)
            {
                return Page();
            }

            _context.Recette.Add(Recette);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
