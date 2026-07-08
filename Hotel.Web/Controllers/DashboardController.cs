using Hotel.Infrastructure.Data;
using Hotel.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly HotelDbContext _context;

        public DashboardController(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var hoy = DateTime.Today;

            // Habitaciones vencidas
            var habitacionesVencidas = await _context.Hospedajes
                .Where(h =>
                    h.Estado == "Activo" &&
                    h.FechaSalida.Date < hoy)
                .Select(h => h.IdHabitacion)
                .Distinct()
                .ToListAsync();

            var totalHabitaciones =
                await _context.Habitaciones.CountAsync();

            var disponibles =
                await _context.Habitaciones
                    .CountAsync(h => h.Estado == "Disponible");

            var ocupadas =
                await _context.Habitaciones
                    .CountAsync(h => h.Estado == "Ocupada");

            var mantenimiento =
                await _context.Habitaciones
                    .CountAsync(h => h.Estado == "Mantenimiento");

            var hospedajesActivos =
                await _context.Hospedajes
                    .CountAsync(h => h.Estado == "Activo");

            decimal ingresosHoy = await _context.Pagos
                .Where(p => p.FechaPago.Date == hoy)
                .SumAsync(p => (decimal?)p.MontoPagado) ?? 0;

            decimal ingresosMes = await _context.Pagos
                .Where(p =>
                    p.FechaPago.Month == hoy.Month &&
                    p.FechaPago.Year == hoy.Year)
                .SumAsync(p => (decimal?)p.MontoPagado) ?? 0;

            var model = new DashboardViewModel
            {
                TotalHabitaciones = totalHabitaciones,

                Disponibles = disponibles,

                Ocupadas = ocupadas,

                Vencidas = habitacionesVencidas.Count,

                Mantenimiento = mantenimiento,

                HospedajesActivos = hospedajesActivos,

                IngresosHoy = ingresosHoy,

                IngresosMes = ingresosMes,

                PorcentajeOcupacion =
                    totalHabitaciones == 0
                    ? 0
                    : Math.Round(
                        (double)ocupadas /
                        totalHabitaciones * 100,
                        1)
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerIngresos(
    DateTime fechaInicio,
    DateTime fechaFin)
        {
            var datos = await _context.Pagos
                .Where(p =>
                    p.FechaPago >= fechaInicio.Date &&
                    p.FechaPago < fechaFin.Date.AddDays(1))
                .Select(p => new
                {
                    p.FechaPago,
                    p.MontoPagado
                })
                .ToListAsync();

            var resultado = datos
                .GroupBy(p => p.FechaPago.Date)
                .OrderBy(g => g.Key)
                .Select(g => new
                {
                    Fecha = g.Key.ToString("dd/MM"),
                    Total = g.Sum(x => x.MontoPagado)
                })
                .ToList();

            return Json(resultado);
        }

    }
}