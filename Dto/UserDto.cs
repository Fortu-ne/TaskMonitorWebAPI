using System.ComponentModel.DataAnnotations;

namespace TaskMonitorWebAPI.Dto
{
    public class UserDto
    {
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        [Compare("PasswordHash", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string DateOfBirth { get; set; }
        public string? Address { get; set; }
    }
}
