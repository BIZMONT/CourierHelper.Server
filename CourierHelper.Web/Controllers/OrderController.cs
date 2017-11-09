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
	public class OrderController : ApiController
    {
		public OrderController()
		{
			string dbConnection = ConfigurationManager.ConnectionStrings["CourierHelperDb"].ConnectionString;
			Service = new OrderService(dbConnection);
		}

		protected OrderService Service { get; private set; }

		[HttpPost]
		[Route("orders/add")]
		public async Task<IHttpActionResult> AddOrder(OrderDto order)
		{
			await Service.AddOrderAsync(order);

			return Ok();
		}
	}
}
