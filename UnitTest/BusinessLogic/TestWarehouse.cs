using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CourierHelper.BusinessLogic.Services;
using System.Configuration;
using CourierHelper.BusinessLogic.DTO;

namespace UnitTest.BusinessLogic
{
	[TestClass]
	public class TestWarehouse
	{
		private WarehouseService warehouseService = new WarehouseService(ConfigurationManager.ConnectionStrings["CourierHelperDb"].ConnectionString);

		[TestMethod]
		public void AddWarehouse()
		{
			WarehouseDto warehouse = new WarehouseDto()
			{
				Location = new PointDto() { Latitude = 25.02, Longitude = 42.03 },
				Name = "SomeWarehouse",
				Address = "Gorodotska 131"
			};

			int id = warehouseService.AddWarehouseAsync(warehouse).Result;

			WarehouseDto warehouseDto = warehouseService.GetById(id);

			Assert.IsNotNull(warehouseDto);
		}
	}
}
