using CourierHelper.BusinessLogic.DTO.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierHelper.BusinessLogic.DTO
{
	public class CourierDto
	{
		public Guid Id { get; set; }

		public CourierStateDto State { get; set; }
	}
}
