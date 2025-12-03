using EyeCareHub.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.BLL.specifications
{
    public class SpecificationEvaluator <T> :  BaseSpecification<T> where T :BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> InputQuery, ISpecifications<T> spec)
        {
            var query = InputQuery;

            if (spec.Criteria != null) //GetById
                query = query.Where(spec.Criteria); //P=>P.Id == 1

            if (spec.OrderBy != null)
                query = query.OrderBy(spec.OrderBy);

            if (spec.OrderByDescending != null)
                query = query.OrderByDescending(spec.OrderByDescending);
            if (spec.IsPaginationEnable)
                query = query.Skip(spec.Skip).Take(spec.Take);

            query = spec.Includes.Aggregate(query, (currentQuery, include) => currentQuery.Include(include));

            return query;
        }
    }
}
