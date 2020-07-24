using SkillsTest.Domain.Entities;
using System.Collections.Generic;

namespace SkillsTest.Models
{
    /// <summary>
    /// Данные каталога фильмов для отображения, разбитые на страницы.
    /// </summary>
    public class PaginatedMoviesViewModel
    {
        /// <summary>
        /// Информация разбиении на страницы.
        /// </summary>
        public PagingInfo Pagination { get; set; }

        /// <summary>
        /// Коллекция фильмов для отображения.
        /// </summary>
        public IList<Movie> Movies { get; set; }
    }
}
