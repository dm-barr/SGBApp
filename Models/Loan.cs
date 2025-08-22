using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGBApp.Models
{
    public class Loan
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Libro")]
        public int BookId { get; set; }
        public Book Book { get; set; }

        [Required]
        [Display(Name = "Estudiante")]
        public int StudentId { get; set; }
        public Student Student { get; set; }

        [Required]
        [Display(Name = "Fecha de préstamo")]
        public DateTime LoanDate { get; set; } = DateTime.UtcNow;

        [Required]
        [Display(Name = "Fecha de devolución esperada")]
        public DateTime DueDate { get; set; }

        [Display(Name = "Fecha de devolución real")]
        public DateTime? ReturnDate { get; set; }

        [Display(Name = "Multa calculada")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal FineAmount { get; set; } = 0m;

        // Estado: Activo, Devuelto
        [Required, StringLength(20)]
        public string Status { get; set; } = "Activo";
    }
}
