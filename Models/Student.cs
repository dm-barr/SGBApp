using System.ComponentModel.DataAnnotations;

namespace SGBApp.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required, StringLength(150)]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required, StringLength(20)]
        [Display(Name = "Documento")]
        public string DocumentNumber { get; set; }

        [StringLength(100)]
        [Display(Name = "Correo")]
        public string Email { get; set; }

        [StringLength(20)]
        [Display(Name = "Teléfono")]
        public string Phone { get; set; }
    }
}
