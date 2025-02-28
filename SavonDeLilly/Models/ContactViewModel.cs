using System.ComponentModel.DataAnnotations;

namespace SavonDeLilly.Models
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "Veuillez entrer votre nom.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Veuillez entrer votre adresse email.")]
        [EmailAddress(ErrorMessage = "Veuillez entrer une adresse email valide.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Veuillez entrer votre numéro de téléphone.")]
        [RegularExpression(@"^(0[1-9][ .]?[0-9]{2}[ .]?[0-9]{2}[ .]?[0-9]{2}[ .]?[0-9]{2})$",
                           ErrorMessage = "Veuillez entrer un numéro de téléphone valide (ex: 03-12-12-09 ou 03 12 12 09).")]
        public string Telephone { get; set; }

        [Required(ErrorMessage = "Veuillez sélectionner un objet.")]
        public string Objet { get; set; }

        [Required(ErrorMessage = "Veuillez entrer votre message.")]
        public string Message { get; set; }

        [Required(ErrorMessage = "Veuillez donner votre accord")]
        public bool Consentement { get; set; }
    }
}
