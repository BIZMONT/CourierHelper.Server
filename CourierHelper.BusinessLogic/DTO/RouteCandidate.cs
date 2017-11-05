using System;
using System.Collections.Generic;
using System.Linq;

namespace CourierHelper.BusinessLogic.DTO
{
	public class RouteCandidate
	{
		public RouteCandidate(IEnumerable<PointDto> routePoints, double distance)
		{
			RoutePoints = routePoints;
			Distance = distance;
		}

		public IEnumerable<PointDto> RoutePoints { get; private set; }
		public CourierDto Courier { get; set; }
		public double Distance { get; private set; }

		public RouteDto GetRoute()
		{
			return new RouteDto()
			{
				Points = RoutePoints.ToList(),
				Distance = Distance,
				Created = DateTime.Now
			};
		}
	}
}
