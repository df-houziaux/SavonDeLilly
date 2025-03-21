using System.ComponentModel.DataAnnotations;

namespace SavonDeLilly.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom complet est requis.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "L'adresse e-mail est requise.")]
        [EmailAddress(ErrorMessage = "L'adresse e-mail n'est pas valide.")]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; } // 🔒 On stocke un mot de passe haché
    }
}
