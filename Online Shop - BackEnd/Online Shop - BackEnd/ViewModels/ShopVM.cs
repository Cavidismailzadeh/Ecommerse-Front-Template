using Online_Shop___BackEnd.Models;
using System.Collections.Generic;

namespace Online_Shop___BackEnd.ViewModels
{
    public class ShopVM
    {
        public IEnumerable<PageHeader> PageHeaders { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public Product Product { get; set; }
        public SubCategory SubCategory { get; set; }
        public IEnumerable<ProductImage> ProductImages { get; set; }
        public IEnumerable<ProductSubCategory> ProductSubCategories { get; set; }
        public IEnumerable<SubCategory> SubCategories { get; set; }
    }
}
