using BookStore.Model;
using Microsoft.AspNetCore.Identity;

namespace BookStore.Repository
{
    public interface IAccountRepository
    {
        Task<IdentityResult> SignUpAsync(SignUpModel signUpModel);
        Task<string> SignInAsync(SignInModel signInModel);
        Task<IdentityResult> ConfirmEmail(string uid, string token);
        Task<IdentityResult> ChangePasswordAsync(ChangePasswordModel changePassword);
    }
}
