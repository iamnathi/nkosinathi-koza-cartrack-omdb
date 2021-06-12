using Cartrack.OMDb.Application.Caching;
using Cartrack.OMDb.Repositories;
using Cartrack.OMDb.Web.Models.Results.Models;
using Mapster;
using System.Threading.Tasks;

namespace Cartrack.OMDb.Web.Api.Bootstrap.Caching
{
    public class MovieCacheSeed
    {
        public static async Task EnsureCacheSeed(IMovieRespository movieRepository, ICacheProvider<MovieResult> cacheProvider)
        {
            var movies = await movieRepository.GetAllMoviesAsync();
            foreach (var movie in movies)
            {
                cacheProvider.AddOrUpdate(movie.IMDbID, movie.Adapt<MovieResult>());
            }
        }
    }
}