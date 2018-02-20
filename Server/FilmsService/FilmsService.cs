using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.EntityFrameworkCore;
using Services.BaseServices;
using Servises.Interfaces;
using Domain;
using Domain.Core;
using Microsoft.EntityFrameworkCore;

namespace FilmsService
{
    public class FilmsService : BaseGenericEfIntKeyDataService<Film>, IFilmsService
    {
        public FilmsService(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public Task<List<Film>> SearchAsync(int? count, int? offset, string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                throw new Exception("The search string must not be empty");
            }

            var query = EntityDbSet
                .Where(f => f.Name.Contains(searchString) || f.Description.Contains(searchString) ||
                            f.Director.Contains(searchString))
                .Skip(offset.GetValueOrDefault());

            if (count == null)
            {
                return query.ToListAsync();
            }

            return query.Take(count.Value).ToListAsync();
        }

        public override async Task<ServiceResult> UpdateAsync(Film entity)
        {
            if (await EntityDbSet.AnyAsync(e => e.Name == entity.Name))
            {
                return new ServiceResult(false, $"Name {entity.Name} already taken");
            }
            return await base.UpdateAsync(entity);
        }

        public override async Task<ServiceResult> CreateAsync(Film entity)
        {
            if (await EntityDbSet.AnyAsync(e => e.Name == entity.Name))
            {
                return new ServiceResult(false, $"Name {entity.Name} already taken");
            }
            return await base.CreateAsync(entity);
        }
    }
}
