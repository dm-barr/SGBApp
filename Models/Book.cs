using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGBApp.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Título")]
        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(250, ErrorMessage = "El título no puede exceder 250 caracteres")]
        public string Title { get; set; }

        [Display(Name = "Nombre del autor")]
        [Required(ErrorMessage = "El autor es obligatorio.")]
        public int? AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        public virtual Author Author { get; set; }

        [StringLength(100, ErrorMessage = "El ISBN no puede exceder 100 caracteres")]
        public string ISBN { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Las copias disponibles deben ser un número positivo")]
        [Display(Name = "Copias disponibles")]
        public int CopiesAvailable { get; set; } = 1;

        [Range(1000, 9999, ErrorMessage = "El año debe ser válido")]
        public int? Year { get; set; }

        [StringLength(250, ErrorMessage = "El nombre del editor no puede exceder 250 caracteres")]
        public string Publisher { get; set; }
    }
}