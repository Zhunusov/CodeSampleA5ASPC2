using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Core;
using Microsoft.EntityFrameworkCore;
using Servises.Interfaces.BaseServices;
using Utils;

namespace Services.BaseServices
{
    public abstract class BaseGenericDataService<TEntity> : IBaseGenericDataService<TEntity> where TEntity : class 
    {
        protected DbContext DbContext;

        public IQueryable<TEntity> Entities { get; }

        protected BaseGenericDataService(DbContext dbContext)
        {
            DbContext = dbContext;
            Entities = dbContext.Set<TEntity>().AsQueryable();
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
                await DbContext.AddAsync(entity);
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new ServiceResult(false, ex.Message);
            }

            return new ServiceResult(true);
        }

        public virtual async Task<ServiceResult> DeleteAsync(TEntity entity)
        {
            try
            {
                DbContext.Remove(entity);
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new ServiceResult(false, ex.Message);
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
                DbContext.Update(entity);
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new ServiceResult(false, ex.Message);
            }

            return new ServiceResult(true);
        }
    }
}
