using System;
using System.Collections.Generic;
using System.Text;

namespace ShimmerBLEAPI.Models
{
    public class VerisenseSerialDevice
    {
        public string Id { get; set; }

        public VerisenseSerialDevice(string id)
        {
            Id = id;
        }
    }
}
