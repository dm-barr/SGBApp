using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SGBApp.Data;
using SGBApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGBApp.Pages.Books
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Book> Books { get; set; } = new List<Book>();

        // Propiedad para capturar el texto del buscador desde la URL
        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        public async Task OnGetAsync()
        {
            var query = _context.Books
                                .Include(b => b.Author)
                                .AsNoTracking()
                                .AsQueryable();

            if (!string.IsNullOrEmpty(SearchString))
            {
                query = query.Where(b =>
                    b.Title.Contains(SearchString) ||
                    b.ISBN.Contains(SearchString) ||
                    (b.Author != null && b.Author.Name.Contains(SearchString)));
            }

            Books = await query.ToListAsync();
        }
    }
}
