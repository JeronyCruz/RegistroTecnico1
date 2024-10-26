using Microsoft.EntityFrameworkCore;
using RegistroTecnico1.DAL;
using RegistroTecnico1.Models;
using System.Linq.Expressions;

namespace RegistroTecnico1.Service;
    public class TecnicoService(IDbContextFactory<Context> DbFactory)
{

    public async Task<bool> Existe(int id)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Tecnicos.AnyAsync(p => p.TecnicoId == id);
        }

        private async Task<bool> Insertar(Tecnicos tecnicos)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            contexto.Tecnicos.Add(tecnicos);
            return await contexto.SaveChangesAsync() > 0;
        }

        private async Task<bool> Modificar(Tecnicos tecnicos)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            var existingTecnico = await contexto.Tecnicos.FindAsync(tecnicos.TecnicoId);
            if (existingTecnico != null)
            {
                contexto.Entry(existingTecnico).CurrentValues.SetValues(tecnicos);
                return await contexto.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<bool> Guardar(Tecnicos tecnicos)
        {
            if (!await Existe(tecnicos.TecnicoId))
                return await Insertar(tecnicos);
            else
                return await Modificar(tecnicos);
        }

        public async Task<bool> Eliminar(int id)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            var tecnico = await contexto.Tecnicos.FindAsync(id);
            if (tecnico != null)
            {
                contexto.Tecnicos.Remove(tecnico);
                await contexto.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Tecnicos?> Buscar(int id)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Tecnicos.Include(x => x.TipoTecnico)
            .AsNoTracking().
                FirstOrDefaultAsync(p => p.TecnicoId == id);
        }

        public async Task<List<Tecnicos>> Listar(Expression<Func<Tecnicos, bool>> criterio)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Tecnicos.Include(x => x.TipoTecnico)
            .AsNoTracking()
                .Where(criterio)
                .ToListAsync();
        }

        public async Task<bool> NombreExiste(string nombreTecnico)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            var nombreTecnicoNormalizado = nombreTecnico.Trim().ToLower();
            return await contexto.Tecnicos
                .AnyAsync(t => t.NombresTecnico.Trim().ToLower() == nombreTecnicoNormalizado);
        }

    }
