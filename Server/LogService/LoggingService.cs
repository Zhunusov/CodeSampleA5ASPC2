using System.Linq;
using System.Threading.Tasks;
using Database.EntityFrameworkCore;
using Domain.Core;
using Domain.Logging;
using Microsoft.EntityFrameworkCore;
using Servises.Interfaces;
using Utils;

namespace LogService
{
    public sealed class LoggingService : ILoggingService
    {
        private readonly ApplicationDbContext _dbContext;

        private readonly DbSet<ServerEvent> _events;

        public IQueryable<ServerEvent> Events { get; }

        public LoggingService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _events = _dbContext.ServerLog;
            Events = _events;
        }

        public async Task<ServiceResult> ServerLog(ServerEvent serverEvent)
        {
            var errors = AttributeValidator.Validation(serverEvent);
            if (errors.Count > 0)
            {
                return new ServiceResult(false, errors);
            }

            await _events.AddAsync(serverEvent);
            await _dbContext.SaveChangesAsync();

            return new ServiceResult(true);
        }
    }
}
