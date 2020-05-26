using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace TestMVC.ViewModels
{
    public class ChangeUserRoleViewModel
    {
        public List<IdentityRole> AllRoles { get; set; }

        public IList<UserRoleViewModel> Users { get; set; }
    }
}