using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SGBApp.Data;
using SGBApp.Models;
using System.Threading.Tasks;

namespace SGBApp.Pages.Students
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public EditModel(ApplicationDbContext context) { _context = context; }

        [BindProperty]
        public Student Student { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Student = await _context.Students.FindAsync(id);
            if (Student == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            try
            {
                _context.Attach(Student).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Students.AnyAsync(s => s.Id == Student.Id))
                    return NotFound();
                throw;
            }

            return RedirectToPage("Index");
        }
    }
}
