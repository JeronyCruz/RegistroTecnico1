using Microsoft.EntityFrameworkCore;
using RegistroTecnico1.DAL;
using RegistroTecnico1.Models;
using System.Linq.Expressions;

namespace RegistroTecnico1.Service
{
    public class TecnicoService
    {
        private readonly Context _context;

        public TecnicoService(Context context)
        {
            _context = context;
        }

        public async Task<bool> Existe(int id)
        {
            return await _context.Tecnicos.AnyAsync(p => p.tecnicoId == id);
        }

        public async Task<bool> Insertar(Tecnicos tecnicos)
        {
            _context.Tecnicos.Add(tecnicos);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Modificar(Tecnicos tecnicos)
        {
            _context.Update(tecnicos);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Guardar(Tecnicos tecnicos)
        {
            if (!await Existe(tecnicos.tecnicoId))
                return await Insertar(tecnicos);
            else
                return await Modificar(tecnicos);
        }

        public async Task<bool> Eliminar(int id)
        {
            var tecnicos = await _context.Tecnicos.FirstOrDefaultAsync(P => P.tecnicoId == id);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Tecnicos?> Buscar(int id)
        {
            return await _context.Tecnicos.AsNoTracking().
                FirstOrDefaultAsync(p => p.tecnicoId == id);
        }

        public async Task<List<Tecnicos>> Listar(Expression<Func<Tecnicos, bool>> criterio)
        {
            return _context.Tecnicos.AsNoTracking()
                .Where(criterio)
                .ToList();
        }

    }
}
