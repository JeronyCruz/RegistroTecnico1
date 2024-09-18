using Microsoft.EntityFrameworkCore;
using RegistroTecnico1.DAL;
using RegistroTecnico1.Models;
using System.Linq.Expressions;

namespace RegistroTecnico1.Service;

public class PrioridadesService(Context context)
{
    private readonly Context _context = context;

    public async Task<bool> Existe(int id)
    {
        return await _context.Prioridades.AnyAsync(a => a.PrioridadId == id);
    }

    private async Task<bool> Insertar(Prioridades prioridades)
    {
        _context.Prioridades.Add(prioridades);
        return await _context.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Prioridades prioridades)
    {
        var existingPrioridades = await _context.Prioridades.FindAsync(prioridades.PrioridadId);
        if(existingPrioridades == null)
        {
            _context.Entry(existingPrioridades).CurrentValues.SetValues(prioridades);
            return await _context.SaveChangesAsync() > 0;
        }
        return false;
    }

    public async Task<bool> Guardar(Prioridades prioridades)
    {
        if (!await Existe(prioridades.PrioridadId))
            return await Insertar(prioridades);
        else
            return await Modificar(prioridades);
    }

    public async Task<bool> Eliminar(int id)
    {
        var prioridad = await _context.Prioridades.FindAsync(id);
        if(prioridad == null)
        {
            _context.Prioridades.Remove(prioridad);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<Prioridades> Buscar(int id)
    {
        return await _context.Prioridades.AsNoTracking().FirstOrDefaultAsync(p => p.PrioridadId == id);
    }

    public async Task<List<Prioridades>> Listar(Expression<Func<Prioridades, bool>> criterio)
    {
        return await _context.Prioridades.AsNoTracking().Where(criterio).ToListAsync();
    }

    public async Task<bool> DescripcionExiste(string descripcion)
    {
        var descripcionNormalizado = descripcion.Trim().ToLower();
        return await _context.Prioridades.AnyAsync(t => t.Descripcion.Trim().ToLower() == descripcionNormalizado);
    }

    public async Task<bool> TiempoExiste(int tiempo)
    {
        if (tiempo <= 0)
        {
            return false;
        }
        return await _context.Prioridades.AnyAsync(t => t.Tiempo == tiempo);
    }


}
