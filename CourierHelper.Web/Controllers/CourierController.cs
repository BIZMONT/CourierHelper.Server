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
	[RoutePrefix("api/couriers")]
	public class CourierController : ApiController
	{
		public CourierController()
		{
			string dbConnection = ConfigurationManager.ConnectionStrings["CourierHelperDb"].ConnectionString;
			CourierService = new CourierService(dbConnection);
			RouteService = new RouteService(dbConnection);
			OrderService = new OrderService(dbConnection);
		}

		protected CourierService CourierService { get; private set; }
		protected RouteService RouteService { get; private set; }
		protected OrderService OrderService { get; private set; }

		#region CRUD
		[HttpGet]
		public IHttpActionResult GetAll()
		{
			List<CourierDto> couriers =  CourierService.GetAllCouriers();

			return Ok(couriers);
		}

		[HttpGet]
		[Route("{courierId}")]
		public IHttpActionResult GetById(Guid courierId)
		{
			CourierDto courier = CourierService.GetCourierById(courierId);

			return Ok(courier);
		}
		#endregion

		#region Main flow
		[HttpPost]
		[Route("{courierId}/location")]
		public async Task<IHttpActionResult> UpdateLocation(Guid courierId, [FromBody]PointDto location)
		{
			await CourierService.ChangeCourierLocationAsync(courierId, location);
			return Ok();
		}

		[HttpPost]
		[Route("{courierId}/orders/sync")]
		public async Task<IHttpActionResult> SyncOrders(Guid courierId)
		{
			IList<OrderDto> orders = await OrderService.SyncOrders(courierId);

			if (orders.Count > 0)
			{
				return Ok(orders);
			}
			else
			{
				return StatusCode(HttpStatusCode.NotModified);
			}
		}

		[HttpPost]
		[Route("{courierId}/orders/take")]
		public async Task<IHttpActionResult> TakeOrder(Guid courierId, OrderDto orderDto)
		{
			await OrderService.ChangeOrderStateAsync(orderDto, OrderStateDto.Fulfillment);
			return Ok();
		}

		[HttpPost]
		[Route("{courierId}/orders/{orderId}/complete")]
		public async Task<IHttpActionResult> CompleteOrder(Guid courierId, long orderId)
		{
			await OrderService.CompleteAsync(courierId, orderId);
			return Ok();
		}

		[HttpPost]
		[Route("{courierId}/routes/sync")]
		public async Task<IHttpActionResult> SyncRoutes(Guid courierId)
		{
			RouteDto route = await RouteService.SyncRoute(courierId);

			if (route != null)
			{
				return Ok(route);
			}
			else
			{
				return StatusCode(HttpStatusCode.NotModified);
			}
		}

		[HttpPost]
		[Route("{courierId}/delivery")]
		public async Task<IHttpActionResult> StartDelivery(Guid courierId, DeliveryDto delivery)
		{
			foreach (var order in delivery.Orders)
			{
				await OrderService.ChangeOrderStateAsync(order, OrderStateDto.Fulfillment);
			}
			await RouteService.AddRouteAsync(courierId, delivery.Route);

			await CourierService.ChangeCourierStateAsync(courierId, CourierStateDto.PerformsDelivery);

			return Ok();
		}
		#endregion
	}
}
