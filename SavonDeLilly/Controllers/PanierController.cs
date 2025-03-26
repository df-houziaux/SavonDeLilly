using Microsoft.AspNetCore.Mvc;

namespace SavonDeLilly.Controllers
{
    public class PanierController : Controller
    {
        // Afficher la page du panier
        public IActionResult Index()
        {
            return View("Panier");
        }
    }
}
