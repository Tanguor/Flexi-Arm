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
      public CommunicationModel CommunicationModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.CommunicationModel == null)
            {
                return NotFound();
            }

            var communicationmodel = await _context.CommunicationModel.FirstOrDefaultAsync(m => m.Id == id);

            if (communicationmodel == null)
            {
                return NotFound();
            }
            else 
            {
                CommunicationModel = communicationmodel;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.CommunicationModel == null)
            {
                return NotFound();
            }
            var communicationmodel = await _context.CommunicationModel.FindAsync(id);

            if (communicationmodel != null)
            {
                CommunicationModel = communicationmodel;
                _context.CommunicationModel.Remove(CommunicationModel);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
