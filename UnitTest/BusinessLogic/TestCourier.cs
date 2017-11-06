using CourierHelper.BusinessLogic.DTO;
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
	public class TestCourier
	{
		private CourierService courierService = new CourierService(ConfigurationManager.ConnectionStrings["CourierHelperDb"].ConnectionString);

		[TestMethod]
		public void AddCourierWithLocation_CorrectData_NewCourierIdIsExists()
		{
			CourierDto courier = new CourierDto()
			{
				FirstName = "Courier",
				LastName = "One",
				PhoneNumber = "testphone",
				Location = new PointDto() { Latitude = 23.02, Longitude = 54.03 }
			};

			Guid id = courierService.AddCourierAsync(courier).Result;
			courier = courierService.GetCourierById(id);

			Assert.IsNotNull(courier);
		}

		[TestMethod]
		public void AddCourierWithoutLocation_CorrectData_NewCourierIdIsExists()
		{
			CourierDto courier = new CourierDto()
			{
				FirstName = "Courier",
				LastName = "Two",
				PhoneNumber = "testphone"
			};

			Guid id = courierService.AddCourierAsync(courier).Result;
			courier = courierService.GetCourierById(id);

			Assert.IsNotNull(courier);
		}

		[TestMethod]
		public void ChangeCourierLocation_CorrectData()
		{
			CourierDto courier = new CourierDto()
			{
				FirstName = "Courier",
				LastName = "change location",
				PhoneNumber = "testphone",
				Location = new PointDto() { Latitude = 23.02, Longitude = 54.03 }
			};

			Guid id = courierService.AddCourierAsync(courier).Result;

			PointDto newLocation = new PointDto() { Latitude = 27.34, Longitude = 43.15 };

			courierService.ChangeCourierLocationAsync(courier.Id, newLocation).Wait();

			CourierDto courierWithNewLocation = courierService.GetCourierById(courier.Id);

			Assert.IsTrue(courier.Location.Latitude != courierWithNewLocation.Location.Latitude &&
				courier.Location.Longitude != courierWithNewLocation.Location.Longitude);
		}

		[TestMethod]
		public void ChangeCourierState_CorrectData()
		{
			CourierDto courier = new CourierDto()
			{
				FirstName = "Courier",
				LastName = "change location",
				PhoneNumber = "testphone",
				Location = new PointDto() { Latitude = 23.02, Longitude = 54.03 }
			};

			Guid id = courierService.AddCourierAsync(courier).Result;

			courierService.ChangeCourierStateAsync(id, CourierStateDto.Idle).Wait();

			courier = courierService.GetCourierById(id);

			Assert.IsTrue(courier.State == CourierStateDto.Idle);
		}

		[TestMethod]
		public void GetDisabledCourier_CorrectData_ReturnsNull()
		{
			CourierDto courier = new CourierDto()
			{
				FirstName = "Courier",
				LastName = "Disabled",
				PhoneNumber = "testphone",
				Location = new PointDto() { Latitude = 23.02, Longitude = 54.03 }
			};

			Guid id = courierService.AddCourierAsync(courier).Result;

			courierService.DisableCourierAsync(id).Wait();

			courier = courierService.GetCourierById(id);

			Assert.IsNull(courier);
		}

		[TestMethod]
		public void GetNearestCouriers_CorrectData_ListOfCouriersIsNotEmpty()
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

			var result = courierService.GetNearestCouriers(new PointDto(49.830213, 24.030651), 3);

			Assert.IsTrue(result.Count ==3 && result.First().Location.Latitude == 49.83498756);
		}

		private CourierDto CourierFactory(PointDto location)
		{
			Random rand = new Random();
			return new CourierDto()
			{
				Location = location,
				FirstName = $"Courier",
				State = CourierStateDto.Idle,
				PhoneNumber = "testnumber"
			};
		}
	}
}
