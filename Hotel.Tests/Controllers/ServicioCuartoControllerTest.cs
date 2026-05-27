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
    public class ServicioCuartoControllerTest
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
                    TipoHabitacion = "Simple",
                    Estado = "Ocupada"
                });

            _context.SaveChanges();
        }

        private ServicioCuartoController CrearController()
        {
            var controller =
                new ServicioCuartoController(_context);

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

        [TestMethod]
        public async Task Registrar_DeberiaGuardar()
        {
            var controller = CrearController();

            await controller.Registrar(
                1,
                "Limpieza");

            var servicio =
                await _context.ServicioCuarto
                .FirstOrDefaultAsync();

            Assert.IsNotNull(servicio);
        }

        [TestMethod]
        public async Task Completar_DeberiaCambiarEstado()
        {
            _context.ServicioCuarto.Add(
                new ServicioCuarto
                {
                    IdHabitacion = 1,
                    Descripcion = "Comida",
                    Estado = "Pendiente",
                    FechaSolicitud = DateTime.Now
                });

            await _context.SaveChangesAsync();

            var servicio =
                await _context.ServicioCuarto
                .FirstAsync();

            var controller = CrearController();

            await controller.Completar(
                servicio.IdServicio);

            Assert.AreEqual(
                "Completado",
                servicio.Estado);
        }

        // =========================
        // COMPLETAR NULL
        // =========================

        [TestMethod]
        public async Task Completar_NoDebeFallar()
        {
            var controller = CrearController();

            var result =
                await controller.Completar(999);

            Assert.IsInstanceOfType(
                result,
                typeof(RedirectToActionResult));
        }

        // =========================
        // INDEX VACIO
        // =========================

        [TestMethod]
        public async Task Index_Vacio_NoDebeFallar()
        {
            var controller = CrearController();

            var result =
                await controller.Index();

            Assert.IsInstanceOfType(
                result,
                typeof(ViewResult));
        }
    }
}
