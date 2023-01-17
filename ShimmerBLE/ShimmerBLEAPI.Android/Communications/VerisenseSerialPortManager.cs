using shimmer.Communications;
using ShimmerBLEAPI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using ShimmerBLEAPI.Android.Communications;
using Hoho.Android.UsbSerial.Driver;
using Hoho.Android.UsbSerial.Extensions;
using Hoho.Android.UsbSerial.Util;
using Android.Hardware.Usb;
using ShimmerBLEAPI.Devices;

[assembly: Dependency(typeof(VerisenseSerialPortManager))]
namespace ShimmerBLEAPI.Android.Communications
{
    public class VerisenseSerialPortManager : IVerisenseSerialPortManager
    {
        public ObservableCollection<UsbSerialPort> resultCollection = new ObservableCollection<UsbSerialPort>();
        public List<VerisenseSerialDevice> GetListOfSerialDevices()
        {
            List<VerisenseSerialDevice> listOfSerialDevices = new List<VerisenseSerialDevice>();
            foreach (var item in resultCollection)
            {
                var device = item.GetDriver().GetDevice();
                string title = string.Format("Vendor {0} Product {1}",
                    HexDump.ToHexString((short)device.VendorId),
                    HexDump.ToHexString((short)device.ProductId));
                listOfSerialDevices.Add(new VerisenseSerialDevice(title));
            }
            return listOfSerialDevices;
        }

        public async Task<bool> StartScanForSerialPorts()
        {
            resultCollection.Clear();
            if(SerialPortByteCommunicationAndroid.context == null)
            {
                return false;
            }
            UsbManager usbManager = SerialPortByteCommunicationAndroid.context.GetSystemService("usb") as UsbManager;
            var drivers = await FindAllDriversAsync(usbManager);
            foreach (var driver in drivers)
            {
                var ports = driver.Ports;
                foreach (var port in ports)
                    resultCollection.Add(port);
            }
            return true;
        }

        public void StopScanForSerialPorts()
        {
            throw new NotImplementedException();
        }

        public VerisenseBLEDevice CreateVerisenseSerialDevice(string uuid, string serialId)
        {
            return new VerisenseBLEDeviceAndroid(uuid, "SensorName", serialId, VerisenseDevice.CommunicationType.SerialPort);
        }

        private Task<IList<IUsbSerialDriver>> FindAllDriversAsync(UsbManager usbManager)
        {
            var table = UsbSerialProber.DefaultProbeTable;

            //Verisense
            table.AddProduct(SerialPortByteCommunicationAndroid.VID, SerialPortByteCommunicationAndroid.PID, typeof(CdcAcmSerialDriver));

            var prober = new UsbSerialProber(table);

            return prober.FindAllDriversAsync(usbManager);
        }
    }
}
