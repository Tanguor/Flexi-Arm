using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Flexi_Arm.Areas.Identity.Data;
using Flexi_Arm.Models;

namespace Flexi_Arm.Areas.Recettes.Pages
{
    public class EditModel : PageModel
    {
        private readonly Flexi_Arm.Areas.Identity.Data.ApplicationDbContext _context;

        public EditModel(Flexi_Arm.Areas.Identity.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Recette Recette { get; set; } = default!;


        //Récupère depuis la Database les informaton de notre Recette 
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Recette == null)
            {
                return NotFound();
            }

            var recette =  await _context.Recette.FirstOrDefaultAsync(m => m.Id == id);
            if (recette == null)
            {
                return NotFound();
            }
            Recette = recette;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Recette).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecetteExists(Recette.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool RecetteExists(int id)
        {
          return (_context.Recette?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
