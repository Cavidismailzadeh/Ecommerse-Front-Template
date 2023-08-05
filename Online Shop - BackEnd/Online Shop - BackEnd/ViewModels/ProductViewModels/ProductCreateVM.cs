using Microsoft.AspNetCore.Http;
using Online_Shop___BackEnd.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Online_Shop___BackEnd.ViewModels.ProductViewModels
{
    public class ProductCreateVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public List<SubCategory> Size { get; set; }
        [Required]
        public List<IFormFile> Photos{ get; set; }
    }
}
