using System;
using System.Collections.Generic;
using System.Text;

namespace SmartHomeApp.Models
{
    public class ShellyPowerStatus
    {

        public List<Meter> meters { get; set; }
        public class Meter
        {
            public double power { get; set; }
        }

    }
}
