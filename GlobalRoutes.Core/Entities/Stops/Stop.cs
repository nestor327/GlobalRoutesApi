﻿using GlobalRoutes.Core.Entities.Routes;
using GlobalRoutes.Core.Entities.Schedules;
using GlobalRoutes.SharedKernel.Entities;

namespace GlobalRoutes.Core.Entities.Stops
{
    public class Stop : BaseEntity
    {
        public string Name { get; set; }
        public int? ScheduleId { get; set; }
        public int? CoordinateId { get; set; }
        public int TotalArrivalTime { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        
        public virtual Schedule? Schedules { get; set; }
        public virtual Coordinate? Coordinate { get; set; }
    }
}
