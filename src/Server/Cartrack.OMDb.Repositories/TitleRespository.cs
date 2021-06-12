using Cartrack.OMDb.Domain.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using System.Collections.Generic;

namespace Cartrack.OMDb.Repositories
{
    public class TitleRespository : ITitleRespository
    {
        private readonly string _connectionString;

        public TitleRespository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("OmdbConnectionString");
        }

        public async Task<IEnumerable<TitleModel>> GetAllMoviesAsync()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                return await connection.QueryAsync<TitleModel>("GetAllMovies", commandType: CommandType.StoredProcedure);
            }
        }

        public async Task SaveOrUpdateAsync(TitleModel movie)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.ExecuteAsync("SaveOrUpdateMovie", new 
                {
                    movie.IMDbID,
                    movie.Title,
                    movie.Type,
                    movie.Year,
                    movie.Rated,
                    movie.Released,
                    movie.Runtime,
                    movie.Genre,
                    movie.Director,
                    movie.Writer,
                    movie.Actors,
                    movie.Plot,
                    movie.Language,
                    movie.Country,
                    movie.Awards,
                    movie.Poster,
                    movie.Metascore,
                    movie.IMDbRating,
                    movie.IMDbVotes
                }, commandType: CommandType.StoredProcedure);

                if (movie.Ratings != null)
                {
                    foreach (var rating in movie.Ratings)
                    {
                        await connection.ExecuteAsync("SaveOrUpdateMovieRating", new
                        {
                            movie.IMDbID,
                            rating.Source,
                            rating.Value
                        }, commandType: CommandType.StoredProcedure);
                    }
                }                
            }
        }

        public async Task DeleteAsync(string imdbId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(
                    @$"DELETE FROM `omdb`.`movies`
                       WHERE IMDbID = '{imdbId}';", commandType: CommandType.Text);
            }
        }
    }
}
