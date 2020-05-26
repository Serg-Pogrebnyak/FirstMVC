using Microsoft.AspNetCore.Identity;

namespace TestMVC.Models
{
    public class User : IdentityUser
    {
        private bool emailConfirmed;

        public string Name { get; set; }

        public string SurName { get; set; }

        public bool EmailConfirmed { get => this.emailConfirmed; set => this.emailConfirmed = value; }
    }
}