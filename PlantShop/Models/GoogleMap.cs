using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PlantShop.Models
{
    public class GoogleMap
    {

        public int Id { get; set; } 

        public string MapUrl { get; set; }
    }
    
}
