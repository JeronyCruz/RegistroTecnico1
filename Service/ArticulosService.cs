using Microsoft.EntityFrameworkCore;
using RegistroTecnico1.DAL;
using RegistroTecnico1.Models;
using System.Linq.Expressions;

namespace RegistroTecnico1.Service
{
    public class ArticulosService
    {
        private readonly Context _context;

        public ArticulosService(Context context)
        {
            _context = context;
        }

        public async Task<Articulos> Buscar(int id)
        {
            return await _context.Articulos.AsNoTracking().
                   FirstOrDefaultAsync(p => p.ArticuloId == id);
        }

        public async Task<List<Articulos>> Listar(Expression<Func<Articulos, bool>> criterio)
        {
            return await _context.Articulos
                .AsNoTracking()
                .Where(criterio)
                .ToListAsync();
        }
    }
}
