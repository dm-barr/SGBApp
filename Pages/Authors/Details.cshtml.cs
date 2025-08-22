using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SGBApp.Data;
using SGBApp.Models;
using System.Threading.Tasks;

namespace SGBApp.Pages.Authors
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public DetailsModel(ApplicationDbContext context) { _context = context; }

        public Author Author { get; set; }

        public async Task OnGetAsync(int id)
        {
            Author = await _context.Authors.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
