using Domain;
using Servises.Interfaces.BaseServices;

namespace Servises.Interfaces
{
    /// <summary>
    /// Provides the APIs for managing films in a persistence store.
    /// </summary>
    public interface IFilmsService : IBaseGenericDataService<Film>
    {

    }
}