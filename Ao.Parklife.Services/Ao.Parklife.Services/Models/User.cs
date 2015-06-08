using System;
using System.Collections.Generic;
using System.Linq;

namespace Ao.Parklife.Services.Models
{
    public enum ConnectionStatus
    {
        Connected,
        Disconnected
    };

    public class User
    {
        public string UserName { get; private set; }
        
        public ConnectionStatus Status 
        { 
            get
            {
                return VisibleRegions.Any() ? ConnectionStatus.Connected : ConnectionStatus.Disconnected;
            } 
        }

        public List<Region> VisibleRegions { get; set; }
        public Region ClosestRegion { get; set; }
        public Beacon ClosestBeacon { get; set; }

        public User(string userName)
        {
            UserName = userName;
            VisibleRegions = new List<Region>();
        }
    }

    public enum RegionIds
    {
        NotInPark,
        Starbucks,
        FoodCounter,
        TeaPoint,
        GamesArea,
        TelephoneBooth
    };

    public class Region
    {
        public RegionIds Id { get; private set; }
        public DateTime EnteredAt { get; set; }

        public List<Beacon> OwnedBeacons { get; set; } 

        public Region(RegionIds id)
        {
            Id = id;
            OwnedBeacons = new List<Beacon>();
        }
    }

    public class Beacon
    {
        public string UUID { get; set; }
        public RegionIds RegionId { get; set; }

        public BeaconReading LatestReading { get; set; }
    }

    public class BeaconReading
    {
        public DateTime TakenAt { get; private set; }
        public double DistanceInMetres { get; private set; }

        public BeaconReading(DateTime takenAt, double distanceInMetres)
        {
            TakenAt = takenAt;
            DistanceInMetres = distanceInMetres;
        }
    }
}