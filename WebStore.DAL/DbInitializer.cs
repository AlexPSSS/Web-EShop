using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Identity;

namespace WebStore.DAL
{
    public static class DbInitializer
    {
        public static async Task InitializeIdentityAsync(IServiceProvider services)
        {
            var _RoleManager = services.GetService<RoleManager<Role>>();
            var _UserManager = services.GetService<UserManager<User>>();

            if (!await _RoleManager.RoleExistsAsync(Role.Administrator))
                await _RoleManager.CreateAsync(new Role { Name = Role.Administrator });

            if (!await _RoleManager.RoleExistsAsync(Role.User))
                await _RoleManager.CreateAsync(new Role { Name = Role.User });

            if (await _UserManager.FindByNameAsync(User.Administrator) is null)
            {
                var admin = new User
                {
                    UserName = User.Administrator,
                    //Email = "admin@server.com"
                };

                var creation_result = await _UserManager.CreateAsync(admin, User.AdminPasswordDefault);

                if (creation_result.Succeeded)
                    await _UserManager.AddToRoleAsync(admin, Role.Administrator);
                else
                    throw new InvalidOperationException($"Ошибка при создании администратора в БД {string.Join(", ", creation_result.Errors.Select(e => e.Description))}");
            }
        }
        
        public static async Task InitializeBaseAsync(WebStoreContext context)
        {

            //context.Database.EnsureCreated();
            //await context.Database.EnsureCreatedAsync();
            //await context.Database.MigrateAsync(); // Автоматическое создание и миграция базы до последней версии

            //IEnumerable<PropertyInfo> dbSetProperties =
            //    context.GetType()
            //        .GetProperties()
            //        .Where(p => p.PropertyType.Name.StartsWith("DbSet"));

            List<string> dbSetProperties = new List<string>() { "Brands", "Categories", "Products" };

            foreach (string tableName in dbSetProperties)
            {
                switch (tableName)
                {
                    case "Products":
                        if (!context.Products.Any())
                        {
                            await FillTable<Product>(context, WebStore.Data.TestData.Products, tableName);
                        }
                        break;
                    case "Categories":
                        if (!context.Categories.Any())
                        {
                            await FillTable<Category>(context, WebStore.Data.TestData.Categories, tableName);
                        }
                        break;
                    case "Brands":
                        if (!context.Brands.Any())
                        {
                            await FillTable<Brand>(context, WebStore.Data.TestData.Brands, tableName);
                        }
                        break;
                }
            }
        }

        private static async Task FillTable<T>(WebStoreContext context, IEnumerable<T> entList, string tableName)
        {
            bool commitNeed = true;

            using (var trans = await context.Database.BeginTransactionAsync())
            {
                switch (tableName)
                {
                    case "Products":
                        context.Products.AddRange((IEnumerable<Product>)entList);
                        break;
                    case "Categories":
                        context.Categories.AddRange((IEnumerable<Category>)entList);
                        break;
                    case "Brands":
                        context.Brands.AddRange((IEnumerable<Brand>)entList);
                        break;
                    default:
                        commitNeed = false;
                        break;
                }

                if (commitNeed)
                {
                    await context.Database.ExecuteSqlRawAsync(String.Format("SET IDENTITY_INSERT [dbo].[{0}] ON", tableName));
                    await context.SaveChangesAsync();
                    await context.Database.ExecuteSqlRawAsync(String.Format("SET IDENTITY_INSERT [dbo].[{0}] OFF", tableName));
                    trans.Commit();
                }
            }
        }
    }
}
