using System;
using System.Collections.Generic;
using System.Text;

namespace Hotel.Domain.Entities
{
    public class Hospedaje
    {
        public int IdHospedaje { get; set; }

        public int IdHabitacion { get; set; }

        public int IdHuesped { get; set; }

        public DateTime FechaEntrada { get; set; }

        public DateTime FechaSalida { get; set; }

        public string Estado { get; set; }

        public Habitacion? Habitacion { get; set; }

        public Huesped? Huesped { get; set; }
    }
}
