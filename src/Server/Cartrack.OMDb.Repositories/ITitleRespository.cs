using Cartrack.OMDb.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cartrack.OMDb.Repositories
{
    public interface ITitleRespository
    {
        Task<IEnumerable<TitleModel>> GetAllMoviesAsync();
        Task SaveOrUpdateAsync(TitleModel movie);
        Task DeleteAsync(string imdbId);
    }
}