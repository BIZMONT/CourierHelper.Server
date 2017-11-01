using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
	}
}
