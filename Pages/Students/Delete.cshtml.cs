using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SGBApp.Data;
using SGBApp.Models;
using System.Threading.Tasks;

namespace SGBApp.Pages.Students
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public DeleteModel(ApplicationDbContext context) { _context = context; }

        [BindProperty]
        public Student Student { get; set; }

        public async Task OnGetAsync(int id)
        {
            Student = await _context.Students.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var studentInDb = await _context.Students.FindAsync(Student.Id);
            if (studentInDb != null)
            {
                var hasActiveLoans = await _context.Loans.AnyAsync(l => l.StudentId == studentInDb.Id && l.Status == "Activo");
                if (hasActiveLoans)
                {
                    ModelState.AddModelError(string.Empty, "No se puede eliminar: el estudiante tiene préstamos activos.");
                    Student = studentInDb;
                    return Page();
                }

                _context.Students.Remove(studentInDb);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("Index");
        }
    }
}
