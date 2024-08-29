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
            var existingTecnico = await _context.Tecnicos.FindAsync(tecnicos.tecnicoId);
            if (existingTecnico != null)
            {
                _context.Entry(existingTecnico).CurrentValues.SetValues(tecnicos);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
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
            var tecnico = await _context.Tecnicos.FindAsync(id);
            if (tecnico != null)
            {
                _context.Tecnicos.Remove(tecnico);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Tecnicos?> Buscar(int id)
        {
            return await _context.Tecnicos.AsNoTracking().
                FirstOrDefaultAsync(p => p.tecnicoId == id);
        }

        public async Task<List<Tecnicos>> Listar(Expression<Func<Tecnicos, bool>> criterio)
        {
            return await _context.Tecnicos.AsNoTracking()
                .Where(criterio)
                .ToListAsync();
        }

    }
}
