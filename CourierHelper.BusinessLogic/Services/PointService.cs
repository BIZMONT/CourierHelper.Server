using AutoMapper;
using CourierHelper.BusinessLogic.Abstract;
using CourierHelper.BusinessLogic.DTO;
using CourierHelper.DataAccess;
using CourierHelper.DataAccess.Entities;
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

				Point orderPoint = order.Destination.Coordinates;
				PointDto orderPointDto = Mapper.Map<PointDto>(orderPoint);

				Point warehousePoint = order.Warehouse.Location.Coordinates;
				PointDto warehousePointDto = Mapper.Map<PointDto>(warehousePoint);

				orderPointDto.After = warehousePointDto;

				return orderPointDto;
			}
		}
	}
}
