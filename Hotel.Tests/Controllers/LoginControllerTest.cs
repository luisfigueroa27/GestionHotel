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
    public class LoginControllerTest
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

            _context.Administrador.Add(
                new Administrador
                {
                    Usuario = "admin",
                    PasswordHash = "123456"
                });

            _context.SaveChanges();
        }

        private LoginController CrearController()
        {
            var controller =
                new LoginController(_context);

            var httpContext =
                new DefaultHttpContext();

            httpContext.Session =
                new TestSession();

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
        // LOGIN CORRECTO
        // =========================

        [TestMethod]
        public async Task Login_Correcto()
        {
            var controller = CrearController();

            var result =
                await controller.Ingresar(
                    "admin",
                    "123456");

            Assert.IsInstanceOfType(
                result,
                typeof(RedirectToActionResult));
        }

        // =========================
        // LOGIN INCORRECTO
        // =========================

        [TestMethod]
        public async Task Login_Incorrecto()
        {
            var controller = CrearController();

            var result =
                await controller.Ingresar(
                    "admin",
                    "xxxx");

            Assert.IsInstanceOfType(
                result,
                typeof(RedirectToActionResult));
        }

        // =========================
        // INDEX
        // =========================

        [TestMethod]
        public void Index_DeberiaRetornarVista()
        {
            var controller = CrearController();

            var result =
                controller.Index();

            Assert.IsInstanceOfType(
                result,
                typeof(ViewResult));
        }

        // =========================
        // LOGOUT
        // =========================

        [TestMethod]
        public void Logout_DeberiaRedireccionar()
        {
            var controller = CrearController();

            var result =
                controller.Logout();

            Assert.IsInstanceOfType(
                result,
                typeof(RedirectToActionResult));
        }

        // =========================
        // LOGOUT LIMPIA SESSION
        // =========================

        [TestMethod]
        public void Logout_DeberiaLimpiarSession()
        {
            var controller = CrearController();

            controller.HttpContext.Session.SetString(
                "Administrador",
                "admin");

            controller.Logout();

            var session =
                controller.HttpContext.Session
                .GetString("Administrador");

            Assert.IsNull(session);
        }
    }
}