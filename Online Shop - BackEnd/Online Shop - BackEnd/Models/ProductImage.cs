﻿namespace Online_Shop___BackEnd.Models
{
    public class ProductImage : BaseEntity
    {
        public string Name { get; set; }
        public bool IsMain { get; set; } = true;
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
