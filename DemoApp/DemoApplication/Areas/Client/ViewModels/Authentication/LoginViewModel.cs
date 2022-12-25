using System.ComponentModel.DataAnnotations;

namespace DemoApplication.Areas.Client.ViewModels.Authentication
{
    public class LoginViewModel
    {

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
        public LoginViewModel()
        {

        }
        public LoginViewModel(string? email, string? password)
        {
            Email = email;
            Password = password;
        }
    }
}
