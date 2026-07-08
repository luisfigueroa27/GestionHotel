using Hotel.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Tests.Integration;

[TestClass]
public class HabitacionesIntegrationTest : BaseIntegrationTest
{
    [TestMethod]
    public async Task DebeGuardarHabitacionEnSqlServerDocker()
    {
        using var context = CreateContext();

        var habitacion =
            await TestDataSeeder
                .CrearHabitacionAsync(
                    context,
                    tipo: "Simple");

        var resultado =
            await context.Habitaciones
                .FirstOrDefaultAsync(h =>
                    h.NumeroHabitacion == "101");

        Assert.IsNotNull(resultado);

        Assert.AreEqual(
            "101",
            resultado.NumeroHabitacion);
    }

    [TestMethod]
    public async Task DebeActualizarHabitacion()
    {
        using var context = CreateContext();

        var habitacion =
            await TestDataSeeder
                .CrearHabitacionAsync(
                    context,
                    tipo: "Simple");

        habitacion.Precio = 150;

        await context.SaveChangesAsync();

        var resultado =
            await context.Habitaciones
                .FirstOrDefaultAsync(h =>
                    h.IdHabitacion ==
                    habitacion.IdHabitacion);

        Assert.IsNotNull(resultado);

        Assert.AreEqual(
            150,
            resultado.Precio);
    }

    [TestMethod]
    public async Task DebeCambiarEstadoAOcupada()
    {
        using var context = CreateContext();

        var habitacion =
            await TestDataSeeder
                .CrearHabitacionAsync(
                    context,
                    tipo: "Simple");

        habitacion.Estado = "Ocupada";

        await context.SaveChangesAsync();

        var resultado =
            await context.Habitaciones
                .FirstOrDefaultAsync(h =>
                    h.IdHabitacion ==
                    habitacion.IdHabitacion);

        Assert.IsNotNull(resultado);

        Assert.AreEqual(
            "Ocupada",
            resultado.Estado);
    }

    [TestMethod]
    public async Task DebeLiberarHabitacion()
    {
        using var context = CreateContext();

        var habitacion =
            await TestDataSeeder
                .CrearHabitacionAsync(
                    context,
                    tipo: "Doble");

        habitacion.Estado = "Ocupada";

        await context.SaveChangesAsync();

        habitacion.Estado = "Disponible";

        await context.SaveChangesAsync();

        var resultado =
            await context.Habitaciones
                .FirstOrDefaultAsync(h =>
                    h.IdHabitacion ==
                    habitacion.IdHabitacion);

        Assert.IsNotNull(resultado);

        Assert.AreEqual(
            "Disponible",
            resultado.Estado);
    }
}