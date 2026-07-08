namespace Hotel.Web.Models
{
    public class DashboardViewModel
    {
        // Habitaciones
        public int TotalHabitaciones { get; set; }

        public int Disponibles { get; set; }

        public int Ocupadas { get; set; }

        public int Vencidas { get; set; }

        public int Mantenimiento { get; set; }

        // Hospedajes
        public int HospedajesActivos { get; set; }

        // Indicadores
        public decimal IngresosHoy { get; set; }

        public decimal IngresosMes { get; set; }

        public double PorcentajeOcupacion { get; set; }
    }
}