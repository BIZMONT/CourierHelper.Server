using AutoMapper;
using CourierHelper.BusinessLogic.DTO;
using CourierHelper.BusinessLogic.DTO.Enums;
using CourierHelper.DataAccess;
using CourierHelper.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierHelper.BusinessLogic.Services
{
	public class OrderService
	{
		public string _connectionString;

		public OrderService(string connectionString)
		{
			_connectionString = connectionString;
		}

		public List<OrderDto> GetUnassignedOrders()
		{
			using (var db = new CourierHelperDb(_connectionString))
			{
				var orders = db.OrdersRepo.Query.Where(order => order.State == OrderState.NotAssigned).ToList();
				var ordersDto = Mapper.Map<List<OrderDto>>(orders);		//todo: automapper config

				return ordersDto;
			}
		}

		public List<OrderDto> GetCourierOrders(Guid courierId)
		{
			using (var db = new CourierHelperDb(_connectionString))
			{
				var courier = db.CouriersRepo.Query.FirstOrDefault(c => c.Id == courierId);

				if (courier == null)
				{
					throw new ArgumentOutOfRangeException(); // todo: exception
				}

				var orders = courier.Orders.ToList();

				var ordersDto = Mapper.Map<List<OrderDto>>(orders);  //todo: automapper config

				return ordersDto;
			}
		}

		public PointDto GetOrderLocation(long orderId)
		{
			using (var db = new CourierHelperDb(_connectionString))
			{
				var order = db.OrdersRepo.Query.FirstOrDefault(o => o.Id == orderId);

				if(order == null)
				{
					throw new ArgumentOutOfRangeException("The order with this id does not exist!");
				}

				var orderPoint = order.Destination;
				var orderPointDto = Mapper.Map<PointDto>(orderPoint);   //todo: automapper config

				var warehousePoint = order.Warehouse.Location;
				var warehousePointDto = Mapper.Map<PointDto>(warehousePoint);   //todo: automapper config

				orderPointDto.After = warehousePointDto;

				return orderPointDto;
			}
		}

		public async Task ChangeOrderStateAsync(OrderDto orderDto, OrderStateDto state)
		{
			using (var db = new CourierHelperDb(_connectionString))
			{
				var order = db.OrdersRepo.Query.FirstOrDefault(o => o.Id == orderDto.Id);

				if (order == null)
				{
					throw new ArgumentOutOfRangeException("The order with this id does not exist!");
				}

				order.State = (OrderState)state;

				db.OrdersRepo.Update(order);
				await db.SaveAsync();

				orderDto.State = state;
			}
		}

		public PointDto GetWarehouseLocation(long id)
		{
			throw new NotImplementedException();
		}
	}
}
