using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using X2CodingLab.SensorTag;
using X2CodingLab.SensorTag.Exceptions;
using X2CodingLab.SensorTag.Sensors;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BLEFileTransfer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void btnShowDevices_Click(object sender, RoutedEventArgs e)
        {
            Exception exc = null;

            try
            {
                using (DeviceInfoService dis = new DeviceInfoService())
                {

                    List<DeviceInformation> list = await GattUtils.GetDevicesOfService(dis.SensorServiceUuid);
                    if (list != null)
                    {
                        String deviceInfo;
                        textBox.Text = "Supported devices: " + list.Count + "\n";
                        foreach (DeviceInformation dI in list)
                        {
                            deviceInfo= "Device Name: " + dI.Name + "\nIs Paired: " + dI.Pairing.IsPaired + "\n";
                            textBox.Text += deviceInfo;
                        }
                    }
                    else
                        textBox.Text = string.Empty;
                }
            }

            catch (Exception ex)
            {
                exc = ex;
            }

            if (exc != null)
                await new MessageDialog(exc.Message).ShowAsync();
        }

    }
}
