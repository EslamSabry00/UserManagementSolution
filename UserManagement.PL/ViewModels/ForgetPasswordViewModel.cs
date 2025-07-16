using System.ComponentModel.DataAnnotations;

namespace UserManagement.PL.ViewModels
{
    public class ForgetPasswordViewModel
    {
        [Required(ErrorMessage = "Email is Requred")]
        [EmailAddress(ErrorMessage = "Email is Invalid")]
        public string Email { get; set; }
    }
}
