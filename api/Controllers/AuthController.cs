using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HealthCalendar.DTOs;
using HealthCalendar.Models;
using HealthCalendar.Shared;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.VisualBasic;

namespace HealthCalendar.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager,
                              IConfiguration configuration, ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _logger = logger;
        }

        // method for registering User with Patient Role
        [HttpPost("registerPatient")]
        public async Task<IActionResult> RegisterPatient([FromBody] RegisterDTO registerDTO)
        {
            try
            {
                // Create User with Role set to Patient
                var patient = new User
                {
                    Name = registerDTO.Name,
                    Email = registerDTO.Email,
                    Role = Roles.Patient
                };

                // Attempts to create new user, automatically hashes passoword
                var result = await _userManager.CreateAsync(patient, registerDTO.Password);

                // if-statement in case registration fails
                if (!result.Succeeded)
                {
                    _logger.LogWarning("[AuthController] Warning from RegisterPatient(): \n" +
                                      $"Registration failed for Patient: {patient.Name}");
                    return BadRequest(result.Errors);
                }

                _logger.LogInformation("[AuthController] Information from RegisterPatient(): \n " +
                                      $"Registration succeeded for Patient: {patient.Name}");
                return Ok(new { Message = "Patient has been registered" });
            }
            catch (Exception e)
            {
                _logger.LogError("[AuthController] Error from RegisterPatient(): \n" +
                                 "Something went wrong when registrating Patient, " +
                                $"Error message: {e}");
                return StatusCode(500, "Internal server error");
            }
        }

        // Most code taken from Demo-React-9-JWTAuthentication-Backend.pdf written by Baifan
        // method for generating JWT token for user with Patient Role
        private string GenerateJwtToken(User user)
        {
            var jwtKey = _configuration["Jwt:Key"]; // Sectret key used for the signature
            if (string.IsNullOrEmpty(jwtKey))       // In case key is either null or empty
            {
                _logger.LogError("[AuthController] Error from GenerateJwtToken(): \n" +
                                 "JWT Key is missing from configuration.");
                throw new InvalidOperationException("JWT Key is missing from configuration.");
            }
            // Reads key from configuration
            var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            // Uses HMAC SHA256 algorithm to sign the token
            var credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
            
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),     // Token's subject
                new Claim(JwtRegisteredClaimNames.Email, user.Email!), // User's Email
                new Claim(ClaimTypes.NameIdentifier, user.Id),         // User's unique Id
                new Claim(ClaimTypes.Role, user.Role),                 // User's Role
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Token's unique Id
                new Claim(JwtRegisteredClaimNames.Iat,
                          DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())    // Timestamp token was Issued at
            };
            // Related Worker's Id (For Patients only)
            if (user.Role == Roles.Patient) claims.Append(new Claim("WorkerId", user.WorkerId!));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),  // Expiration time set to 1 hour
                signingCredentials: credentials);   // Token is signed with specified credentials

            _logger.LogInformation("[AuthController] Information from GenerateJwtToken(): \n " +
                                  $"JWT Token was generated for User: {user.Name}");
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}