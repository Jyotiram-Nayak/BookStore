using System.ComponentModel.DataAnnotations;
namespace BookStore.Model
{
    public class SignUpModel
    {
        [Required]
        public string FirstName{ get; set; }
        [Required]
        public string LastName{ get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
