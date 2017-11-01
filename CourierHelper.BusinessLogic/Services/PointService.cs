using AutoMapper;
using CourierHelper.BusinessLogic.Abstract;
using CourierHelper.BusinessLogic.DTO;
using CourierHelper.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierHelper.BusinessLogic.Services
{
	public class PointService : ServiceBase
	{
		public string _connectionString;

		public PointService(string connectionString)
		{
			_connectionString = connectionString;
		}

		public PointDto GetOrderLocation(long orderId)
		{
			using (var db = new CourierHelperDb(_connectionString))
			{
				var order = db.OrdersRepo.Query.FirstOrDefault(o => o.Id == orderId);

				if (order == null)
				{
					throw new ArgumentOutOfRangeException("The order with this id does not exist!");
				}

				var orderPoint = order.Destination;
				var orderPointDto = Mapper.Map<PointDto>(orderPoint);

				var warehousePoint = order.Warehouse.Location;
				var warehousePointDto = Mapper.Map<PointDto>(warehousePoint);

				orderPointDto.After = warehousePointDto;

				return orderPointDto;
			}
		}

		public PointDto GetWarehouseLocation(long warehouseId)
		{
			using (var db = new CourierHelperDb(_connectionString))
			{
				var warehouse = db.WarehousesRepo.Query.FirstOrDefault(o => o.Id == warehouseId);

				if (warehouse == null)
				{
					throw new ArgumentOutOfRangeException("The warehouse with this id does not exist!");
				}

				var warehousePoint = warehouse.Location;
				var warehousePointDto = Mapper.Map<PointDto>(warehousePoint);

				return warehousePointDto;
			}
		}

	}
}
