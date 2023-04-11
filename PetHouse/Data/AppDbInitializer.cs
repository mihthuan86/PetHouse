using Microsoft.AspNetCore.Identity;
using PetHouse.Data.Static;
using PetHouse.Models;

namespace PetHouse.Data
{
	public class AppDbInitializer
	{
		public static async Task SeedUsersAndRoleAsync(IApplicationBuilder applicationBuilder)
		{
			using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
			{

				//Roles
				var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

				if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
					await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
				if (!await roleManager.RoleExistsAsync(UserRoles.Staff))
					await roleManager.CreateAsync(new IdentityRole(UserRoles.Staff));
				if (!await roleManager.RoleExistsAsync(UserRoles.Customer))
					await roleManager.CreateAsync(new IdentityRole(UserRoles.Customer));

				//Users
				var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
				string adminUserEmail = "admin@gmail.com";

				var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
				if (adminUser == null)
				{
					var newAdminUser = new User()
					{
						FullName = "Admin User",
						UserName = "admin",
						Email = adminUserEmail,
						Address = "123, Ngô Quyền",
						Status = 1

					};
					await userManager.CreateAsync(newAdminUser, "Coding@1234?");
					await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
				}

				string appUserEmail = "customer@gmail.com";
				var appUser = await userManager.FindByEmailAsync(appUserEmail);
				if (appUser == null)
				{
					var newAppUser = new User()
					{
						FullName = "Customer User",
						UserName = "customer",
						Email = appUserEmail,
						Address = "123, Ngô Quyền",
						Status = 1

					};
					await userManager.CreateAsync(newAppUser, "Coding@1234?");
					await userManager.AddToRoleAsync(newAppUser, UserRoles.Customer);
				}
				string appStaffEmail = "staff@gmail.com";

				var appStaff = await userManager.FindByEmailAsync(appStaffEmail);
				if (appUser == null)
				{
					var newAppUser = new User()
					{
						FullName = "Staff User",
						UserName = "staff",
						Email = appUserEmail,
						Address = "123, Ngô Quyền",
						Status = 1

					};
					await userManager.CreateAsync(newAppUser, "Coding@1234?");
					await userManager.AddToRoleAsync(newAppUser, UserRoles.Staff);
				}
			}
		}
	}

}
