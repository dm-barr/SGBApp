using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SGBApp.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using SGBApp.Models;
using System.Linq;
using System;

namespace SGBApp.Pages.Reports
{
    public class OverdueModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public OverdueModel(ApplicationDbContext context) { _context = context; }

        public class OverdueRow
        {
            public string BookTitle { get; set; }
            public string StudentName { get; set; }
            public DateTime DueDate { get; set; }
            public int DaysLate { get; set; }
            public decimal CurrentFine { get; set; }
        }

        public List<OverdueRow> Overdues { get; set; }

        public async Task OnGetAsync()
        {
            var today = DateTime.UtcNow.Date;
            var loans = await _context.Loans.Include(l => l.Book).Include(l => l.Student)
                .Where(l => l.Status == "Prestado" && l.DueDate.Date < today)
                .ToListAsync();

            Overdues = loans.Select(l =>
            {
                int daysLate = (today - l.DueDate.Date).Days;
                decimal finePerDay = 1.27m;
                var fine = finePerDay * Math.Max(0, daysLate);
                return new OverdueRow
                {
                    BookTitle = l.Book.Title,
                    StudentName = l.Student.Name,
                    DueDate = l.DueDate,
                    DaysLate = daysLate,
                    CurrentFine = fine
                };
            }).ToList();
        }
    }
}
