using Microsoft.AspNetCore.Http;
using Online_Shop___BackEnd.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Online_Shop___BackEnd.ViewModels.BlogViewModels
{
    public class BlogUpdateVM
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public ICollection<BlogImage> Images { get; set; }
        [Required]
        public List<IFormFile> Photos { get; set; }
    }
}
