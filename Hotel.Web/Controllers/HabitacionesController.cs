using Hotel.Domain.Entities;
using Hotel.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Web.Controllers
{
    public class HabitacionesController : Controller
    {
        private readonly HotelDbContext _context;

        public HabitacionesController(HotelDbContext context)
        {
            _context = context;
        }

        // =========================
        // LISTAR HABITACIONES
        // =========================

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString(
                "Administrador") == null)
            {
                return RedirectToAction(
                    "Index",
                    "Login");
            }

            var habitaciones = await _context.Habitaciones
                .ToListAsync();

            return View(habitaciones);
        }

        // =========================
        // REGISTRAR HOSPEDAJE
        // =========================

        [HttpPost]
        public async Task<IActionResult> RegistrarHospedaje(
            int IdHabitacion,
            string NombreCompleto,
            string DNI,
            string Telefono,
            DateTime FechaSalida,
            decimal MontoTotal,
            decimal Pago,
            decimal SaldoPendiente)
        {
            // =========================
            // BUSCAR HUESPED
            // =========================

            var huesped = await _context.Huespedes
                .FirstOrDefaultAsync(h => h.DNI == DNI);

            // =========================
            // SI NO EXISTE -> CREAR
            // =========================

            if (huesped == null)
            {
                huesped = new Huesped
                {
                    NombreCompleto = NombreCompleto,
                    DNI = DNI,
                    Telefono = Telefono
                };

                _context.Huespedes.Add(huesped);

                await _context.SaveChangesAsync();
            }

            // =========================
            // CREAR HOSPEDAJE
            // =========================

            var hospedaje = new Hospedaje
            {
                IdHabitacion = IdHabitacion,

                IdHuesped = huesped.IdHuesped,

                FechaEntrada = DateTime.Now,

                FechaSalida = FechaSalida,

                Estado = "Activo"
            };

            _context.Hospedajes.Add(hospedaje);

            // =========================
            // ACTUALIZAR HABITACION
            // =========================

            var habitacion = await _context.Habitaciones
                .FindAsync(IdHabitacion);

            if (habitacion != null)
            {
                habitacion.Estado = "Ocupada";
            }

            // =========================
            // GUARDAR HOSPEDAJE
            // =========================

            await _context.SaveChangesAsync();

            // =========================
            // REGISTRAR PAGO
            // =========================

            var pago = new Pago
            {
                IdHospedaje = hospedaje.IdHospedaje,

                MontoTotal = MontoTotal,

                MontoPagado = Pago,

                SaldoPendiente = SaldoPendiente,

                EstadoPago =
                    SaldoPendiente > 0
                    ? "Pendiente"
                    : "Pagado",

                FechaPago = DateTime.Now
            };

            _context.Pagos.Add(pago);

            await _context.SaveChangesAsync();

            TempData["success"] =
                "Hospedaje registrado correctamente";

            return RedirectToAction("Index");
        }

        // =========================
        // OBTENER HOSPEDAJE
        // =========================

        [HttpGet]
        public async Task<IActionResult> ObtenerHospedaje(
            int idHabitacion)
        {
            var hospedaje = await _context.Hospedajes
                .Include(h => h.Huesped)
                .Include(h => h.Habitacion)
                .Where(h =>
                    h.IdHabitacion == idHabitacion &&
                    h.Estado == "Activo")
                .FirstOrDefaultAsync();

            if (hospedaje == null)
            {
                return Json(null);
            }

            return Json(new
            {
                idHospedaje = hospedaje.IdHospedaje,

                habitacion =
                    hospedaje.Habitacion.NumeroHabitacion,

                huesped =
                    hospedaje.Huesped.NombreCompleto,

                dni =
                    hospedaje.Huesped.DNI,

                telefono =
                    hospedaje.Huesped.Telefono,

                fechaEntrada =
                    hospedaje.FechaEntrada
                    .ToString("yyyy-MM-dd"),

                fechaSalida =
                    hospedaje.FechaSalida
                    .ToString("yyyy-MM-dd")
            });
        }

        // =========================
        // FINALIZAR HOSPEDAJE
        // =========================

        [HttpPost]
        public async Task<IActionResult> FinalizarHospedaje(
            int idHospedaje)
        {
            var hospedaje = await _context.Hospedajes
                .Include(h => h.Habitacion)
                .FirstOrDefaultAsync(h =>
                    h.IdHospedaje == idHospedaje);

            if (hospedaje != null)
            {
                hospedaje.Estado = "Finalizado";

                if (hospedaje.Habitacion != null)
                {
                    hospedaje.Habitacion.Estado =
                        "Mantenimiento";
                }

                await _context.SaveChangesAsync();
            }

            return Json(new { success = true });
        }

        // =========================
        // LIBERAR HABITACION
        // =========================

        [HttpPost]
        public async Task<IActionResult> LiberarHabitacion(
            int idHabitacion)
        {
            var habitacion = await _context.Habitaciones
                .FindAsync(idHabitacion);

            if (habitacion != null)
            {
                habitacion.Estado = "Disponible";

                await _context.SaveChangesAsync();
            }

            return Json(new { success = true });
        }

        // =========================
        // REGISTRAR HABITACION
        // =========================

        [HttpPost]
        public async Task<IActionResult> RegistrarHabitacion(
            string NumeroHabitacion,
            decimal Precio,
            string TipoHabitacion)
        {
            // VALIDAR DUPLICADO

            var existe = await _context.Habitaciones
                .AnyAsync(h =>
                    h.NumeroHabitacion ==
                    NumeroHabitacion);

            if (existe)
            {
                TempData["error"] =
                    "La habitación ya existe";

                return RedirectToAction("Index");
            }

            // CREAR HABITACION

            var habitacion = new Habitacion
            {
                NumeroHabitacion = NumeroHabitacion,

                TipoHabitacion = TipoHabitacion,

                Precio = Precio,

                Estado = "Disponible"
            };

            _context.Habitaciones.Add(habitacion);

            await _context.SaveChangesAsync();

            TempData["success"] =
                "Habitación registrada correctamente";

            return RedirectToAction("Index");
        }

        // =========================
        // BUSCAR HUESPED POR DNI
        // =========================

        [HttpGet]
        public async Task<IActionResult> BuscarHuesped(
            string dni)
        {
            var huesped = await _context.Huespedes
                .FirstOrDefaultAsync(h => h.DNI == dni);

            if (huesped == null)
            {
                return Json(new
                {
                    existe = false
                });
            }

            return Json(new
            {
                existe = true,

                nombreCompleto =
                    huesped.NombreCompleto,

                telefono =
                    huesped.Telefono
            });
        }


    }
}