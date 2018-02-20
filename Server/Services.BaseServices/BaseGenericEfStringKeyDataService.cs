using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Core;
using Domain.Core.Base;
using Microsoft.EntityFrameworkCore;
using Servises.Interfaces.Base;
using Utils;

namespace Services.BaseServices
{
    public abstract class BaseGenericEfStringKeyDataService<TEntity> : BaseGenericEfDataService<TEntity>, IBaseGenericDataService<string, TEntity> where TEntity : class, IBaseEntity<string>
    {
        protected BaseGenericEfStringKeyDataService(DbContext dbContext) : base(dbContext)
        {
        }

        public virtual Task<TEntity> GetByIdOrDefaultAsync(string id)
        {
            return EntityDbSet.FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
