using System.Collections.Generic;

namespace TestMVC.ViewModels
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