using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SGBApp.Data;
using SGBApp.Models;
using System.Threading.Tasks;

namespace SGBApp.Pages.Students
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public DetailsModel(ApplicationDbContext context) { _context = context; }

        public Student Student { get; set; }

        public async Task OnGetAsync(int id)
        {
            Student = await _context.Students.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
