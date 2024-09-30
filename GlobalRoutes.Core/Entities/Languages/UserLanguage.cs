using GlobalRoutes.Core.Entities.Users;
using GlobalRoutes.SharedKernel.Entities;

namespace GlobalRoutes.Core.Entities.Languages
{
    public class UserLanguage : BaseEntity
    {
        public string? UserId { get; set; }
        public int? LanguageId { get; set; }

        public virtual User? User { get; set; }
        public virtual Language? Language { get; set; }
    }
}
