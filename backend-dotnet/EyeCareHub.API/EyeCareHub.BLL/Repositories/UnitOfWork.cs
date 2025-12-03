using EyeCareHub.BLL.Interface;
using EyeCareHub.DAL.Data;
using EyeCareHub.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.BLL.Repositories
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext

    {
        private Hashtable _repositories; //
        private readonly TContext context;

        public UnitOfWork(TContext context)
        {
            this.context = context;
        }
        public async Task<int> Complete()
        {
            return await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public IGenericRepository<TContext,TEntity> Repository<TEntity>() where TEntity : BaseEntity 
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repository = new GenericRepository<TContext,TEntity>(context);
                _repositories.Add(type, repository);
            }

            return (IGenericRepository<TContext,TEntity>)_repositories[type];
        }
    }

}
