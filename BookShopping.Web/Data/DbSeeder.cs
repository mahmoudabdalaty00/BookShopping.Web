using Microsoft.AspNetCore.Identity;
using BookShopping.Web.Constants;


namespace BookShopping.Web.Data
{
    public class DbSeeder
    {
        public static async Task SeedDefultData(IServiceProvider server)
        {
            var UserMgr = server.GetService<UserManager<IdentityUser>>();
            var RoleMgr = server.GetService<RoleManager<IdentityRole>>();

            //adding roles to db 
            await RoleMgr.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await RoleMgr.CreateAsync(new IdentityRole(Roles.User.ToString()));

            //create admin user

            var admin = new IdentityUser
            {
                UserName = "admin@gmail.com ",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
              };

            var userInDb = await UserMgr.FindByEmailAsync(admin.Email);

             
                await UserMgr.CreateAsync(admin,"Admin@123"); 
                await UserMgr.AddToRoleAsync(admin,Roles.Admin.ToString());
             




        }
    }
}
