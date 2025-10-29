using Domain.Entites.IdentityModule;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Abstraction.Contracts;
using Shared.Common;
using Shared.Dtos.IdentityModule;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Net.WebRequestMethods;

namespace Services.Implementations
{
    internal class AuthenticationService(UserManager<User> _userManager ,IOptions<JwtOptions> _options) : IAuthenticationService
    {
       
        public async Task<UserResultDto> LoginAsync(LoginDto loginDto)
        {
         var user =await  _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                throw new UnauthorizedException();
            }
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isPasswordValid)
            {
                throw new UnauthorizedException();
            }
            return new UserResultDto(user.DisplayName, await CreateTokenAsync(user), user.Email);
          

        }

        public async Task<UserResultDto> RegisterAsync(RegisterDto registerDto)
        {
            var user = new User
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                PhoneNumber = registerDto.PhoneNumber
            };
        var result= await _userManager.CreateAsync(user, registerDto.Password);
            //validation
            if(!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                throw new ValidationException(errors);
            }

            return new UserResultDto(user.DisplayName, await CreateTokenAsync(user), user.Email);
        }

        //TOKEN ==> encrypted string ==> JWT
        //helper method
        private async Task<string> CreateTokenAsync(User user)
        {
           var JwtOptions  = _options.Value;
            //claims
            //name,email ,roles
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.DisplayName),
                new Claim(ClaimTypes.Email,user.Email),
                
            };
            var roles = await _userManager.GetRolesAsync(user);
            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            //secret key
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.SecretKey));

            //algorithm [algorithm + key]
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //create token 
            var token =new JwtSecurityToken
                (
                issuer: JwtOptions.Issuer,
                audience:JwtOptions.Audience,
                claims:claims,
                expires:DateTime.UtcNow.AddDays(JwtOptions.ExpirationInDays),
                signingCredentials:signingCredentials
                );

            //write token

            return new JwtSecurityTokenHandler().WriteToken(token);


        }
    }
}
