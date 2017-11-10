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
	[RoutePrefix("api/warehouses")]
    public class WarehouseController : ApiController
    {
		public WarehouseController()
		{
			string dbConnection = ConfigurationManager.ConnectionStrings["CourierHelperDb"].ConnectionString;
			WarehouseService = new WarehouseService(dbConnection);
		}

		protected WarehouseService WarehouseService { get; private set; }

		[HttpPost]
		public async Task<IHttpActionResult> AddOrder(WarehouseDto warehouseDto)
		{
			await WarehouseService.AddWarehouseAsync(warehouseDto);

			return Ok();
		}

		[HttpGet]
		public IHttpActionResult GetAll()
		{
			List<WarehouseDto> warehousesDto = WarehouseService.GetAll();

			return Ok(warehousesDto);
		}
	}
}
