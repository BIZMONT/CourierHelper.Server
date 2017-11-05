using AutoMapper;
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
	public class RouteService
	{
		public string _connectionString;

		public RouteService(string connectionString)
		{
			_connectionString = connectionString;
		}

		public async Task<Guid> AddRouteAsync(Guid courierId, RouteDto newCurrentRoute)
		{
			using (var db = new CourierHelperDb(_connectionString))
			{
				Courier courier = db.CouriersRepo.Query.FirstOrDefault(c => c.Id == courierId);

				if (courier == null)
				{
					throw new ArgumentException(); //todo: exception
				}

				if (courier.Routes.Any(r => r.IsCurrent))
				{
					throw new Exception(); //todo: exception
				}

				DataAccess.Entities.Route route = new DataAccess.Entities.Route
				{
					Created = DateTime.Now,
					IsCurrent = true,
					Distance = newCurrentRoute.Distance
				};

				foreach (var point in newCurrentRoute.Points)
				{
					route.Points.Add(new ActivePoint() { Coordinates = new Point(point.Longitude, point.Latitude) });
				}

				courier.Routes.Add(route);

				await db.SaveAsync();

				return route.Id;
			}
		}


		public RouteDto GetCourierCurrentRoute(Guid courierId)
		{
			using (var db = new CourierHelperDb(_connectionString))
			{
				Courier courier = db.CouriersRepo.Query.FirstOrDefault(c => c.Id == courierId);

				if (courier == null)
				{
					throw new ArgumentOutOfRangeException(""); //todo: exception
				}

				DataAccess.Entities.Route currentRoute = courier.Routes.FirstOrDefault(route => route.IsCurrent);

				RouteDto routeDto = Mapper.Map<RouteDto>(currentRoute);

				return routeDto;
			}
		}

		public List<RouteDto> GetCourierAllRoutes(Guid courierId)
		{
			using (var db = new CourierHelperDb(_connectionString))
			{
				Courier courier = db.CouriersRepo.Query.FirstOrDefault(c => c.Id == courierId);

				if (courier == null)
				{
					throw new ArgumentOutOfRangeException(""); //todo: exception
				}

				List<DataAccess.Entities.Route> routes = courier.Routes.ToList();

				List<RouteDto> routesDto = Mapper.Map<List<RouteDto>>(routes);

				return routesDto;
			}
		}

		public async Task ChangeCourierCurrentRouteAsync(Guid courierId, RouteDto newCurrentRouteDto)
		{
			using (var db = new CourierHelperDb(_connectionString))
			{
				Courier courier = db.CouriersRepo.Query.FirstOrDefault(c => c.Id == courierId);

				if (courier == null)
				{
					throw new ArgumentException(); //todo: exception
				}

				DataAccess.Entities.Route currentRoute = courier.Routes.FirstOrDefault(r => r.IsCurrent);

				foreach (var point in currentRoute.Points.ToList())
				{
					db.ActivePointsRepo.Delete(point);
				}

				foreach (var point in newCurrentRouteDto.Points)
				{
					currentRoute.Points.Add(new ActivePoint() { Coordinates = new Point(point.Longitude, point.Latitude) });
				}

				db.RoutesRepo.Update(currentRoute);

				await db.SaveAsync();
			}
		}
	}
}
