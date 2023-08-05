using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Shop___BackEnd.Models
{
    public class Banner : BaseEntity
    {
        public string Title { get; set; }
        public string Image { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Can't be empty!")]
        public IFormFile Photo { get; set; }
    }
}
