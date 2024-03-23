using BookStore.Model;
using BookStore.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(SignUpModel signUpModel)
        {
            var result = await _accountRepository.SignUpAsync(signUpModel);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return Unauthorized(result.Errors);
        }
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(SignInModel signInModel)
        {
            var result = await _accountRepository.SignInAsync(signInModel);
            if (result == null)
            {
                return Unauthorized();
            }
            return Ok(result);
        }
    }
}
