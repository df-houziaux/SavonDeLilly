using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SavonDeLilly.Models;

namespace SavonDeLilly.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> // Changement ici
    {
        // Constructeur du DbContext
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet pour les modèles existants
        public DbSet<Product> Products { get; set; }
        public DbSet<Client> Clients { get; set; }

        // DbSet pour le modèle PaymentInfo
        public DbSet<PaymentInfo> PaymentInfos { get; set; } // Ajoutez ceci

        // Configuration de la base de données
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=SavonDeLilly;Trusted_Connection=True;MultipleActiveResultSets=True");
            }
        }
    }
}
