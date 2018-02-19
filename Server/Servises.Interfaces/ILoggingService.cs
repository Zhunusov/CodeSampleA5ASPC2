using System.Linq;
using System.Threading.Tasks;
using Domain.Core;
using Domain.Logging;

namespace Servises.Interfaces
{
    /// <summary>
    /// Service for logging events.
    /// </summary>
    public interface ILoggingService
    {
        /// <summary>
        /// Returns an IQueryable of server events.
        /// </summary>
        IQueryable<ServerEvent> Events { get; }

        /// <summary>
        /// Add server event to the backing store.
        /// </summary>
        /// <param name="serverEvent">The server event to add.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the Domain.Core.ServiceResult of the operation.</returns>
        Task<ServiceResult> ServerLog(ServerEvent serverEvent);
    }
}