using Microsoft.EntityFrameworkCore;
using RegistroTecnico1.DAL;
using RegistroTecnico1.Models;
using System.Linq.Expressions;

namespace RegistroTecnico1.Service;

public class ClientesService(IDbContextFactory<Context> DbFactory)
{

    public async Task<bool> Existe(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Clientes.AnyAsync(a => a.ClienteId == id);
    }

    private async Task<bool> Insertar(Clientes clientes)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Clientes.Add(clientes);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Clientes clientes)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var existingCliente = await contexto.Clientes.FindAsync(clientes.ClienteId);
        if(existingCliente != null)
        {
            contexto.Entry(existingCliente).CurrentValues.SetValues(clientes);
                return await contexto.SaveChangesAsync() > 0;
        }
        return false;
    }

    public async Task<bool> Guardar(Clientes clientes)
    {
        if (!await Existe(clientes.ClienteId))
            return await Insertar(clientes);
        else
            return await Modificar(clientes);
    }

    public async Task<bool> Eliminar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var cliente = await contexto.Clientes.FindAsync(id);
        if(cliente != null)
        {
            contexto.Clientes.Remove(cliente);
             await contexto.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<Clientes> Buscar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Clientes
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.ClienteId == id);
    }

    public async Task<List<Clientes>> Listar(Expression<Func<Clientes, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Clientes
            .AsNoTracking()
            .Where(criterio)
            .ToListAsync();

    }

    public async Task<bool> NombreExiste(string nombreCliente)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var nombreClienteNormalizado = nombreCliente.Trim().ToLower();
           return await contexto.Clientes
            .AnyAsync(t => t.Nombres.Trim().ToLower() == nombreClienteNormalizado);
    }

    public async Task<bool> NumeroExiste(string numeroWhatsApp)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var numeroNormalizado = numeroWhatsApp.Trim().ToLower();
        return await contexto.Clientes
            .AnyAsync(t => t.WhatsApp.Trim().ToLower() == numeroNormalizado);
    }

}
