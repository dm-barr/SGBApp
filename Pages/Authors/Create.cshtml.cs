using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SGBApp.Data;
using SGBApp.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SGBApp.Pages.Authors
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public CreateModel(ApplicationDbContext context) { _context = context; }

        [BindProperty]
        public Author Author { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            _context.Authors.Add(Author);
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}
