using ShimmerBLEAPI.Communications;
using System;

namespace ShimmerAdvanceBLEAPI
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