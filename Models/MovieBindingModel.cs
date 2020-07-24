using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SkillsTest.Models
{
    /// <summary>
    /// Данные нового или обновляемого фильма.
    /// </summary>
    public class MovieBindingModel
    {
        /// <summary>
        /// Идентификатор фильма.
        /// </summary>
        [Required(ErrorMessage = "Поле обязательно для заполнения.")]
        [Range(0, int.MaxValue, ErrorMessage = "Значение поля не должно быть меньше 0.")]
        [HiddenInput]
        public int Id { get; set; }

        /// <summary>
        /// Название фильма.
        /// </summary>
        [Required(ErrorMessage = "Поле обязательно для заполнения.")]
        [MaxLength(100, ErrorMessage = "Длина поля не должна быть больше 100 символов.")]
        [DataType(DataType.Text)]
        [DisplayName("Название")]
        public string Title { get; set; }

        /// <summary>
        /// Описание фильма.
        /// </summary>
        [Required(ErrorMessage = "Поле обязательно для заполнения.")]
        [MaxLength(1000, ErrorMessage = "Длина поля не должна быть больше 1000 символов.")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Описание")]
        public string Description { get; set; }

        /// <summary>
        /// Год выхода в прокат или на DVD.
        /// </summary>
        [Required(ErrorMessage = "Поле обязательно для заполнения.")]
        [Range(1895, int.MaxValue, ErrorMessage = "Значение поля не должно быть меньше 1895.")]
        [DisplayName("Год выпуска")]
        public int Year { get; set; } = DateTime.Now.Year;

        /// <summary>
        /// Режиссёр.
        /// </summary>
        [Required(ErrorMessage = "Поле обязательно для заполнения.")]
        [MaxLength(50, ErrorMessage = "Длина поля не должна быть больше 50 символов.")]
        [DataType(DataType.Text)]
        [DisplayName("Режиссёр")]
        public string Director { get; set; }

        /// <summary>
        /// Идентификационное имя пользователя, загрузившего информацию о фильме.
        /// </summary>
        [Required(ErrorMessage = "Поле обязательно для заполнения.")]
        [HiddenInput]
        public string CreatedBy { get; set; }

        /// <summary>
        /// Пусть к файлу изображения постера фильма.
        /// </summary>
        [DataType(DataType.ImageUrl)]
        [HiddenInput]
        public string Poster { get; set; }

        /// <summary>
        /// Изображение постера фильма, загруженное пользователем.
        /// </summary>
        [DisplayName("Постер")]
        public IFormFile Image { get; set; }
    }
}
