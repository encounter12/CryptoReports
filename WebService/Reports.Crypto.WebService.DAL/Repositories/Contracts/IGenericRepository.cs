using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reports.Crypto.WebService.DAL.Repositories.Contracts
{
    public interface IGenericRepository<T>
    {
        IQueryable<T> All();

        T GetById(Guid id);

        T GetById(long id);

        T GetById(int id);

        T GetById(string id);

        Task<T> GetByIdAsync(long id);
        
        void Add(T entity);

        void AddRange(IEnumerable<T> entities);

        Task AddRangeAsync(IEnumerable<T> entities);

        void Update(T entity);

        void Delete(T entity);

        Task Delete(long entityId);

        void HardDelete(T entity);

        void SaveChanges();

        Task SaveChangesAsync();
    }
}