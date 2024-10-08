﻿using Microsoft.EntityFrameworkCore;
using RegistroTecnico1.DAL;
using RegistroTecnico1.Models;
using System.Linq.Expressions;

namespace RegistroTecnico1.Service;
    public class TecnicoService(Context context)
{
        private readonly Context _context = context;

    public async Task<bool> Existe(int id)
        {
            return await _context.Tecnicos.AnyAsync(p => p.TecnicoId == id);
        }

        private async Task<bool> Insertar(Tecnicos tecnicos)
        {
            _context.Tecnicos.Add(tecnicos);
            return await _context.SaveChangesAsync() > 0;
        }

        private async Task<bool> Modificar(Tecnicos tecnicos)
        {
            var existingTecnico = await _context.Tecnicos.FindAsync(tecnicos.TecnicoId);
            if (existingTecnico != null)
            {
                _context.Entry(existingTecnico).CurrentValues.SetValues(tecnicos);
                return await _context.SaveChangesAsync() > 0;
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
            return await _context.Tecnicos.Include(x => x.TipoTecnico)
            .AsNoTracking().
                FirstOrDefaultAsync(p => p.TecnicoId == id);
        }

        public async Task<List<Tecnicos>> Listar(Expression<Func<Tecnicos, bool>> criterio)
        {
            return await _context.Tecnicos.Include(x => x.TipoTecnico)
            .AsNoTracking()
                .Where(criterio)
                .ToListAsync();
        }

        public async Task<bool> NombreExiste(string nombreTecnico)
        {
            var nombreTecnicoNormalizado = nombreTecnico.Trim().ToLower();
            return await _context.Tecnicos
                .AnyAsync(t => t.NombresTecnico.Trim().ToLower() == nombreTecnicoNormalizado);
        }

    }
