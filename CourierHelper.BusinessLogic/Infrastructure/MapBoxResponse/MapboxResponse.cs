using Newtonsoft.Json;

namespace CourierHelper.BusinessLogic.Infrastructure.MapBoxResponse
{
	public struct MapBoxProfile
	{
		public const string Driving = "mapbox/driving";
		public const string Walking = "mapbox/walking";
		public const string Cycling = "mapbox/cycling";
	}

	public struct MapBoxCodes
	{
		public const string Ok = "Ok";
	}

	internal partial class OptimizationResponse
	{
		[JsonProperty("trips")]
		public Trip[] Trips { get; set; }

		[JsonProperty("code")]
		public string Code { get; set; }

		[JsonProperty("waypoints")]
		public Waypoint[] Waypoints { get; set; }
	}

	internal partial class Trip
	{
		[JsonProperty("duration")]
		public double Duration { get; set; }

		[JsonProperty("legs")]
		public Leg[] Legs { get; set; }

		[JsonProperty("distance")]
		public double Distance { get; set; }

		[JsonProperty("geometry")]
		public string Geometry { get; set; }

		[JsonProperty("weight")]
		public double Weight { get; set; }

		[JsonProperty("weight_name")]
		public string WeightName { get; set; }
	}

	internal partial class Leg
	{
		[JsonProperty("duration")]
		public double Duration { get; set; }

		[JsonProperty("summary")]
		public string Summary { get; set; }

		[JsonProperty("distance")]
		public double Distance { get; set; }

		[JsonProperty("steps")]
		public object[] Steps { get; set; }

		[JsonProperty("weight")]
		public double Weight { get; set; }
	}

	internal partial class Waypoint
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("location")]
		public double[] Location { get; set; }

		[JsonProperty("trips_index")]
		public long TripsIndex { get; set; }

		[JsonProperty("waypoint_index")]
		public long WaypointIndex { get; set; }
	}

	internal partial class DirectionsResponse
	{
		[JsonProperty("routes")]
		public Route[] Routes { get; set; }

		[JsonProperty("code")]
		public string Code { get; set; }

		[JsonProperty("uuid")]
		public string Uuid { get; set; }

		[JsonProperty("waypoints")]
		public Waypoint[] Waypoints { get; set; }
	}

	internal partial class Route
	{
		[JsonProperty("duration")]
		public double Duration { get; set; }

		[JsonProperty("legs")]
		public Leg[] Legs { get; set; }

		[JsonProperty("distance")]
		public double Distance { get; set; }

		[JsonProperty("geometry")]
		public Geometry Geometry { get; set; }

		[JsonProperty("weight")]
		public double Weight { get; set; }

		[JsonProperty("weight_name")]
		public string WeightName { get; set; }
	}

	internal partial class Geometry
	{
		[JsonProperty("coordinates")]
		public double[][] Coordinates { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }
	}
}
