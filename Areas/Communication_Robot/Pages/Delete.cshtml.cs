using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Flexi_Arm.Areas.Identity.Data;
using Flexi_Arm.Models;

namespace Flexi_Arm.Areas.Communication_Robot.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly Flexi_Arm.Areas.Identity.Data.ApplicationDbContext _context;

        public DeleteModel(Flexi_Arm.Areas.Identity.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Bras_Robot Bras_Robot { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Bras_Robot == null)
            {
                return NotFound();
            }

            var bras_robot = await _context.Bras_Robot.FirstOrDefaultAsync(m => m.Id_Robot == id);

            if (bras_robot == null)
            {
                return NotFound();
            }
            else 
            {
                Bras_Robot = bras_robot;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Bras_Robot == null)
            {
                return NotFound();
            }
            var bras_robot = await _context.Bras_Robot.FindAsync(id);

            if (bras_robot != null)
            {
                Bras_Robot = bras_robot;
                _context.Bras_Robot.Remove(Bras_Robot);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
