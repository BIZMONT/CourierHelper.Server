using CourierHelper.BusinessLogic.DTO;
using CourierHelper.BusinessLogic.DTO.Enums;
using CourierHelper.BusinessLogic.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CourierHelper.Web.Controllers
{
	[RoutePrefix("api/orders")]
	public class OrderController : ApiController
	{
		public OrderController()
		{
			string dbConnection = ConfigurationManager.ConnectionStrings["CourierHelperDb"].ConnectionString;
			OrderService = new OrderService(dbConnection);
		}

		protected OrderService OrderService { get; private set; }

		[HttpPost]
		public async Task<IHttpActionResult> AddOrder(OrderDto order)
		{
			await OrderService.AddOrderAsync(order);

			return Ok();
		}

		[HttpGet]
		public IHttpActionResult GetAll()
		{
			List<OrderDto> ordersDto = OrderService.GetAll();

			return Ok(ordersDto);
		}

		[HttpGet]
		[Route("{orderId}")]
		public IHttpActionResult GetOrder(long orderId)
		{
			OrderDto orderDto = OrderService.GetOrderById(orderId);

			return Ok(orderDto);
		}
		[HttpGet]
		[Route("state/{state}")]
		public IHttpActionResult GetOrderByState(OrderStateDto state)
		{
			List<OrderDto> ordersDto = OrderService.GetOrdersByState(state);

			return Ok(ordersDto);
		}
	}
}
