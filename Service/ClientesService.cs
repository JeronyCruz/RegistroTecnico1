using Microsoft.EntityFrameworkCore;
using RegistroTecnico1.DAL;
using RegistroTecnico1.Models;
using System.Linq.Expressions;

namespace RegistroTecnico1.Service;

public class ClientesService(Context context)
{
    private readonly Context _context = context;

    public async Task<bool> Existe(int id)
    {
        return await _context.Clientes.AnyAsync(a => a.ClienteId == id);
    }

    private async Task<bool> Insertar(Clientes clientes)
    {
        _context.Clientes.Add(clientes);
        return await _context.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Clientes clientes)
    {
        var existingCliente = await _context.Clientes.FindAsync(clientes.ClienteId);
        if(existingCliente != null)
        {
            _context.Entry(existingCliente).CurrentValues.SetValues(clientes);
                return await _context.SaveChangesAsync() > 0;
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
        var cliente = await _context.Clientes.FindAsync(id);
        if(cliente != null)
        {
            _context.Clientes.Remove(cliente);
             await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<Clientes> Buscar(int id)
    {
        return await _context.Clientes.AsNoTracking().
               FirstOrDefaultAsync(p => p.ClienteId == id);
    }

    public async Task<List<Clientes>> Listar(Expression<Func<Clientes, bool>> criterio)
    {
            return await _context.Clientes.AsNoTracking().Where(criterio).ToListAsync();

    }

    public async Task<bool> NombreExiste(string nombreCliente)
    {
        var nombreClienteNormalizado = nombreCliente.Trim().ToLower();
           return await _context.Clientes.AnyAsync(t => t.Nombres.Trim().ToLower() == nombreClienteNormalizado);
    }

}
