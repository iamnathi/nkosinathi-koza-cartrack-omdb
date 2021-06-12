using Cartrack.OMDb.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cartrack.OMDb.Repositories
{
    public interface IMovieRespository
    {
        Task<IEnumerable<Movie>> GetAllMoviesAsync();
        Task SaveOrUpdateAsync(Movie movie);
        Task DeleteAsync(string imdbId);
    }
}