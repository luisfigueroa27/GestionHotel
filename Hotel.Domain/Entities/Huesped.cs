using System;
using System.Collections.Generic;
using System.Text;


namespace Hotel.Domain.Entities
{
    public class Huesped
    {
        public int IdHuesped { get; set; }

        public string NombreCompleto { get; set; } 

        public string DNI { get; set; }

        public string Telefono { get; set; }

        public ICollection<Hospedaje>? Hospedajes { get; set; }
    }
}