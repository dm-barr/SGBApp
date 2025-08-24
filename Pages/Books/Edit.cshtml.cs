using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SGBApp.Data;
using SGBApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SGBApp.Pages.Books
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Book Book { get; set; }

        public SelectList AuthorsSelect { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Book = await _context.Books.FindAsync(id);

            if (Book == null)
                return NotFound();

            AuthorsSelect = new SelectList(
                _context.Authors.OrderBy(a => a.Name).ToList(),
                "Id",
                "Name",
                Book.AuthorId
            );

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Book.Author");

            AuthorsSelect = new SelectList(
                _context.Authors.OrderBy(a => a.Name).ToList(),
                "Id",
                "Name",
                Book.AuthorId
            );

            if (!ModelState.IsValid)
                return Page();

            _context.Attach(Book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(Book.Id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToPage("Index");
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
