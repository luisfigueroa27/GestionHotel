using Microsoft.EntityFrameworkCore;

namespace Hotel.Tests.Integration;

[TestClass]
public class HuespedesIntegrationTest
    : BaseIntegrationTest
{
    [TestMethod]
    public async Task DebeRegistrarHuesped()
    {
        using var context = CreateContext();

        var huesped =
            await TestDataSeeder
                .CrearHuespedAsync(
                    context);

        var resultado =
            await context.Huespedes
                .FirstOrDefaultAsync(h =>
                    h.DNI == huesped.DNI);

        Assert.IsNotNull(resultado);

        Assert.AreEqual(
            "Luis Figueroa",
            resultado.NombreCompleto);
    }

    //------------------------------------------------

    [TestMethod]
    public async Task DebeBuscarHuespedPorDni()
    {
        using var context = CreateContext();

        var huesped =
            await TestDataSeeder
                .CrearHuespedAsync(
                    context,
                    "Carlos Perez",
                    "12345678",
                    "999999999");

        var resultado =
            await context.Huespedes
                .FirstOrDefaultAsync(h =>
                    h.DNI == "12345678");

        Assert.IsNotNull(resultado);

        Assert.AreEqual(
            "Carlos Perez",
            resultado.NombreCompleto);
    }

    //------------------------------------------------

    [TestMethod]
    public async Task DebeActualizarTelefonoHuesped()
    {
        using var context = CreateContext();

        var huesped =
            await TestDataSeeder
                .CrearHuespedAsync(
                    context,
                    "Miguel Torres",
                    "87654321",
                    "111111111");

        huesped.Telefono =
            "999999999";

        await context.SaveChangesAsync();

        var resultado =
            await context.Huespedes
                .FirstOrDefaultAsync(h =>
                    h.IdHuesped ==
                    huesped.IdHuesped);

        Assert.IsNotNull(resultado);

        Assert.AreEqual(
            "999999999",
            resultado.Telefono);
    }

    //------------------------------------------------

    [TestMethod]
    public async Task DebeEliminarHuesped()
    {
        using var context = CreateContext();

        var huesped =
            await TestDataSeeder
                .CrearHuespedAsync(
                    context,
                    "Jose Ramos",
                    "55555555",
                    "955555555");

        context.Huespedes.Remove(
            huesped);

        await context.SaveChangesAsync();

        var resultado =
            await context.Huespedes
                .FirstOrDefaultAsync(h =>
                    h.DNI == "55555555");

        Assert.IsNull(resultado);
    }
}