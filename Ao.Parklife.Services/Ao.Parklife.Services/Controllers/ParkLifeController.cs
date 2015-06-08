using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Ao.Parklife.Services.Models;
using Newtonsoft.Json;

namespace Ao.Parklife.Services.Controllers
{
    [RoutePrefix("api")]
    public class ParkLifeController : ApiController
    {
        private static readonly List<User> _visibleUsers = new List<User>();

        [HttpGet]
        [Route("GetAll")]
        public string GetAllUsers()
        {
            return JsonConvert.SerializeObject(_visibleUsers);
        }



        [System.Web.Http.HttpGet]
        [Route("DummyGetAll")]
        public string DummyGetAllUsers()
        {
            var closestBeacon = new Beacon
            {
                UUID = "food2",
                LatestReading = new BeaconReading(DateTime.UtcNow, 1.0)
            };
            var closestRegion = new Region(RegionIds.FoodCounter)
            {
                EnteredAt = DateTime.UtcNow,
                OwnedBeacons = new List<Beacon>()
                {
                    new Beacon
                    {
                        UUID = "food1",
                        LatestReading = new BeaconReading(DateTime.UtcNow, 2.4)
                    },
                    closestBeacon
                }
            };
            var dummyUsers = new List<User>
            {
                new User("Vincent Lee")
                {
                    VisibleRegions = new List<Region>
                    {
                        closestRegion,
                        new Region(RegionIds.GamesArea)
                        {
                            EnteredAt = DateTime.UtcNow.AddMinutes(-4),
                            OwnedBeacons = new List<Beacon>()
                            {
                                new Beacon
                                {
                                     UUID = "games1",
                                     LatestReading = new BeaconReading(DateTime.UtcNow, 8.4)
                                },
                                new Beacon
                                {
                                     UUID = "games2",
                                     LatestReading = new BeaconReading(DateTime.UtcNow, 12.0)
                                }
                            }
                        }
                    },
                    ClosestRegion = closestRegion,
                        ClosestBeacon = closestBeacon
                }
            };
            return JsonConvert.SerializeObject(dummyUsers);
        }

        [Route("enterregion/{regionId}/{userid}")]
        [HttpGet]
        [HttpPost]
        public HttpResponseMessage EnterRegion(RegionIds regionId, string userid)
        {
            var user = _visibleUsers.FirstOrDefault(x => x.UserName == userid);
            if (user == null)
            {
                user = new User(userid);
                _visibleUsers.Add(user);
            }
            if (user.VisibleRegions.FirstOrDefault(r => r.Id == regionId) == null)
            {
                user.VisibleRegions.Add(new Region(regionId) { EnteredAt = DateTime.Now });
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.Created, regionId.ToString());
        }

        [Route("exitregion/{regionId}/{userid}")]
        [HttpPost]
        [HttpGet]
        public HttpResponseMessage ExitRegion(RegionIds regionId, string userid)
        {
            var user = _visibleUsers.FirstOrDefault(x => x.UserName == userid);
            if (user != null)
            {
                user.VisibleRegions.RemoveAll(r => r.Id == regionId);
                if (!user.VisibleRegions.Any())
                    _visibleUsers.Remove(user);
            }

            return Request.CreateResponse<string>(System.Net.HttpStatusCode.Created, regionId.ToString());
        }
    }
}