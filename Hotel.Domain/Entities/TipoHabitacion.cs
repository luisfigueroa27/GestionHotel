using Hotel.Domain.Entities;

public class TipoHabitacion
{
    public int IdTipoHabitacion { get; set; }

    public string Nombre { get; set; }

    public ICollection<Habitacion>? Habitaciones { get; set; }
}