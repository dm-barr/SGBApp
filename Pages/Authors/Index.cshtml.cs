using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SGBApp.Data;
using SGBApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGBApp.Pages.Authors
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Author> Authors { get; set; } = new List<Author>();

        public async Task OnGetAsync()
        {
            Authors = await _context.Authors.AsNoTracking().ToListAsync();
        }
    }
}
