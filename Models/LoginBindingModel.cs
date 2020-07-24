using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SkillsTest.Models
{
    /// <summary>
    /// Учётные данные пользователя, используемый при входе на сайт.
    /// </summary>
    public class LoginBindingModel
    {
        /// <summary>
        /// Адрес электронной почты (и одновременно UserName).
        /// </summary>
        [Required(ErrorMessage = "Поле обязательно для заполнения.")]
        [DataType(DataType.EmailAddress)]
        [MaxLength(20, ErrorMessage = "Длина поля не должна быть больше 20 символов.")]
        [DisplayName("E-mail")]
        public string Email { get; set; }

        /// <summary>
        /// Пароль.
        /// </summary>
        [Required(ErrorMessage = "Поле обязательно для заполнения.")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Длина поля должна быть от 6 до 20 символов.")]
        [DisplayName("Пароль")]
        public string Password { get; set; }

        /// <summary>
        /// Флаг, показывающий необходимость долговременного хранения cookie в браузере после успешного входа на сайт.
        /// </summary>
        [DisplayName("Запомнить меня")]
        public bool RememberMe { get; set; }
    }
}
