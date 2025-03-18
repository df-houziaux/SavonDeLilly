using Microsoft.AspNetCore.Mvc;
using SavonDeLilly.Models; // Assurez-vous d'importer le bon espace de noms pour vos modèles
using System.Net;
using System.Net.Mail;

namespace SavonDeLilly.Controllers
{
    public class MonCompteController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            if (email == "test@savon.com" && password == "123456")
            {
                TempData["SuccessMessage"] = "Connexion réussie !";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ErrorMessage = "Identifiants incorrects.";
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        // Affichage du formulaire de mot de passe oublié
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(string Email)
        {
            Console.WriteLine($"Email reçu : {Email}");

            if (string.IsNullOrEmpty(Email))
            {
                ViewBag.Message = "Veuillez entrer une adresse e-mail valide.";
                Console.WriteLine($"Message affiché : {ViewBag.Message}");
                return View();
            }

            // Générer un token
            string token = GenerateToken(); // Génère un token unique

            // Créer le lien de réinitialisation
            string resetLink = Url.Action("ResetPassword", "MonCompte", new { token = token }, Request.Scheme);

            // Configuration de l'envoi de l'email
            try
            {
                MailMessage mail = new MailMessage
                {
                    From = new MailAddress("df.houziaux@gmail.com"), // Mon email
                    Subject = "Réinitialisation de votre mot de passe",
                    Body = $"Cliquez sur ce lien pour réinitialiser votre mot de passe : <a href='{resetLink}'>Réinitialiser mon mot de passe</a>",
                    IsBodyHtml = true
                };
                mail.To.Add(Email);

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("df.houziaux@gmail.com", "fnui srzh xvio ycwj"), // Mon mot de passe d'application
                    EnableSsl = true
                };
                smtp.Send(mail);

                ViewBag.Message = "Un lien de réinitialisation a été envoyé à votre adresse e-mail.";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Erreur lors de l'envoi de l'email : " + ex.Message;
            }

            Console.WriteLine($"Message affiché : {ViewBag.Message}");
            return View();
        }

        // Action pour afficher le formulaire de réinitialisation
        public IActionResult ResetPassword(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                ViewBag.ErrorMessage = "Le token est invalide.";
                return View("Error");
            }

            var model = new ResetPassword
            {
                Token = token
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPassword model)
        {
            if (ModelState.IsValid)
            {
                // Vérifier la validité du token (ajoute ta logique ici)

                // Vérifier si le mot de passe et la confirmation correspondent
                if (model.Password != model.ConfirmPassword)
                {
                    ViewBag.ErrorMessage = "Les mots de passe ne correspondent pas.";
                    return View(model);
                }

                // Logique pour mettre à jour le mot de passe dans la base de données
                // Par exemple, trouver l'utilisateur par e-mail et mettre à jour le mot de passe

                ViewBag.Message = "Votre mot de passe a été réinitialisé avec succès.";
                return View();
            }

            return View(model); // Retourne le modèle avec des messages d'erreur si la validation échoue
        }

        // Méthode pour générer un token
        private string GenerateToken()
        {
            return Guid.NewGuid().ToString(); // Génère un token unique
        }
    }
}
