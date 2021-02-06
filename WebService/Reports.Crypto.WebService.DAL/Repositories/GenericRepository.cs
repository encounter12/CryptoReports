using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reports.Crypto.WebService.DAL.Context;
using Reports.Crypto.WebService.DAL.Repositories.Contracts;

namespace Reports.Crypto.WebService.DAL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DbSet<T> DbSet;

        private readonly CryptocurrenciesDbContext _context;

        public GenericRepository(CryptocurrenciesDbContext context)
        {
            _context = context ?? throw new ArgumentException(
                "An instance of DbContext is required to use this repository.", nameof(context));

            DbSet = context.Set<T>();
        }

        public CryptocurrenciesDbContext Context => _context;
        
        public virtual IQueryable<T> All()
            => DbSet;

        public virtual T GetById(Guid id) 
            => DbSet.Find(id);

        public virtual T GetById(long id)
            => DbSet.Find(id);

        public virtual T GetById(int id)
            => DbSet.Find(id);

        public virtual T GetById(string id)
            => DbSet.Find(id);
        
        public virtual async Task<T> GetByIdAsync(long id)
            => await DbSet.FindAsync(id);

        public virtual void Add(T entity)
            => DbSet.Add(entity);

        public virtual void AddRange(IEnumerable<T> entities)
            => DbSet.AddRange(entities);
        
        public async Task AddRangeAsync(IEnumerable<T> entities)
            => await DbSet.AddRangeAsync(entities);

        public virtual void Update(T entity) 
            => DbSet.Update(entity);

        public virtual void Delete(T entity)
            => DbSet.Remove(entity);

        public virtual async Task Delete(long entityId)
        {
            T entity = await GetByIdAsync(entityId);
            
            if (entity == null)
            {
                throw new ArgumentException(
                    $"Delete failed. No {typeof(T).Name} with id: {entityId} has been found");
            }

            Delete(entity);
        }
        
        public virtual void HardDelete(T entity) 
            => DbSet.Remove(entity);

        public void SaveChanges() 
            => _context.SaveChanges();

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();

        public void ChangeEntityState(T entity, EntityState state)
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.DbSet.Attach(entity);
            }

            entry.State = state;
        }
    }
}