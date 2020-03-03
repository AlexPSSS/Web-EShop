using Microsoft.AspNetCore.Identity;
using WebStore.Domain.Entities.Identity;

namespace WebSore.Interfaces.Services
{
    public interface IRolesClient : IRoleStore<Role> { }
}
