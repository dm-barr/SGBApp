using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SGBApp.Data;
using SGBApp.Models;
using System.Threading.Tasks;
using System.Linq;

namespace SGBApp.Pages.Loans
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Loan Loan { get; set; }

        public SelectList BooksSelect { get; set; }
        public SelectList StudentsSelect { get; set; }

        public void OnGet()
        {
            // Inicializar Loan para evitar NullReferenceException
            Loan = new Loan();

            // Cargar libros disponibles y estudiantes
            BooksSelect = new SelectList(
                _context.Books.Where(b => b.CopiesAvailable > 0).OrderBy(b => b.Title).ToList(),
                "Id",
                "Title"
            );

            StudentsSelect = new SelectList(
                _context.Students.OrderBy(s => s.Name).ToList(),
                "Id",
                "Name"
            );
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Evitar validación de propiedades virtuales (Book y Student)
            ModelState.Remove("Loan.Book");
            ModelState.Remove("Loan.Student");

            // Recargar SelectLists para mantener selección en caso de error
            BooksSelect = new SelectList(
                _context.Books.Where(b => b.CopiesAvailable > 0).OrderBy(b => b.Title).ToList(),
                "Id",
                "Title",
                Loan.BookId
            );

            StudentsSelect = new SelectList(
                _context.Students.OrderBy(s => s.Name).ToList(),
                "Id",
                "Name",
                Loan.StudentId
            );

            if (!ModelState.IsValid)
                return Page();

            // Verificar disponibilidad del libro
            var book = await _context.Books.FindAsync(Loan.BookId);
            if (book == null || book.CopiesAvailable <= 0)
            {
                ModelState.AddModelError(string.Empty, "Libro no disponible.");
                return Page();
            }

            // Decrementar copias disponibles y marcar estado prestado
            book.CopiesAvailable -= 1;
            Loan.Status = "Prestado";

            _context.Loans.Add(Loan);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
