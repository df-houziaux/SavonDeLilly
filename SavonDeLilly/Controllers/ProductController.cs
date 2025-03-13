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

        // Afficher tous les produits avec Category.cshtml
        public async Task<IActionResult> Index()
        {
            var allProducts = await _context.Products.ToListAsync();
            ViewBag.Category = "Tous les produits";
            return View("Category", allProducts); // Afficher avec Category.cshtml
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

        public async Task<IActionResult> Search(string query, string filter = "name")
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return RedirectToAction("Index");
            }

            // Initialiser la requête de base
            var productsQuery = _context.Products.AsQueryable();

            // Appliquer les filtres en fonction des paramètres
            switch (filter)
            {
                case "name":
                    // Recherche par nom
                    productsQuery = productsQuery.Where(p => p.Name.Contains(query));
                    break;
                case "ingredient":
                    // Recherche par ingrédient
                    productsQuery = productsQuery.Where(p => p.Ingredients.Contains(query));
                    break;
                case "price-asc":
                    // Tri par prix ascendant
                    productsQuery = productsQuery.Where(p => p.Name.Contains(query) || p.Ingredients.Contains(query))
                                               .OrderBy(p => p.Price);
                    break;
                case "price-desc":
                    // Tri par prix descendant
                    productsQuery = productsQuery.Where(p => p.Name.Contains(query) || p.Ingredients.Contains(query))
                                               .OrderByDescending(p => p.Price);
                    break;
                default:
                    // Si aucun filtre valide n'est spécifié, rechercher par nom puis par ingrédient
                    var productsByName = await _context.Products
                        .Where(p => p.Name.Contains(query))
                        .ToListAsync();

                    if (!productsByName.Any())
                    {
                        // Si aucun produit trouvé par nom, chercher dans les ingrédients
                        productsQuery = _context.Products.Where(p => p.Ingredients.Contains(query));
                    }
                    else
                    {
                        // Si des produits sont trouvés par nom, les utiliser
                        return View("Category", productsByName);
                    }
                    break;
            }

            // Exécuter la requête finale
            var products = await productsQuery.ToListAsync();

            if (!products.Any())
            {
                TempData["SearchMessage"] = "Aucun produit trouvé.";
            }

            ViewBag.SearchQuery = query;
            ViewBag.SearchFilter = filter;

            return View("Category", products);
        }
    }
}
