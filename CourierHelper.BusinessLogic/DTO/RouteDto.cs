using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierHelper.BusinessLogic.DTO
{
	public class RouteDto
	{
		public Guid Id { get; set; }

		public double Distance { get; set; }
		public bool IsCurrent { get; set; }

		public DateTime? Created { get; set; }
		public DateTime? Edited { get; set; }
		public DateTime? Completed { get; set; }

		public List<PointDto> Points { get; set; }
	}
}
