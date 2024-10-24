using Microsoft.EntityFrameworkCore;
using RegistroTecnico1.DAL;
using RegistroTecnico1.Models;
using System.Linq.Expressions;

namespace RegistroTecnico1.Service;

public class TrabajosDetalleService(IDbContextFactory<Context> DbFactory)
{

    public async Task<List<Articulos>> Listar(Expression<Func<Articulos, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Articulos
			.AsNoTracking()
			.Where(criterio)
			.ToListAsync();
    }
}
   