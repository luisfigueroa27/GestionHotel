using Microsoft.EntityFrameworkCore;

namespace Hotel.Tests.Integration;

[TestClass]
public class PagosIntegrationTest
    : BaseIntegrationTest
{
    //------------------------------------------------

    [TestMethod]
    public async Task DebeRegistrarPago()
    {
        using var context = CreateContext();

        var pago =
            await TestDataSeeder
                .CrearPagoAsync(context);

        var resultado =
            await context.Pagos
                .FindAsync(
                    pago.IdPago);

        Assert.IsNotNull(resultado);

        Assert.AreEqual(
            200,
            resultado.MontoTotal);

        Assert.AreEqual(
            "Pendiente",
            resultado.EstadoPago);
    }

    //------------------------------------------------

    [TestMethod]
    public async Task DebeCompletarPago()
    {
        using var context = CreateContext();

        var pago =
            await TestDataSeeder
                .CrearPagoAsync(context);

        pago.MontoPagado =
            pago.MontoTotal;

        pago.SaldoPendiente =
            0;

        pago.EstadoPago =
            "Pagado";

        await context.SaveChangesAsync();

        var resultado =
            await context.Pagos
                .FindAsync(
                    pago.IdPago);

        Assert.IsNotNull(resultado);

        Assert.AreEqual(
            "Pagado",
            resultado.EstadoPago);

        Assert.AreEqual(
            0,
            resultado.SaldoPendiente);
    }

    //------------------------------------------------

    [TestMethod]
    public async Task DebeMantenerSaldoPendiente()
    {
        using var context = CreateContext();

        var pago =
            await TestDataSeeder
                .CrearPagoAsync(
                    context,
                    montoTotal: 200,
                    montoPagado: 150,
                    saldoPendiente: 50);

        var resultado =
            await context.Pagos
                .FindAsync(
                    pago.IdPago);

        Assert.IsNotNull(resultado);

        Assert.AreEqual(
            50,
            resultado.SaldoPendiente);

        Assert.AreEqual(
            "Pendiente",
            resultado.EstadoPago);
    }

    //------------------------------------------------

    [TestMethod]
    public async Task DebeCambiarEstadoPago()
    {
        using var context = CreateContext();

        var pago =
            await TestDataSeeder
                .CrearPagoAsync(
                    context,
                    montoTotal: 300,
                    montoPagado: 300,
                    saldoPendiente: 0,
                    estado: "Pagado");

        var resultado =
            await context.Pagos
                .FindAsync(
                    pago.IdPago);

        Assert.IsNotNull(resultado);

        Assert.AreEqual(
            "Pagado",
            resultado.EstadoPago);

        Assert.AreEqual(
            300,
            resultado.MontoPagado);

        Assert.AreEqual(
            0,
            resultado.SaldoPendiente);
    }
}