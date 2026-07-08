using Hotel.Domain.Entities;
using Hotel.Infrastructure.Data;
using Hotel.Web.Controllers;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

using Moq;

namespace Hotel.Tests.Controllers
{
    [TestClass]
    public class HabitacionesControllerTest
    {
        private HotelDbContext _context;

        [TestInitialize]
        public void Setup()
        {
            var options =
                new DbContextOptionsBuilder<HotelDbContext>()
                .UseInMemoryDatabase(
                    Guid.NewGuid().ToString())
                .Options;

            _context = new HotelDbContext(options);

            _context.Habitaciones.Add(
                new Habitacion
                {
                    IdHabitacion = 1,
                    NumeroHabitacion = "101",
                    IdTipoHabitacion = 1,

                    TipoHabitacion = new TipoHabitacion
                    {
                        IdTipoHabitacion = 1,
                        Nombre = "Simple"
                    },
                    Precio = 80,
                    Estado = "Disponible"
                });

            _context.SaveChanges();
        }

        // =========================
        // CREAR CONTROLLER
        // =========================

        private HabitacionesController CrearController()
        {
            var controller =
                new HabitacionesController(_context);

            var httpContext =
                new DefaultHttpContext();

            httpContext.Session =
                new TestSession();

            httpContext.Session.SetString(
                "Administrador",
                "admin");

            controller.ControllerContext =
                new ControllerContext()
                {
                    HttpContext = httpContext
                };

            controller.TempData =
                new TempDataDictionary(
                    httpContext,
                    Mock.Of<ITempDataProvider>());

            return controller;
        }

        // =========================
        // INDEX
        // =========================

        [TestMethod]
        public async Task Index_DeberiaRetornarVista()
        {
            var controller = CrearController();

            var result =
                await controller.Index();

            Assert.IsInstanceOfType(
                result,
                typeof(ViewResult));
        }

        // =========================
        // REGISTRAR HABITACION
        // =========================

        [TestMethod]
        public async Task RegistrarHabitacion_DeberiaGuardar()
        {
            var controller = CrearController();

            await controller.RegistrarHabitacion(
                "102",     // Número
                2,         // IdTipoHabitacion (Doble)
                120,       // Precio
                1,         // Piso
                2,         // Capacidad
                "TV, WIFI");

            var habitacion =
                await _context.Habitaciones
                .FirstOrDefaultAsync(h =>
                    h.NumeroHabitacion == "102");

            Assert.IsNotNull(habitacion);
        }

        // =========================
        // NO DUPLICAR
        // =========================

        [TestMethod]
        public async Task RegistrarHabitacion_NoDebeDuplicar()
        {
            var controller = CrearController();

            await controller.RegistrarHabitacion(
                 "101",
                 1,         // IdTipoHabitacion (Simple)
                 80,
                 1,
                 1,
                 "TV");

            var total =
                await _context.Habitaciones
                .CountAsync(h =>
                    h.NumeroHabitacion == "101");

            Assert.AreEqual(1, total);
        }

        // =========================
        // REGISTRAR HOSPEDAJE
        // =========================

        [TestMethod]
        public async Task RegistrarHospedaje_DeberiaOcuparHabitacion()
        {
            var controller = CrearController();

            await controller.RegistrarHospedaje(
                1,
                "Juan Perez",
                "12345678",
                "999999999",
                DateTime.Now.AddDays(2),
                200,
                100,
                100);

            var habitacion =
                await _context.Habitaciones
                .FindAsync(1);

            Assert.AreEqual(
                "Ocupada",
                habitacion.Estado);
        }

        // =========================
        // OBTENER HOSPEDAJE
        // =========================

