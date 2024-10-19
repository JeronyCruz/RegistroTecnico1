using Microsoft.EntityFrameworkCore;
using RegistroTecnico1.DAL;
using RegistroTecnico1.Models;
using System.Linq.Expressions;

namespace RegistroTecnico1.Service;

public class TrabajosService(Context context)
{
    private readonly Context _context = context;

    public async Task<bool> Existe(int id)
    {
        return await _context.Trabajos.AnyAsync(a => a.TrabajoId == id);
    }

    private async Task<bool> Insertar(Trabajos trabajos)
    {

        await AfectarArticulo(trabajos.TrabajosDetalles.ToArray(), true);
        _context.Trabajos.Add(trabajos);
        return await _context.SaveChangesAsync() > 0;
    }

    private async Task AfectarArticulo(TrabajosDetalle[] detalle, bool resta = true)
    {
        foreach(var item in detalle)
        {
            var Articulo = await _context.Articulos.SingleAsync(p => p.ArticuloId ==  item.ArticuloId);
            if(resta)
                Articulo.Existencia -= item.Cantidad;
            else
				Articulo.Existencia += item.Cantidad;
		}
    }

    private async Task<bool> Modificar(Trabajos trabajos)
    {
        var trabajoOriginal = await _context.Trabajos
        .Include(t => t.TrabajosDetalles)
        .AsNoTracking()
        .FirstOrDefaultAsync(t => t.TrabajoId == trabajos.TrabajoId);

        await AfectarArticulo(trabajoOriginal.TrabajosDetalles.ToArray(), false);

        await AfectarArticulo(trabajos.TrabajosDetalles.ToArray(), true);

        _context.Update(trabajos);
        return await _context.SaveChangesAsync() > 0;
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
        var trabajos = _context.Trabajos.Find(id);

        await AfectarArticulo(trabajos.TrabajosDetalles.ToArray(), resta: false);

        _context.TrabajosDetalles.RemoveRange(trabajos.TrabajosDetalles);
        _context.Trabajos.Remove(trabajos);
        var cantidad = await _context.SaveChangesAsync();
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

    public async Task<Trabajos> Buscar(int id)
    {
        return await _context.Trabajos
            .Include(t => t.Tecnico)
            .Include(t => t.Cliente)
            .Include(t => t.Prioridad)
            .Include(t => t.TrabajosDetalles)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.TrabajoId == id);
    }

    public async Task<Trabajos> BuscarConDetalles(int id)
    {
        return await _context.Trabajos
            .Include(t => t.Tecnico)
            .Include(t => t.Cliente)
            .Include(t => t.Prioridad)
            .Include(t => t.TrabajosDetalles)
            .ThenInclude(td => td.Articulo)
            .FirstOrDefaultAsync(p => p.TrabajoId == id);
    }

    public async Task<List<Trabajos>> Listar(Expression<Func<Trabajos, bool>> criterio)
    {
        return await _context.Trabajos
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
