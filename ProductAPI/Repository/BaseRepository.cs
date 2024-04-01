using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ProductAPI.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class, IEntityWithName
    {
        private readonly DataContext _context;

        public BaseRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<List<T>> GetAllOrderedBy(Expression<Func<T, object>> orderBy)
        {
            return await _context.Set<T>().OrderBy(orderBy).ToListAsync();
        }

        public async Task<List<T>> Search(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<T> GetById(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task Add(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var entity = await GetById(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<T>> SearchByName(string name)
        {
            return await _context.Set<T>().Where(p => p.Name.Contains(name)).ToListAsync();
        }

    }
}