        [TestMethod]
        public async Task ObtenerHospedaje_DeberiaRetornarJson()
        {
            var huesped = new Huesped
            {
                NombreCompleto = "Carlos",
                DNI = "12345678",
                Telefono = "999999999"
            };

            _context.Huespedes.Add(huesped);

            await _context.SaveChangesAsync();

            _context.Hospedajes.Add(
                new Hospedaje
                {
                    IdHabitacion = 1,
                    IdHuesped = huesped.IdHuesped,
                    FechaEntrada = DateTime.Now,
                    FechaSalida =
                        DateTime.Now.AddDays(1),
                    Estado = "Activo"
                });

            await _context.SaveChangesAsync();

            var controller = CrearController();

            var result =
                await controller.ObtenerHospedaje(1);

            Assert.IsInstanceOfType(
                result,
                typeof(JsonResult));
        }

        // =========================
        // FINALIZAR HOSPEDAJE
        // =========================

        [TestMethod]
        public async Task FinalizarHospedaje_DeberiaCambiarEstado()
        {
            var controller = CrearController();

            var result =
                await controller.FinalizarHospedaje(999);

            Assert.IsInstanceOfType(
                result,
                typeof(JsonResult));
        }

        // =========================
        // LIBERAR HABITACION
        // =========================

        [TestMethod]
        public async Task LiberarHabitacion_DeberiaDisponible()
        {
            var habitacion =
                await _context.Habitaciones
                .FindAsync(1);

            habitacion.Estado = "Mantenimiento";

            await _context.SaveChangesAsync();

            var controller = CrearController();

            await controller.LiberarHabitacion(1);

            Assert.AreEqual(
                "Disponible",
                habitacion.Estado);
        }

        // =========================
        // BUSCAR HUESPED EXISTE
        // =========================

        [TestMethod]
        public async Task BuscarHuesped_DeberiaRetornarExiste()
        {
            var huesped = new Huesped
            {
                NombreCompleto = "Luis",
                DNI = "77601331",
                Telefono = "999999999"
            };

            _context.Huespedes.Add(huesped);

            await _context.SaveChangesAsync();

            var controller = CrearController();

            var result =
                await controller.BuscarHuesped(
                    "77601331");

            Assert.IsInstanceOfType(
                result,
                typeof(JsonResult));
        }

        // =========================
        // HUESPED NO EXISTE
        // =========================

        [TestMethod]
        public async Task BuscarHuesped_NoExiste()
        {
            var controller = CrearController();

            var result =
                await controller.BuscarHuesped(
                    "00000000");

            Assert.IsInstanceOfType(
                result,
                typeof(JsonResult));
        }

        [TestMethod]
        public async Task FinalizarHospedaje_DeberiaFinalizarYMantenimiento()
        {
            // =========================
            // CREAR HUESPED
            // =========================

            var huesped = new Huesped
            {
                NombreCompleto = "Luis",
                DNI = "77601331",
                Telefono = "999999999"
            };

            _context.Huespedes.Add(huesped);

            await _context.SaveChangesAsync();

            // =========================
            // OBTENER HABITACION
            // =========================

            var habitacion =
                await _context.Habitaciones
                .FindAsync(1);

            habitacion.Estado = "Ocupada";

            // =========================
            // CREAR HOSPEDAJE
            // =========================

            var hospedaje = new Hospedaje
            {
                IdHabitacion = 1,

                IdHuesped = huesped.IdHuesped,

                FechaEntrada = DateTime.Now,

                FechaSalida =
                    DateTime.Now.AddDays(2),

                Estado = "Activo"
            };

            _context.Hospedajes.Add(hospedaje);

            await _context.SaveChangesAsync();

            // =========================
            // CONTROLLER
            // =========================

            var controller = CrearController();

            // =========================
            // ACT
            // =========================

            await controller.FinalizarHospedaje(
                hospedaje.IdHospedaje);

            // =========================
            // ASSERT
            // =========================

            var hospedajeActualizado =
                await _context.Hospedajes
                .Include(h => h.Habitacion)
                .FirstAsync(h =>
                    h.IdHospedaje ==
                    hospedaje.IdHospedaje);

            Assert.AreEqual(
                "Finalizado",
                hospedajeActualizado.Estado);

            Assert.AreEqual(
                "Mantenimiento",
                hospedajeActualizado
                .Habitacion
                .Estado);
        }
    }
}