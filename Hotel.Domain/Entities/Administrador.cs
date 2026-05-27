namespace Hotel.Domain.Entities
{
    public class Administrador
    {
        public int IdAdministrador { get; set; }

        public string Usuario { get; set; }

        public string PasswordHash { get; set; }
    }
}