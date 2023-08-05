using Microsoft.AspNetCore.Http;
using Online_Shop___BackEnd.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Online_Shop___BackEnd.ViewModels.ProductViewModels
{
    public class ProductUpdateVM
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public IEnumerable<ProductSubCategory> SizeName { get; set; }
        public string CategoryName { get; set; }
        public string BrandName { get; set; }
        public List<SubCategory> Size { get; set; }
        public ICollection<ProductImage> Images { get; set; }
        [Required]
        public List<IFormFile> Photos { get; set; }
    }
}
