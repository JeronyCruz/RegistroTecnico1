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
        _context.Trabajos.Add(trabajos);
        return await _context.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Trabajos trabajos)
    {
        var existingTrabajo = await _context.Trabajos.FindAsync(trabajos.TrabajoId);
        if (existingTrabajo != null)
        {
            _context.Entry(existingTrabajo).CurrentValues.SetValues(trabajos);
            return await _context.SaveChangesAsync() > 0;
        }
        return false;
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
        var trabajo = await _context.Trabajos.FindAsync(id);
        if (trabajo != null)
        {
            _context.Trabajos.Remove(trabajo);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<Trabajos> Buscar(int id)
    {
        return await _context.Trabajos.Include(t => t.Tecnico).Include(t => t.Cliente).Include(t => t.Prioridad)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.TrabajoId == id);
    }

    public async Task<List<Trabajos>> Listar(Expression<Func<Trabajos, bool>> criterio)
    {
        return await _context.Trabajos.Include(t => t.Tecnico).Include(t => t.Cliente).Include(t => t.Prioridad)
            .AsNoTracking().Where(criterio).ToListAsync();

    }

   /* public async Task<bool> NombreExiste(string nombreCliente)
    {
        var nombreClienteNormalizado = nombreCliente.Trim().ToLower();
        return await _context.Clientes.AnyAsync(t => t.Nombres.Trim().ToLower() == nombreClienteNormalizado);
    }*/

}
