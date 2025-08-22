using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SGBApp.Data;
using SGBApp.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SGBApp.Pages.Students
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public CreateModel(ApplicationDbContext context) { _context = context; }

        [BindProperty]
        public Student Student { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            // Optional: enforce unique DocumentNumber
            var exists = await _context.Students.AnyAsync(s => s.DocumentNumber == Student.DocumentNumber);
            if (exists)
            {
                ModelState.AddModelError(nameof(Student.DocumentNumber), "El número de documento ya existe.");
                return Page();
            }

            _context.Students.Add(Student);
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}
