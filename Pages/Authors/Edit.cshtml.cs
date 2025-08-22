using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SGBApp.Data;
using SGBApp.Models;
using System.Threading.Tasks;

namespace SGBApp.Pages.Authors
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public EditModel(ApplicationDbContext context) { _context = context; }

        [BindProperty]
        public Author Author { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Author = await _context.Authors.FindAsync(id);
            if (Author == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            try
            {
                _context.Attach(Author).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Authors.AnyAsync(a => a.Id == Author.Id))
                    return NotFound();
                throw;
            }

            return RedirectToPage("Index");
        }
    }
}
