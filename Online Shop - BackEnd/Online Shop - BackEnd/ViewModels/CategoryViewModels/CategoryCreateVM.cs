using System.ComponentModel.DataAnnotations;

namespace Online_Shop___BackEnd.ViewModels.CategoryViewModels
{
    public class CategoryCreateVM
    {
        [Required]
        public string Name { get; set; }
    }
}
