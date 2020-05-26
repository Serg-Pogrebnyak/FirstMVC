using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TestMVC.Models;
using TestMVC.ViewModels;

namespace TestMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<User> signInManager;

        public UsersController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<UserRoleViewModel> userList = new List<UserRoleViewModel>();
            foreach (var user in this.userManager.Users.ToList())
            {
                var userRoles = await this.userManager.GetRolesAsync(user);
                UserRoleViewModel viewModel = new UserRoleViewModel
                {
                    UserRoles = userRoles,
                    Name = user.Name,
                    SurName = user.SurName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed
                };
                userList.Add(viewModel);
            }

            ChangeUserRoleViewModel changeUserRole = new ChangeUserRoleViewModel
            {
                AllRoles = this.roleManager.Roles.ToList(),
                Users = userList
            };
            return this.View(changeUserRole);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUserRole(string role, string userEmail, bool addOrRemove) // add - true, remove - false
        {
            User user = await this.userManager.FindByEmailAsync(userEmail);
            string[] arrayOfRole = new string[] { role };
            if (addOrRemove)
            {
                await this.userManager.AddToRolesAsync(user, arrayOfRole);
            }
            else
            {
                await this.userManager.RemoveFromRolesAsync(user, arrayOfRole);
            }

            await this.signInManager.RefreshSignInAsync(user);
            return this.RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userEmail) // add - true, remove - false
        {
            User user = await this.userManager.FindByEmailAsync(userEmail);
            await this.userManager.DeleteAsync(user);

            return this.RedirectToAction("Index");
        }
    }
}