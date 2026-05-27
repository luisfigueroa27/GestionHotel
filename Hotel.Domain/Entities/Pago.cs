namespace Hotel.Domain.Entities
{
    public class Pago
    {
        public int IdPago { get; set; }

        public int IdHospedaje { get; set; }

        public decimal MontoTotal { get; set; }

        public decimal MontoPagado { get; set; }

        public decimal SaldoPendiente { get; set; }

        public string EstadoPago { get; set; }
            = string.Empty;

        public DateTime FechaPago { get; set; }

        // RELACION

        public Hospedaje? Hospedaje { get; set; }
    }
}