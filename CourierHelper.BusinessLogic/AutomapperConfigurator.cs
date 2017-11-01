using AutoMapper;
using CourierHelper.BusinessLogic.DTO;
using CourierHelper.DataAccess.Entities;

namespace CourierHelper.BusinessLogic
{
	internal class AutomapperConfigurator
	{
		private static bool _configured;

		public static void Configure()
		{
			if(_configured)
			{
				return;
			}

			Mapper.Initialize(cfg => {
				cfg.CreateMap<Order, OrderDto>()
					.ReverseMap();

				cfg.CreateMap<ActivePoint, PointDto>()
					.ReverseMap();

				cfg.CreateMap<Courier, CourierDto>()
					.ReverseMap();

				cfg.CreateMap<Route, RouteDto>()
					.ReverseMap();
			});

			_configured = true;
		}
	}
}
