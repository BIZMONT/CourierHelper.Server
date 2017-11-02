using System;
using System.Data.Entity.Spatial;

namespace CourierHelper.DataAccess.Entities
{
    public class ActivePoint
    {
        public Guid Id { get; set; }

        internal DbGeography _Coordinates { get; set; }

        public Point Coordinates
        {
            get
            {
                return new Point(_Coordinates);
            }
            set
            {
                _Coordinates = value._Geography;
            }
        }

        #region Relations
        //Not required relations
        //But there must be at least one of these relations for each point

        public virtual Courier Courier { get; set; }

        public virtual Order Order { get; set; }

		public Guid RouteId { get; set; }
        public virtual Route Route { get; set; }

        public virtual Warehouse Warehouse { get; set; }
        #endregion
    }

    public class Point
    {
        internal DbGeography _Geography;

        internal Point(DbGeography geography)
        {
            _Geography = geography;
        }

        public Point(double longitude, double latitude)
        {
            _Geography = DbGeography.FromText($"POINT({longitude} {latitude})");
        }

        public double Latitude { get { return _Geography.Latitude.Value; } }

        public double Longitude { get { return _Geography.Longitude.Value; } }

        public double Distance(Point point)
        {
            return _Geography.Distance(point._Geography).Value;
        }
    }
}
