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
	public class WarehouseService : ServiceBase
	{
		private string _connectionString;

		public WarehouseService(string connectionString)
		{
			_connectionString = connectionString;
		}

		public async Task<int> AddWarehouseAsync(WarehouseDto warehouseDto)
		{
			using (var db = new CourierHelperDb(_connectionString))
			{
				if (warehouseDto.Location == null)
				{
					throw new ArgumentException("Warehouse location is required!");
				}

				ActivePoint location = new ActivePoint
				{
					Coordinates = new Point(warehouseDto.Location.Longitude, warehouseDto.Location.Latitude)
				};

				Warehouse warehouse = new Warehouse
				{
					Location = location,
					Address = warehouseDto.Address,
					Name = warehouseDto.Name
				};

				db.WarehousesRepo.Create(warehouse);
				await db.SaveAsync();

				return warehouse.Id;
			}
		}

		public List<WarehouseDto> GetAll()
		{
			using (var db = new CourierHelperDb(_connectionString))
			{
				List<Warehouse> warehouses = db.WarehousesRepo.GetAll().ToList();

				List<WarehouseDto> warehousesDto = Mapper.Map<List<WarehouseDto>>(warehouses);

				return warehousesDto;
			}
		}

		public WarehouseDto GetById(int warehouseId)
		{
			using (var db = new CourierHelperDb(_connectionString))
			{
				Warehouse warehouse = db.WarehousesRepo.Get(warehouseId);

				WarehouseDto warehouseDto = Mapper.Map<WarehouseDto>(warehouse);

				return warehouseDto;
			}
		}

		public async Task Update(WarehouseDto warehouseDto)
		{
			using (var db = new CourierHelperDb(_connectionString))
			{
				if (warehouseDto.Location == null)
				{
					throw new ArgumentException("Warehouse location is required!");
				}

				Warehouse warehouse = db.WarehousesRepo.Get(warehouseDto.Id);

				if (warehouse == null)
				{
					throw new ArgumentOutOfRangeException(""); //todo: make better exception
				}

				warehouse.Location.Coordinates = new Point(warehouseDto.Location.Longitude, warehouseDto.Location.Latitude);
				warehouse.Address = warehouseDto.Address;
				warehouse.Name = warehouseDto.Name;

				db.WarehousesRepo.Update(warehouse);

				await db.SaveAsync();
			}
		}
	}
}
