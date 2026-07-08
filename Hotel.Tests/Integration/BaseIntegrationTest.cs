using Hotel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Tests.Integration;

public abstract class BaseIntegrationTest
{
    protected HotelDbContext CreateContext()
    {
        return TestDbFactory.Create();
    }

    [TestInitialize]
    public virtual async Task LimpiarBaseDatos()
    {
        using var context = CreateContext();

        await context.Database.ExecuteSqlRawAsync(@"
DELETE FROM ServicioCuarto;
DELETE FROM Pagos;
DELETE FROM Hospedajes;
DELETE FROM Huespedes;
DELETE FROM Habitaciones;
DELETE FROM TiposHabitacion;

DBCC CHECKIDENT ('ServicioCuarto', RESEED, 0);
DBCC CHECKIDENT ('Pagos', RESEED, 0);
DBCC CHECKIDENT ('Hospedajes', RESEED, 0);
DBCC CHECKIDENT ('Huespedes', RESEED, 0);
DBCC CHECKIDENT ('Habitaciones', RESEED, 0);
DBCC CHECKIDENT ('TiposHabitacion', RESEED, 0);
");
    }
}