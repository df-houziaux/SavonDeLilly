using Microsoft.AspNetCore.Mvc;
using SavonDeLilly.Models;
using SavonDeLilly.Data;
using BCrypt.Net;
using System.Linq;

namespace SavonDeLilly.Controllers
{
    public class ClientController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Inscription()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Register model)
        {
            if (ModelState.IsValid)
            {
                // Vérifie si l'email est déjà utilisé
                if (_context.Clients.Any(c => c.Email == model.Email))
                {
                    TempData["ErrorMessage"] = "Cet email est déjà utilisé.";
                    return View("Inscription", model);
                }

                // Hachage du mot de passe
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

                // Création du client
                var client = new Client
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    PasswordHash = hashedPassword
                };

                _context.Clients.Add(client);
                _context.SaveChanges();

                TempData["Message"] = "Inscription réussie !";
                return RedirectToAction("Inscription");
            }

            TempData["ErrorMessage"] = "Erreur lors de l'inscription. Veuillez vérifier vos informations.";
            return View("Inscription", model);
        }
    }
}
