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
        public DbSet<Articulos> Articulos { get; set; }
        public DbSet<TrabajosDetalle> TrabajosDetalles { get; set; }
        public DbSet<Cotizaciones> Cotizaciones { get; set; }
        public DbSet<CotizacionesDetalle> CotizacionesDetalles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Articulos>().HasData(new List<Articulos>()
        {
            new Articulos(){ArticuloId = 1, Descripcion = "Memoria USB", Costo = 370, Precio = 450, Existencia = 30},
			new Articulos(){ArticuloId = 2, Descripcion = "JoyStick", Costo = 890, Precio = 1200, Existencia = 70},
			new Articulos(){ArticuloId = 3, Descripcion = "Cable UTP", Costo = 250, Precio = 410, Existencia = 60},
			new Articulos(){ArticuloId = 4, Descripcion = "Pasta Termica", Costo = 610, Precio = 730, Existencia = 50}
		});
	}
}

