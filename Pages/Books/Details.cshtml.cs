using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SGBApp.Data;
using SGBApp.Models;
using System.Threading.Tasks;

namespace SGBApp.Pages.Books
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public DetailsModel(ApplicationDbContext context) { _context = context; }

        public Book Book { get; set; }

        public async Task OnGetAsync(int id)
        {
            Book = await _context.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}
