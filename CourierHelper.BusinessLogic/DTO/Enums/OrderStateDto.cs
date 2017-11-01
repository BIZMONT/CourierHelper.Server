using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierHelper.BusinessLogic.DTO.Enums
{
	public enum OrderStateDto
	{
		NotAssigned,
		Assignment,
		WaitingOnWarehouse,
		Fulfillment,
		Completed
	}
}
