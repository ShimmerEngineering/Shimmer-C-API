using shimmer.Models;
using ShimmerBLEAPI.Communications;
using ShimmerBLEAPI.UWP.Communications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(VerisenseBLEPairing))]
namespace ShimmerBLEAPI.UWP.Communications
{
    public class VerisenseBLEPairing : IVerisenseBLEPairing
    {
        public Task<BondingStatus> BondASMAutomatically(string deviceID)
        {
            System.Console.WriteLine("UWP BOND");
            return null;
        }
    }
}
