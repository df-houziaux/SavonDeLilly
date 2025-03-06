using Microsoft.AspNetCore.Mvc;
using SavonDeLilly.Models;

namespace SavonDeLilly.Controllers
{
    public class ContactController(MailService mailService, IConfiguration configuration) : Controller
    {
        private readonly MailService _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
        private readonly IConfiguration _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    SendMail(model);

                    TempData["Success"] = "Votre message a été envoyé avec succès.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Une erreur est survenue lors de l'envoi de l'email : " + ex.Message);
                }
            }
            return View(model);
        }

        /// <summary>
        /// Envoie un email avec les informations du modèle de contact en utilisant MailService
        /// </summary>
        /// <param name="model">Modèle contenant les informations de contact</param>
        private void SendMail(ContactViewModel model)
        {
            string? senderEmail = _configuration["EmailSettings:SenderEmail"];

            if (string.IsNullOrEmpty(senderEmail))

            {
                throw new InvalidOperationException("L'email de l'expéditeur n'est pas configuré.");
            }

            string emailBody = $"<p><strong>Nom :</strong> {model.Name}</p>" +
                               $"<p><strong>Email :</strong> {model.Email}</p>" +
                               $"<p><strong>Téléphone :</strong> {model.Telephone}</p>" +
                               $"<p><strong>Objet :</strong> {model.Objet}</p>" +
                               $"<p><strong>Message :</strong> {model.Message}</p>";

            _mailService.CreateMail(
                from: senderEmail,
                to: senderEmail,
                subject: "Nouveau message de contact",
                body: emailBody
            )
            .SetReplyTo(model.Email)
            .Send();
        }
    }
}