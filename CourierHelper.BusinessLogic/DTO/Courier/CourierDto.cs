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

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string MiddleName { get; set; }

		public string Email { get; set; }
		public string PhoneNumber { get; set; }

		public CourierStateDto State { get; set; }

		public PointDto Location { get; set; }
	}
}
