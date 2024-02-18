using System.ComponentModel.DataAnnotations.Schema;

namespace PlantShop.Models
{
    public class HeaderInfo
    {
        
        public int Id { get; set; }

        
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
