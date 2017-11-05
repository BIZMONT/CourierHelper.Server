﻿using AutoMapper;
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
					.ReverseMap()
					.ForMember(o => o.Courier, opt => opt.Ignore())
					.ForMember(o => o.Destination, opt => opt.Ignore())
					.ForMember(o => o.Receiver, opt => opt.Ignore())
					.ForMember(o => o.Sender, opt => opt.Ignore())
					.ForMember(o => o.Destination, opt => opt.Ignore());
			});

			_configured = true;
		}
	}
}
