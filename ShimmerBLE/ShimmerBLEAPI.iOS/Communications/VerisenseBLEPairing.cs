using shimmer.Models;
using ShimmerBLEAPI.Communications;
using ShimmerBLEAPI.iOS.Communications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(VerisenseBLEPairing))]
namespace ShimmerBLEAPI.iOS.Communications
{
    public class VerisenseBLEPairing : IVerisenseBLEPairing
    {
        public Task<BondingStatus> BondASMAutomatically(string deviceID)
        {
            throw new NotImplementedException();
        }
    }
}
