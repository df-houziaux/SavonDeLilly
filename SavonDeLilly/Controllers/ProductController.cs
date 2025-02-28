using Microsoft.AspNetCore.Mvc;
using SavonDeLilly.Models;
using SavonDeLilly.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace SavonDeLilly.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Afficher tous les produits
        public async Task<IActionResult> Index()
        {
            var allProducts = await _context.Products.ToListAsync();
            return View(allProducts);
        }

        // Afficher les produits par catégories
        public async Task<IActionResult> Category1()
        {
            ViewBag.Category = "category1";
            var products = await _context.Products.Where(p => p.Name.Contains("Lavande")).ToListAsync();
            return View("Category", products);
        }

        public async Task<IActionResult> Category2()
        {
            ViewBag.Category = "category2";
            var products = await _context.Products.Where(p => p.Name.Contains("Vanille")).ToListAsync();
            return View("Category", products);
        }

        public async Task<IActionResult> Category3()
        {
            ViewBag.Category = "category3";
            var products = await _context.Products.Where(p => p.Name.Contains("Rosé")).ToListAsync();
            return View("Category", products);
        }

        // Nouvelle méthode pour rechercher des produits
        [HttpGet("/api/products/search")]
        // Afficher les résultats de la recherche
        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return RedirectToAction("Index");
            }

            var products = await _context.Products
                .Where(p => p.Name.Contains(query) || p.Ingredients.Contains(query))
                .ToListAsync();

            if (!products.Any())
            {
                TempData["SearchMessage"] = "Aucun produit trouvé.";
                return RedirectToAction("Index");
            }

            return View("Category", products);
        }


        // API pour sauvegarder le panier dans la session
        [HttpPost("/api/cart/save")]
        public IActionResult SaveCart([FromBody] List<CartItem> cartDetails)
        {
            var cartJson = JsonConvert.SerializeObject(cartDetails);
            HttpContext.Session.SetString("CartDetails", cartJson);
            return Ok();
        }

        // API pour récupérer les niveaux de stock
        [HttpGet("/api/products/stock")]
        public IActionResult GetStockLevels()
        {
            var stockLevels = _context.Products.Select(p => new { p.Name, p.Stock }).ToList();
            return Ok(stockLevels);
        }

        // Afficher la page de paiement
        public IActionResult Payment()
        {
            var cartItems = GetCartFromSession();
            return View(cartItems);
        }

        // Récupérer le panier depuis la session
        private List<CartItem> GetCartFromSession()
        {
            var cartJson = HttpContext.Session.GetString("CartDetails");
            return string.IsNullOrEmpty(cartJson) ? new List<CartItem>() : JsonConvert.DeserializeObject<List<CartItem>>(cartJson);
        }
    }
}
