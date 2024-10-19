using Microsoft.EntityFrameworkCore;
using RegistroTecnico1.DAL;
using RegistroTecnico1.Models;
using System.Linq.Expressions;

namespace RegistroTecnico1.Service;

public class TrabajosDetalleService
{
	private readonly Context _context;

	public TrabajosDetalleService(Context context)
	{
		_context = context;
	}

    public async Task<List<Articulos>> Listar(Expression<Func<Articulos, bool>> criterio)
    {
        return await _context.Articulos
			.AsNoTracking()
			.Where(criterio)
			.ToListAsync();
    }
}
   