using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SavonDeLilly.Models;

namespace ProjetValilou.Controllers
{
    public class PaymentController : Controller
    {
        // Afficher la page de paiement
        public IActionResult Payment()
        {
            // Récupérer les détails du panier depuis la session
            var cartDetailsJson = HttpContext.Session.GetString("CartDetails");
            var cartItems = string.IsNullOrEmpty(cartDetailsJson)
                ? new List<CartItem>()
                : JsonConvert.DeserializeObject<List<CartItem>>(cartDetailsJson);

            // Si le panier est vide, rediriger vers la page des produits
            if (cartItems == null || cartItems.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            // Calculer le total du panier de manière sécurisée
            ViewBag.CartTotal = cartItems
                .Where(item => item != null && item.Product != null)
                .Sum(item => item.Quantity * item.Product.Price);

            return View("Payment", cartItems);  // Assurez-vous que le nom de la vue est "Payment"
        }

        // Traiter le paiement
        [HttpPost]
        public IActionResult ProcessPayment(PaymentInfo paymentInfo)
        {
            if (ModelState.IsValid)
            {
                // Logique pour traiter le paiement (intégration d'une API de paiement)
                // Envoi à un service de paiement (comme Stripe ou PayPal)

                // Suppression des informations du panier après le paiement
                HttpContext.Session.Remove("CartDetails");

                // Redirection vers la page de confirmation
                return RedirectToAction("Confirmation");
            }

            // Si le modèle n'est pas valide, retourner la vue "Payment" avec les erreurs
            return View("Payement");  // Retourner explicitement la vue "Payment"
        }

        // Page de confirmation
        public IActionResult Confirmation()
        {
            ViewBag.Message = "Votre paiement a été traité avec succès et votre commande est en cours de traitement.";
            return View();  // Retourne la vue Confirmation.cshtml
        }
    }
}