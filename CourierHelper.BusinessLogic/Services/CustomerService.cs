using AutoMapper;
using CourierHelper.BusinessLogic.Abstract;
using CourierHelper.BusinessLogic.DTO;
using CourierHelper.DataAccess;
using CourierHelper.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierHelper.BusinessLogic.Services
{
	public class CustomerService : ServiceBase
	{
		private string _connectionString;

		public CustomerService(string connectionString)
		{
			_connectionString = connectionString;
		}

		public async Task CreateCustomerAsync(CustomerDto customerDto)
		{
			using (var db = new CourierHelperDb(_connectionString))
			{
				Customer customer = Mapper.Map<Customer>(customerDto);
				db.CustomersRepo.Create(customer);

				await db.SaveAsync();
			}
		}

		public async Task UpdateCustomerAsync(CustomerDto customerDto)
		{
			using (var db = new CourierHelperDb(_connectionString))
			{
				Customer customer = Mapper.Map<Customer>(customerDto);
				db.CustomersRepo.Update(customer);

				await db.SaveAsync();
			}
		}

		public async Task DeleteCustomerAsync(Guid customerId)
		{
			using (var db = new CourierHelperDb(_connectionString))
			{
				db.CustomersRepo.Delete(customerId);

				await db.SaveAsync();
			}
		}

		public CustomerDto GetCustomerById(Guid customerId)
		{
			using (var db = new CourierHelperDb(_connectionString))
			{
				Customer customer = db.CustomersRepo.Query.FirstOrDefault(c => c.Id == customerId);

				CustomerDto customerDto = Mapper.Map<CustomerDto>(customer);

				return customerDto;
			}
		}
	}
}
