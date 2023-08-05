namespace Online_Shop___BackEnd.Models
{
    public class BlogImage : BaseEntity
    {
        public string Name { get; set; }
        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}
