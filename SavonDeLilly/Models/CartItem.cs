using System.ComponentModel.DataAnnotations;

namespace SavonDeLilly.Models
{
    public class CartItem
    {
        public Product Product { get; set; } = new Product();

        [Range(1, int.MaxValue, ErrorMessage = "La quantité doit être au moins 1.")]
        public int Quantity { get; set; }
    }
}
