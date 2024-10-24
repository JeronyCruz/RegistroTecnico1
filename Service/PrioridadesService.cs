using Microsoft.EntityFrameworkCore;
using RegistroTecnico1.DAL;
using RegistroTecnico1.Models;
using System.Linq.Expressions;

namespace RegistroTecnico1.Service;

public class PrioridadesService(IDbContextFactory<Context> DbFactory)
{

    public async Task<bool> Existe(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Prioridades.AnyAsync(a => a.PrioridadId == id);
    }

    private async Task<bool> Insertar(Prioridades prioridades)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Prioridades.Add(prioridades);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Prioridades prioridades)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var existingPrioridades = await contexto.Prioridades.FindAsync(prioridades.PrioridadId);
        if(existingPrioridades != null)
        {
            contexto.Entry(existingPrioridades).CurrentValues.SetValues(prioridades);
            return await contexto.SaveChangesAsync() > 0;
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
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var prioridad = await contexto.Prioridades.FindAsync(id);
        if(prioridad != null)
        {
            contexto.Prioridades.Remove(prioridad);
            await contexto.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<Prioridades> Buscar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Prioridades
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.PrioridadId == id);
    }

    public async Task<List<Prioridades>> Listar(Expression<Func<Prioridades, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Prioridades
            .AsNoTracking()
            .Where(criterio)
            .ToListAsync();
    }

    public async Task<bool> DescripcionExiste(string descripcion)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var descripcionNormalizado = descripcion.Trim().ToLower();
        return await contexto.Prioridades
            .AnyAsync(t => t.Descripcion.Trim().ToLower() == descripcionNormalizado);
    }

    public async Task<bool> TiempoExiste(int tiempo)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Prioridades
            .AnyAsync(t => t.Tiempo == tiempo);
    }


}
