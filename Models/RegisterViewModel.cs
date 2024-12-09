using System.ComponentModel.DataAnnotations;

namespace courseA4.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Логин")]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Подтвердите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Электронная почта")]
        public string Email { get; set; }
    }
}
