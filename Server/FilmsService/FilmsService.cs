using Database.EntityFrameworkCore;
using Services.BaseServices;
using Servises.Interfaces;
using Domain;

namespace FilmsService
{
    public class FilmsService : BaseBaseGenericDataService<Film>, IFilmsService
    {
        public FilmsService(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
