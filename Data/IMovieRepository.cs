using SkillsTest.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillsTest.Data
{
    /// <summary>
    /// Репозиторий для хранения информации о фильмах.
    /// </summary>
    public interface IMovieRepository
    {
        /// <summary>
        /// Общее количество фильмов в репозитории.
        /// </summary>
        int TotalCount { get; }

        /// <summary>
        /// Удаляет информацию о фильме из репозитория.
        /// </summary>
        /// <param name="movie">Данные удаляемого фильма.</param>
        void DeleteMovie(Movie movie);

        /// <summary>
        /// Асинхронно извлекает из репозитория фильм с заданным значением идентификатора.
        /// </summary>
        /// <param name="movieId">Значение идентификатора фильма.</param>
        /// <returns>Данные фильма в виде объекта <see cref="Movie"/> или null, если фильм не найден.</returns>
        Task<Movie> GetMovieAsync(int movieId);

        /// <summary>
        /// Асинхронно извлекает из репозитория коллекцию фильмов.
        /// </summary>
        /// <param name="limit">Количество фильмов в выборке.</param>
        /// <param name="offset">
        /// Количество пропускаемых фильмов от начала общей коллекции фильмов в репозитории.
        /// </param>
        Task<IList<Movie>> GetMoviesAsync(int limit = 0, int offset = 0);

        /// <summary>
        /// Создаёт новый или обновляет существующий фильм в репозитории.
        /// </summary>
        /// <param name="movie">Данные фильма для создания/обновления.</param>
        void SaveMovie(Movie movie);
    }
}
