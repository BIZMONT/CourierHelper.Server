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
	public class TestCourier
	{
		private CourierService courierService = new CourierService(ConfigurationManager.ConnectionStrings["CourierHelperDb"].ConnectionString);
		private RouteService routeService = new RouteService(ConfigurationManager.ConnectionStrings["CourierHelperDb"].ConnectionString);

		[TestMethod]
		public void AddCourierWithLocation()
		{
			CourierDto courier = new CourierDto()
			{
				FirstName = "Courier",
				LastName = "One",
				PhoneNumber = "+380984436465",
				Location = new PointDto() { Latitude = 23.02, Longitude = 54.03 }
			};

			Guid id = courierService.AddCourierAsync(courier).Result;

			Assert.IsTrue(id != Guid.Empty);
		}

		[TestMethod]
		public void AddCourierWithoutLocation()
		{
			CourierDto courier = new CourierDto()
			{
				FirstName = "Courier",
				LastName = "Two",
				PhoneNumber = "+380984436466"
			};

			Guid id = courierService.AddCourierAsync(courier).Result;

			Assert.IsTrue(id != Guid.Empty);
		}

		[TestMethod]
		public void ChangeCourierLocation()
		{
			CourierDto courier = courierService.GetAllCouriers().First(c => c.Location != null);
			PointDto newLocation = new PointDto() { Latitude = 27.34, Longitude = 43.15 };

			courierService.ChangeCourierLocationAsync(courier.Id, newLocation).Wait();

			CourierDto courierWithNewLocation = courierService.GetCourierById(courier.Id);

			Assert.IsTrue(courier.Location.Latitude != courierWithNewLocation.Location.Latitude);
		}

		[TestMethod]
		public void AddCurrentRoute()
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
		public void ChangeCurrentRoute()
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
