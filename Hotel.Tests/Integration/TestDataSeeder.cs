using Hotel.Domain.Entities;
using Hotel.Infrastructure.Data;

namespace Hotel.Tests.Integration;

public static class TestDataSeeder
{
    //---------------------------------------------------
    // TIPO HABITACION
    //---------------------------------------------------

    public static async Task<TipoHabitacion>
        CrearTipoHabitacionAsync(
            HotelDbContext context,
            string nombre = "Simple")
    {
        var existente =
            context.TiposHabitacion
                   .FirstOrDefault(x =>
                        x.Nombre == nombre);

        if (existente != null)
            return existente;

        var tipo =
            new TipoHabitacion
            {
                Nombre = nombre
            };

        context.TiposHabitacion.Add(tipo);

        await context.SaveChangesAsync();

        return tipo;
    }

    //---------------------------------------------------
    // HABITACION
    //---------------------------------------------------

    public static async Task<Habitacion>
        CrearHabitacionAsync(
            HotelDbContext context,
            string numero = "101",
            string tipo = "Simple")
    {
        var tipoHabitacion =
            await CrearTipoHabitacionAsync(
                context,
                tipo);

        var habitacion =
            new Habitacion
            {
                NumeroHabitacion = numero,

                IdTipoHabitacion =
                    tipoHabitacion.IdTipoHabitacion,

                Precio = 80,

                Piso = 1,

                Capacidad = 2,

                Comodidades = "TV, WIFI",

                Estado = "Disponible"
            };

        context.Habitaciones.Add(habitacion);

        await context.SaveChangesAsync();

        return habitacion;
    }

    //---------------------------------------------------
    // HUESPED
    //---------------------------------------------------

    public static async Task<Huesped>
    CrearHuespedAsync(
        HotelDbContext context,
        string nombre = "Luis Figueroa",
        string dni = "77601331",
        string telefono = "973736386")
    {
        var huesped =
            new Huesped
            {
                NombreCompleto = nombre,

                DNI = dni,

                Telefono = telefono
            };

        context.Huespedes.Add(huesped);

        await context.SaveChangesAsync();

        return huesped;
    }

    //---------------------------------------------------
    // HOSPEDAJE
    //---------------------------------------------------

    public static async Task<Hospedaje>
    CrearHospedajeAsync(
        HotelDbContext context,
        string numeroHabitacion = "101",
        string tipoHabitacion = "Simple",
        string nombre = "Luis Figueroa",
        string dni = "77601331",
        string telefono = "973736386")
    {
        var habitacion =
            await CrearHabitacionAsync(
                context,
                numeroHabitacion,
                tipoHabitacion);

        var huesped =
            await CrearHuespedAsync(
                context,
                nombre,
                dni,
                telefono);

        var hospedaje =
            new Hospedaje
            {
                IdHabitacion =
                    habitacion.IdHabitacion,

                IdHuesped =
                    huesped.IdHuesped,

                FechaEntrada =
                    DateTime.Today,

                FechaSalida =
                    DateTime.Today.AddDays(2),

                Estado = "Activo"
            };

        context.Hospedajes.Add(hospedaje);

        await context.SaveChangesAsync();

        return hospedaje;
    }

    //---------------------------------------------------
    // PAGO
    //---------------------------------------------------

    public static async Task<Pago>
        CrearPagoAsync(
            HotelDbContext context,
            decimal montoTotal = 200,
            decimal montoPagado = 100,
            decimal saldoPendiente = 100,
            string estado = "Pendiente")
    {
        var hospedaje =
            await CrearHospedajeAsync(context);

        var pago =
            new Pago
            {
                IdHospedaje =
                    hospedaje.IdHospedaje,

                MontoTotal =
                    montoTotal,

                MontoPagado =
                    montoPagado,

                SaldoPendiente =
                    saldoPendiente,

                EstadoPago =
                    estado,

                FechaPago =
                    DateTime.Today
            };

        context.Pagos.Add(pago);

        await context.SaveChangesAsync();

        return pago;
    }

    //---------------------------------------------------
    // SERVICIO DE CUARTO
    //---------------------------------------------------

    public static async Task<ServicioCuarto>
        CrearServicioCuartoAsync(
            HotelDbContext context,
            string descripcion = "Almuerzo Ejecutivo",
            string estado = "Pendiente")
    {
        var habitacion =
            await CrearHabitacionAsync(
                context,
                tipo: "Suite");

        habitacion.Estado = "Ocupada";

        await context.SaveChangesAsync();

        var servicio =
            new ServicioCuarto
            {
                IdHabitacion =
                    habitacion.IdHabitacion,

                Descripcion =
                    descripcion,

                Estado =
                    estado,

                FechaSolicitud =
                    DateTime.Now
            };

        context.ServicioCuarto.Add(servicio);

        await context.SaveChangesAsync();

        return servicio;
    }
}