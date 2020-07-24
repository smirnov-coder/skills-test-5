using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SkillsTest.Models
{
    public class MovieBindingModel
    {
        [Required(ErrorMessage = "Поле обязательно для заполнения.")]
        [Range(0, int.MaxValue, ErrorMessage = "Значение поля не должно быть меньше 0.")]
        [HiddenInput]
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения.")]
        [MaxLength(100, ErrorMessage = "Длина поля не должна быть больше 100 символов.")]
        [DataType(DataType.Text)]
        [DisplayName("Название")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения.")]
        [MaxLength(500, ErrorMessage = "Длина поля не должна быть больше 500 символов.")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Описание")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения.")]
        [Range(1895, int.MaxValue, ErrorMessage = "Значение поля не должно быть меньше 1895.")]
        [DisplayName("Год выпуска")]
        public int Year { get; set; } = DateTime.Now.Year;

        [Required(ErrorMessage = "Поле обязательно для заполнения.")]
        [MaxLength(50, ErrorMessage = "Длина поля не должна быть больше 50 символов.")]
        [DataType(DataType.Text)]
        [DisplayName("Режиссёр")]
        public string Director { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения.")]
        [HiddenInput]
        public string CreatedBy { get; set; }

        [DataType(DataType.ImageUrl)]
        [HiddenInput]
        public string Poster { get; set; }

        [DisplayName("Постер")]
        public IFormFile Image { get; set; }
    }
}
