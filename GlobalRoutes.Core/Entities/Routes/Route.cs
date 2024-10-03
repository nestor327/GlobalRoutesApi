using GlobalRoutes.Core.Entities.Languages;
using GlobalRoutes.SharedKernel.Entities;

namespace GlobalRoutes.Core.Entities.Routes
{
    public class Route : BaseEntity
    {
        public string OriginName { get; set; }
        public string DestinationName { get; set; }

        public virtual ICollection<Coordinate> Coordinates { get; set; }
    }
}
