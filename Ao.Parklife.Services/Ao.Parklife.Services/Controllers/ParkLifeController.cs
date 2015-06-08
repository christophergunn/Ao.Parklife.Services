using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Ao.Parklife.Services.Models;
using Newtonsoft.Json;

namespace Ao.Parklife.Services.Controllers
{
    public class ParkLifeController : ApiController
    {
        private readonly Users[] Users =
        {
            new Users
            {
                Id = 1,
                UserName = "Chris",
                Status = ConnectionStatus.Connected,
                Uuid = "445A2A8B-5D25-4364-8300-B4A6E5088518",
                TimeStampTime = new DateTime(2015, 6, 8, 10, 10, 0),
                Regions = new Regions[2],
                ClosestRegion = Regions.Starbucks,
                ReceivedSignalStrength = 5432
            },
            new Users
            {
                Id = 2,
                UserName = "Raj",
                Status = ConnectionStatus.Disconnected,
                Uuid = "F7826DA6-4FA2-4E98-8024-BC5B71E0893E"
            },
            new Users
            {
                Id = 3,
                UserName = "Vince",
                Status = ConnectionStatus.Disconnected,
                Uuid = "A4950001-C5B1-4B44-B512-1370F02D74DE"
            }
        };

        [HttpGet]
        public string GetAllUsers()
        {
            return JsonConvert.SerializeObject(Users);
        }

        
        [HttpPost]
        public HttpResponseMessage EnterRegion(int userId, Regions region)
        {
            var user = Users.FirstOrDefault(x => x.Id == userId);
            if (user != null) user.ClosestRegion = region;

            var response = Request.CreateResponse<string>(System.Net.HttpStatusCode.Created, region.ToString());

            return response;
        }
    }
}