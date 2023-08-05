using Online_Shop___BackEnd.Models;
using System.Collections.Generic;

namespace Online_Shop___BackEnd.ViewModels
{
    public class AboutVM
    {
        public PageHeader PageHeader { get; set; }
        public IEnumerable<About> Abouts { get; set; }
        public IEnumerable<Brand> Brands { get; set; }
    }
}
