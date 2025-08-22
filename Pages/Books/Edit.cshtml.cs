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
            // Traer el libro de la BD
            Book = await _context.Books.FindAsync(id);

            if (Book == null)
                return NotFound();

            // Cargar lista de autores y mantener selección actual
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
            // Evitar validación de la propiedad virtual Author
            ModelState.Remove("Book.Author");

            // Volver a cargar lista de autores para POST en caso de error
            AuthorsSelect = new SelectList(
                _context.Authors.OrderBy(a => a.Name).ToList(),
                "Id",
                "Name",
                Book.AuthorId
            );

            if (!ModelState.IsValid)
                return Page();

            // Marcar el libro como modificado
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
