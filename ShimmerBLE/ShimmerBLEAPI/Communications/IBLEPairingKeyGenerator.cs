using System;
using System.Collections.Generic;
using System.Text;

namespace ShimmerBLEAPI.Communications
{
    public interface IBLEPairingKeyGenerator
    {
        string CalculatePairingPin(string deviceName);
    }
}
