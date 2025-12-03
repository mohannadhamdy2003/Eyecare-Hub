using EyeCareHub.BLL.specifications;
using EyeCareHub.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.BLL.Interface
{
    public interface IGenericRepository< TContext,T>
        where TContext : DbContext
        where T : BaseEntity
        
    {
        Task<T> GetByIdAsync(int Id);

        Task<IReadOnlyList<T>> GetAllAsync();

        Task<T> GetByIdWithspecAsync(ISpecifications<T> spec);

        Task<IReadOnlyList<T>> GetAllWithspecAsync(ISpecifications<T> spec);

        Task<int> GetCountAsync(ISpecifications<T> spec);

        Task Add(T Entity);
        void Update(T Entity);
        void Delete(T Entity);

        Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetQueryable();
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    }
}
