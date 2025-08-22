using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGBApp.Models
{
    public class Fine
    {
        public int Id { get; set; }

        [Required]
        public int LoanId { get; set; }
        public Loan Loan { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }

        public DateTime DateIssued { get; set; } = DateTime.UtcNow;

        [StringLength(250)]
        public string Observations { get; set; }
    }
}
