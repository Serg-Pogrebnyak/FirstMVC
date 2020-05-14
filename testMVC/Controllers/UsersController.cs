using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CustomIdentityApp.Models;
using CustomIdentityApp.ViewModels;
using System.Collections.Generic;
using testMVC.ViewModels;
using System;

namespace testMVC.Controllers
{
    public class UsersController : Controller
    {
        UserManager<User> _userManager;
        RoleManager<IdentityRole> _roleManager;
        SignInManager<User> _signInManager;

        public UsersController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
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

        [HttpPost]
        public async Task<IActionResult> ChangeUserRole(string role, string userEmail, bool addOrRemove)//add - true, remove - false
        {
            User user = await _userManager.FindByEmailAsync(userEmail);
            string[] arrayOfRole = new string[] { role };
            if (addOrRemove)
            {
                await _userManager.AddToRolesAsync(user, arrayOfRole);
            } else
            {
                await _userManager.RemoveFromRolesAsync(user, arrayOfRole);
            }
            
            await _signInManager.RefreshSignInAsync(user);
            return RedirectToAction("Index");
        }
    }
}
