// AccountRepository.cs
using BookStore.Model;
using BookStore.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<BookStoreUser> _userManager;
        private readonly SignInManager<BookStoreUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailRepository _emailRepository;
        private readonly IUserService _userService;

        public AccountRepository(UserManager<BookStoreUser> userManager, SignInManager<BookStoreUser> signInManager,
            IConfiguration configuration, IEmailRepository emailRepository,
            IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _emailRepository = emailRepository;
            _userService = userService;
        }

        public async Task<IdentityResult> SignUpAsync(SignUpModel signUpModel)
        {
            var user = new BookStoreUser()
            {
                FirstName = signUpModel.FirstName,
                LastName = signUpModel.LastName,
                Email = signUpModel.Email,
                UserName = signUpModel.Email,
            };

            var result = await _userManager.CreateAsync(user, signUpModel.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User"); // Assigning role as User upon signup
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            if (!string.IsNullOrEmpty(token))
            {
                await SendConfirmationEmail(user, token);
            }
            return result;
        }

        public async Task<string> SignInAsync(SignInModel signInModel)
        {
            var result = await _signInManager.PasswordSignInAsync(signInModel.Email, signInModel.Password, false, false);

            if (result.IsNotAllowed)
            {
                return "Not Allow";
            }
            else if (!result.Succeeded)
            {
                return null;
            }
            var user = await _userManager.FindByEmailAsync(signInModel.Email);
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, signInModel.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<IdentityResult> ConfirmEmail(string uid, string token)
        {
            return await _userManager.ConfirmEmailAsync(await _userManager.FindByIdAsync(uid), token);
        }
        public async Task<IdentityResult> ChangePasswordAsync(ChangePasswordModel changePassword)
        {
            var uid = _userService.GetUserId();
            var user = await _userManager.FindByIdAsync(uid);
            if (user == null)
            {
                return null;
            }
            return await _userManager.ChangePasswordAsync(user, changePassword.CurrentPassword, changePassword.NewPassword);
        }
        private async Task SendConfirmationEmail(BookStoreUser bookStoreUser, string token)
        {
            string appDomain = _configuration.GetSection("Application:AppDomain").Value;
            string confirmLink = _configuration.GetSection("Application:EmailConfirmation").Value;
            EmailMessage emailMessage = new EmailMessage
            {
                Subject = "ConfirmEmail",
                ToEmails = new List<string>() { bookStoreUser.Email },
                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}",bookStoreUser.FirstName),
                    new KeyValuePair<string, string>("{{Link}}",string.Format(appDomain+confirmLink,bookStoreUser.Id,token))
                }
            };
            await _emailRepository.SendEmailConfirmationMessage(emailMessage);
        }
    }
}
