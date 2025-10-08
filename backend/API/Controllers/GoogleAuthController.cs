using API.Application.DTOs;
using API.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OAuthController : ControllerBase
    {
        private readonly IAuthService _authenticationService;

        public OAuthController(IAuthService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("google")]
        public async Task<IActionResult> GoogleAuthenticate([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid model state");

            var result = await _authenticationService.AuthenticateAsync(request);

            if (result == null || result.Data == null)
                return Unauthorized(new { Message = "Invalid Credentials" });

            return Ok(result);
        }
    }
}
