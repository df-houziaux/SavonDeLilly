namespace SavonDeLilly.Models
{
    public class ResetPassword
    {
        public string Token { get; set; }
        public string Email { get; set; }

        // Propriétés pour le nouveau mot de passe
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
