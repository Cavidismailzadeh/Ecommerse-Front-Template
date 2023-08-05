using Online_Shop___BackEnd.Models;
using System.Collections.Generic;

namespace Online_Shop___BackEnd.ViewModels.ProductViewModels
{
    public class ProductDetailVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
        public IEnumerable<ProductSubCategory> Size { get; set; }
        public ICollection<ProductImage> Images { get; set; }
    }
}
