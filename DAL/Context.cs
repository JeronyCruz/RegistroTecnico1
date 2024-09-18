using Microsoft.EntityFrameworkCore;
using RegistroTecnico1.Models;

namespace RegistroTecnico1.DAL;
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
        : base(options) { }

        public DbSet<Tecnicos> Tecnicos { get; set; }
        public DbSet<TiposTecnicos> TiposTecnicos { get; set; }
        public DbSet<Clientes> Clientes { get; set; }
        public DbSet<Trabajos> Trabajos { get; set; }
        public DbSet<Prioridades> Prioridades { get; set; }
}

