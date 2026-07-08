using Microsoft.EntityFrameworkCore;

namespace Hotel.Tests.Integration;

[TestClass]
public class HospedajesIntegrationTest
    : BaseIntegrationTest
{
    //----------------------------------------------------

    [TestMethod]
    public async Task DebeRegistrarHospedaje()
    {
        using var context = CreateContext();

        var hospedaje =
            await TestDataSeeder
                .CrearHospedajeAsync(context);

        var resultado =
            await context.Hospedajes
                .FirstOrDefaultAsync(h =>
                    h.IdHospedaje ==
                    hospedaje.IdHospedaje);

        Assert.IsNotNull(resultado);
    }

    //----------------------------------------------------

    [TestMethod]
    public async Task DebeCambiarHabitacionAOcupada()
    {
        using var context = CreateContext();

        var hospedaje =
            await TestDataSeeder
                .CrearHospedajeAsync(context);

        var habitacion =
            await context.Habitaciones
                .FindAsync(
                    hospedaje.IdHabitacion);

        Assert.IsNotNull(habitacion);

        habitacion!.Estado =
            "Ocupada";

        await context.SaveChangesAsync();

        Assert.AreEqual(
            "Ocupada",
            habitacion.Estado);
    }

    //----------------------------------------------------

    [TestMethod]
    public async Task DebeExtenderEstadia()
    {
        using var context = CreateContext();

        var hospedaje =
            await TestDataSeeder
                .CrearHospedajeAsync(context);

        var fechaOriginal =
            hospedaje.FechaSalida;

        hospedaje.FechaSalida =
            fechaOriginal.AddDays(3);

        await context.SaveChangesAsync();

        var resultado =
            await context.Hospedajes
                .FindAsync(
                    hospedaje.IdHospedaje);

        Assert.IsNotNull(resultado);

        Assert.AreEqual(
            fechaOriginal.AddDays(3),
            resultado.FechaSalida);
    }

    //----------------------------------------------------

    [TestMethod]
    public async Task DebeFinalizarHospedaje()
    {
        using var context = CreateContext();

        var hospedaje =
            await TestDataSeeder
                .CrearHospedajeAsync(context);

        hospedaje.Estado =
            "Finalizado";

        await context.SaveChangesAsync();

        var resultado =
            await context.Hospedajes
                .FindAsync(
                    hospedaje.IdHospedaje);

        Assert.IsNotNull(resultado);

        Assert.AreEqual(
            "Finalizado",
            resultado.Estado);
    }

    //----------------------------------------------------

    [TestMethod]
    public void NoDebePermitirFechaSalidaMenorEntrada()
    {
        var fechaEntrada =
            DateTime.Today;

        var fechaSalida =
            DateTime.Today.AddDays(-1);

        Assert.IsTrue(
            fechaSalida <
            fechaEntrada);
    }
}