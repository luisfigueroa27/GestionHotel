using Hotel.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace Hotel.Web.Controllers
{
    public class HistorialController : Controller
    {
        private readonly HotelDbContext _context;

        public HistorialController(HotelDbContext context)
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

            var historial = await _context.Hospedajes
                .Include(h => h.Habitacion)
                .Include(h => h.Huesped)
                .OrderByDescending(h => h.IdHospedaje)
                .ToListAsync();

            return View(historial);
        }

        [HttpPost]
        public async Task<IActionResult> Finalizar(int id)
        {
            var hospedaje = await _context.Hospedajes
                .Include(h => h.Habitacion)
                .FirstOrDefaultAsync(h => h.IdHospedaje == id);

            if (hospedaje != null)
            {
                hospedaje.Estado = "Finalizado";

                // VALIDAR HABITACION

                if (hospedaje.Habitacion != null)
                {
                    hospedaje.Habitacion.Estado =
                        "Disponible";
                }

                await _context.SaveChangesAsync();
            }

            TempData["success"] = "Hospedaje finalizado correctamente";

            return RedirectToAction("Index");
        }
    }
}
