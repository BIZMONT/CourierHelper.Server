using CourierHelper.BusinessLogic.DTO;
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
	public class TestRoute
	{
		private CourierService courierService = new CourierService(ConfigurationManager.ConnectionStrings["CourierHelperDb"].ConnectionString);
		private RouteService routeService = new RouteService(ConfigurationManager.ConnectionStrings["CourierHelperDb"].ConnectionString);

		[TestMethod]
		public void AddCurrentRoute_CorrectData_RouteCreated()
		{
			CourierDto courier = new CourierDto()
			{
				FirstName = "Courier",
				LastName = "With route",
				PhoneNumber = "test"
			};

			Guid id = courierService.AddCourierAsync(courier).Result;

			RouteDto route = new RouteDto()
			{
				Created = DateTime.Now,
				Points = new List<PointDto>
				{
					new PointDto{Latitude = 49.8333981, Longitude = 24.0125249},
					new PointDto{Latitude = 49.8306805, Longitude = 24.034673},
					new PointDto{Latitude = 49.8388715, Longitude = 24.0311097}
				}
			};

			Guid routeId = routeService.AddRouteAsync(id, route).Result;

			Assert.IsTrue(routeId != Guid.Empty);
		}

		[TestMethod]
		public void ChangeCurrentRoute_CorrectData_CurrentRouteChanged()
		{
			CourierDto courier = new CourierDto()
			{
				FirstName = "Courier",
				LastName = "With route 2",
				PhoneNumber = "test"
			};

			Guid id = courierService.AddCourierAsync(courier).Result;

			RouteDto route = new RouteDto()
			{
				Created = DateTime.Now,
				Points = new List<PointDto>
				{
					new PointDto{Latitude = 49.8333981, Longitude = 24.0125249},
					new PointDto{Latitude = 49.8306805, Longitude = 24.034673},
					new PointDto{Latitude = 49.8388715, Longitude = 24.0311097}
				}
			};

			Guid routeId = routeService.AddRouteAsync(id, route).Result;

			route.Points.Add(new PointDto { Latitude = 49.846458, Longitude = 24.0257161 });
			routeService.ChangeCourierCurrentRouteAsync(id, route).Wait();

			route = routeService.GetCourierCurrentRoute(id);

			Assert.IsTrue(route.Points.Count == 4 && route.Points.Last().Latitude == 49.846458);
		}
	}
}
