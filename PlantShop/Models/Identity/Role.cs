using Microsoft.AspNetCore.Identity;

namespace PlantShop.Models.Identity
{
    public class Role:IdentityRole
    {
     public   RoleModel RoleModel { get; set; }
    }
}
