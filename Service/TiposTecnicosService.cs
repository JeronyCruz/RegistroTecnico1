using Microsoft.EntityFrameworkCore;
using RegistroTecnico1.DAL;
using RegistroTecnico1.Models;
using System.Linq.Expressions;

namespace RegistroTecnico1.Service;

public class TiposTecnicosService(IDbContextFactory<Context> DbFactory)
{

    public async Task<bool> Existe(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.TiposTecnicos.AnyAsync(p => p.Id == id);
    }

    private async Task<bool> Insertar(TiposTecnicos tiposTecnicos)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.TiposTecnicos.Add(tiposTecnicos);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(TiposTecnicos tiposTecnicos)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var existingTecnico = await contexto.TiposTecnicos.FindAsync(tiposTecnicos.Id);
        if (existingTecnico != null)
        {
            contexto.Entry(existingTecnico).CurrentValues.SetValues(tiposTecnicos);
            return await contexto.SaveChangesAsync() > 0;
        }
        return false;
    }

    public async Task<bool> Guardar(TiposTecnicos tiposTecnicos)
    {
        if (!await Existe(tiposTecnicos.Id))
            return await Insertar(tiposTecnicos);
        else
            return await Modificar(tiposTecnicos);
    }

    public async Task<bool> Eliminar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var tecnico = await contexto.TiposTecnicos.FindAsync(id);
        if (tecnico != null)
        {
            contexto.TiposTecnicos.Remove(tecnico);
            await contexto.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<TiposTecnicos?> Buscar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.TiposTecnicos.AsNoTracking().
            FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<TiposTecnicos>> Listar(Expression<Func<TiposTecnicos, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.TiposTecnicos.AsNoTracking()
            .Where(criterio)
            .ToListAsync();
    }

    public async Task<bool> DescripcionExiste(string descripcionTecnico)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var descripccionNormalizado = descripcionTecnico.Trim().ToLower();
        return await contexto.TiposTecnicos
            .AnyAsync(t => t.Descripcion.Trim().ToLower() == descripccionNormalizado);
    }

}
