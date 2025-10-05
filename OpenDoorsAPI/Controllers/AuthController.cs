using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OpenDoorsAPI.Models;
using OpenDoorsAPI.Services;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OpenDoorsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly MemberService _memberService;
        private readonly string _jwtSecret;

        public AuthController(MemberService memberService, IConfiguration config)
        {
            _memberService = memberService;
            _jwtSecret = config["Jwt:Secret"];
            if (string.IsNullOrEmpty(_jwtSecret))
                throw new ArgumentNullException("JWT secret is missing!");
        }

        // ✅ Đăng nhập
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Email và mật khẩu là bắt buộc");

            var member = await _memberService.GetByEmailAsync(request.Email);
            if (member == null || !_memberService.VerifyPassword(request.Password, member.Password))
                return Unauthorized("Email hoặc mật khẩu không đúng");

            // ✅ Tạo JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, member.Id),
                new Claim(ClaimTypes.NameIdentifier, member.Id),
                new Claim(ClaimTypes.Name, member.Name ?? ""),
                new Claim(ClaimTypes.Email, member.Email ?? ""),
                new Claim(ClaimTypes.Role, member.Role ?? "member")
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            return Ok(new
            {
                token = jwtToken,
                id = member.Id,
                name = member.Name,
                email = member.Email,
                role = member.Role
            });
        }

        // ✅ Lấy thông tin user hiện tại từ JWT
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Token không hợp lệ hoặc đã hết hạn");

            var member = await _memberService.GetByIdAsync(userId);
            if (member == null)
                return NotFound("Không tìm thấy người dùng");

            return Ok(new
            {
                member.Id,
                member.Name,
                member.Email,
                member.Role
            });
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
