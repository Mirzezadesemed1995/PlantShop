using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Intrinsics.Arm;

namespace PlantShop.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public double  OldPrice { get; set; }    
        public double  NewPrice { get; set; }
        public string? FilePath { get; set; }

        public string?  Description { get; set; }    
        public Discount Discount { get; set; }  
        public int CategoryId { get; set; }    
        public Category Categories { get; set; }   
        [NotMapped]
        public ICollection<Files>? Files { get; set; }

        public ICollection<UserBasket>? UserBaskets { get; set; }
    }
}
