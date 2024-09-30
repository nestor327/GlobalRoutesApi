using GlobalRoutes.SharedKernel.Entities;

namespace GlobalRoutes.Core.Entities.Stops
{
    public class Stop : BaseEntity
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
