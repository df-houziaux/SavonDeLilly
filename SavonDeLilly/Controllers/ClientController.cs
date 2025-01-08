using Microsoft.AspNetCore.Mvc;
using SavonDeLilly.Models;
using SavonDeLilly.Data;
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
        public IActionResult Register(Client model)
        {
            if (ModelState.IsValid)
            {
                _context.Clients.Add(model);
                _context.SaveChanges();


                return RedirectToAction("Index", "Home");
            }


            return View(model);
        }
    }
}
