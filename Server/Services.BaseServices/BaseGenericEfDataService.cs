using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Core;
using Microsoft.EntityFrameworkCore;
using Utils;

namespace Services.BaseServices
{
    public abstract class BaseGenericEfDataService<TEntity> where TEntity : class 
    {
        protected DbContext DbContext;

        protected DbSet<TEntity> EntityDbSet { get; }

        protected BaseGenericEfDataService(DbContext dbContext)
        {
            DbContext = dbContext;
            EntityDbSet = dbContext.Set<TEntity>();
        }

        public virtual Task<int> CountAsync()
        {
            return EntityDbSet.CountAsync();
        }

        public virtual Task<List<TEntity>> GetListAsync(int? count = null, int? offset = null)
        {
            var query = EntityDbSet.Skip(offset.GetValueOrDefault());

            if (count == null)
            {
                return query.ToListAsync();
            }

            return query.Take(count.Value).ToListAsync();
        }

        public virtual async Task<ServiceResult> CreateAsync(TEntity entity)
        {
            var errors = AttributeValidator.Validation(entity);
            if (errors != null)
            {
                return new ServiceResult(false, errors);
            }

            try
            {
                await EntityDbSet.AddAsync(entity);
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new ServiceResult(false, "Database Error");
            }

            return new ServiceResult(true);
        }

        public virtual async Task<ServiceResult> DeleteAsync(TEntity entity)
        {
            try
            {
                EntityDbSet.Remove(entity);
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new ServiceResult(false, "Database Error");
            }
            return new ServiceResult(true);
        }

        public virtual async Task<ServiceResult> UpdateAsync(TEntity entity)
        {
            var errors = AttributeValidator.Validation(entity);
            if (errors != null)
            {
                return new ServiceResult(false, errors);
            }

            try
            {
                EntityDbSet.Update(entity);
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new ServiceResult(false, "Database Error");
            }

            return new ServiceResult(true);
        }
    }
}
