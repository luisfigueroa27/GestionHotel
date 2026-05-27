using System;
using System.Collections.Generic;
using System.Text;

namespace Hotel.Domain.Entities
{
    public class ServicioCuarto
    {
        public int IdServicio { get; set; }

        public int IdHabitacion { get; set; }

        public string Descripcion { get; set; }

        public string Estado { get; set; }

        public DateTime FechaSolicitud { get; set; }

        public Habitacion? Habitacion { get; set; }
    }
}
