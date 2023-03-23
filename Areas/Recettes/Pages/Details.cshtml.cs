using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Flexi_Arm.Areas.Identity.Data;
using Flexi_Arm.Models;

namespace Flexi_Arm.Areas.Recettes.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly Flexi_Arm.Areas.Identity.Data.ApplicationDbContext _context;

        public DetailsModel(Flexi_Arm.Areas.Identity.Data.ApplicationDbContext context)
        {
            _context = context;
        }

      public Recette Recette { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Recette == null)
            {
                return NotFound();
            }

            var recette = await _context.Recette.FirstOrDefaultAsync(m => m.Id == id);
            if (recette == null)
            {
                return NotFound();
            }
            else 
            {
                Recette = recette;
            }
            return Page();
        }
    }
}
