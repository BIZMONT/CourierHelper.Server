using CourierHelper.BusinessLogic.DTO;
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

		[HttpPost]
		[Route("courier/{courierId}/location")]
		public async Task<IHttpActionResult> UpdateLocation(Guid courierId, [FromBody]PointDto location)
		{
			await CourierService.ChangeCourierLocationAsync(courierId, location);
			return Ok();
		}

		[HttpGet]
		[Route("courier/{courierId}/orders/sync")]
		public async Task<IHttpActionResult> SyncOrders(Guid courierId)
		{
			IList<OrderDto> orders = await OrderService.SyncOrders(courierId);

			if(orders.Count > 0)
			{
				return Ok(orders);
			}
			else
			{
				return StatusCode(HttpStatusCode.NotModified);
			}
		}

		[HttpPost]
		[Route("courier/{courierId}/routes/sync")]
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
		[Route("courier/{courierId}/orders/{orderId}/complete")]
		public async Task<IHttpActionResult> CompleteOrder(Guid courierId, long orderId)
		{
			//todo: complete order
			return Ok();
		}

		[HttpPost]
		[Route("courier/{courierId}/routes")]
		public async Task<IHttpActionResult> StartRoute(Guid courierId, RouteDto route, IEnumerable<OrderDto> orders)
		{
			//todo: add logic for new route with orders
			await RouteService.AddRouteAsync(courierId, route);
			return Ok();
		}
	}
}
