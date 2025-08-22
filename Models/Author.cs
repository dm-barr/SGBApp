using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SGBApp.Models
{
    public class Author
    {
        public int Id { get; set; }

        [Required, StringLength(150)]
        [Display(Name = "Nombre del autor")]
        public string Name { get; set; }

        [StringLength(500)]
        [Display(Name = "Biografía")]
        public string Biography { get; set; }

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
