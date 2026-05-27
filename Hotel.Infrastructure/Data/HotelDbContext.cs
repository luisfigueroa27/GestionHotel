using Hotel.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Infrastructure.Data
{
    public class HotelDbContext : DbContext
    {
        public HotelDbContext(DbContextOptions<HotelDbContext> options)
            : base(options)
        {
        }

        public DbSet<Administrador> Administrador { get; set; }

        public DbSet<Habitacion> Habitaciones { get; set; }

        public DbSet<Huesped> Huespedes { get; set; }

        public DbSet<Hospedaje> Hospedajes { get; set; }

        public DbSet<ServicioCuarto> ServicioCuarto { get; set; }
        public DbSet<Pago> Pagos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =========================
            // HABITACIONES
            // =========================

            modelBuilder.Entity<Habitacion>(entity =>
            {
                entity.ToTable("Habitaciones");

                entity.HasKey(h => h.IdHabitacion);
            });

            // =========================
            // HUESPEDES
            // =========================

            modelBuilder.Entity<Huesped>(entity =>
            {
                entity.ToTable("Huespedes");

                entity.HasKey(h => h.IdHuesped);
            });

            // =========================
            // HOSPEDAJES
            // =========================

            modelBuilder.Entity<Hospedaje>(entity =>
            {
                entity.ToTable("Hospedajes");

                entity.HasKey(h => h.IdHospedaje);

                // RELACION HABITACION

                entity.HasOne(h => h.Habitacion)
                      .WithMany(h => h.Hospedajes)
                      .HasForeignKey(h => h.IdHabitacion);

                // RELACION HUESPED

                entity.HasOne(h => h.Huesped)
                      .WithMany(h => h.Hospedajes)
                      .HasForeignKey(h => h.IdHuesped);
            });

            // =========================
            // SERVICIO CUARTO
            // =========================

            modelBuilder.Entity<ServicioCuarto>(entity =>
            {
                entity.ToTable("ServicioCuarto");

                entity.HasKey(s => s.IdServicio);

                entity.HasOne(s => s.Habitacion)
                      .WithMany(h => h.Servicios)
                      .HasForeignKey(s => s.IdHabitacion);
            });

            modelBuilder.Entity<Administrador>(entity =>
            {
                entity.ToTable("Administrador");

                entity.HasKey(a => a.IdAdministrador);
            });

            // =========================
            // PAGOS
            // =========================

            modelBuilder.Entity<Pago>(entity =>
            {
                entity.ToTable("Pagos");

                entity.HasKey(p => p.IdPago);

                entity.Property(p => p.MontoTotal)
                    .HasColumnType("decimal(10,2)");

                entity.Property(p => p.MontoPagado)
                    .HasColumnType("decimal(10,2)");

                entity.Property(p => p.SaldoPendiente)
                    .HasColumnType("decimal(10,2)");

                // RELACION FK

                entity.HasOne(p => p.Hospedaje)
                    .WithMany()
                    .HasForeignKey(p => p.IdHospedaje)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
