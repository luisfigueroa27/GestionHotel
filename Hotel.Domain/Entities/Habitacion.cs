namespace Hotel.Domain.Entities
{
    public class Habitacion
    {
        public int IdHabitacion { get; set; }

        public string NumeroHabitacion { get; set; }

        public string TipoHabitacion { get; set; }

        public string Estado { get; set; }

        public ICollection<Hospedaje>? Hospedajes { get; set; }

        public ICollection<ServicioCuarto>? Servicios { get; set; }
        public decimal Precio { get; set; }
    }
}