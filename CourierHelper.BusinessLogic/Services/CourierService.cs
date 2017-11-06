using AutoMapper;
using CourierHelper.BusinessLogic.Abstract;
using CourierHelper.BusinessLogic.DTO;
using CourierHelper.BusinessLogic.DTO.Enums;
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


		public async Task<Guid> AddCourierAsync(CourierDto courierDto)
		{
			if (string.IsNullOrWhiteSpace(courierDto.PhoneNumber))
			{
				throw new ArgumentException("Phone number is required"); //todo: better exception
			}

			using (var db = new CourierHelperDb(_connectionString))
			{
				Courier courier = new Courier
				{
					FirstName = courierDto.FirstName,
					LastName = courierDto.LastName,
					MiddleName = courierDto.MiddleName,
					Email = courierDto.Email,
					PhoneNumber = courierDto.PhoneNumber,
					State = (CourierState)courierDto.State
				};

				if (courierDto.Location != null)
				{
					ActivePoint location = new ActivePoint
					{
						Coordinates = new Point(courierDto.Location.Longitude, courierDto.Location.Latitude)
					};

					courier.Location = location;
				}

				db.CouriersRepo.Create(courier);
				await db.SaveAsync();

				courierDto.Id = courier.Id;
				return courier.Id;
			}
		}


		public async Task ChangeCourierLocationAsync(Guid courierId, PointDto newLocation)
		{
			using (var db = new CourierHelperDb(_connectionString))
			{
				Courier courier = db.CouriersRepo.Get(courierId);

				if(courier == null)
				{
					throw new ArgumentException($"Can`t find courier with id {courierId}");
				}

				courier.Location.Coordinates = new Point(newLocation.Longitude, newLocation.Latitude);

				db.CouriersRepo.Update(courier);

				await db.SaveAsync();
			}
		}

		public async Task ChangeCourierStateAsync(Guid courierId, CourierStateDto newState)
		{
			using (var db = new CourierHelperDb(_connectionString))
			{
				Courier courier = db.CouriersRepo.Get(courierId);

				if (courier == null)
				{
					throw new ArgumentException($"Can`t find courier with id {courierId}");
				}

				courier.State = (CourierState)newState;

				db.CouriersRepo.Update(courier);

				await db.SaveAsync();
			}
		}

		public async Task CompleteCurrentRoute(Guid courierId)
		{
			using (var db = new CourierHelperDb(_connectionString))
			{
				Courier courier = db.CouriersRepo.Get(courierId);

				if (courier == null)
				{
					throw new ArgumentException($"Can`t find courier with id {courierId}");
				}

				var route = courier.Routes.FirstOrDefault(r => r.IsCurrent);

				if(route == null)
				{
					throw new Exception(); //todo: exception
				}

				route.IsCurrent = false;
				route.Completed = DateTime.Now;

				db.RoutesRepo.Update(route);

				await db.SaveAsync();
			}
		}


		public CourierDto GetCourierById(Guid courierId)
		{
			using (var db = new CourierHelperDb(_connectionString))
			{
				Courier courier = db.CouriersRepo.Get(courierId);

				CourierDto courierDto = Mapper.Map<CourierDto>(courier);

				return courierDto;
			}
		}

		public List<CourierDto> GetAllCouriers()
		{
			using (var db = new CourierHelperDb(_connectionString))
			{
				List<Courier> couriers = db.CouriersRepo.GetAll().ToList();

				List<CourierDto> couriersDto = Mapper.Map<List<CourierDto>>(couriers);

				return couriersDto;
			}
		}


		public async Task DisableCourierAsync(Guid courierId)
		{
			using (var db = new CourierHelperDb(_connectionString))
			{
				db.CouriersRepo.Delete(courierId);
				await db.SaveAsync();
			}
		}


		public List<CourierDto> GetNearestCouriers(PointDto pointDto, int count = 5)
		{
			using (var db = new CourierHelperDb(_connectionString))
			{
				var nearestPoints = db.ActivePointsRepo.Query
					.Where(point => (point.Courier != null && point.Courier.Deleted != null) || point.Route != null)
					.AsEnumerable()
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
					.OrderBy(courier => courier.Orders.Count())
					.ThenBy(courier => courier.State)
					.Take(count)
					.ToList();

				var couriersDto = Mapper.Map<List<CourierDto>>(couriers);   //todo: automapper config

				return couriersDto;
			}
		}

		public async Task AssignOrder(Guid courierId, long orderId)
		{
			using (var db = new CourierHelperDb(_connectionString))
			{
				Courier courier = db.CouriersRepo.Get(courierId);
				Order order = db.OrdersRepo.Get(orderId);

				if (courier == null || order == null)
				{
					throw new ArgumentOutOfRangeException(""); //todo: exception
				}

				courier.Orders.Add(order);

				db.CouriersRepo.Update(courier);

				await db.SaveAsync();
			}
		}
	}
}
