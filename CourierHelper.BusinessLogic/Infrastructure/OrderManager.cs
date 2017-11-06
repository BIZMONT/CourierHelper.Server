using CourierHelper.BusinessLogic.DTO;
using CourierHelper.BusinessLogic.DTO.Enums;
using CourierHelper.BusinessLogic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierHelper.BusinessLogic.Infrastructure
{
	public class OrderManager
	{
		private MapboxService _mapBoxService;
		private OrderService _orderService;
		private CourierService _courierService;
		private PointService _pointService;
		private RouteService _routeService;

		private Queue<OrderDto> _ordersQueue;

		public OrderManager(string connectionString, string mapBoxAccessToken)
		{
			_mapBoxService = new MapboxService(mapBoxAccessToken, MapBoxProfile.Driving);
			_orderService = new OrderService(connectionString);
			_courierService = new CourierService(connectionString);
			_pointService = new PointService(connectionString);
			_routeService = new RouteService(connectionString);

			_ordersQueue = new Queue<OrderDto>();
		}

		public void UpdateQueue()
		{
			var newOrders = _orderService.GetUnassignedOrders();
			_ordersQueue = new Queue<OrderDto>(_ordersQueue.Concat(newOrders).Distinct());
		}

		public async Task ProceedNextOrderAsync()
		{
			if(_ordersQueue.Count == 0)
			{
				return;
			}

			OrderDto order = _ordersQueue.Dequeue();
			if (order == null)
			{
				return;
			}

			await _orderService.ChangeOrderStateAsync(order, OrderStateDto.Assignment);                 //2. Change order state to Assignment

			PointDto orderLocationPoint = _pointService.GetOrderLocation(order.Id);                     //3. Get order destination point coordinates

			IEnumerable<CourierDto> couriers = _courierService.GetNearestCouriers(orderLocationPoint);  //4. Find nearest couriers by next criterias (distance to point > number of assigned orders > courier state)

			var possibleCandidate = couriers.FirstOrDefault(courier => courier.State == CourierStateDto.Idle);

			if (possibleCandidate != null)
			{
				await _courierService.AssignOrder(possibleCandidate.Id, order.Id);
				//todo: add recomended route
			}
			else
			{
				RouteCandidate bestRoute = await FindBestCandidateAsync(couriers, orderLocationPoint);

				if (bestRoute == null)
				{
					_ordersQueue.Enqueue(order);
					//todo: Can`t find best courier
				}
				else
				{
					await _courierService.AssignOrder(bestRoute.Courier.Id, order.Id);
					await _routeService.ChangeCourierCurrentRouteAsync(bestRoute.Courier.Id, bestRoute.GetRoute());
				}
			}
		}

		private async Task<RouteCandidate> FindBestCandidateAsync(IEnumerable<CourierDto> couriers, PointDto orderPoint)
		{
			List<PointDto> pointsDto = new List<PointDto>();
			SortedList<double, RouteCandidate> candidates = new SortedList<double, RouteCandidate>();

			foreach (var courier in couriers)
			{
				pointsDto.Add(courier.Location);
				pointsDto.Add(orderPoint);

				List<OrderDto> courierOrders = _orderService.GetCourierOrders(courier.Id);
				foreach (var courierOrder in courierOrders)
				{
					var location = _pointService.GetOrderLocation(courierOrder.Id);
					pointsDto.Add(location);
				}

				var optimizedPointsOrder = await _mapBoxService.OptimizeRouteAsync(pointsDto.ToArray());
				var optimizedRoute = await _mapBoxService.BuildRouteAsync(optimizedPointsOrder);

				RouteDto currentRoute = _routeService.GetCourierCurrentRoute(courier.Id);
				candidates.Add(optimizedRoute.Distance - currentRoute.Distance, optimizedRoute);
			}

			return candidates.First().Value;
		}
	}
}
