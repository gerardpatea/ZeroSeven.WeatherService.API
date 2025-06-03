using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroSeven.WillyWeather.Client.Models
{
    //Auto-Generated from https://json2csharp.com/
    public class Day
    {
        [JsonProperty("dateTime")]
        public string DateTime { get; set; }

        [JsonProperty("entries")]
        public List<Entry> Entries { get; set; }
    }

    public class Entry
    {
        [JsonProperty("dateTime")]
        public string DateTime { get; set; }

        [JsonProperty("precisCode")]
        public string PrecisCode { get; set; }

        [JsonProperty("precis")]
        public string Precis { get; set; }

        [JsonProperty("precisOverlayCode")]
        public string PrecisOverlayCode { get; set; }

        [JsonProperty("night")]
        public bool Night { get; set; }

        [JsonProperty("min")]
        public int Min { get; set; }

        [JsonProperty("max")]
        public int Max { get; set; }
    }

    public class Forecasts
    {
        [JsonProperty("weather")]
        public Weather Weather { get; set; }
    }

    public class Location
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("postcode")]
        public string Postcode { get; set; }

        [JsonProperty("timeZone")]
        public string TimeZone { get; set; }

        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lng")]
        public double Lng { get; set; }

        [JsonProperty("typeId")]
        public int TypeId { get; set; }
    }

    public class Root
    {
        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("forecasts")]
        public Forecasts Forecasts { get; set; }
    }

    public class Units
    {
        [JsonProperty("temperature")]
        public string Temperature { get; set; }
    }

    public class Weather
    {
        [JsonProperty("days")]
        public List<Day> Days { get; set; }

        [JsonProperty("units")]
        public Units Units { get; set; }

        [JsonProperty("issueDateTime")]
        public string IssueDateTime { get; set; }
    }
}
