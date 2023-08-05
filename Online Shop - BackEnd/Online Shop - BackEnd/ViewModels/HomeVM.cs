
using Online_Shop___BackEnd.Models;
using System.Collections.Generic;

namespace Online_Shop___BackEnd.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Banner> Banners { get; set; }
        public IEnumerable <HomeCategory> HomeCategories { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
