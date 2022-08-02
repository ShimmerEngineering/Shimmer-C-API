using shimmer.Communications;
using ShimmerBLEAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using ShimmerBLEAPI.Android.Communications;
using Hoho.Android.UsbSerial.Driver;
using Hoho.Android.UsbSerial.Extensions;
using Android.Hardware.Usb;

[assembly: Dependency(typeof(VerisenseSerialPortManager))]
namespace ShimmerBLEAPI.Android.Communications
{
    public class VerisenseSerialPortManager : IVerisenseSerialPortManager
    {
        public List<VerisenseSerialDevice> GetListOfSerialDevices()
        {
            throw new NotImplementedException();
        }

        public Task<bool> StartScanForSerialPorts()
        {
            throw new NotImplementedException();
        }

        public void StopScanForSerialPorts()
        {
            throw new NotImplementedException();
        }

        internal static Task<IList<IUsbSerialDriver>> FindAllDriversAsync(UsbManager usbManager)
        {
            var table = UsbSerialProber.DefaultProbeTable;

            //Verisense
            table.AddProduct(0x1915, 0x520F, typeof(CdcAcmSerialDriver));

            var prober = new UsbSerialProber(table);

            return prober.FindAllDriversAsync(usbManager);
        }
    }
}
