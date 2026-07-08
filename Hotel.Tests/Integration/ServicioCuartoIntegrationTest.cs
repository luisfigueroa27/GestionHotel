using Microsoft.EntityFrameworkCore;

namespace Hotel.Tests.Integration;

[TestClass]
public class ServicioCuartoIntegrationTest
    : BaseIntegrationTest
{
    //----------------------------------------------------

    [TestMethod]
    public async Task DebeRegistrarServicioCuarto()
    {
        using var context = CreateContext();

        var servicio =
            await TestDataSeeder
                .CrearServicioCuartoAsync(
                    context);

        var resultado =
            await context.ServicioCuarto
                .FindAsync(
                    servicio.IdServicio);

        Assert.IsNotNull(resultado);

        Assert.AreEqual(
            "Pendiente",
            resultado.Estado);
    }

    //----------------------------------------------------

    [TestMethod]
    public async Task DebeCompletarServicioCuarto()
    {
        using var context = CreateContext();

        var servicio =
            await TestDataSeeder
                .CrearServicioCuartoAsync(
                    context,
                    descripcion: "Limpieza");

        servicio.Estado =
            "Completado";

        await context.SaveChangesAsync();

        var resultado =
            await context.ServicioCuarto
                .FindAsync(
                    servicio.IdServicio);

        Assert.IsNotNull(resultado);

        Assert.AreEqual(
            "Completado",
            resultado.Estado);
    }

    //----------------------------------------------------

    [TestMethod]
    public async Task DebeBuscarServicioPorHabitacion()
    {
        using var context = CreateContext();

        var servicio =
            await TestDataSeeder
                .CrearServicioCuartoAsync(
                    context,
                    descripcion: "Cena");

        var resultado =
            await context.ServicioCuarto
                .FirstOrDefaultAsync(
                    s => s.IdHabitacion ==
                    servicio.IdHabitacion);

        Assert.IsNotNull(resultado);

        Assert.AreEqual(
            "Cena",
            resultado.Descripcion);
    }

    //----------------------------------------------------

    [TestMethod]
    public async Task DebeGuardarFechaSolicitud()
    {
        using var context = CreateContext();

        var servicio =
            await TestDataSeeder
                .CrearServicioCuartoAsync(
                    context,
                    descripcion: "Desayuno");

        var resultado =
            await context.ServicioCuarto
                .FindAsync(
                    servicio.IdServicio);

        Assert.IsNotNull(resultado);

        Assert.AreEqual(
            servicio.FechaSolicitud.Date,
            resultado.FechaSolicitud.Date);
    }
}