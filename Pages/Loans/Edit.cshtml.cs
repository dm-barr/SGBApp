using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SGBApp.Data;
using SGBApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SGBApp.Pages.Loans
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Loan Loan { get; set; }

        public SelectList BooksSelect { get; set; }
        public SelectList StudentsSelect { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Loan = await _context.Loans.FindAsync(id);
            if (Loan == null)
                return NotFound();

            BooksSelect = new SelectList(
                _context.Books.OrderBy(b => b.Title).ToList(),
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

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Loan.Book");
            ModelState.Remove("Loan.Student");

            BooksSelect = new SelectList(
                _context.Books.OrderBy(b => b.Title).ToList(),
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

            var existingLoan = await _context.Loans.AsNoTracking().FirstOrDefaultAsync(l => l.Id == Loan.Id);
            if (existingLoan == null)
                return NotFound();

            if (Loan.BookId != existingLoan.BookId)
            {
                var oldBook = await _context.Books.FindAsync(existingLoan.BookId);
                if (oldBook != null) oldBook.CopiesAvailable += 1;

                var newBook = await _context.Books.FindAsync(Loan.BookId);
                if (newBook == null || newBook.CopiesAvailable <= 0)
                {
                    ModelState.AddModelError(string.Empty, "El nuevo libro no tiene copias disponibles.");
                    return Page();
                }
                newBook.CopiesAvailable -= 1;
            }

            _context.Attach(Loan).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Loans.Any(l => l.Id == Loan.Id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToPage("Index");
        }
    }
}
