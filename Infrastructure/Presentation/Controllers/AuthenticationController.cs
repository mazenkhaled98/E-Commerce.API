using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contracts;
using Shared.Dtos.IdentityModule;

namespace Presentation.Controllers
{
    public class AuthenticationController(IServiceManager _serviceManager) :ApiController
    {
        //login
        [HttpPost("Login")]
        public async Task<ActionResult> LoginAsync([FromBody] LoginDto loginDto)
        {
            var userResultDto = await _serviceManager.authenticationService.LoginAsync(loginDto);
            return Ok(userResultDto);
        }

        //register
        [HttpPost("Register")]
        public async Task<ActionResult> RegisterAsync([FromBody] RegisterDto registerDto)
        {
            var userResultDto = await _serviceManager.authenticationService.RegisterAsync(registerDto);
            return Ok(userResultDto);
        }
    }
}
