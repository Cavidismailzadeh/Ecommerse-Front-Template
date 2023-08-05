using System.ComponentModel.DataAnnotations;

namespace Online_Shop___BackEnd.ViewModels.AccountViewModels
{
    public class ForgetPasswordVM
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
