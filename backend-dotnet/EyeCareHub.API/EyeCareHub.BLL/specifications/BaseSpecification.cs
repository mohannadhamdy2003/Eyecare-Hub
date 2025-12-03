using EyeCareHub.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.BLL.specifications
{
    public class BaseSpecification<T> : ISpecifications<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDescending { get; set; }
        public List<Expression<Func<T, object>>> FilterByBrand_Type { get; set; }
        public bool IsPaginationEnable { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }

        public BaseSpecification(Expression<Func<T, bool>> Criteria) //by Id
        {
            this.Criteria = Criteria;
        }
        public BaseSpecification()
        {

        }

        public void AddIncludes(Expression<Func<T, object>> Include)
        {
            Includes.Add(Include);
        }

        public void AddOrderBy(Expression<Func<T, object>> orderBy)
        {
            OrderBy = orderBy;
        }

        public void AddOrderByDesc(Expression<Func<T, object>> orderByDesc)
        {
            OrderByDescending = orderByDesc;
        }

        public void ApplyPagination(int skip, int take)
        {
            IsPaginationEnable = true;
            Skip = skip;
            Take = take;
        }
    }
}
