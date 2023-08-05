using Online_Shop___BackEnd.Models;
using System.Collections.Generic;

namespace Online_Shop___BackEnd.ViewModels
{
    public class BlogVM
    {
        public IEnumerable<PageHeader> PageHeader { get; set; }
        public IEnumerable<Blog> Blogs { get; set; }
        public IEnumerable<SubCategory> SubCategories { get; set; }
        public IEnumerable<BlogSubCategory> BlogSubCategories { get; set; }
    }
}
