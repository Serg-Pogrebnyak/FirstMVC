using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CustomIdentityApp.Models;
using CustomIdentityApp.ViewModels;
using System.Collections.Generic;
using testMVC.ViewModels;

namespace testMVC.Controllers
{
    public class UsersController : Controller
    {
        UserManager<User> _userManager;
        RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<UserRoleViewModel> userList = new List<UserRoleViewModel>();
            foreach (var user in _userManager.Users.ToList())
            {
                var userRoles = await _userManager.GetRolesAsync(user);
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
                AllRoles = _roleManager.Roles.ToList(),
                Users = userList
            };
            return View(changeUserRole);
        }
    }
}
