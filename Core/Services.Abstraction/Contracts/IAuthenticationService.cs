using Shared.Dtos.IdentityModule;

namespace Services.Abstraction.Contracts
{
    public interface IAuthenticationService
    {
        //login ==>userresultdto [displayname ,token,email] return from function ,take prameter login dto ==>[email,token,displayname]
        Task<UserResultDto> LoginAsync(LoginDto loginDto);


        //register ==>userresultdto [displayname ,token,email] return from function, take prameter register dto ==>[phonenumber,email,password,username,displayname]
        Task<UserResultDto> RegisterAsync(RegisterDto registerDto);

    }
}
