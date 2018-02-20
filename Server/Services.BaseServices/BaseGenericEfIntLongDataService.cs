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
    public abstract class BaseGenericEfLongKeyDataService<TEntity> : BaseGenericEfDataService<TEntity>, IBaseGenericDataService<long, TEntity> where TEntity : class, IBaseEntity<long>
    {
        protected BaseGenericEfLongKeyDataService(DbContext dbContext) : base(dbContext)
        {
        }

        public virtual Task<TEntity> GetByIdOrDefaultAsync(long id)
        {
            return EntityDbSet.FirstOrDefaultAsync(e => e.Id == id);
        }

    }
}
