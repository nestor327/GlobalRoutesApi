using GlobalRoutes.SharedKernel.Entities;

namespace GlobalRoutes.Core.Entities.TimeZones
{
    public class TimeZone : BaseEntity
    {
        public double Offset { get; set; }
        public string Text { get; set; }
    }
}
