using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TestMVC.ViewModels;

namespace TestMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Index() => this.View(this.roleManager.Roles.ToList());

        [HttpGet]
        public IActionResult Create() => this.View();

        [HttpPost]
        public async Task<IActionResult> CreateAsync(RoleViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                IdentityResult result = await this.roleManager.CreateAsync(new IdentityRole(model.Name));
                if (result.Succeeded)
                {
                    return this.RedirectToAction("Index", "Role");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        this.ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string role)
        {
            IdentityRole roleForDelete = await this.roleManager.FindByNameAsync(role);
            await this.roleManager.DeleteAsync(roleForDelete);

            return this.RedirectToAction("Index");
        }
    }
}