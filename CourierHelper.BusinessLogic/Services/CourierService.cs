using AutoMapper;
using CourierHelper.BusinessLogic.Abstract;
using CourierHelper.BusinessLogic.DTO;
using CourierHelper.DataAccess;
using CourierHelper.DataAccess.Entities;
using CourierHelper.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierHelper.BusinessLogic.Services
{
	public class CourierService : ServiceBase
	{
		public string _connectionString;

		public CourierService(string connectionString)
		{
			_connectionString = connectionString;
		}

		public List<CourierDto> GetNearestCouriers(PointDto pointDto, int count = 5)
		{
			using (var db = new CourierHelperDb(_connectionString))
			{
				var nearestPoints = db.ActivePointsRepo.Query
					.Where(point => point.Courier != null || point.Route != null)
					.OrderBy(point => point.Coordinates.Distance(new Point(pointDto.Longitude, pointDto.Latitude)))
					.ToList();

				var couriers = nearestPoints
					.Select(point =>
					{
						if (point.Courier != null)
						{
							return point.Courier;
						}
						else
						{
							return point.Route.Courier;
						}
					})
					.Where(courier => courier.State != CourierState.NotAccessible)
					.Distinct()
					.OrderBy(courier=>courier.Orders.Count())
					.ThenBy(courier=>courier.State)
					.Take(count)
					.ToList();

				var couriersDto = Mapper.Map<List<CourierDto>>(couriers);   //todo: automapper config

				return couriersDto;
			}
		}

		public PointDto GetCourierLocation(Guid courierId)
		{
			using (var db = new CourierHelperDb(_connectionString))
			{
				var courier = db.CouriersRepo.Query.FirstOrDefault(c => c.Id == courierId);

				if (courier == null)
				{
					throw new ArgumentOutOfRangeException(); // todo: exception
				}

				var location = courier.Location;

				if(location == null)
				{
					return null;
				}

				var point = Mapper.Map<PointDto>(location);  //todo: automapper config

				return point;
			}
		}

		public async Task AssignOrder(Guid courierId, long orderId)
		{
			using (var db = new CourierHelperDb(_connectionString))
			{
				Courier courier = db.CouriersRepo.Query.FirstOrDefault(c => c.Id == courierId);
				Order order = db.OrdersRepo.Query.FirstOrDefault(o => o.Id == orderId);

				if(courier == null || order == null)
				{
					throw new ArgumentOutOfRangeException(""); //todo: exception
				}

				courier.Orders.Add(order);

				db.CouriersRepo.Update(courier);

				await db.SaveAsync();
			}
		}

		public RouteDto GetCourierCurrentRoute(Guid id)
		{
			throw new NotImplementedException();
		}
	}
}
