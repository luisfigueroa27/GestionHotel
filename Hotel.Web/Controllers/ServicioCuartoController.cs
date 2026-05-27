using Hotel.Domain.Entities;
using Hotel.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Web.Controllers
{
    public class ServicioCuartoController : Controller
    {
        private readonly HotelDbContext _context;

        public ServicioCuartoController(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString(
                "Administrador") == null)
            {
                return RedirectToAction(
                    "Index",
                    "Login");
            }

            ViewBag.Habitaciones = await _context.Habitaciones
                .Where(h => h.Estado == "Ocupada")
                .ToListAsync();

            var servicios = await _context.ServicioCuarto
                .Include(s => s.Habitacion)
                .OrderByDescending(s => s.IdServicio)
                .ToListAsync();

            return View(servicios);
        }

        [HttpPost]
        public async Task<IActionResult> Registrar(
            int IdHabitacion,
            string Descripcion)
        {
            var servicio = new ServicioCuarto
            {
                IdHabitacion = IdHabitacion,
                Descripcion = Descripcion,
                Estado = "Pendiente",
                FechaSolicitud = DateTime.Now
            };

            _context.ServicioCuarto.Add(servicio);

            await _context.SaveChangesAsync();

            TempData["success"] = "Servicio registrado correctamente";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Completar(int id)
        {
            var servicio = await _context.ServicioCuarto
                .FindAsync(id);

            if (servicio != null)
            {
                servicio.Estado = "Completado";

                await _context.SaveChangesAsync();
            }

            TempData["success"] = "Servicio completado";

            return RedirectToAction("Index");
        }
    }
}