// AccountController.cs
using BookStore.Model;
using BookStore.Repository;
using BookStore.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IEmailRepository _emailRepository;
        private readonly IUserService _userService;
        private object responce;

        public AccountController(IAccountRepository accountRepository, IEmailRepository emailRepository,
            IUserService userService)
        {
            _accountRepository = accountRepository;
            _emailRepository = emailRepository;
            _userService = userService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpModel signUpModel)
        {
            var result = await _accountRepository.SignUpAsync(signUpModel);

            if (!result.Succeeded)
            {
                return BadRequest(new { success = false, message = "SignUp failed.", data = result.Errors });
            }
            return Ok(new { success = true, message = "SignUp successfully." });
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInModel signInModel)
        {
            var token = await _accountRepository.SignInAsync(signInModel);

            if (token == null)
            {
                return Unauthorized(new { success = false, message = "SignIn failed." });
            }
            var uid = _userService.GetUserId();
            responce = new
            {
                success = true,
                message = "SignIn successfully.",
                result = new { uid, token }
            };
            return Ok(responce);
        }
        [HttpPost("sendemail")]
        public async Task<IActionResult> SendEmail(EmailMessage emailMessage)
        {
            //emailMessage = new EmailMessage
            //{
            //    ToEmails = new List<string>() { "testc@gmail.com" }
            //};
            await _emailRepository.SendEmailMessage(emailMessage);
            return Ok();
        }
        [HttpGet("confirm-email")]
        public async Task<IActionResult> SendConfirmEmail([FromQuery] string uid, [FromQuery] string token)
        {
            if (string.IsNullOrEmpty(uid) || string.IsNullOrEmpty(token))
            {
                return BadRequest();
            }
            token = token.Replace(" ", "+");
            var result = await _accountRepository.ConfirmEmail(uid, token);
            if (!result.Succeeded)
            {
                return Unauthorized();
            }
            return Ok("Thank you for varification");
        }
        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel changePassword)
        {
            var result = await _accountRepository.ChangePasswordAsync(changePassword);
            if (!result.Succeeded)
            {
                return Unauthorized();
            }
            return Ok(result);
        }

    }
}
