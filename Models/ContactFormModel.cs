using System.ComponentModel.DataAnnotations;

namespace Bluetech.Models
{
    public class ContactFormModel
    {
        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Phone, StringLength(30)]
        public string? Phone { get; set; }

        [Required, StringLength(2000, MinimumLength = 10)]
        public string Message { get; set; } = string.Empty;

        // простая анти-бот проверка
        [Required]
        public string Check { get; set; } = string.Empty;
    }
}
