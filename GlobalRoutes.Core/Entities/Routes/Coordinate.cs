using GlobalRoutes.SharedKernel.Entities;

namespace GlobalRoutes.Core.Entities.Routes
{
    public class Coordinate : BaseEntity
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int? RouteId { get; set; }

        public virtual Route? Route { get; set; }
    }
}
