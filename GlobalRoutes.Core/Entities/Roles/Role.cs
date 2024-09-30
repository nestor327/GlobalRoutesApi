using Microsoft.AspNetCore.Identity;

namespace GlobalRoutes.Core.Entities.Roles
{
    public class Role : IdentityRole<string>
    {
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
