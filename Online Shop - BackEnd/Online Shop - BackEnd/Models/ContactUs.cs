using System.ComponentModel.DataAnnotations;

namespace Online_Shop___BackEnd.Models
{
    public class ContactUs : BaseEntity
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
