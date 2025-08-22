using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SGBApp.Data;
using SGBApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGBApp.Pages.Loans
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public IndexModel(ApplicationDbContext context) { _context = context; }

        public IList<Loan> Loans { get; set; }

        public async Task OnGetAsync()
        {
            Loans = await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.Student)
                .ToListAsync();
        }
    }
}
