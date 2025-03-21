using Microsoft.AspNetCore.Identity;

namespace SavonDeLilly.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Vous pouvez ajouter d'autres propriétés si nécessaire
        public string FullName { get; set; }
    }
}
