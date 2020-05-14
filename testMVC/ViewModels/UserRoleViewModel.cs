using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace testMVC.ViewModels
{
    public class UserRoleViewModel
    {
        public string Name { get; set; }
        public string SurName { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Email { get; set; }

        public IList<string> UserRoles { get; set; }
}
}
