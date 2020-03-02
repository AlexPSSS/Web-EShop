using Microsoft.AspNetCore.Identity;

namespace WebStore.Domain.Entities
{
    public class User : IdentityUser
    {
        public const string Administrator = "Admin";
        public const string AdminPasswordDefault = "Admin@123";

        //public string Description { get; set; }
    }
}