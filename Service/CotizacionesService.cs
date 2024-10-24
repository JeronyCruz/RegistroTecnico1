using Microsoft.EntityFrameworkCore;
using RegistroTecnico1.DAL;
using RegistroTecnico1.Models;
using System.Linq.Expressions;

namespace RegistroTecnico1.Service;

public class CotizacionesService(IDbContextFactory<Context> DbFactory)
{
    public async Task<bool> Existe(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Cotizaciones.AnyAsync(a => a.CotizacionId == id);
    }

    private async Task<bool> Insertar(Cotizaciones cotizaciones)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        await AfectarArticulo(cotizaciones.CotizacionesDetalle.ToArray(), true);
        contexto.Cotizaciones.Add(cotizaciones);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task AfectarArticulo(CotizacionesDetalle[] detalle, bool resta = true)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        foreach (var item in detalle)
        {
            var Articulo = await contexto.Articulos.SingleAsync(p => p.ArticuloId == item.ArticuloId);
            if (resta)
                Articulo.Existencia -= item.Cantidad;
            else
                Articulo.Existencia += item.Cantidad;
        }
    }

    private async Task<bool> Modificar(Cotizaciones cotizaciones)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var cotizacionOriginal = await contexto.Cotizaciones
        .Include(t => t.CotizacionesDetalle)
        .AsNoTracking()
        .FirstOrDefaultAsync(t => t.CotizacionId == cotizaciones.CotizacionId);

        await AfectarArticulo(cotizacionOriginal.CotizacionesDetalle.ToArray(), false);

        await AfectarArticulo(cotizaciones.CotizacionesDetalle.ToArray(), true);

        contexto.Update(cotizaciones);
        return await contexto.SaveChangesAsync() > 0;
    }



    public async Task<bool> Guardar(Cotizaciones cotizaciones)
    {
        if (!await Existe(cotizaciones.CotizacionId))
            return await Insertar(cotizaciones);
        else
            return await Modificar(cotizaciones);
    }

    public async Task<bool> Eliminar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var cotizacion = contexto.Cotizaciones.Find(id);

        await AfectarArticulo(cotizacion.CotizacionesDetalle.ToArray(), resta: false);

        contexto.CotizacionesDetalles.RemoveRange(cotizacion.CotizacionesDetalle);
        contexto.Cotizaciones.Remove(cotizacion);
        var cantidad = await contexto.SaveChangesAsync();
        return cantidad > 0;


        /*var trabajo = await _context.Trabajos.FindAsync(id);
        if (trabajo != null)
        {
            _context.Trabajos.Remove(trabajo);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;*/
    }

    public async Task<Cotizaciones> Buscar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Cotizaciones
            .Include(t => t.CotizacionesDetalle)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.CotizacionId == id);
    }

    public async Task<Cotizaciones> BuscarConDetalles(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Cotizaciones
            .Include(t => t.CotizacionesDetalle)
            .ThenInclude(td => td.Articulo)
            .FirstOrDefaultAsync(p => p.CotizacionId == id);
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
}
