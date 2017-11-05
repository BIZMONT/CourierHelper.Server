using AutoMapper;
using CourierHelper.BusinessLogic.DTO;
using CourierHelper.DataAccess.Entities;
using System.Linq;

namespace CourierHelper.BusinessLogic
{
	internal class AutomapperConfigurator
	{
		private static bool _configured;

		public static void Configure()
		{
			if (_configured)
			{
				return;
			}

			Mapper.Initialize(cfg =>
			{
				cfg.CreateMap<Point, PointDto>()
					.ForMember(p => p.After, opt => opt.Ignore())
				.ReverseMap();

				cfg.CreateMap<ActivePoint, PointDto>()
					.ForMember(p => p.Latitude, opt => opt.MapFrom(ap=>ap.Coordinates.Latitude))
					.ForMember(p => p.Longitude, opt => opt.MapFrom(ap => ap.Coordinates.Longitude))
					.ReverseMap();

				cfg.CreateMap<Customer, CustomerDto>()
				.ReverseMap();

				cfg.CreateMap<Warehouse, WarehouseDto>()
					.ForMember(w => w.Location, opt => opt.MapFrom(w => w.Location.Coordinates));

				cfg.CreateMap<Courier, CourierDto>()
					.ForMember(c => c.Location, opt => opt.AllowNull())
					.ForMember(c => c.Location, opt => opt.MapFrom(c => c.Location.Coordinates));

				cfg.CreateMap<Route, RouteDto>()
					.ReverseMap();
				//Not checked

				cfg.CreateMap<Order, OrderDto>()
					.ForMember(o => o.CourierId, opt => opt.MapFrom(o => o.Courier.Id))
					.ForMember(o => o.WarehouseId, opt => opt.MapFrom(o => o.Warehouse.Id))
					.ForMember(o => o.Destination, opt => opt.MapFrom(o => o.Destination.Coordinates));
			});

			_configured = true;
		}
	}
}
