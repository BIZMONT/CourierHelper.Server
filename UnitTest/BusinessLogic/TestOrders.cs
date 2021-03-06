﻿using CourierHelper.BusinessLogic.DTO;
using CourierHelper.BusinessLogic.DTO.Enums;
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
	public class TestOrders
	{
		private OrderService orderService = new OrderService(ConfigurationManager.ConnectionStrings["CourierHelperDb"].ConnectionString);
		private WarehouseService warehouseService = new WarehouseService(ConfigurationManager.ConnectionStrings["CourierHelperDb"].ConnectionString);

		[TestMethod]
		public void AddOrder_AllNewData_NewOrderIsExistsInDb()
		{
			WarehouseDto warehouse = new WarehouseDto
			{
				Address = "somewhere",
				Name = "Warehouse 42",
				Location = new PointDto { Longitude = 23.42, Latitude = 42.23 }
			};

			var warehouseId = warehouseService.AddWarehouseAsync(warehouse).Result;

			OrderDto order = new OrderDto
			{
				WarehouseId = warehouseId,
				Destination = new PointDto { Longitude = 23.23, Latitude = 42.42 },
				Sender = new CustomerDto { FirstName = "Sender", LastName = "1", PhoneNumber = "testnumbers", Email = "testemail" },
				Receiver = new CustomerDto { FirstName = "Receiver", LastName = "1", PhoneNumber = "testnumbers", Email = "testemail" }
			};

			var orderId = orderService.AddOrderAsync(order).Result;
			order = orderService.GetOrderById(orderId);

			Assert.IsNotNull(order);
		}

		[TestMethod]
		public void GetUnassignedOrders_CorrectData_ReturnsOrderData()
		{
			WarehouseDto warehouse = new WarehouseDto
			{
				Address = "somewhere",
				Name = "Warehouse 43",
				Location = new PointDto { Longitude = 24.24, Latitude = 49.49 }
			};

			var warehouseId = warehouseService.AddWarehouseAsync(warehouse).Result;

			OrderDto order = new OrderDto
			{
				WarehouseId = warehouseId,
				State = OrderStateDto.NotAssigned,
				Destination = new PointDto { Longitude = 24.49, Latitude = 49.24 },
				Sender = new CustomerDto { FirstName = "Sender", LastName = "1", PhoneNumber = "testnumbers", Email = "testemail" },
				Receiver = new CustomerDto { FirstName = "Receiver", LastName = "1", PhoneNumber = "testnumbers", Email = "testemail" }
			};

			var orderId = orderService.AddOrderAsync(order).Result;
			order = orderService.GetUnassignedOrders().FirstOrDefault(o=>o.Id == orderId);

			Assert.IsNotNull(order);
		}
	}
}
