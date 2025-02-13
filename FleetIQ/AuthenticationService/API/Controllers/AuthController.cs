using System.Threading.Tasks;
using AuthenticationService.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AuthenticationService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly EmailService _emailService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(EmailService emailService, ILogger<AuthController> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] string userEmail)
        {
            if (string.IsNullOrEmpty(userEmail))
            {
                return BadRequest("Email is required.");
            }

            // TODO: Add user registration logic here

            // Send a welcome email
            string subject = "Welcome to Authentication Service!";
            string body = "<h1>Thank you for registering!</h1><p>We are happy to have you onboard.</p>";

            var emailSent = await _emailService.SendEmailAsync(userEmail, subject, body);

            if (emailSent)
            {
                return Ok("User registered successfully. A confirmation email has been sent.");
            }
            else
            {
                _logger.LogWarning($"Failed to send registration email to {userEmail}");
                return StatusCode(500, "User registered but email sending failed.");
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] string userEmail)
        {
            if (string.IsNullOrEmpty(userEmail))
            {
                return BadRequest("Email is required.");
            }

            // TODO: Generate a password reset token

            string resetLink = "https://yourapp.com/reset-password?token=xyz123"; // Placeholder
            string subject = "Password Reset Request";
            string body = $"<p>Click <a href='{resetLink}'>here</a> to reset your password.</p>";

            var emailSent = await _emailService.SendEmailAsync(userEmail, subject, body);

            if (emailSent)
            {
                return Ok("Password reset link has been sent to your email.");
            }
            else
            {
                return StatusCode(500, "Failed to send password reset email.");
            }
        }
    }
}
