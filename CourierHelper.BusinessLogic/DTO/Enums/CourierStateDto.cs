using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierHelper.BusinessLogic.DTO.Enums
{
	public enum CourierStateDto
	{
		NotAccessible,
		Idle,
		OnWarehouse,
		PerformsDelivery,
		AtDestinationPoint
	}
}
