using CourierHelper.BusinessLogic.DTO;
using CourierHelper.BusinessLogic.DTO.Enums;
using CourierHelper.BusinessLogic.Infrastructure;
using CourierHelper.BusinessLogic.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.BusinessLogic
{
	[TestClass]
	public class TestOrderManager
	{
		private CourierService courierService = new CourierService(ConfigurationManager.ConnectionStrings["CourierHelperDb"].ConnectionString);
		private WarehouseService warehouseService = new WarehouseService(ConfigurationManager.ConnectionStrings["CourierHelperDb"].ConnectionString);
		private CustomerService customerService = new CustomerService(ConfigurationManager.ConnectionStrings["CourierHelperDb"].ConnectionString);
		private OrderService orderService = new OrderService(ConfigurationManager.ConnectionStrings["CourierHelperDb"].ConnectionString);

		[TestMethod]
		public void AssignOrder_CorrectData_CourierHasAssignedTask()
		{
			var orderId = PrepareData();

			OrderManager manager = new OrderManager(ConfigurationManager.ConnectionStrings["CourierHelperDb"].ConnectionString, ConfigurationManager.AppSettings["Mapbox:AccessToken"]);
			manager.UpdateQueue();
			manager.ProceedNextOrderAsync().Wait();

			OrderDto order = orderService.GetOrderById(orderId);

			Assert.IsTrue(order.CourierId != null && order.CourierId != Guid.Empty);
		}

		private long PrepareData()
		{
			CourierDto[] couriers = new CourierDto[]
			{
				CourierFactory(new PointDto(49.83498756, 24.03488874)),
				CourierFactory(new PointDto(49.83609475, 24.02265787)),
				CourierFactory(new PointDto(49.84495134, 24.03587579)),
				CourierFactory(new PointDto(49.84401041, 23.98257493)),
				CourierFactory(new PointDto(49.82712580, 23.98296117)),
				CourierFactory(new PointDto(49.82014880, 23.98823976)),
				CourierFactory(new PointDto(49.82390000, 24.02079100)),
				CourierFactory(new PointDto(49.83226000, 24.01255100))
			};

			foreach (var courier in couriers)
			{
				courierService.AddCourierAsync(courier).Wait();
			}

			var warehouseId = warehouseService.AddWarehouseAsync(new WarehouseDto() { Location = new PointDto(49.83425819, 24.01567876), Name = "Warehouse for order" }).Result;

			var order = OrderFactory(new PointDto(49.830213, 24.030651), warehouseId);

			long orderId = orderService.AddOrderAsync(order).Result;
			return orderId;
		}

		private CourierDto CourierFactory(PointDto location)
		{
			return new CourierDto()
			{
				Location = location,
				FirstName = $"Courier",
				State = CourierStateDto.Idle,
				PhoneNumber = "testnumber"
			};
		}

		private OrderDto OrderFactory(PointDto destination, int warehouseId)
		{
			return new OrderDto()
			{
				WarehouseId = warehouseId,
				Destination = destination,
				State = OrderStateDto.NotAssigned,
				Receiver = new CustomerDto { FirstName="Receiver", PhoneNumber="testnumber"},
				Sender = new CustomerDto { FirstName = "Sender", PhoneNumber = "testnumber" }
			};
		}
	}
}
