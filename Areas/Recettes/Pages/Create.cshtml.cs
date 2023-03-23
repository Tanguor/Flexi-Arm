using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Flexi_Arm.Areas.Identity.Data;
using Flexi_Arm.Models;
using Microsoft.AspNetCore.Authorization;

namespace Flexi_Arm.Areas.Recettes.Pages
{
    [Authorize(Policy = "RequireAdmin")]
    public class CreateModel : PageModel
    {

        private readonly Flexi_Arm.Areas.Identity.Data.ApplicationDbContext _context;

        public CreateModel(Flexi_Arm.Areas.Identity.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Recette Recette { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
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
