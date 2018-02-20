using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Core;
using Domain.Core.Base;
using Microsoft.EntityFrameworkCore;
using Servises.Interfaces.Base;
using Utils;

namespace Services.BaseServices
{
    public abstract class BaseGenericEfIntKeyDataService<TEntity> : BaseGenericEfDataService<TEntity>, IBaseGenericDataService<int, TEntity> where TEntity : class, IBaseEntity<int>
    {
        protected BaseGenericEfIntKeyDataService(DbContext dbContext) : base(dbContext)
        {
        }

        public virtual Task<TEntity> GetByIdOrDefaultAsync(int id)
        {
            return EntityDbSet.FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
