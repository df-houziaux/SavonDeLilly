using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using SavonDeLilly.Data;
using SavonDeLilly.Models;
using BCrypt.Net;
using System.Net;
using System.Net.Mail;

namespace SavonDeLilly.Controllers
{
    public class MonCompteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MonCompteController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, bool rememberMe)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.ErrorMessage = "Veuillez entrer votre adresse e-mail et votre mot de passe.";
                return View();
            }

            var client = _context.Clients.SingleOrDefault(c => c.Email == email);

            if (client != null && BCrypt.Net.BCrypt.Verify(password, client.PasswordHash))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, client.Email),
                    new Claim("FullName", client.FullName),
                    new Claim(ClaimTypes.Role, client.Role) // Utiliser le rôle de l'utilisateur
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = rememberMe
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                TempData["SuccessMessage"] = "Connexion réussie !";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ErrorMessage = "Identifiants incorrects.";
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["SuccessMessage"] = "Déconnexion réussie !";
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Register model)
        {
            if (ModelState.IsValid)
            {
                // Vérifie si l'e-mail est déjà utilisé
                if (_context.Clients.Any(c => c.Email == model.Email))
                {
                    TempData["ErrorMessage"] = "Cet e-mail est déjà utilisé.";
                    return View(model);
                }

                // Hachage du mot de passe
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

                // Créer un nouveau client
                var client = new Client
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    PasswordHash = hashedPassword,
                    Role = "Client" // Assigner le rôle par défaut
                };

                // Ajoute le client à la base de données
                _context.Clients.Add(client);
                _context.SaveChanges();

                TempData["Message"] = "Inscription réussie ! Connectez-vous.";
                return RedirectToAction("Login");
            }

            TempData["ErrorMessage"] = "Erreur lors de l'inscription. Vérifiez vos informations.";
            return View(model);
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ViewBag.Message = "Veuillez entrer une adresse e-mail valide.";
                return View();
            }

            var client = _context.Clients.SingleOrDefault(c => c.Email == email);
            if (client == null)
            {
                ViewBag.Message = "Adresse e-mail non trouvée.";
                return View();
            }

            string token = GenerateToken();
            string resetLink = Url.Action("ResetPassword", "MonCompte", new { token = token, email = client.Email }, Request.Scheme);

            try
            {
                MailMessage mail = new MailMessage
                {
                    From = new MailAddress("df.houziaux@gmail.com"),
                    Subject = "Réinitialisation de votre mot de passe",
                    Body = $"Cliquez sur ce lien pour réinitialiser votre mot de passe : <a href='{resetLink}'>Réinitialiser mon mot de passe</a>",
                    IsBodyHtml = true
                };
                mail.To.Add(email);

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("df.houziaux@gmail.com", "fnui srzh xvio ycwj"),
                    EnableSsl = true
                };
                smtp.Send(mail);

                ViewBag.Message = "Un lien de réinitialisation a été envoyé à votre adresse e-mail.";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Erreur lors de l'envoi de l'email : " + ex.Message;
            }

            return View();
        }

        public IActionResult ResetPassword(string token, string email)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
            {
                ViewBag.ErrorMessage = "Le token ou l'e-mail est invalide.";
                return View("Error");
            }

            var model = new ResetPassword { Token = token, Email = email };
            return View(model);
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPassword model)
        {
            if (ModelState.IsValid)
            {
                // Vérifie si le nouveau mot de passe et la confirmation correspondent
                if (model.Password != model.ConfirmPassword)
                {
                    ViewBag.ErrorMessage = "Les mots de passe ne correspondent pas.";
                    return View(model);
                }

                // Récupérer l'utilisateur à partir de l'e-mail
                var client = _context.Clients.SingleOrDefault(c => c.Email == model.Email);
                if (client != null)
                {
                    // Vérifie si le nouveau mot de passe est identique à l'ancien
                    if (BCrypt.Net.BCrypt.Verify(model.Password, client.PasswordHash))
                    {
                        ViewBag.ErrorMessage = "Le nouveau mot de passe ne peut pas être le même que l'ancien mot de passe.";
                        return View(model);
                    }

                    // Hachage du nouveau mot de passe et mise à jour de l'utilisateur
                    client.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
                    _context.SaveChanges();

                    ViewBag.Message = "Votre mot de passe a été réinitialisé avec succès.";
                    return View();
                }

                // Message d'erreur si l'utilisateur n'est pas trouvé
                ViewBag.ErrorMessage = "Utilisateur non trouvé.";
            }

            // Si le modèle n'est pas valide, retourne la vue avec les messages d'erreur
            return View(model);
        }

        private string GenerateToken()
        {
            return Guid.NewGuid().ToString();
        }

        // Méthode pour créer un administrateur
        public IActionResult CreateAdmin()
        {
            // Vérifie si l'administrateur existe déjà
            if (_context.Clients.Any(c => c.Email == "admin@example.com"))
            {
                TempData["ErrorMessage"] = "L'administrateur existe déjà.";
                return RedirectToAction("Index", "Home"); // Redirige vers la page d'accueil
            }

            // Créer un nouvel utilisateur administrateur
            var admin = new Client
            {
                FullName = "Admin User",
                Email = "admin@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("AdminPassword123"), // Hachage du mot de passe
                Role = "Admin" // Définir le rôle sur Admin
            };

            // Ajoute l'administrateur à la base de données
            _context.Clients.Add(admin);

            // Enregistrez les changements et vérifiez le succès
            try
            {
                _context.SaveChanges();
                TempData["Message"] = "Administrateur créé avec succès !";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Erreur lors de la création de l'administrateur : " + ex.Message;
            }

            return RedirectToAction("Index", "Home"); // Redirige vers la page d'accueil
        }

        // Méthode pour initialiser l'administrateur
        [HttpGet]
        public IActionResult InitAdmin()
        {
            return CreateAdmin();
        }
    }
}
