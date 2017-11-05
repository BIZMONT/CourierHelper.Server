using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CourierHelper.BusinessLogic.Services;
using System.Configuration;
using CourierHelper.BusinessLogic.DTO;

namespace UnitTest.BusinessLogic
{
	[TestClass]
	public class TestCustomer
	{
		private CustomerService customerService = new CustomerService(ConfigurationManager.ConnectionStrings["CourierHelperDb"].ConnectionString);

		[TestMethod]
		public void CreateCustomer_CorrectData_CreateCustomer()
		{
			CustomerDto customer = new CustomerDto
			{
				FirstName = "Customer",
				LastName = "1",
				Email = "testemail",
				PhoneNumber = "testnumber"
			};

			Guid id = customerService.CreateCustomerAsync(customer).Result;
			customer = customerService.GetCustomerById(id);

			Assert.IsNotNull(customer);
		}
	}
}
