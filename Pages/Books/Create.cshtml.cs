using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SGBApp.Data;
using SGBApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SGBApp.Pages.Books
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Book Book { get; set; }

        public SelectList AuthorsSelect { get; set; }

        public void OnGet()
        {
            Book = new Book();

            AuthorsSelect = new SelectList(
                _context.Authors.OrderBy(a => a.Name).ToList(),
                "Id",
                "Name"
            );
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
            {
                return Page();
            }

            _context.Books.Add(Book);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
