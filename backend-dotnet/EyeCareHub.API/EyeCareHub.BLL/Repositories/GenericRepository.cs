using EyeCareHub.BLL.Interface;
using EyeCareHub.BLL.specifications;
using EyeCareHub.DAL.Data;
using EyeCareHub.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.BLL.Repositories
{
    public class GenericRepository<TContext,T> : IGenericRepository<TContext,T>
    where T : BaseEntity
    where TContext : DbContext
    {
        #region Inject
        private readonly TContext context;

        public GenericRepository(TContext context)
        {
            this.context = context;
        }


        #endregion


        public async Task<IReadOnlyList<T>> GetAllAsync()
            =>  await context.Set<T>().ToListAsync();

        public async Task<IReadOnlyList<T>> GetAllWithspecAsync(ISpecifications<T> spec)
            => await ApplaySpecification(spec).ToListAsync();

        public async Task<T> GetByIdAsync(int Id)
            => await context.Set<T>().FindAsync(Id);

        public async Task<T> GetByIdWithspecAsync(ISpecifications<T> spec)
            => await ApplaySpecification(spec).FirstOrDefaultAsync();

        public async Task<int> GetCountAsync(ISpecifications<T> spec)
            => await ApplaySpecification(spec).CountAsync();


        public async Task Add(T Entity)
            => await context.Set<T>().AddAsync(Entity);

        public async void Delete(T Entity)
            => context.Set<T>().Remove(Entity);

        public void Update(T Entity)
            => context.Set<T>().Update(Entity);

        public async Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await context.Set<T>().Where(predicate).ToListAsync();
        }


        public IQueryable<T> GetQueryable()
        {
            return context.Set<T>().AsQueryable();
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await context.Set<T>().AnyAsync(predicate);
        }

        private IQueryable<T> ApplaySpecification(ISpecifications<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(context.Set<T>(), spec);
        }

    }
}
