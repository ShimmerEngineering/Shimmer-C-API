using SensorTagLibrary.Test.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
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
namespace SensorTagLibrary.Test
{
    public sealed partial class MainPage : Page
    {
        Accelerometer acc;
        Gyroscope gyro;
        HumiditySensor hum;
        SimpleKeyService ks;
        Magnetometer mg;
        PressureSensor ps;
        IRTemperatureSensor tempSen;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void SetupSensors(bool serviceAsParameter)
        {
            pbar.Visibility = Windows.UI.Xaml.Visibility.Visible;
            ClearSensors();
            btnSetup.IsEnabled = false;
            btnSetupParam.IsEnabled = false;
            spTestButtons.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            Exception ex = null;
            try
            {
                if (serviceAsParameter)
                {
                    acc = new Accelerometer();
                    acc.SensorValueChanged += SensorValueChanged;
                    await acc.Initialize((await GattUtils.GetDevicesOfService(acc.SensorServiceUuid))[0]);
                    await acc.EnableSensor();

                    gyro = new Gyroscope();
                    gyro.SensorValueChanged += SensorValueChanged;
                    await gyro.Initialize((await GattUtils.GetDevicesOfService(gyro.SensorServiceUuid))[0]);
                    await gyro.EnableSensor();

                    hum = new HumiditySensor();
                    hum.SensorValueChanged += SensorValueChanged;
                    await hum.Initialize((await GattUtils.GetDevicesOfService(hum.SensorServiceUuid))[0]);
                    await hum.EnableSensor();

                    ks = new SimpleKeyService();
                    ks.SensorValueChanged += SensorValueChanged;
                    await ks.Initialize((await GattUtils.GetDevicesOfService(ks.SensorServiceUuid))[0]);
                    await ks.EnableSensor();

                    mg = new Magnetometer();
                    mg.SensorValueChanged += SensorValueChanged;
                    await mg.Initialize((await GattUtils.GetDevicesOfService(mg.SensorServiceUuid))[0]);
                    await mg.EnableSensor();

                    ps = new PressureSensor();
                    ps.SensorValueChanged += SensorValueChanged;
                    await ps.Initialize((await GattUtils.GetDevicesOfService(ps.SensorServiceUuid))[0]);
                    await ps.EnableSensor();

                    tempSen = new IRTemperatureSensor();
                    tempSen.SensorValueChanged += SensorValueChanged;
                    await tempSen.Initialize((await GattUtils.GetDevicesOfService(tempSen.SensorServiceUuid))[0]);
                    await tempSen.EnableSensor();

                    spTestButtons.Visibility = Windows.UI.Xaml.Visibility.Visible;
                }
                else
                {
                    acc = new Accelerometer();
                    acc.SensorValueChanged += SensorValueChanged;
                    await acc.Initialize();
                    await acc.EnableSensor();

                    gyro = new Gyroscope();
                    gyro.SensorValueChanged += SensorValueChanged;
                    await gyro.Initialize();
                    await gyro.EnableSensor();

                    hum = new HumiditySensor();
                    hum.SensorValueChanged += SensorValueChanged;
                    await hum.Initialize();
                    await hum.EnableSensor();

                    ks = new SimpleKeyService();
                    ks.SensorValueChanged += SensorValueChanged;
                    await ks.Initialize();
                    await ks.EnableSensor();

                    mg = new Magnetometer();
                    mg.SensorValueChanged += SensorValueChanged;
                    await mg.Initialize();
                    await mg.EnableSensor();

                    ps = new PressureSensor();
                    ps.SensorValueChanged += SensorValueChanged;
                    await ps.Initialize();
                    await ps.EnableSensor();

                    tempSen = new IRTemperatureSensor();
                    tempSen.SensorValueChanged += SensorValueChanged;
                    await tempSen.Initialize();
                    await tempSen.EnableSensor();

                    spTestButtons.Visibility = Windows.UI.Xaml.Visibility.Visible;
                }
            }
            catch (Exception exc)
            {
                ex = exc;
            }

            if (ex != null)
                await new MessageDialog(ex.Message).ShowAsync();

            pbar.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            btnSetup.IsEnabled = true;
            btnSetupParam.IsEnabled = true;
        }

