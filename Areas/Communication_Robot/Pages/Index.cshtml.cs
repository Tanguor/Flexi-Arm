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
    public class IndexModel : PageModel
    {
        private readonly Flexi_Arm.Areas.Identity.Data.ApplicationDbContext _context;

        public IndexModel(Flexi_Arm.Areas.Identity.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Bras_Robot> Bras_Robot { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Bras_Robot != null)
            {
                Bras_Robot = await _context.Bras_Robot.ToListAsync();
            }
        }
    }
}
