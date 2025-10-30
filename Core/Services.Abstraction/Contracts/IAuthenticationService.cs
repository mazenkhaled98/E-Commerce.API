using Shared.Dtos.IdentityModule;
using Shared.Dtos.OrderModule;

namespace Services.Abstraction.Contracts
{
    public interface IAuthenticationService
    {
        //login ==>userresultdto [displayname ,token,email] return from function ,take prameter login dto ==>[email,token,displayname]
        Task<UserResultDto> LoginAsync(LoginDto loginDto);


        //register ==>userresultdto [displayname ,token,email] return from function, take prameter register dto ==>[phonenumber,email,password,username,displayname]
        Task<UserResultDto> RegisterAsync(RegisterDto registerDto);

        //Get current user
        Task<UserResultDto> GetCurrentUserAsync(string userEmail);
        //Check if email exist
        Task<bool> CheckEmailExistAsync(string userEmail);
        //Get address
        Task<AddressDto> GetUserAddressAsync(string userEmail);
        //Update address
        Task<AddressDto> UpdateUserAddressAsync(string userEmail, AddressDto addressDto);

    }
}
