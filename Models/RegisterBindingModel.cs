using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SkillsTest.Models
{
    public class RegisterBindingModel
    {
        [Required(ErrorMessage = "Поле обязательно для заполнения.")]
        [DataType(DataType.EmailAddress)]
        [MaxLength(20, ErrorMessage = "Длина поля не должна быть больше 20 символов.")]
        [DisplayName("E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения.")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Длина поля должна быть от 6 до 20 символов.")]
        [DisplayName("Пароль")]
        public string Password { get; set; }
    }
}
