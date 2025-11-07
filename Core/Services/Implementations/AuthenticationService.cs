using AutoMapper;
using Domain.Entites.IdentityModule;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Abstraction.Contracts;
using Shared.Common;
using Shared.Dtos.IdentityModule;
using Shared.Dtos.OrderModule;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Net.WebRequestMethods;

namespace Services.Implementations
{
    public class AuthenticationService(UserManager<User> _userManager, IOptions<JwtOptions> _options,IMapper _mapper) : IAuthenticationService
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


        public async Task<bool> CheckEmailExistAsync(string userEmail)
        {
           
            var user = await _userManager.FindByEmailAsync(userEmail);
            return user != null;
        }

        public async Task<UserResultDto> GetCurrentUserAsync(string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail)
                 ?? throw new UserNotFoundException(userEmail);
            return new UserResultDto(user.DisplayName, await CreateTokenAsync(user), user.Email);
        }

        public async Task<AddressDto> GetUserAddressAsync(string userEmail)
        {
            //var user = await _userManager.FindByEmailAsync(userEmail);
            var user = await _userManager.Users.Include(user => user.Address)
                .FirstOrDefaultAsync(u => u.Email == userEmail) ??
                throw new UserNotFoundException(userEmail);
            return _mapper.Map<AddressDto>(user.Address);
        }
        public async Task<AddressDto> UpdateUserAddressAsync(string userEmail, AddressDto addressDto)
        {
            var user = await _userManager.Users.Include(user => user.Address)
                     .FirstOrDefaultAsync(u => u.Email == userEmail) ??
                         throw new UserNotFoundException(userEmail);

            if (user.Address != null) //Update
            {
                user.Address.FirstName = addressDto.FirstName;
                user.Address.LastName = addressDto.LastName;
                user.Address.Country = addressDto.Country;
                user.Address.City = addressDto.City;
                user.Address.Street = addressDto.Street;
            }
            else //Create
            {
                var address = _mapper.Map<Address>(addressDto);
                user.Address = address;
            }

            await _userManager.UpdateAsync(user);
            return  _mapper.Map<AddressDto>(user.Address);
        }
    }
}
