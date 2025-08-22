using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SGBApp.Models;

namespace SGBApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Fine> Fines { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Author>().HasIndex(a => a.Name);
            builder.Entity<Book>()
                .HasOne(b => b.Author) // Relación muchos-a-uno
                .WithMany(a => a.Books) // Relación uno-a-muchos
                .HasForeignKey(b => b.AuthorId) // Clave foránea
                .OnDelete(DeleteBehavior.Cascade); // Eliminar en cascada (opcional)
        }
    }
}
