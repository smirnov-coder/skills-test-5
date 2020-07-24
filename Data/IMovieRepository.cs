using SkillsTest.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillsTest.Data
{
    public interface IMovieRepository
    {
        int TotalCount { get; }

        void DeleteMovie(Movie movie);

        Task<Movie> GetMovieAsync(int movieId);

        Task<IList<Movie>> GetMoviesAsync(int limit = 0, int offset = 0);

        void SaveMovie(Movie movie);
    }
}