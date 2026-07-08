namespace Hotel.Domain.Entities
{
    public class Habitacion
    {
        public int IdHabitacion { get; set; }

        public string NumeroHabitacion { get; set; }

        public int IdTipoHabitacion { get; set; }

        public TipoHabitacion TipoHabitacion { get; set; }

        public string Estado { get; set; }

        public decimal Precio { get; set; }

        public int Piso { get; set; }

        public int Capacidad { get; set; }

        public string? Comodidades { get; set; }

        public ICollection<Hospedaje>? Hospedajes { get; set; }

        public ICollection<ServicioCuarto>? Servicios { get; set; }
    }
}