using System.Collections.Generic;
using System.Web.Http;
using Ao.Parklife.Services.Models;

namespace Ao.Parklife.Services.Controllers
{
    public class ParkLifeController : ApiController
    {
        private readonly Users[] Users =
        {
            new Users {Id = 1, Name = "Chris"},
            new Users {Id = 2, Name = "Raj"},
            new Users {Id = 3, Name = "Vince"}
        };

        public Users[] GetAllUsers()
        {
            return Users;
        }
    }
}