using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Shop___BackEnd.Models
{
    public class Favorite : BaseEntity
    {
        public string Image { get; set; }
        public string Brand { get; set; }
        public string Name { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }
    }
}
