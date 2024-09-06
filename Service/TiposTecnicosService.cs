using Microsoft.EntityFrameworkCore;
using RegistroTecnico1.DAL;
using RegistroTecnico1.Models;
using System.Linq.Expressions;

namespace RegistroTecnico1.Service;

public class TiposTecnicosService(Context context)
{
    private readonly Context _context = context;

    public async Task<bool> Existe(int id)
    {
        return await _context.TiposTecnicos.AnyAsync(p => p.Id == id);
    }

    private async Task<bool> Insertar(TiposTecnicos tiposTecnicos)
    {
        _context.TiposTecnicos.Add(tiposTecnicos);
        return await _context.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(TiposTecnicos tiposTecnicos)
    {
        var existingTecnico = await _context.TiposTecnicos.FindAsync(tiposTecnicos.Id);
        if (existingTecnico != null)
        {
            _context.Entry(existingTecnico).CurrentValues.SetValues(tiposTecnicos);
            return await _context.SaveChangesAsync() > 0;
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
        var tecnico = await _context.TiposTecnicos.FindAsync(id);
        if (tecnico != null)
        {
            _context.TiposTecnicos.Remove(tecnico);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<TiposTecnicos?> Buscar(int id)
    {
        return await _context.TiposTecnicos.AsNoTracking().
            FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<TiposTecnicos>> Listar(Expression<Func<TiposTecnicos, bool>> criterio)
    {
        return await _context.TiposTecnicos.AsNoTracking()
            .Where(criterio)
            .ToListAsync();
    }

    public async Task<bool> DescripcionExiste(string descripcionTecnico)
    {
        var descripccionNormalizado = descripcionTecnico.Trim().ToLower();
        return await _context.TiposTecnicos
            .AnyAsync(t => t.Descripcion.Trim().ToLower() == descripccionNormalizado);
    }

}
