using CourierHelper.BusinessLogic.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CourierHelper.BusinessLogic.Services
{
    public class MapboxService
    {
        private static string MapBoxApiHost = "https://api.mapbox.com";
        private static string MapBoxOptimizationApiPath = "/optimized-trips/v1";
        private static string MapBoxDirectionApiPath = "/directions/v5";

        private string _accessToken;
        private string _profile;

        public MapboxService(string accessToken, string profile)
        {
            _accessToken = accessToken;
            _profile = profile;
        }

        public MapboxService(string accessToken)
        {
            _accessToken = accessToken;
            _profile = MapBoxProfile.Driving;
        }

        public async Task<IEnumerable<PointDto>> OptimizeRouteAsync(PointDto[] points)
        {
            string pointsString = string.Join(";", points.Select(point => point.ToString()));

            string requestPath = $"{MapBoxOptimizationApiPath}/{_profile}/{pointsString}?" +
                $"access_token={_accessToken}";

            List<string> distributions = GetDistributions(points);
            if (distributions.Count > 0)
            {
                requestPath += $"&distributions={string.Join(";", distributions)}";
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(MapBoxApiHost);

                var response = await client.GetAsync(requestPath);
                if (response.IsSuccessStatusCode)
                {
                    var contentJson = await response.Content.ReadAsStringAsync();
                    var content = JsonConvert.DeserializeObject<OptimizationResponse>(contentJson);

                    if(content.Code != MapBoxCodes.Ok)
                    {
                        throw new Exception(content.Code); //todo: beter exception
                    }

                    Array.Sort(content.Waypoints, (x, y) => x.WaypointIndex.CompareTo(y.WaypointIndex));

                    return content.Waypoints.Select(waypoint => new PointDto()
                    {
                        Longitude = waypoint.Location[0],
                        Latitude = waypoint.Location[1]
                    }).ToList();
                }
                throw new Exception(); //todo: beter exception
            }
        }

        public async Task<RouteCandidate> BuildRouteAsync(IEnumerable<PointDto> points)
        {
            string pointsString = string.Join(";", points.Select(point => point.ToString()));

            string requestPath = $"{MapBoxDirectionApiPath}/{_profile}/{pointsString}?" +
                $"access_token={_accessToken}&geometries=geojson";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(MapBoxApiHost);

                var response = await client.GetAsync(requestPath);
                if (response.IsSuccessStatusCode)
                {
                    var contentJson = await response.Content.ReadAsStringAsync();
                    var content = JsonConvert.DeserializeObject<DirectionsResponse>(contentJson);

                    if (content.Code != MapBoxCodes.Ok)
                    {
                        throw new Exception(content.Code); //todo: beter exception
                    }

                    var routePoints = content.Routes[0].Geometry.Coordinates.Select(coordinate => new PointDto()
                    {
                        Longitude = coordinate[0],
                        Latitude = coordinate[1]
                    });

					var route = new RouteCandidate(routePoints, content.Routes[0].Distance);

					return route;
                }
                throw new Exception(); //todo: beter exception
            }
        }

        private List<string> GetDistributions(PointDto[] points)
        {
            List<string> distributions = new List<string>();

            for (int i = 0; i < points.Length; i++)
            {
                if (points[i].After != null)
                {
                    int index = Array.IndexOf(points, points[i].After);

                    if (index < 0)
                    {
                        throw new Exception(); //todo: better exception
                    }

                    distributions.Add($"{index},{i}");
                }
            }

            return distributions;
        }
    }

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

    #region Generated from JSON
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
    #endregion
}
