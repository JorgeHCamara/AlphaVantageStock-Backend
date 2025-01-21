using AssessmentTaskHS.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AssessmentTaskHS.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task UpsertAsync(T entity, Expression<Func<T, bool>> predicate)
        {
            var existingEntity = await _dbSet.FirstOrDefaultAsync(predicate);

            if (existingEntity != null)
            {
                var entry = _dbContext.Entry(existingEntity);
                foreach (var property in entry.Properties)
                {
                    if (!property.Metadata.IsPrimaryKey())
                    {
                        property.CurrentValue = _dbContext.Entry(entity).Property(property.Metadata.Name).CurrentValue;
                    }
                }
            }
            else
            {
                await _dbSet.AddAsync(entity);
            }

            await _dbContext.SaveChangesAsync();
        }


        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
