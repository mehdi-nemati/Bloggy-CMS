using System.ComponentModel.DataAnnotations;

namespace Bloggy.Web.Areas.Admin.Models
{
    public class ChangePasswordDto
    {
        [Display(Name = "current-password")]
        [Required(ErrorMessage = "requierd")]
        [MinLength(6, ErrorMessage = "min-length")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$", ErrorMessage = "password-policy")]
        [Display(Name = "password")]
        [MaxLength(100, ErrorMessage = "max-length")]
        [Required(ErrorMessage = "requierd")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "confirm-password")]
        [Compare(nameof(Password), ErrorMessage = "equal-password")]
        [MaxLength(100, ErrorMessage = "max-length")]
        [Required(ErrorMessage = "requierd")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }

    public class UserDisplayNameDto
    {
        [Display(Name = "user-displasy-name")]
        [MaxLength(100, ErrorMessage = "max-length")]
        [Required(ErrorMessage = "requierd")]
        [MinLength(6,ErrorMessage = "min-length")]
        public string DisplayName { get; set; }
    }
}
