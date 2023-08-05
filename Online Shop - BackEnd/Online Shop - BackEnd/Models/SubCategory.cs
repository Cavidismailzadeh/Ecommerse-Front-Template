using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Shop___BackEnd.Models
{
    public class SubCategory : BaseEntity
    {
        public string Name { get; set; }
        [NotMapped]
        public bool IsSelected { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<ProductSubCategory> ProductSubCategories { get; set; }
    }
}
