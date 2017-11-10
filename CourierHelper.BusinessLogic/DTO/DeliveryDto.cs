using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierHelper.BusinessLogic.DTO
{
	public class DeliveryDto
	{
		public RouteDto Route { get; set; }
		public List<OrderDto> Orders { get; set; }
	}
}
