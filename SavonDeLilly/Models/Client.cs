using System.ComponentModel.DataAnnotations;

namespace SavonDeLilly.Models
{
    public class Client

    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom complet est requis.")]
        [RegularExpression(@"^[A-Z][a-z]+ [A-Z][a-z]+$", ErrorMessage = "Le prénom et le nom doivent commencer par une majuscule, être séparés par un espace, et ne contenir que des lettres.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "L'adresse e-mail est requise.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "L'adresse e-mail n'est pas valide.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Le mot de passe est requis.")]
        [StringLength(100, ErrorMessage = "Le mot de passe doit avoir au moins {2} caractères.", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Le mot de passe doit contenir au moins 8 caractères, une majuscule, une minuscule, un chiffre et un caractère spécial.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Veuillez confirmer votre mot de passe.")]
        [Compare("Password", ErrorMessage = "Les mots de passe ne correspondent pas.")]
        public string ConfirmPassword { get; set; }
    }
}
