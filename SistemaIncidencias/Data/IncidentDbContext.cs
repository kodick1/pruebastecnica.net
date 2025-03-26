using System.Data.Entity;
using SistemaIncidencias.Models;
using static System.Data.Entity.Migrations.Model.UpdateDatabaseOperation;

namespace SistemaIncidencias.Data
{
    public class IncidentDbContext : DbContext
    {
        public IncidentDbContext() : base("name=DefaultConnection")
        {
            // Configuración para habilitar migraciones
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<IncidentDbContext, Migrations.Configuration>());
        }

        public DbSet<Incident> Incidencias { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Tecnico> Tecnicos { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Configuraciones adicionales de modelo si son necesarias
            base.OnModelCreating(modelBuilder);
        }
    }
}