using CourierHelper.BusinessLogic.DTO.Enums;
using CourierHelper.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierHelper.BusinessLogic.DTO
{
	public class OrderDto : IEquatable<OrderDto>
	{
		public long Id { get; set; }

		public OrderStateDto State { get; set; }
		public string Address { get; set; }
		public string Content { get; set; }
		public string Comment { get; set; }

		public CustomerDto Sender { get; set; }
		public CustomerDto Receiver { get; set; }
		public int WarehouseId { get; set; }
		public Guid? CourierId { get; set; }
		public PointDto Destination { get; set; }

		public bool Equals(OrderDto other)
		{
			if(Id == default(long) || other.Id == default(long))
			{
				return false;
			}

			return Id == other.Id;
		}
	}
}
