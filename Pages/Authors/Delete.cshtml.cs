using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SGBApp.Data;
using SGBApp.Models;
using System.Threading.Tasks;

namespace SGBApp.Pages.Authors
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public DeleteModel(ApplicationDbContext context) { _context = context; }

        [BindProperty]
        public Author Author { get; set; }

        public async Task OnGetAsync(int id)
        {
            Author = await _context.Authors.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var authorInDb = await _context.Authors.FindAsync(Author.Id);
            if (authorInDb != null)
            {
                // If there are related books, deletion will throw exception due to FK constraints.
                // Could handle explicitly by checking for related books and showing message.
                var hasBooks = await _context.Books.AnyAsync(b => b.AuthorId == authorInDb.Id);
                if (hasBooks)
                {
                    ModelState.AddModelError(string.Empty, "No se puede eliminar: existen libros asociados al autor.");
                    // reload the author to display message
                    Author = authorInDb;
                    return Page();
                }

                _context.Authors.Remove(authorInDb);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("Index");
        }
    }
}
