using API.Application.DTOs;
using API.Application.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authenticationService;
        private readonly IMapper _mapper;

        public AuthController(IAuthService authenticationService, IMapper mapper)
        {
            _authenticationService = authenticationService;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid model state");

            var result = await _authenticationService.AuthenticateAsync(request);

            if (result == null || result.Data == null)
                return Unauthorized(result);

            var accessToken = result.Data.Token;
            Response.Cookies.Append("access_token", accessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(1)
            });

            var basicUser = _mapper.Map<BasicUserDTO>(result.Data);
            return Ok(new
            {
                result.IsSuccess,
                result.Message,
                Data = new
                {
                    user = basicUser
                }
            });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var accessToken = Request.Cookies["access_token"];
            if (string.IsNullOrEmpty(accessToken))
                return BadRequest(new { Message = "Access token is missing" });
            
            _authenticationService.RevokeRefreshTokenAsync(accessToken);

            Response.Cookies.Delete("access_token", new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                SameSite = SameSiteMode.Strict
            });

            return Ok();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid model state");

            var result = await _authenticationService.RegisterUserAsync(request);

            if (!result.IsSuccess)
                return BadRequest(new { result.Message });
                
            var token = result.Data.Token;
            Response.Cookies.Append("access_token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(1)
            });

            return Created(string.Empty, new
            {
                result.IsSuccess,
                result.Message,
                Data = new
                {
                    user = new
                    {
                        result.Data.Email,
                        result.Data.FirstName,
                        result.Data.LastName,
                        result.Data.Username
                    },
                }
            });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto refreshTokenRequest)
        {
            if (string.IsNullOrEmpty(refreshTokenRequest.RefreshToken))
            {
                return Unauthorized(new { Message = "Refresh token is required" });
            }

            var response = await _authenticationService.RefreshTokenAsync(refreshTokenRequest.RefreshToken);

            if (response.IsSuccess)
            {
                return Ok(response.Data);
            }

            return Unauthorized(new { Message = response.Message });
        }
    }
}
