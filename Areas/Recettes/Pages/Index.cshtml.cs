using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Flexi_Arm.Areas.Identity.Data;
using Flexi_Arm.Models;
using Microsoft.AspNetCore.Authorization;

namespace Flexi_Arm.Areas.Recettes.Pages
{
    [Authorize(Policy = "RequireMaintenance")]
    public class IndexModel : PageModel
    {
        private readonly Flexi_Arm.Areas.Identity.Data.ApplicationDbContext _context;

        public IndexModel(Flexi_Arm.Areas.Identity.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Recette> Recette { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Recette != null)
            {
                Recette = await _context.Recette.ToListAsync();
            }
        }
    }
}

