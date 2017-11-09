using System;

namespace CourierHelper.BusinessLogic.DTO
{
	public class CustomerDto
	{
		public Guid Id { get; set; }

		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }

		public string PhoneNumber { get; set; }
		public string Email { get; set; }
		public string Address { get; set; }

		public DateTime? Created { get; set; }
		public DateTime? Edited { get; set; }
	}
}
