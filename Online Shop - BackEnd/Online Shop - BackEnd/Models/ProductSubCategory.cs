﻿namespace Online_Shop___BackEnd.Models
{
    public class ProductSubCategory : BaseEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }
    }
}
