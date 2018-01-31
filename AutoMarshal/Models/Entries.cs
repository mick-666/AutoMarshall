using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMarshal.Models
{
    /*
     Классы моделей журнала
    */
    public class VehicleImages
    {
        public List<int> images { get; set; }
        public int mainImage { get; set; }
    }

    public class VideoChannel
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class LinkField
    {
        public int id { get; set; }
        public string name { get; set; }
        public string displayName { get; set; }
        public string value { get; set; }
    }

    public class Link
    {
        public string name { get; set; }
        public string displayName { get; set; }
        public string color { get; set; }
        public List<LinkField> fields { get; set; }
    }

    public class EntryField
    {
        public int id { get; set; }
        public string name { get; set; }
        public string displayName { get; set; }
        public string value { get; set; }
    }

    public class VehicleType
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string comment { get; set; }
        public double spaceRatio { get; set; }
        public bool isDefault { get; set; }
    }

    public class Entry
    {
        public int id { get; set; }
        public string plate { get; set; }
        public string plateStencilId { get; set; }
        public VehicleImages vehicleImages { get; set; }
        public DateTime timestamp { get; set; }
        public VideoChannel videoChannel { get; set; }
        public object database { get; set; }
        public int status { get; set; }
        public string passage { get; set; }
        public string direction { get; set; }
        public string directionName { get; set; }
        public List<Link> links { get; set; }
        public List<EntryField> fields { get; set; }
        public int vehicleTypeId { get; set; }
        public VehicleType vehicleType { get; set; }
    }

    public class Metadata
    {
        public string snapshotHash { get; set; }
        public int offset { get; set; }
        public int limit { get; set; }
        public int totalCount { get; set; }
    }

    public class RootObject
    {
        public List<Entry> entries { get; set; }
        public Metadata _metadata { get; set; }
    }
}
