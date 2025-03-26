using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SavonDeLilly.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProjetValilou.Controllers
{
    public class PaymentController : Controller
    {
        public IActionResult Payment()
        {
            var cartDetailsJson = HttpContext.Session.GetString("CartDetails");
            var cartItems = string.IsNullOrEmpty(cartDetailsJson)
                ? new List<CartItem>()
                : JsonConvert.DeserializeObject<List<CartItem>>(cartDetailsJson);

            if (cartItems == null || cartItems.Count == 0)
                return RedirectToAction("Index", "Home");

            ViewBag.CartTotal = cartItems.Sum(item => item.Quantity * item.Product.Price);
            return View("Payment", cartItems);
        }

        [HttpPost]
        public IActionResult ProcessPayment(string method)
        {
            if (method == "cash")
            {
                HttpContext.Session.Remove("CartDetails");
                return RedirectToAction("Confirmation");
            }
            else if (method == "paypal")
            {
                return RedirectToAction("PayWithPayPal");
            }
            return RedirectToAction("Payment");
        }

        public IActionResult PayWithPayPal()
        {
            var total = HttpContext.Session.GetString("CartTotal");
            string paypalUrl = $"https://www.paypal.com/cgi-bin/webscr?cmd=_xclick&business=sb-afnwc39290877@personal.example.com&item_name=Commande&amount={total}&currency_code=EUR&return={Url.Action("Confirmation", "Payment", null, Request.Scheme)}&cancel_return={Url.Action("Payment", "Payment", null, Request.Scheme)}";

            return Redirect(paypalUrl);
        }


        public IActionResult Confirmation()
        {
            ViewBag.Message = "Votre paiement a été validé.";
            return View();
        }
    }
}
