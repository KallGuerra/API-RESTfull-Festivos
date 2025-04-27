using Festivo.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;


namespace Festivo.Infraestructura.Persistencia.Contexto
{
    public class FestivoContext : DbContext
    {
        public FestivoContext(DbContextOptions<FestivoContext> options) : base(options) { }
        public DbSet<Tipo> tipos { get; set; }
        public DbSet<Feestivo> festivos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Feestivo>(entidad =>
            {
                entidad.HasKey(e => e.Id);
                entidad.HasIndex(e => e.Nombre);

            });
            modelBuilder.Entity<Feestivo>()
                .HasOne(e => e.Tipo)
                .WithMany()
                .HasForeignKey(e => e.IdTipo);


            modelBuilder.Entity<Tipo>(entidad =>
            {
                entidad.HasKey(e => e.Id);
                entidad.HasIndex(e => e.Nombre);
            });
        }
    }
}
