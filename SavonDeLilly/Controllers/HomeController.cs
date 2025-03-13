using Microsoft.AspNetCore.Mvc;
using SavonDeLilly.Models;
using System.Diagnostics;

namespace SavonDeLilly.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Récupération des trois dernières créations
            var latestProducts = GetLatestProducts();
            return View(latestProducts);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private List<Product> GetLatestProducts()
        {
            // Remplacez ceci par la logique pour obtenir les données réelles depuis votre base de données
            return new List<Product>
        {
            new Product { Name = "Savon 1", ImageUrl = "img_carrousel_1.jpg" },
            new Product { Name = "Savon 2", ImageUrl = "img_carrousel_2.jpg" },
            new Product { Name = "Savon 3", ImageUrl = "img_carrousel_3.jpg" }
        };
        }

    }
}
