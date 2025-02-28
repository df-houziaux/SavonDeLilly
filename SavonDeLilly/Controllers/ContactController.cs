using Microsoft.AspNetCore.Mvc; // Assurez-vous que cette directive est présente
using System.Net.Mail;
using System.Net;
using SavonDeLilly.Models;
namespace SavonDeLilly.Controllers
{
    public class ContactController : Controller // Hérite de Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View(); // Retourne la vue pour le formulaire de contact
        }

        [HttpPost]
        public IActionResult Index(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Envoi de l'email
                    bool isMailSent = SendMail(model);

                    if (isMailSent)
                    {
                        TempData["Success"] = "Votre message a été envoyé avec succès.";
                        return RedirectToAction("Index"); // Redirige vers la vue Index
                    }
                    else
                    {
                        ModelState.AddModelError("", "L'email n'a pas pu être envoyé. Veuillez réessayer.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Une erreur est survenue lors de l'envoi de l'email : " + ex.Message);
                }
            }

            return View(model); // Retourne le modèle avec les erreurs
        }

        /// <summary>
        /// Envoie un email avec les informations du modèle de contact
        /// </summary>
        /// <param name="model">Modèle contenant les informations de contact</param>
        /// <returns>Booléen indiquant si l'email a été envoyé avec succès</returns>
        private bool SendMail(ContactViewModel model)
        {
            try
            {
                using (MailMessage mailMessage = new MailMessage())
                {
                    // Configuration du message
                    mailMessage.From = new MailAddress("david.houziaux@wanadoo.fr");
                    mailMessage.To.Add("df.houziaux@gmail.com");
                    mailMessage.Subject = "Nouveau message de contact";
                    mailMessage.IsBodyHtml = true;
                    mailMessage.Body = $"<p><strong>Nom :</strong> {model.Name}</p>" +
                                       $"<p><strong>Email :</strong> {model.Email}</p>" +
                                       $"<p><strong>Téléphone :</strong> {model.Telephone}</p>" +
                                       $"<p><strong>Objet :</strong> {model.Objet}</p>" +
                                       $"<p><strong>Message :</strong> {model.Message}</p>";

                    // Configuration du client SMTP
                    using (SmtpClient smtpClient = new SmtpClient("smtp.simply.com", 587))
                    {
                        smtpClient.Credentials = new NetworkCredential("david.houziaux@wanadoo.fr", "Les12calamity"); // Remplacez par vos identifiants SMTP
                        smtpClient.EnableSsl = true;

                        // Envoi de l'email
                        smtpClient.Send(mailMessage);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                // Journalisez ou affichez l'erreur pour le débogage
                Console.WriteLine("Erreur lors de l'envoi de l'email : " + ex.Message);
                return false;
            }
        }
    }
}
