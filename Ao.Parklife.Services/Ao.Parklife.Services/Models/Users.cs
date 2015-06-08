using System;
using System.Collections.Generic;

namespace Ao.Parklife.Services.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Uuid { get; set; }
        public ConnectionStatus Status { get; set; }
        public DateTime TimeStampTime { get; set; }
        public List<Regions> Regions { get; set; }
        public Regions ClosestRegion { get; set; }
        public int ReceivedSignalStrength { get; set; }
    }

    public enum Regions
    {
        NotInPark,
        Starbucks,
        FoodCounter,
        TeaPoint,
        GamesArea,
        TelephoneBooth
    };

    public enum ConnectionStatus
    {
        Connected,
        Disconnected
    };
}