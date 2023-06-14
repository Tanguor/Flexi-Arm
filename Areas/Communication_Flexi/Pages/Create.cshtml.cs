using Flexi_Arm.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Flexi_Arm.Areas.Communication.Pages
{
    public class CreateModel : PageModel
    {
        private readonly Identity.Data.ApplicationDbContext _context;

        public CreateModel(Identity.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Flexibowl Flexibowl { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Flexibowl == null || Flexibowl == null)
            {
                return Page();
            }

            _context.Flexibowl.Add(Flexibowl);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
