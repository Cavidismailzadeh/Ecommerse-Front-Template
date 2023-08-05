using Online_Shop___BackEnd.Models;
using System.Collections.Generic;

namespace Online_Shop___BackEnd.ViewModels.BlogViewModels
{
    public class BlogDetailVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public ICollection<BlogImage> Images { get; set; }

    }
}