        async void SensorValueChanged(object sender, X2CodingLab.SensorTag.SensorValueChangedEventArgs e)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                {
                    switch (e.Origin)
                    {
                        case SensorName.Accelerometer:
                            double[] accValues = Accelerometer.CalculateCoordinates(e.RawData, 1 / 64.0);
                            tbAccelerometer.Text = "X: " + accValues[0].ToString("0.00") + " Y: " + accValues[1].ToString("0.00") + " Z: " + accValues[2].ToString("0.00");
                            break;
                        case SensorName.Gyroscope:
                            float[] axisValues = Gyroscope.CalculateAxisValue(e.RawData, GyroscopeAxis.XYZ);
                            tbGyroscope.Text = "X: " + axisValues[0].ToString("0.00") + " Y: " + axisValues[1].ToString("0.00") + " Z: " + axisValues[2].ToString("0.00");
                            break;
                        case SensorName.HumiditySensor:
                            tbHumidity.Text = HumiditySensor.CalculateHumidityInPercent(e.RawData).ToString("0.00") + "%";
                            break;
                        case SensorName.Magnetometer:
                            float[] magnetValues = Magnetometer.CalculateCoordinates(e.RawData);
                            tbMagnetometer.Text = "X: " + magnetValues[0].ToString("0.00") + " Y: " + magnetValues[1].ToString("0.00") + " Z: " + magnetValues[2].ToString("0.00");
                            break;
                        case SensorName.PressureSensor:
                            try
                            {
                            tbPressure.Text = (PressureSensor.CalculatePressure(e.RawData, ps.CalibrationData) / 100).ToString("0.00");
                            }
                            catch(NullReferenceException)
                            {
                                // in case another(!) setup is executed, so ps is null
                            }
                            break;
                        case SensorName.SimpleKeyService:
                            if (SimpleKeyService.LeftKeyHit(e.RawData))
                            {
                                tbLeftKey.Text = "hit!";
                                await Task.Delay(200);
                                tbLeftKey.Text = "";
                            }
                            else if (SimpleKeyService.RightKeyHit(e.RawData))
                            {
                                tbRightKey.Text = "hit!";
                                await Task.Delay(200);
                                tbRightKey.Text = "";
                            }
                            break;
                        case SensorName.TemperatureSensor:
                            double ambient = IRTemperatureSensor.CalculateAmbientTemperature(e.RawData, TemperatureScale.Celsius);
                            double target = IRTemperatureSensor.CalculateTargetTemperature(e.RawData, ambient, TemperatureScale.Celsius);
                            tbTemperature.Text = ambient.ToString("0.00");
                            tbTargetTemperature.Text = target.ToString("0.00");
                            break;
                    }
                });
        }

        private async void btnDisableNotifications_Click(object sender, RoutedEventArgs e)
        {
            await acc.DisableNotifications();
            await gyro.DisableNotifications();
            await hum.DisableNotifications();
            await ks.DisableNotifications();
            await mg.DisableNotifications();
            await ps.DisableNotifications();
            await tempSen.DisableNotifications();
        }

        private async void btnEnableNotifications_Click(object sender, RoutedEventArgs e)
        {
            await acc.EnableNotifications();
            await gyro.EnableNotifications();
            await hum.EnableNotifications();
            await ks.EnableNotifications();
            await mg.EnableNotifications();
            await ps.EnableNotifications();
            await tempSen.EnableNotifications();
        }

        private async void btnReadData_Click(object sender, RoutedEventArgs e)
        {
            byte[] tempValue = await tempSen.ReadValue();
            double ambientTemp = IRTemperatureSensor.CalculateAmbientTemperature(tempValue, TemperatureScale.Celsius);
            double targetTemp = IRTemperatureSensor.CalculateTargetTemperature(tempValue, ambientTemp, TemperatureScale.Celsius);
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                tbTemperature.Text = ambientTemp.ToString("0.00");
                tbTargetTemperature.Text = targetTemp.ToString("0.00");
            });

            byte[] accValue = await acc.ReadValue();
            double[] accAxis = Accelerometer.CalculateCoordinates(accValue, 1 / 64.0);
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                tbAccelerometer.Text = "X: " + accAxis[0].ToString("0.00") + " Y: " + accAxis[1].ToString("0.00") + " Z: " + accAxis[2].ToString("0.00");
            });

            byte[] humValue = await hum.ReadValue();
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                tbHumidity.Text = HumiditySensor.CalculateHumidityInPercent(humValue).ToString("0.00") + "%";
            });
        }

        private async void btnAccelerometer_Click(object sender, RoutedEventArgs e)
        {
            await acc.SetReadPeriod(255);
            await mg.SetReadPeriod(255);
        }

        private async void btnMagnetometer_Click(object sender, RoutedEventArgs e)
        {
            await acc.SetReadPeriod(10);
            await mg.SetReadPeriod(10);
        }

        private async void btnShowSensorTags_Click(object sender, RoutedEventArgs e)
        {
            Accelerometer acc = new Accelerometer();
            List<DeviceInformation> list = await GattUtils.GetDevicesOfService(acc.SensorServiceUuid);
            lbTags.ItemsSource = list;
            if (list != null)
            {
                tbNumberOfTags.Text = "Total: " + list.Count;
            }
            else
                tbNumberOfTags.Text = string.Empty;
        }

        private void btnSetup_Click(object sender, RoutedEventArgs e)
        {
            SetupSensors(false);
        }

        private void btnSetupWithParameter(object sender, RoutedEventArgs e)
        {
            SetupSensors(true);
        }

        private async void btnGetSysid_Click(object sender, RoutedEventArgs e)
        {
            Exception exc = null;

            try
            {
                using (DeviceInfoService dis = new DeviceInfoService())
                {
                    await dis.Initialize();
                    tbSystemId.Text = "System ID: " + await dis.ReadSystemId();
                    tbModelNr.Text = "Model Nr: " + await dis.ReadModelNumber();
                    tbSerielNr.Text = "Serial Nr: " + await dis.ReadSerialNumber();
                    tbFWRev.Text = "Firmware Revision: " + await dis.ReadFirmwareRevision();
                    tbHWRev.Text = "Hardware Revision: " + await dis.ReadHardwareRevision();
                    tbSWRev.Text = "Sofware Revision: " + await dis.ReadSoftwareRevision();
                    tbManufacturerName.Text = "Manufacturer Name: " + await dis.ReadManufacturerName();
                    tbCert.Text = "Cert: " + await dis.ReadCert();
                    tbPNP.Text = "PNP ID: " + await dis.ReadPnpId();
                }
            }
            catch (Exception ex)
            {
                exc = ex;
            }

            if (exc != null)
                await new MessageDialog(exc.Message).ShowAsync();
        }

        private void ClearSensors()
        {
            if(acc != null)
                acc.Dispose();
            if (gyro != null)
                gyro.Dispose();
            if (hum != null)
                hum.Dispose();
            if (ks != null)
                ks.Dispose();
            if (mg != null)
                mg.Dispose();
            if (ps != null)
                ps.Dispose();
            if (tempSen != null)
                tempSen.Dispose();
        }
    }
}