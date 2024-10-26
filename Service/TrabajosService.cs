using Microsoft.EntityFrameworkCore;
using RegistroTecnico1.DAL;
using RegistroTecnico1.Models;
using System.Linq.Expressions;

namespace RegistroTecnico1.Service;

public class TrabajosService(IDbContextFactory<Context> DbFactory)
{

    public async Task<bool> Existe(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Trabajos.AnyAsync(a => a.TrabajoId == id);
    }

    private async Task<bool> Insertar(Trabajos trabajos)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        await AfectarArticulo(trabajos.TrabajosDetalles.ToArray(), true);
        contexto.Trabajos.Add(trabajos);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task AfectarArticulo(TrabajosDetalle[] detalle, bool resta = true)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        foreach (var item in detalle)
        {
            var Articulo = await contexto.Articulos.SingleAsync(p => p.ArticuloId ==  item.ArticuloId);
            if(resta)
                Articulo.Existencia -= item.Cantidad;
            else
				Articulo.Existencia += item.Cantidad;
		}

		await contexto.SaveChangesAsync();
	}

	private async Task<bool> Modificar(Trabajos trabajos)
	{
		await using var contexto = await DbFactory.CreateDbContextAsync();

		var trabajoOriginal = await contexto.Trabajos
			.Include(t => t.TrabajosDetalles)
			.FirstOrDefaultAsync(t => t.TrabajoId == trabajos.TrabajoId);

		if (trabajoOriginal == null)
			return false;

		await AfectarArticulo(trabajoOriginal.TrabajosDetalles.ToArray(), false);

		foreach (var detalleOriginal in trabajoOriginal.TrabajosDetalles)
		{
			if (!trabajos.TrabajosDetalles.Any(d => d.DetalleId == detalleOriginal.DetalleId))
			{
				contexto.TrabajosDetalles.Remove(detalleOriginal);
			}
		}

		await AfectarArticulo(trabajos.TrabajosDetalles.ToArray(), true);

		contexto.Entry(trabajoOriginal).CurrentValues.SetValues(trabajos);

		foreach (var detalle in trabajos.TrabajosDetalles)
		{
			var detalleExistente = trabajoOriginal.TrabajosDetalles
				.FirstOrDefault(d => d.DetalleId == detalle.DetalleId);

			if (detalleExistente != null)
			{
				contexto.Entry(detalleExistente).CurrentValues.SetValues(detalle);
			}
			else
			{
				trabajoOriginal.TrabajosDetalles.Add(detalle);
			}
		}

		return await contexto.SaveChangesAsync() > 0;
	}




	public async Task<bool> Guardar(Trabajos trabajos)
    {
        if (!await Existe(trabajos.TrabajoId))
            return await Insertar(trabajos);
        else
            return await Modificar(trabajos);
    }

	public async Task<bool> Eliminar(int id)
	{
		await using var contexto = await DbFactory.CreateDbContextAsync();
		var trabajos = await contexto.Trabajos
			.Include(t => t.TrabajosDetalles)
			.ThenInclude(td => td.Articulo)
			.FirstOrDefaultAsync(t => t.TrabajoId == id);

		if (trabajos == null)
			return false;

		await AfectarArticulo(trabajos.TrabajosDetalles.ToArray(), resta: false);

		contexto.TrabajosDetalles.RemoveRange(trabajos.TrabajosDetalles);
		contexto.Trabajos.Remove(trabajos);

		var cantidad = await contexto.SaveChangesAsync();
		return cantidad > 0;
	}


	public async Task<Trabajos> Buscar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Trabajos
            .Include(t => t.Tecnico)
            .Include(t => t.Cliente)
            .Include(t => t.Prioridad)
            .Include(t => t.TrabajosDetalles)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.TrabajoId == id);
    }

    public async Task<Trabajos> BuscarConDetalles(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Trabajos
            .Include(t => t.Tecnico)
            .Include(t => t.Cliente)
            .Include(t => t.Prioridad)
            .Include(t => t.TrabajosDetalles)
            .ThenInclude(td => td.Articulo)
            .FirstOrDefaultAsync(p => p.TrabajoId == id);
    }

    public async Task<List<Trabajos>> Listar(Expression<Func<Trabajos, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Trabajos
            .Include(t => t.Tecnico)
            .Include(t => t.Cliente)
            .Include(t => t.Prioridad)
			.Include(t => t.TrabajosDetalles)
			.AsNoTracking().Where(criterio).ToListAsync();

    }

   /* public async Task<bool> NombreExiste(string nombreCliente)
    {
        var nombreClienteNormalizado = nombreCliente.Trim().ToLower();
        return await _context.Clientes.AnyAsync(t => t.Nombres.Trim().ToLower() == nombreClienteNormalizado);
    }*/

}
