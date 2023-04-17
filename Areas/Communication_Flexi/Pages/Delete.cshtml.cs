using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Flexi_Arm.Areas.Identity.Data;
using Flexi_Arm.Models;

namespace Flexi_Arm.Areas.Communication.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly Flexi_Arm.Areas.Identity.Data.ApplicationDbContext _context;

        public DeleteModel(Flexi_Arm.Areas.Identity.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Flexibowl Flexibowl { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Flexibowl == null)
            {
                return NotFound();
            }

            var communicationmodel = await _context.Flexibowl.FirstOrDefaultAsync(m => m.Id_flexi == id);

            if (communicationmodel == null)
            {
                return NotFound();
            }
            else 
            {
                Flexibowl = communicationmodel;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Flexibowl == null)
            {
                return NotFound();
            }
            var communicationmodel = await _context.Flexibowl.FindAsync(id);

            if (communicationmodel != null)
            {
                Flexibowl = communicationmodel;
                _context.Flexibowl.Remove(Flexibowl);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
