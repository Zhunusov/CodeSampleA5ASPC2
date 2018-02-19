using Database.EntityFrameworkCore;
using Services.BaseServices;
using Servises.Interfaces;
using Domain;

namespace FilmsService
{
    public class FilmsService : BaseGenericDataService<Film>, IFilmsService
    {
        public FilmsService(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
