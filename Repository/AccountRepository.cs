using BookStore.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookStore.Repository
{
    public class AccountRepository:IAccountRepository
    {
        private readonly UserManager<BookStoreUser> _userManager;
        private readonly SignInManager<BookStoreUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountRepository(UserManager<BookStoreUser> userManager, SignInManager<BookStoreUser> signInManager,IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
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
            return await _userManager.CreateAsync(user, signUpModel.Password);
        }
        public async Task<string> SignInAsync(SignInModel signInModel)
        {
            var result = await _signInManager.PasswordSignInAsync(signInModel.Email, signInModel.Password, false, false);
            if(!result.Succeeded)
            {
                return null;
            }
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,signInModel.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
            var authSigninKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires:DateTime.Now.AddDays(1),
                claims:authClaims,
                signingCredentials:new SigningCredentials(authSigninKey,SecurityAlgorithms.HmacSha256Signature)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        } 
    }
}
