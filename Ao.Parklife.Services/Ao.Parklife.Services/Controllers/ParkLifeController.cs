using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Ao.Parklife.Services.Models;
using Newtonsoft.Json;

namespace Ao.Parklife.Services.Controllers
{
    [RoutePrefix("api")]
    public class ParkLifeController : ApiController
    {
        private readonly Users[] Users =
        {
            new Users
            {
                UserName = "Chris",
                Status = ConnectionStatus.Connected,
                Uuid = "445A2A8B-5D25-4364-8300-B4A6E5088518",
                TimeStampTime = new DateTime(2015, 6, 8, 10, 10, 0),
                ClosestRegion = Regions.Starbucks,
                ReceivedSignalStrength = 5432
            },
            new Users
            {
                UserName = "Raj",
                Status = ConnectionStatus.Disconnected,
                Uuid = "F7826DA6-4FA2-4E98-8024-BC5B71E0893E"
            },
            new Users
            {
                UserName = "Vince",
                Status = ConnectionStatus.Disconnected,
                Uuid = "A4950001-C5B1-4B44-B512-1370F02D74DE"
            }
        };

        [HttpGet]
        [Route("GetAll")]
        public string GetAllUsers()
        {
            return JsonConvert.SerializeObject(Users);
        }

        [Route("enterregion/{region}/{userid}")]
        [HttpGet]
        [HttpPost]
        public HttpResponseMessage EnterRegion(Regions region, string userid)
        {
            var user = Users.FirstOrDefault(x => x.UserName == userid);
            if (user != null && !user.Regions.Contains(region)) 
                user.Regions.Add(region);

            var response = Request.CreateResponse(System.Net.HttpStatusCode.Created, region.ToString());

            return response;
        }

        [Route("exitregion/{region}/{userid}")]
        [HttpPost]
        [HttpGet]
        public HttpResponseMessage ExitRegion(Regions region, string userid)
        {
            var user = Users.FirstOrDefault(x => x.UserName == userid);
            if (user != null && user.Regions != null)
            {
                user.Regions.Remove(region);
            }

            var response = Request.CreateResponse<string>(System.Net.HttpStatusCode.Created, region.ToString());

            return response;
        }
    }
}