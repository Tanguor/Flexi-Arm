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
    public class DetailsModel : PageModel
    {
        private readonly Flexi_Arm.Areas.Identity.Data.ApplicationDbContext _context;

        public DetailsModel(Flexi_Arm.Areas.Identity.Data.ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
