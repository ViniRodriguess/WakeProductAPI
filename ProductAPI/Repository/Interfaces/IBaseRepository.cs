using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ProductAPI.Repository.Interfaces
{
    public interface IEntityWithName
    {
        string Name { get; set; }
    }
    public interface IBaseRepository<T> where T : class, IEntityWithName
    {
        Task<List<T>> GetAll();
        Task<List<T>> GetAllOrderedBy(Expression<Func<T, object>> orderBy);
        Task<List<T>> SearchByName(string name);
        Task<List<T>> Search(Expression<Func<T, bool>> predicate); 
        Task<T> GetById(Guid id);
        Task Add(T entity);
        Task Update(T entity);
        Task Delete(Guid id);
    }
}
