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
			if (_configured)
			{
				return;
			}

			Mapper.Initialize(cfg =>
			{
				cfg.CreateMap<Customer, CustomerDto>()
					.ReverseMap();

				cfg.CreateMap<Order, OrderDto>()
					.ReverseMap()
					.ForMember(o => o.Courier, opt => opt.Ignore())
					.ForMember(o => o.Destination, opt => opt.Ignore())
					.ForMember(o => o.Receiver, opt => opt.Ignore())
					.ForMember(o => o.Sender, opt => opt.Ignore())
					.ForMember(o => o.Destination, opt => opt.Ignore());

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
