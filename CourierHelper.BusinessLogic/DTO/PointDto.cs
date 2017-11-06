using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierHelper.BusinessLogic.DTO
{
	public class PointDto
	{
		public PointDto() { }

		public PointDto(double latitude, double longitude)
		{
			Longitude = longitude;
			Latitude = latitude;
		}

		public double Longitude { get; set; }
		public double Latitude { get; set; }

		public PointDto After { get; set; }

		public override string ToString()
		{
			CultureInfo ci = new CultureInfo("en-US");
			return $"{Longitude.ToString("N8", ci)},{Latitude.ToString("N8", ci)}";
		}
	}
}
