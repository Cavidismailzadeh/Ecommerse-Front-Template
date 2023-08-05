using System.Collections.Generic;

namespace Online_Shop___BackEnd.Models
{
    public class Blog : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<BlogSubCategory> BlogSubCategories { get; set; }
        public ICollection<BlogImage> BlogImages { get; set; }
    }
}
