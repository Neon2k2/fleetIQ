using AuthenticationService.Data.Dtos;
using AuthenticationService.Models;
using AuthenticationService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly JwtTokenGenerator _tokenGenerator;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, JwtTokenGenerator tokenGenerator)
        {
            _tokenGenerator = tokenGenerator;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        //Register Endpoint
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto register)
        {
            Console.WriteLine("This point!");
            var user = new ApplicationUser
            {
                UserName = register.UserName,
                Email = register.Email,
                FullName = register.FullName
            };
            var result = await _userManager.CreateAsync(user, register.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok("user registered successfully");
        }

        //login endpoint
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, login.Password))
                return Unauthorized("Invalid credentials");

            var token = _tokenGenerator.GenerateJwtTokenAsync(user);
            return Ok(new
            {
                Token = token
            });

        }
    }
}
