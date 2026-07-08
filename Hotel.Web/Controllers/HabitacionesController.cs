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

            // HABITACIONES
            var habitaciones =
    await _context.Habitaciones
        .Include(h => h.TipoHabitacion)
        .ToListAsync();

            // FECHA ACTUAL
            var hoy = DateTime.Today;

            // HABITACIONES CON HOSPEDAJE VENCIDO
            ViewBag.HabitacionesVencidas =
                await _context.Hospedajes
                .Where(h =>
                    h.Estado == "Activo" &&
                    h.FechaSalida.Date < hoy)
                .Select(h => h.IdHabitacion)
                .ToListAsync();

            ViewBag.TiposHabitacion =
        await _context.TiposHabitacion
            .OrderBy(t => t.Nombre)
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

            return RedirectToAction("Index",
    new
    {
        tab = "recepcion"
    });
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
        public async Task<IActionResult>
FinalizarHospedaje(int idHospedaje)
        {
            var hospedaje =
                await _context.Hospedajes
                .Include(h => h.Habitacion)
                .FirstOrDefaultAsync(
                    h => h.IdHospedaje ==
                    idHospedaje);

            if (hospedaje == null)
            {
                return Json(new
                {
                    success = false,
                    mensaje =
                        "Hospedaje no encontrado."
                });
            }

            var pago =
                await _context.Pagos
                .FirstOrDefaultAsync(
                    p => p.IdHospedaje ==
                    idHospedaje);

            // VALIDAR DEUDA
            if (pago != null &&
                pago.SaldoPendiente > 0)
            {
                return Json(new
                {
                    success = false,
                    mensaje =
                        $"El huesped tiene un saldo pendiente de S/ {pago.SaldoPendiente}"
                });
            }

            hospedaje.Estado =
                "Finalizado";

            if (hospedaje.Habitacion != null)
            {
                hospedaje.Habitacion.Estado =
                    "Mantenimiento";
            }

            await _context.SaveChangesAsync();

            return Json(new
            {
                success = true
            });
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
    int IdTipoHabitacion,
    int Piso,
    int Capacidad,
    string? Comodidades)
        {
            bool existe = await _context.Habitaciones
                .AnyAsync(h =>
                    h.NumeroHabitacion ==
                    NumeroHabitacion);

            if (existe)
            {
                TempData["error"] =
                    "Ya existe una habitacion con ese numero";

                return RedirectToAction("Index",
    new
    {
        tab = "administracion"
    });
            }

            if (Piso < 1)
            {
                TempData["error"] =
                    "El piso debe ser mayor a cero";

                return RedirectToAction("Index",
    new
    {
        tab = "administracion"
    });
            }

            if (Capacidad < 1)
            {
                TempData["error"] =
                    "La capacidad debe ser mayor a cero";

                return RedirectToAction("Index",
    new
    {
        tab = "administracion"
    });
            }

            var habitacion = new Habitacion
            {
                NumeroHabitacion = NumeroHabitacion,
                IdTipoHabitacion = IdTipoHabitacion,
                Precio = Precio,
                Piso = Piso,
                Capacidad = Capacidad,
                Comodidades = Comodidades,
                Estado = "Disponible"
            };

            _context.Habitaciones.Add(habitacion);

            await _context.SaveChangesAsync();

            TempData["success"] =
                "Habitacion registrada correctamente";

            return RedirectToAction("Index",
    new
    {
        tab = "administracion"
    });
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

        // =========================
        // OBTENER HABITACION
        // =========================

        [HttpGet]
        public async Task<IActionResult> ObtenerHabitacion(
    int id)
        {
            var habitacion =
    await _context.Habitaciones
        .Include(h => h.TipoHabitacion)
        .FirstOrDefaultAsync(
            h => h.IdHabitacion == id);

            if (habitacion == null)
                return Json(null);

            return Json(new
            {
                idHabitacion = habitacion.IdHabitacion,

                numeroHabitacion = habitacion.NumeroHabitacion,

                idTipoHabitacion = habitacion.IdTipoHabitacion,

                tipoHabitacion = habitacion.TipoHabitacion.Nombre,

                precio = habitacion.Precio,

                estado = habitacion.Estado,

                piso = habitacion.Piso,

                capacidad = habitacion.Capacidad,

                comodidades = habitacion.Comodidades
            });
        }

        // =========================
        // EDITAR HABITACION
        // =========================
        [HttpPost]
        public async Task<IActionResult> EditarHabitacion(
    int IdHabitacion,
    string NumeroHabitacion,
    int IdTipoHabitacion,
    decimal Precio,
    string Estado,
    int Piso,
    int Capacidad,
    string? Comodidades)
        {
            var habitacion =
                await _context.Habitaciones
                .FindAsync(IdHabitacion);

            if (habitacion == null)
            {
                TempData["error"] =
                    "Habitacion no encontrada";

                return RedirectToAction("Index",
    new
    {
        tab = "administracion"
    });
            }

            bool existe =
                await _context.Habitaciones
                .AnyAsync(h =>
                    h.IdHabitacion !=
                    IdHabitacion &&
                    h.NumeroHabitacion ==
                    NumeroHabitacion);

            if (existe)
            {
                TempData["error"] =
                    "Ya existe otra habitacion con ese numero";

                return RedirectToAction("Index",
    new
    {
        tab = "administracion"
    });
            }

            habitacion.NumeroHabitacion =
                NumeroHabitacion;

            habitacion.IdTipoHabitacion =
                IdTipoHabitacion;

            habitacion.Precio =
                Precio;

            habitacion.Estado =
                Estado;

            habitacion.Piso =
                Piso;

            habitacion.Capacidad =
                Capacidad;

            habitacion.Comodidades =
                Comodidades;

            await _context.SaveChangesAsync();

            TempData["success"] =
                "Habitacion actualizada correctamente";

            return RedirectToAction("Index",
    new
    {
        tab = "administracion"
    });
        }

        // =========================
        // ELIMINAR HABITACION
        // =========================

        [HttpPost]
        public async Task<IActionResult> EliminarHabitacion(
            int idHabitacion)
        {
            var habitacion = await _context.Habitaciones
                .Include(h => h.Hospedajes)
                .Include(h => h.Servicios)
                .FirstOrDefaultAsync(
                    h => h.IdHabitacion == idHabitacion);

            if (habitacion == null)
            {
                return Json(new
                {
                    success = false
                });
            }

            if (habitacion.Hospedajes != null &&
                habitacion.Hospedajes.Any())
            {
                return Json(new
                {
                    success = false,
                    mensaje =
                        "La habitacion tiene historial registrado"
                });
            }

            _context.Habitaciones.Remove(habitacion);

            await _context.SaveChangesAsync();

            return Json(new
            {
                success = true
            });
        }

        [HttpPost]
        public async Task<IActionResult> ExtenderHospedaje(
    int idHospedaje,
    DateTime nuevaFecha)
        {
            var hospedaje = await _context.Hospedajes
                .FirstOrDefaultAsync(
                    h => h.IdHospedaje == idHospedaje);

            if (hospedaje == null)
            {
                return Json(new
                {
                    success = false
                });
            }

            if (nuevaFecha <= hospedaje.FechaSalida)
            {
                return Json(new
                {
                    success = false
                });
            }

            hospedaje.FechaSalida = nuevaFecha;

            var pago = await _context.Pagos
                .FirstOrDefaultAsync(
                    p => p.IdHospedaje ==
                    idHospedaje);

            var habitacion =
                await _context.Habitaciones
                .FindAsync(
                    hospedaje.IdHabitacion);

            if (pago != null &&
                habitacion != null)
            {
                int dias =
                    (nuevaFecha -
                     hospedaje.FechaEntrada)
                    .Days;

                if (dias < 1)
                    dias = 1;

                pago.MontoTotal =
                    dias *
                    habitacion.Precio;

                pago.SaldoPendiente =
                    pago.MontoTotal -
                    pago.MontoPagado;

                pago.EstadoPago =
                    pago.SaldoPendiente > 0
                    ? "Pendiente"
                    : "Pagado";
            }

            await _context.SaveChangesAsync();

            return Json(new
            {
                success = true
            });
        }


        [HttpGet]
        public async Task<IActionResult>
ValidarNumeroHabitacion(
    string numero)
        {
            bool existe =
                await _context.Habitaciones
                .AnyAsync(h =>
                    h.NumeroHabitacion ==
                    numero);

            return Json(new
            {
                existe
            });
        }

        [HttpPost]
        public async Task<IActionResult> AgregarTipoHabitacion(string nombre)
        {
            nombre = nombre?.Trim();

            if (string.IsNullOrWhiteSpace(nombre))
            {
                return Json(new { success = false, mensaje = "Ingrese un nombre." });
            }

            bool existe = await _context.TiposHabitacion
                .AnyAsync(t => t.Nombre == nombre);

            if (existe)
            {
                return Json(new { success = false, mensaje = "El tipo ya existe." });
            }

            _context.TiposHabitacion.Add(new TipoHabitacion
            {
                Nombre = nombre
            });

            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult>
EditarTipoHabitacion(
int id,
string nombre)
        {
            nombre = nombre.Trim();

            var tipo =
                await _context.TiposHabitacion
                .FindAsync(id);

            if (tipo == null)
            {
                return Json(new
                {
                    success = false
                });
            }

            bool existe =
                await _context.TiposHabitacion
                .AnyAsync(t =>
                t.IdTipoHabitacion != id &&
                t.Nombre == nombre);

            if (existe)
            {
                return Json(new
                {
                    success = false,
                    mensaje = "Ya existe."
                });
            }

            tipo.Nombre = nombre;

            await _context.SaveChangesAsync();

            return Json(new
            {
                success = true
            });
        }

        [HttpPost]
        public async Task<IActionResult> EliminarTipoHabitacion(int id)
        {
            var tipo = await _context.TiposHabitacion
                .FirstOrDefaultAsync(t => t.IdTipoHabitacion == id);

            if (tipo == null)
            {
                return Json(new { success = false, mensaje = "Tipo no encontrado." });
            }

            bool estaEnUso = await _context.Habitaciones
                .AnyAsync(h => h.IdTipoHabitacion == tipo.IdTipoHabitacion);

            if (estaEnUso)
            {
                return Json(new
                {
                    success = false,
                    mensaje = "No se puede eliminar porque este tipo de habitación está siendo utilizado."
                });
            }

            _context.TiposHabitacion.Remove(tipo);

            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }


        [HttpGet]
        public async Task<IActionResult> ListarTiposHabitacion()
        {
            var tipos = await _context.TiposHabitacion
                .OrderBy(t => t.Nombre)
                .ToListAsync();

            return PartialView("_ListaTiposHabitacion", tipos);
        }


    }
}