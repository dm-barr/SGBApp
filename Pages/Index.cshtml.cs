using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SGBApp.Data;
using SGBApp.Models;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public Dashboard Dashboard { get; set; } = new Dashboard();

    public async Task OnGetAsync()
    {
        Dashboard.TotalBooks = await _context.Books.CountAsync();
        Dashboard.TotalStudents = await _context.Students.CountAsync();
        Dashboard.TotalLoans = await _context.Loans.CountAsync();
        Dashboard.ActiveLoans = await _context.Loans.CountAsync(l => l.Status == "Prestado");
        Dashboard.TotalFines = await _context.Fines.SumAsync(f => f.Amount);

        Dashboard.LoansByStatus = await _context.Loans
            .GroupBy(l => l.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Status, x => x.Count);

        Dashboard.TopBooks = (await _context.Loans
            .GroupBy(l => l.Book.Title)
            .Select(g => new { BookTitle = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(5)
            .ToListAsync())
            .Select(x => (x.BookTitle, x.Count))
            .ToList();

        Dashboard.TopStudents = (await _context.Loans
            .GroupBy(l => l.Student.Name)
            .Select(g => new { StudentName = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(5)
            .ToListAsync())
            .Select(x => (x.StudentName, x.Count))
            .ToList();

        int totalAvailable = await _context.Books.SumAsync(b => b.CopiesAvailable);
        int totalBorrowed = Dashboard.TotalLoans - Dashboard.ActiveLoans;
        Dashboard.BooksDistribution = (totalAvailable, totalBorrowed);
    }
}
