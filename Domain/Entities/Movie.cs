using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillsTest.Domain.Entities
{
    /// <summary>
    /// Фильм.
    /// </summary>
    public class Movie
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Описание.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Год выхода в прокат или на DVD.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Режиссёр.
        /// </summary>
        public string  Director { get; set; }

        /// <summary>
        /// Идентификационное имя пользователя, загрузившего информацию и фильме.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Путь к файлу изображения постера фильма.
        /// </summary>
        public string Poster { get; set; }
    }
}
