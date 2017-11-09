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
		}

		protected CourierService CourierService { get; private set; }
		protected RouteService RouteService { get; private set; }

		[HttpPost]
		[Route("courier/{courierId}/location")]
		public async Task<IHttpActionResult> UpdateLocation(Guid courierId, [FromBody]PointDto location)
		{
			await CourierService.ChangeCourierLocationAsync(courierId, location);
			return Ok();
		}

		[HttpGet]
		[Route("courier/{courierId}/orders/sync")]
		public IHttpActionResult SyncOrders(Guid courierId)
		{
			//todo: get changed or new orders
			return Ok();
		}

		[HttpGet]
		[Route("courier/{courierId}/routes/sync")]
		public IHttpActionResult SyncRoutes(Guid courierId)
		{
			//todo: get changed or new orders
			return Ok();
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
