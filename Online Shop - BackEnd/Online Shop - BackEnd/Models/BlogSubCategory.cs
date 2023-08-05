namespace Online_Shop___BackEnd.Models
{
    public class BlogSubCategory : BaseEntity
    {
        public int BlogId { get; set; }
        public Blog Blog { get; set; }
        public int SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }
    }
}
