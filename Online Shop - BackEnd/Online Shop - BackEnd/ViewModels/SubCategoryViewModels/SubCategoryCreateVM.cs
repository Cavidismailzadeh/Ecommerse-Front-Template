using System.ComponentModel.DataAnnotations;

namespace Online_Shop___BackEnd.ViewModels.CategoryViewModels
{
    public class SubCategoryCreateVM
    {
        [Required]
        public string SubCategory { get; set; }
        public int CategoryId { get; set; }
    }
}
