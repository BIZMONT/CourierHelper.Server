using AutoMapper;
using CourierHelper.BusinessLogic.DTO;
using CourierHelper.DataAccess;
using CourierHelper.DataAccess.Entities;
using CourierHelper.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
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
				Courier courier = db.CouriersRepo.Get(courierId);

				if (courier == null)
				{
					throw new ArgumentException(); //todo: exception
				}

				if (courier.Routes.Any(r => r.IsCurrent))
				{
					throw new Exception(); //todo: exception
				}

				Route route = new Route
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
				Courier courier = db.CouriersRepo.Get(courierId);

				if (courier == null)
				{
					throw new ArgumentOutOfRangeException(""); //todo: exception
				}

				Route currentRoute = courier.Routes.FirstOrDefault(route => route.IsCurrent);

				RouteDto routeDto = Mapper.Map<RouteDto>(currentRoute);

				return routeDto;
			}
		}

		public List<RouteDto> GetCourierAllRoutes(Guid courierId)
		{
			using (var db = new CourierHelperDb(_connectionString))
			{
				Courier courier = db.CouriersRepo.Get(courierId);

				if (courier == null)
				{
					throw new ArgumentOutOfRangeException(""); //todo: exception
				}

				List<Route> routes = courier.Routes.ToList();

				List<RouteDto> routesDto = Mapper.Map<List<RouteDto>>(routes);

				return routesDto;
			}
		}

		public async Task ChangeCourierCurrentRouteAsync(Guid courierId, RouteDto newCurrentRouteDto)
		{
			using (var db = new CourierHelperDb(_connectionString))
			{
				Courier courier = db.CouriersRepo.Get(courierId);
				if (courier == null)
				{
					throw new ArgumentException(); //todo: exception
				}

				Route currentRoute = courier.Routes.FirstOrDefault(r => r.IsCurrent);

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

		public RouteDto GetCourieRemainingRoute(Guid courierId)
		{
			using (var db = new CourierHelperDb(_connectionString))
			{
				Courier courier = db.CouriersRepo.Get(courierId);
				if (courier == null)
				{
					throw new ArgumentException(); //todo: exception
				}

				if (courier.State < CourierState.PerformsDelivery)
				{
					throw new Exception(); //todo: exception
				}

				Route currentRoute = courier.Routes.Single(r => r.IsCurrent);

				return null; //todo: add logic for getting remaining route
			}
		}
	}
}
