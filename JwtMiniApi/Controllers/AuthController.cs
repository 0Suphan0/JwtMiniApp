using JwtMiniApi.Models;
using JwtMiniApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace JwtMiniApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtTokenService _jwt;

        public AuthController(JwtTokenService jwt)
        {
            _jwt = jwt;
        }

        // Demo user (şimdilik hardcoded)
        private const string DemoUser = "admin";
        private const string DemoPass = "1234";

        [HttpPost("login")]
        public ActionResult<AuthResponse> Login([FromBody] Models.LoginRequest request)
        {
            // Burada normalde DB/Identity kontrolü olur
            if (request.Username != DemoUser || request.Password != DemoPass)
                return Unauthorized(new { message = "Invalid username or password" });

            var (token, expiresAt) = _jwt.CreateToken(request.Username);
            return Ok(new AuthResponse(token, expiresAt));
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            // token doğrulandıysa buraya gelir
            var username = User.Identity?.Name;
            return Ok(new
            {
                username,
                claims = User.Claims.Select(c => new { c.Type, c.Value })
            });
        }
    }
}
