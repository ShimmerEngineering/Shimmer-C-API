using shimmer.Models;
using ShimmerBLEAPI.Android.Communications;
using ShimmerBLEAPI.Communications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(VerisenseBLEPairing))]
namespace ShimmerBLEAPI.Android.Communications
{
    public class VerisenseBLEPairing : IVerisenseBLEPairing
    {
        public Task<BondingStatus> BondASMAutomatically(string deviceID)
        {
            System.Console.WriteLine("ANDROID BOND");
            return null;
        }
    }
}
