using Microsoft.AspNetCore.Identity;

namespace CustomIdentityApp.Models
{
    public class User : IdentityUser
    {
        public int Name { get; set; }
        public int Surname { get; set; }
        public int Email { get; set; }
    }
}