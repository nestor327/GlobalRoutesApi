using GlobalRoutes.SharedKernel.Entities;

namespace GlobalRoutes.Core.Entities.Languages
{
    public class Language : BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<UserLanguage> UserLanguages { get; set; }
    }
}
