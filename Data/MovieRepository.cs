using Microsoft.EntityFrameworkCore;
using SkillsTest.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillsTest.Data
{
    public class MovieRepository : IMovieRepository
    {
        private MoviesDbContext _db;

        public MovieRepository(MoviesDbContext dbContext)
        {
            _db = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IList<Movie>> GetMoviesAsync(int limit = 0, int offset = 0)
        {
            var movies = _db.Movies.Skip(offset);
            movies = limit == 0 ? movies : movies.Take(limit);
            return await movies.ToListAsync();
        }

        public Task<Movie> GetMovieAsync(int movieId)
        {
            return _db.Movies.FirstOrDefaultAsync(movie => movie.Id == movieId);
        }

        public int TotalCount => _db.Movies.Count();

        public void SaveMovie(Movie movie)
        {
            _db.Movies.Update(movie);
            _db.SaveChanges();
        }

        public void DeleteMovie(Movie movie)
        {
            _db.Movies.Remove(movie);
            _db.SaveChanges();
        }
    }
}
