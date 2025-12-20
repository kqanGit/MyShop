using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyShop.Infrastructure.Data;

namespace MyShop.Infrastructure.Repositories
{
    public class Repository<T> where T : class
    {
        protected readonly AppDbContext Context;
        protected readonly DbSet<T> Set;

        public Repository(AppDbContext context)
        {
            Context = context;
            Set = context.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(object id)
        {
            return await Set.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Set.ToListAsync();
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await Set.AddAsync(entity);
            await Context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task UpdateAsync(T entity)
        {
            Set.Update(entity);
            await Context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(T entity)
        {
            Set.Remove(entity);
            await Context.SaveChangesAsync();
        }

        public virtual async Task<bool> ExistsAsync(object id)
        {
            var existing = await Set.FindAsync(id);
            return existing != null;
        }
    }
}