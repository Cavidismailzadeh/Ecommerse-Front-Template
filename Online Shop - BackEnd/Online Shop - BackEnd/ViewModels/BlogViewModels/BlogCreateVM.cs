using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Online_Shop___BackEnd.ViewModels.BlogViewModels
{
    public class BlogCreateVM
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public List<IFormFile> Photos { get; set; }
    }
}
