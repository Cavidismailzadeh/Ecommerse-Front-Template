using System.Collections.Generic;

namespace Online_Shop___BackEnd.Models
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<SubCategory> SubCategory { get; set; }
    }
}
