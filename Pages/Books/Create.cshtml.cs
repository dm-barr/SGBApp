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
            // Inicializar Book para evitar NullReferenceException
            Book = new Book();

            // Cargar lista de autores
            AuthorsSelect = new SelectList(
                _context.Authors.OrderBy(a => a.Name).ToList(),
                "Id",
                "Name"
            );
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Quitar validación de la propiedad de navegación para evitar errores
            ModelState.Remove("Book.Author");

            // Volver a cargar lista de autores si hay error de validación
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

            // Solo AuthorId se enlaza; EF Core llenará la relación automáticamente
            _context.Books.Add(Book);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
