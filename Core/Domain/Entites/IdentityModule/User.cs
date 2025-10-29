using Microsoft.AspNetCore.Identity;

namespace Domain.Entites.IdentityModule
{
    public class User :IdentityUser
    {

        public string DisplayName { get; set; } = string.Empty;

        public Address Address { get; set; }
    }
}
