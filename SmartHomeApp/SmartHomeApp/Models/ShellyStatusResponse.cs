using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartHomeApp.Models
{
    public class ShellyStatusResponse
    {

        public List<Meter> meters { get; set; }
        public class Meter
        {
            public double power { get; set; }
        }

        public List<Relay> relays { get; set; }
        public class Relay
        {
            [JsonProperty("ison")]  // Verwendung des JsonProperty-Attributs für korrektes Mapping
            public bool isOn { get; set; }
        }
    }
}
