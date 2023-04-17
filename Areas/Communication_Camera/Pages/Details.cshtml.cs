using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Flexi_Arm.Areas.Identity.Data;
using Flexi_Arm.Models;

namespace Flexi_Arm.Areas.Communication_Camera.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly Flexi_Arm.Areas.Identity.Data.ApplicationDbContext _context;

        public DetailsModel(Flexi_Arm.Areas.Identity.Data.ApplicationDbContext context)
        {
            _context = context;
        }

      public Camera Camera { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Camera == null)
            {
                return NotFound();
            }

            var camera = await _context.Camera.FirstOrDefaultAsync(m => m.Id_Camera == id);
            if (camera == null)
            {
                return NotFound();
            }
            else 
            {
                Camera = camera;
            }
            return Page();
        }
    }
}
