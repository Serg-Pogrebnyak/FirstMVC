using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace testMVC.ViewModels
{
    public class ChangeUserRoleViewModel
    {
        public List<IdentityRole> AllRoles { get; set; }
        public IList<UserRoleViewModel> Users { get; set; }

    }
}
