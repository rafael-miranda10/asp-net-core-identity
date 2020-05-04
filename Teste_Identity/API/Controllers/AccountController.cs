using Auth.EmailService.Interfaces;
using Auth.EmailService.Services;
using Auth.Models;
using Auth.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;

        public AccountController(IMapper mapper, UserManager<User> userManager, 
            SignInManager<User> signInManager, IEmailSender emailSender)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register(UserRegistrationViewModel userRegistration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = _mapper.Map<User>(userRegistration);

            var result = await _userManager.CreateAsync(user, userRegistration.Password);
            if (!result.Succeeded)
            {
                return BadRequest();
            }

            //para confirmação de email
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token, email = user.Email }, Request.Scheme);

            var message = new Message(new string[] { user.Email }, "Confirmation email link", confirmationLink, null);
            await _emailSender.SendEmailAsync(message);

            //adicionando roles
            await _userManager.AddToRoleAsync(user, "Visitor");
            return Ok();
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginViewModel userRegistration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _signInManager.PasswordSignInAsync(userRegistration.Email, userRegistration.Password, false, false);
            if (!result.Succeeded)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByEmailAsync(userRegistration.Email);

            //VER A PARTE DE TOKEN DO EDUARDO PIRES
            //var identity = new ClaimsIdentity(IdentityConstants.ApplicationScheme);
            //identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
            //identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));

            return Ok(user);
        }

        [Route("forgot-password")]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPasswordModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByEmailAsync(forgotPasswordModel.Email);
            if (user == null)
                return BadRequest();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callback = Url.Action(nameof(ResetPassword), "Account", new { token, email = user.Email }, Request.Scheme);

            var message = new Message(new string[] { user.Email }, "Reset password token", callback, null);
            await _emailSender.SendEmailAsync(message);

            return Ok();

        }

        [Route("reset-password")]
        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            //pensar melhor como fazer, aqui só um exemplo. talvez um redircionamento para um front... sei la
            var model = new ResetPasswordViewModel { Token = token, Email = email };
            return Ok(model);
        }

        [Route("confirm-registration")]
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest();

            var result = await _userManager.ConfirmEmailAsync(user, token);
            return Ok(result);
        }

        [Route("reset-password")]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user == null)
                return BadRequest();

            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password);
            if (!resetPassResult.Succeeded)
            {
                return BadRequest();
            }

            return Ok(resetPassResult);

        }
    }
}