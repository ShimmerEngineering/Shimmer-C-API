using shimmer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShimmerBLEAPI.Communications
{
    public interface IVerisenseBLEPairing
    {
        Task<BondingStatus> BondASMAutomatically(String deviceID);
    }
}
