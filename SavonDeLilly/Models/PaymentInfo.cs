using System.ComponentModel.DataAnnotations;

namespace SavonDeLilly.Models
{
    public class PaymentInfo
    {
        // Clé primaire
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Address { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{5}(?:-[0-9]{4})?$", ErrorMessage = "Code postal invalide.")]
        public string PostalCode { get; set; }

        [Required]
        [StringLength(50)]
        public string Country { get; set; }

        [Required]
        [CreditCard(ErrorMessage = "Numéro de carte invalide.")]
        public string CardNumber { get; set; }

        [Required]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/\d{2}$", ErrorMessage = "Date d'expiration invalide.")]
        public string ExpiryDate { get; set; }

        [Required]
        [RegularExpression(@"^\d{3}$", ErrorMessage = "CVV invalide.")]
        public string CVV { get; set; }
    }
}
