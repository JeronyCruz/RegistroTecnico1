using Microsoft.EntityFrameworkCore;
using RegistroTecnico1.Models;

namespace RegistroTecnico1.DAL;
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
        : base(options) { }

        public DbSet<Tecnicos> Tecnicos { get; set; }
        public DbSet<TiposTecnicos> TiposTecnicos { get; set; }
}

