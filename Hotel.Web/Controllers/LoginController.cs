using Hotel.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly HotelDbContext _context;

        public LoginController(HotelDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Ingresar(
            string Usuario,
            string Password)
        {
            var admin = await _context.Administrador
                .FirstOrDefaultAsync(a =>
                    a.Usuario == Usuario &&
                    a.PasswordHash == Password);

            if (admin == null)
            {
                TempData["error"] =
                    "Usuario o contraseña incorrectos";

                return RedirectToAction("Index");
            }

            HttpContext.Session.SetString(
                "Administrador",
                admin.Usuario);

            return RedirectToAction(
                "Index",
                "Dashboard");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index");
        }
    }
}