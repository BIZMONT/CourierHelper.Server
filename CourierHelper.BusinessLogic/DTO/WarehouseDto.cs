using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierHelper.BusinessLogic.DTO
{
	public class WarehouseDto
	{
		public int Id { get; set; }

		public string Name { get; set; }
		public string Address { get; set; }

		public PointDto Location { get; set; }
	}
}
