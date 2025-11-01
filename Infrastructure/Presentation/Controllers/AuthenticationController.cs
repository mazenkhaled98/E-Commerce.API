using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contracts;
using Shared.Dtos.IdentityModule;
using Shared.Dtos.OrderModule;
using System.Security.Claims;

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

        [HttpGet("EmailExist")]
        public async Task<ActionResult<bool>> CheckEmailExistAsync(string email)
    => Ok(await _serviceManager.authenticationService.CheckEmailExistAsync(email));

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserResultDto>> GetCurrentUserAsync()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _serviceManager.authenticationService.GetCurrentUserAsync(email);
            return Ok(user);
        }

        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDto>> GetUserAddressAsync()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var address = await _serviceManager.authenticationService.GetUserAddressAsync(email);
            return Ok(address);
        }

        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddressAsync(AddressDto addressDto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var address = await _serviceManager.authenticationService.UpdateUserAddressAsync(email, addressDto);
            return Ok(address);
        }
    }
}
