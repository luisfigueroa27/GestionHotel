using Hotel.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace Hotel.Web.Controllers
{
    public class PagosController : Controller
    {
        private readonly HotelDbContext _context;

        public PagosController(HotelDbContext context)
        {
            _context = context;
        }

        // LISTAR PAGOS

        public async Task<IActionResult> Index(string dni)
        {
            if (HttpContext?.Session != null)
            {
                if (HttpContext.Session.GetString(
                    "Administrador") == null)
                {
                    return RedirectToAction(
                        "Index",
                        "Login");
                }
            }

            var pagos = _context.Pagos
                .Include(p => p.Hospedaje)
                .ThenInclude(h => h.Huesped)
                .AsQueryable();

            if (!string.IsNullOrEmpty(dni))
            {
                pagos = pagos.Where(p =>
                    p.Hospedaje.Huesped.DNI
                    .Contains(dni));
            }

            return View(
                await pagos.ToListAsync());
        }

        // COMPLETAR PAGO

        [HttpPost]
        public async Task<IActionResult> CompletarPago(
            int idPago,
            decimal monto)
        {
            var pago = await _context.Pagos
                .FirstOrDefaultAsync(
                    p => p.IdPago == idPago);

            if (pago == null)
            {
                TempData["error"] =
                    "No se encontró el pago.";

                return RedirectToAction(
                    "Index");
            }

            // SUMAR NUEVO PAGO

            pago.MontoPagado += monto;

            // RECALCULAR SALDO

            pago.SaldoPendiente =
                pago.MontoTotal -
                pago.MontoPagado;

            // EVITAR SALDOS NEGATIVOS

            if (pago.SaldoPendiente < 0)
            {
                pago.SaldoPendiente = 0;
            }

            // ACTUALIZAR ESTADO

            if (pago.SaldoPendiente == 0)
            {
                pago.EstadoPago = "Pagado";
            }
            else
            {
                pago.EstadoPago = "Pendiente";
            }

            await _context.SaveChangesAsync();

            TempData["success"] =
                "Pago actualizado correctamente.";

            return RedirectToAction(
                "Index");
        }
    }
}