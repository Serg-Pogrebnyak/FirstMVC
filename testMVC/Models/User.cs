using Microsoft.AspNetCore.Identity;

namespace CustomIdentityApp.Models
{
    public class User : IdentityUser
    {
        private bool emailConfirmed;

        public string Name { get; set; }
        public string SurName { get; set; }
        public bool EmailConfirmed { get => emailConfirmed; set => emailConfirmed = value; }
    }
}