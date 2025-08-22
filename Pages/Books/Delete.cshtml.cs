using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SGBApp.Data;
using SGBApp.Models;
using System.Threading.Tasks;

namespace SGBApp.Pages.Books
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public DeleteModel(ApplicationDbContext context) { _context = context; }

        [BindProperty]
        public Book Book { get; set; }

        public async Task OnGetAsync(int id)
        {
            Book = await _context.Books.FindAsync(id);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var bookInDb = await _context.Books.FindAsync(Book.Id);
            if (bookInDb != null)
            {
                _context.Books.Remove(bookInDb);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("Index");
        }
    }
}
