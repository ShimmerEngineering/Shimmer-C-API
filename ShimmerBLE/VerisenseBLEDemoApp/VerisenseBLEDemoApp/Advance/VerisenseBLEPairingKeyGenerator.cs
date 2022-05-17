using ShimmerBLEAPI.Communications;
using System;
using System.Collections.Generic;
using System.Text;

namespace VerisenseBLEDemoApp.Advance
{
    public class VerisenseBLEPairingKeyGenerator : IBLEPairingKeyGenerator
    {
        public string CalculatePairingPin(string deviceName)
        {
            string defaultPin = "123456";
            return defaultPin;
        }
    }
}