using EyeCareHub.DAL.Data;
using EyeCareHub.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.BLL.Interface
{
    public interface IUnitOfWork<TContext> where TContext : DbContext
    {
        public IGenericRepository<TContext, TEntity> Repository<TEntity>() where TEntity : BaseEntity;
        Task<int> Complete();
    }
}
