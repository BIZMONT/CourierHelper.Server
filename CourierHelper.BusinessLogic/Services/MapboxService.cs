using CourierHelper.BusinessLogic.DTO;
using CourierHelper.BusinessLogic.Infrastructure.MapBoxResponse;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
}
