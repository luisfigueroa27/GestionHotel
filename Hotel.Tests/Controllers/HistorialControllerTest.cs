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
    public class HistorialControllerTest
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
        }

        // =========================
        // CREAR CONTROLLER
        // =========================

        private HistorialController CrearController()
        {
            var controller =
                new HistorialController(_context);

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

            // TEMPDATA

            controller.TempData =
                new TempDataDictionary(
                    httpContext,
                    Mock.Of<ITempDataProvider>());

            return controller;
        }

        // =========================
        // INDEX NORMAL
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
        // INDEX SIN SESION
        // =========================

        [TestMethod]
        public async Task Index_SinSesion_DeberiaRedireccionar()
        {
            var controller =
                new HistorialController(_context);

            var httpContext =
                new DefaultHttpContext();

            httpContext.Session =
                new TestSession();

            controller.ControllerContext =
                new ControllerContext()
                {
                    HttpContext = httpContext
                };

            var result =
                await controller.Index();

            Assert.IsInstanceOfType(
                result,
                typeof(RedirectToActionResult));
        }

        // =========================
        // FINALIZAR
        // =========================

        [TestMethod]
        public async Task Finalizar_DeberiaCambiarEstado()
        {
            // ARRANGE

            var controller = CrearController();

            // ACT

            var result =
                await controller.Finalizar(999);

            // ASSERT

            Assert.IsInstanceOfType(
                result,
                typeof(RedirectToActionResult));

            var redirect =
                result as RedirectToActionResult;

            Assert.AreEqual(
                "Index",
                redirect.ActionName);
        }

        // =========================
        // FINALIZAR NULL
        // =========================

        [TestMethod]
        public async Task Finalizar_NoDebeFallar()
        {
            var controller = CrearController();

            var result =
                await controller.Finalizar(999);

            Assert.IsInstanceOfType(
                result,
                typeof(RedirectToActionResult));
        }
    }
}