using Microsoft.EntityFrameworkCore;
using Restaurante.Model;

namespace Restaurante.DA
{
    /// <summary>
    /// Contexto EF Core mapeado al esquema existente de la base de datos
    /// ExamenRestaurante. NO genera ni modifica esquemas: el diseño de la BD
    /// es externo (script SQL provisto) y la aplicación solo lo consume.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Mesa> Mesas => Set<Mesa>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Mesa>(entidad =>
            {
                entidad.ToTable("Mesas");
                entidad.HasKey(m => m.Id);

                entidad.Property(m => m.NumeroMesa).IsRequired();
                entidad.Property(m => m.Estado).IsRequired();
                entidad.Property(m => m.Capacidad).IsRequired();
                entidad.Property(m => m.Seccion).IsRequired().HasColumnType("varchar(max)");
                entidad.Property(m => m.PrecioPorPersona).IsRequired().HasColumnType("money");
                entidad.Property(m => m.IdentificacionCliente).HasColumnType("varchar(max)");
                entidad.Property(m => m.NombreCliente).HasColumnType("varchar(max)");
                entidad.Property(m => m.FechaReserva).HasColumnType("date");
                entidad.Property(m => m.DepositoDeGarantia).HasColumnType("money");

                // EstadoNombre es solo de presentación, no se persiste.
                entidad.Ignore(m => m.EstadoNombre);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
