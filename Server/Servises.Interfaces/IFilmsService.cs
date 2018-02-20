using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Domain.Core;
using Servises.Interfaces.Base;

namespace Servises.Interfaces
{
    public interface IFilmsService: IBaseGenericDataService<int, Film>
    {
        Task<List<Film>> SearchAsync(int? count, int? offset, string searchString);
    }
}