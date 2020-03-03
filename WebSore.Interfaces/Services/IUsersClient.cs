using Microsoft.AspNetCore.Identity;
using WebStore.Domain.Entities;

namespace WebSore.Interfaces.Services
{
    public interface IUsersClient :
        IUserRoleStore<User>,
        IUserPasswordStore<User>,
        IUserEmailStore<User>,
        IUserPhoneNumberStore<User>,
        IUserTwoFactorStore<User>,
        IUserLockoutStore<User>,
        IUserClaimStore<User>,
        IUserLoginStore<User>
    {
    }
}
