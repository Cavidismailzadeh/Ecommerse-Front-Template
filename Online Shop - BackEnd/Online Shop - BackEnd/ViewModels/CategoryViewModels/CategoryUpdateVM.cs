using System.ComponentModel.DataAnnotations;

namespace Online_Shop___BackEnd.ViewModels.CategoryViewModels
{
    public class CategoryUpdateVM
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
