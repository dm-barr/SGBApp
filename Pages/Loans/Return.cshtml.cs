using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SGBApp.Data;
using SGBApp.Models;
using System.Threading.Tasks;
using System;

namespace SGBApp.Pages.Loans
{
    public class ReturnModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public ReturnModel(ApplicationDbContext context) { _context = context; }

        [BindProperty]
        public Loan Loan { get; set; }

        [BindProperty]
        public DateTime ReturnDate { get; set; }

        public string TodayString => DateTime.UtcNow.Date.ToString("yyyy-MM-dd");

        public async Task OnGetAsync(int id)
        {
            Loan = await _context.Loans.Include(l => l.Book).Include(l => l.Student).FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var loanInDb = await _context.Loans.Include(l => l.Book).FirstOrDefaultAsync(l => l.Id == Loan.Id);
            if (loanInDb == null) return NotFound();

            // Set return date and calculate fine
            loanInDb.ReturnDate = ReturnDate.ToUniversalTime();
            loanInDb.Status = "Devuelto";

            // increase copies
            var book = loanInDb.Book;
            book.CopiesAvailable += 1;

            // Calculate days late
            var due = loanInDb.DueDate.Date;
            var ret = loanInDb.ReturnDate.Value.Date;
            int daysLate = (ret - due).Days;
            if (daysLate > 0)
            {
                // Policy: 1.00 currency unit per day (ajustable)
                decimal finePerDay = 1.27m;
                loanInDb.FineAmount = finePerDay * daysLate;

                // Store a Fine record (optional)
                var fine = new Fine
                {
                    LoanId = loanInDb.Id,
                    Amount = loanInDb.FineAmount,
                    DateIssued = DateTime.UtcNow,
                    Observations = $"Retraso de {daysLate} días."
                };
                _context.Fines.Add(fine);
            }
            else
            {
                loanInDb.FineAmount = 0m;
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}
