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
	public class OrderService : ServiceBase
	{
		public string _connectionString;

		public OrderService(string connectionString)
		{
			_connectionString = connectionString;
		}

		public async Task<long> AddOrder(OrderDto orderDto)
		{
			if (orderDto.WarehouseId <= 0 || orderDto.Receiver == null ||
				orderDto.Sender == null || orderDto.Destination == null)
			{
				throw new ArgumentException(); //todo: exception
			}

			using (var db = new CourierHelperDb(_connectionString))
			{
				Warehouse warehouse = db.WarehousesRepo.Query.FirstOrDefault(w => w.Id == orderDto.WarehouseId);

				if (warehouse == null)
				{
					throw new ArgumentException(); //todo: exception
				}


				Customer sender = db.CustomersRepo.Query.FirstOrDefault(c => c.Id == orderDto.Sender.Id);
				if (sender == null)
				{
					sender = Mapper.Map<Customer>(orderDto.Sender);

					db.CustomersRepo.Create(sender);
				}
				else
				{
					sender.Email = orderDto.Sender.Email;
					sender.FirstName = orderDto.Sender.FirstName;
					sender.MiddleName = orderDto.Sender.MiddleName;
					sender.PhoneNumber = orderDto.Sender.PhoneNumber;

					db.CustomersRepo.Update(sender);
				}

				Customer receiver = db.CustomersRepo.Query.FirstOrDefault(c => c.Id == orderDto.Receiver.Id);
				if (receiver == null)
				{
					receiver = Mapper.Map<Customer>(orderDto.Receiver);

					db.CustomersRepo.Create(sender);
				}
				else
				{
					receiver.Email = orderDto.Receiver.Email;
					receiver.FirstName = orderDto.Receiver.FirstName;
					receiver.MiddleName = orderDto.Receiver.MiddleName;
					receiver.PhoneNumber = orderDto.Receiver.PhoneNumber;

					db.CustomersRepo.Update(receiver);
				}

				Order order = new Order
				{
					State = OrderState.NotAssigned,
					Receiver = receiver,
					Sender = sender,
					Warehouse = warehouse,
					Destination = new ActivePoint { Coordinates = new Point(orderDto.Destination.Longitude, orderDto.Destination.Latitude) }
				};

				db.OrdersRepo.Create(order);

				await db.SaveAsync();

				return order.Id;
			}
		}

		public OrderDto GetOrderById(long id)
		{
			using (var db = new CourierHelperDb(_connectionString))
			{
				Order order = db.OrdersRepo.Query.FirstOrDefault(o => o.Id == id);

				OrderDto orderDto = Mapper.Map<OrderDto>(order);

				return orderDto;
			}
		}

		public List<OrderDto> GetUnassignedOrders()
		{
			using (var db = new CourierHelperDb(_connectionString))
			{
				var orders = db.OrdersRepo.Query.Where(order => order.State == OrderState.NotAssigned).ToList();
				var ordersDto = Mapper.Map<List<OrderDto>>(orders);

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

				var ordersDto = Mapper.Map<List<OrderDto>>(orders);

				return ordersDto;
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

		public async Task DeleteOrder(long orderId)
		{
			using (var db = new CourierHelperDb(_connectionString))
			{
				db.OrdersRepo.Delete(orderId);

				await db.SaveAsync();
			}
		}
	}
}
