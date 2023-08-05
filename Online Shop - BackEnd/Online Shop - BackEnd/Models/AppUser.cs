using Microsoft.AspNetCore.Identity;

namespace Online_Shop___BackEnd.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
