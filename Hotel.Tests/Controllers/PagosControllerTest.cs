using Hotel.Domain.Entities;
using Hotel.Infrastructure.Data;
using Hotel.Web.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Tests.Controllers
{
    [TestClass]
    public class PagosControllerTest
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

            // HABITACION

            var habitacion = new Habitacion
            {
                NumeroHabitacion = "101",
                TipoHabitacion = "Simple",
                Precio = 80,
                Estado = "Disponible"
            };

            _context.Habitaciones.Add(habitacion);

            // HUESPED

            var huesped = new Huesped
            {
                NombreCompleto = "Luis",
                DNI = "77601331",
                Telefono = "999999999"
            };

            _context.Huespedes.Add(huesped);

            _context.SaveChanges();

            // HOSPEDAJE

            var hospedaje = new Hospedaje
            {
                IdHabitacion = habitacion.IdHabitacion,
                IdHuesped = huesped.IdHuesped,
                FechaEntrada = DateTime.Now,
                FechaSalida = DateTime.Now.AddDays(2),
                Estado = "Activo"
            };

            _context.Hospedajes.Add(hospedaje);

            _context.SaveChanges();

            // PAGO

            _context.Pagos.Add(
                new Pago
                {
                    IdHospedaje =
                        hospedaje.IdHospedaje,

                    MontoTotal = 160,

                    MontoPagado = 100,

                    SaldoPendiente = 60,

                    EstadoPago = "Pendiente",

                    FechaPago = DateTime.Now
                });

            _context.SaveChanges();
        }

        // =========================
        // INDEX
        // =========================

        [TestMethod]
        public async Task Index_DeberiaRetornarVista()
        {
            var controller =
                new PagosController(_context);

            var result =
                await controller.Index(null);

            Assert.IsInstanceOfType(
                result,
                typeof(ViewResult));
        }

        // =========================
        // BUSCAR DNI
        // =========================

        [TestMethod]
        public async Task Index_DeberiaFiltrarPorDni()
        {
            var controller =
                new PagosController(_context);

            var result =
                await controller.Index("77601331");

            Assert.IsInstanceOfType(
                result,
                typeof(ViewResult));
        }

        // =========================
        // DNI VACIO
        // =========================

        [TestMethod]
        public async Task Index_DniVacio_NoDebeFallar()
        {
            var controller =
                new PagosController(_context);

            var result =
                await controller.Index("");

            Assert.IsInstanceOfType(
                result,
                typeof(ViewResult));
        }
    }
}