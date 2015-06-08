using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Ao.Parklife.Services.Models;
using Newtonsoft.Json;

namespace Ao.Parklife.Services.Controllers
{
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api")]
    public class ParkLifeController : ApiController
    {
        private static readonly List<User> _visibleUsers = new List<User>();
        private readonly TwitterClient _twitter;

        public ParkLifeController()
        {
            _twitter = new TwitterClient();
        }

        [HttpGet]
        [Route("GetAll")]
        public string GetAllUsers()
        {
            return JsonConvert.SerializeObject(_visibleUsers.Where(u => u.VisibleRegions.Any()));
        }

        [HttpGet]
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
                OwnedBeacons = new List<Beacon>
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
                            OwnedBeacons = new List<Beacon>
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
            user = AddUser(userid, user);
            if (user.VisibleRegions.FirstOrDefault(r => r.Id == regionId) == null)
            {
                user.VisibleRegions.Add(new Region(regionId) {EnteredAt = DateTime.Now});
            }
            if (user.VisibleRegions.Count() == 1)
            {
                user.ClosestRegion = user.VisibleRegions.First();
            }


            _twitter.Send(user.UserName + "has entered the Park!");

            return Request.CreateResponse(HttpStatusCode.Created, regionId.ToString());
        }

        private static User AddUser(string userid, User user)
        {
            if (user != null) return user;
            user = new User(userid);
            _visibleUsers.Add(user);
            return user;
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
            if (user != null && user.VisibleRegions.Count() == 1)
            {
                user.ClosestRegion = user.VisibleRegions.First();
            }

            if (user != null) _twitter.Send(user.UserName + "has left the Park!");

            return Request.CreateResponse(HttpStatusCode.Created, regionId.ToString());
        }

        [Route("signalupdateclosest/{regionId}/{userName}/{uuid}/{receivedSignalStrength:double}")]
        [HttpPost]
        [HttpGet]
        public HttpResponseMessage SignalUpdate(RegionIds regionId, string userName, string uuid,
            double receivedSignalStrength)
        {
            var user = _visibleUsers.FirstOrDefault(u => u.UserName == userName);
            user = AddUser(userName, user);
            user.ClosestBeacon = new Beacon
            {
                UUID = uuid,
                RegionId = regionId,
                LatestReading = new BeaconReading(DateTime.Now, receivedSignalStrength)
            };

            var matchingRegion = user.VisibleRegions.FirstOrDefault(r => r.Id == regionId);
            if (matchingRegion == null)
            {
                matchingRegion = new Region(regionId)
                {
                    EnteredAt = DateTime.Now
                };
                user.VisibleRegions.Add(matchingRegion);
            }
            user.ClosestRegion = matchingRegion;

            return Request.CreateResponse(HttpStatusCode.OK, "OK");
        }
    }
}