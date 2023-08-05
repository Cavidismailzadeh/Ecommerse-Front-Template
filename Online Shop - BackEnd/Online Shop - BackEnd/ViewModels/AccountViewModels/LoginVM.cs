using Online_Shop___BackEnd.Models;
using System.ComponentModel.DataAnnotations;

namespace Online_Shop___BackEnd.ViewModels.AccountViewModels
{
    public class LoginVM
    {
        [Required]
        public string UsernameOrEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
