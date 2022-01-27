# ShimmerVerisenseBLEDemo

This demonstrates the use of the ShimmerBLEAPI. The following dlls are to be noted:-

VerisenseBLEDemoApp project
- Plugin.BLE.Abstractions.dll
- Plugin.BLE.dll (_note this is different from the file listed below_)

VerisenseBLEDemoApp.UWP project
- Plugin.BLE.Abstractions.dll
- Plugin.BLE.dll (_note this is different from the file listed above_)

Also note the nuget packages in the demo, among the important ones are:-
- Microsoft.Toolkit.Uwp.Connectivity
- AWSSDK.S3

**_Note that the Plugin BLE library for the moment is built [using](https://github.com/xabre/xamarin-bluetooth-le/tree/uwp-support), currently there is a fix on the branch not pushed to [nuget](https://www.nuget.org/packages/Plugin.BLE/) hence the reason we are currently using the dlls from the github repository_**

The purpose of this demo is mostly to illustrate the use of four different interfaces
- ISensor
- IVersenseBLEManager
- IVerisenseBLE
- ICloudManager

For the purpose of the demo, the four Interfaces above have been implemented by the following classes
- S3CloudManager
- SensorGSR, SensorLIS2DW12, SensorLSM6DS3
- VerisenseBLEDevice
- VerisenseBLEManager

The important thing to take note are the EventHandlers
- BLEManagerEvent 
- CloudManagerEvent
- ShimmerBLEEvent

# IVerisenseBLEManager
This interface determines how Bluetooth scanning and pairing will be done. In the example this interface is implemented by VerisenseBLEManager. Key events to take note off are 
```
BLEManagerEvent.BLEAdapterEvent.ScanCompleted
BLEManagerEvent.BLEAdapterEvent.DevicePaired

```
Note in the example on the app layer the Verisense Device with the largest RSS will be selected for pairing.
## Specifying the pairing key
The pairing key can be updated via VerisenseBLEPairingKeyGenerator.cs

# IVerisenseBLE
This interface determines how communication with the Verisense sensor will be conducted over Bluetooth Low Energy. In the example this interface is implemented by VerisenseBLEDevice. Key events to take note off are
```
//below signals the succesful completion of the syncing process, where logged data from the verisense sensor is transmitted back to the application
VerisenseBLEEvent.SyncLoggedDataComplete
//when the bluetooth state of the device changes the following event is called
VerisenseBLEEvent.StateChange
//when there is a new sensor data packet received from the physical Verisense device
VerisenseBLEEvent.NewDataPacket
             
```

# ICloudManager
This interface determines how bin files will be uploaded to the S3 cloud. Bin files are raw sensor data which have been downloaded/synced between the app and the verisense sensor. In the example this interface is implemented by S3CloudManager. Key event to take note off are
```
CloudManagerEvent.CloudEvent.UploadSuccessful
```

# ISensor
This interface determines how the individual sensors on the Verisense device can be configured. The idea will be to create a 'clone' of the verisense device, update the sensor/setting within the clone. Generate the operation config bytes and transmit said bytes to the physical Verisense device.
```
//create a clone, new settings are applied to the clone
VerisenseBLEDevice cloneDevice = new VerisenseBLEDevice(verisenseBLEDevice);
var sensor = cloneDevice.GetSensor(SensorLIS2DW12.SensorName);
((SensorLIS2DW12)sensor).SetAccelEnabled(true);
((SensorLIS2DW12)sensor).SetAccelRate(SensorLIS2DW12.LowPerformanceAccelSamplingRate.Freq_25Hz);
sensor = cloneDevice.GetSensor(SensorGSR.SensorName);
((SensorGSR)sensor).SetGSREnabled(false);
((SensorGSR)sensor).SetBattEnabled(false);
sensor = cloneDevice.GetSensor(SensorLSM6DS3.SensorName);
((SensorLSM6DS3)sensor).SetAccelEnabled(false);
((SensorLSM6DS3)sensor).SetGyroEnabled(false);
//once the clone is updated with the new settings we generate the new configuration bytes to be sent over Bluetooth
byte[] opconfigBytes = cloneDevice.GenerateConfigurationBytes();
verisenseBLEDevice.ExecuteRequest(RequestType.WriteOperationalConfig, opconfigBytes);
```

# Demo
Note the purpose of this demo is to give users an idea how to interact with the various components required to develop a succesful application which can leverage the verisense device. In order for the demo to work you will require an S3 account and bucket for the binary files to be uploaded to. Once the binary file is succesfully uploaded the binary file will be parsed into a csv file and placed in the same S3 bucket. To setup an S3 account with binary file parsing capabilities please contact us for further details. We have future plans to make use of the FWVersion, but for now this is just a placeholder. The file name should be VerisenseAPIDemoSettings.json
```
{
	"S3CloudInfo": {
 	   "S3AccessKey" : "",
 	   "S3SecretKey" : "",
  	   "S3RegionName" : "",
   	   "S3BucketName" : "verisense-api-demo"
	},
	
	"FWVersion" : "1.2.72"    
}
```
## Windows
Place the VerisenseAPIDemoSettings.json file in the localstate folder of your package. 
```
ApplicationData.Current.LocalFolder.Path
```
If you are not sure where the location is, just run the app and you will be greeted with the following warning.
![2021-07-28_22h41_51](https://user-images.githubusercontent.com/2862032/127342579-39797cf1-292a-45ed-9848-1bff48bff329.png)
## Android
Please the VerisenseAPIDemoSettings.json file in the public directory called Document. 
```
Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments).Path;
```

## iOS
Place the VerisenseAPIDemoSettings.json file in the application folder right **before** launching the app. 
```
Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
```
![image](https://user-images.githubusercontent.com/2862032/149460315-76b4318f-a376-4bd7-b822-1996435a8bef.png)
![image](https://user-images.githubusercontent.com/2862032/149460539-e4701257-9d2a-4cdb-9a2a-9d1ef3e8bca5.png)
_Note you will require entering the pairing key manually_

## Demo Video
The video below shows the full demo process where
1) First a Bluetooth scan is done
2) The closest (RSS) Verisense device is paired to
3) Said device is connected to and configured
4) Device data streaming and stopped
5) Device sync is done where logged data is transfered to the PC and saved as a binary file
6) The binary file is uploaded to the S3 server where is it stored and parsed into CSV format

https://user-images.githubusercontent.com/2862032/127337040-3fc9c18c-a176-489d-93a5-b20754e30b0f.mp4

#Additional Notes
Currently the auto reconnect logic is located on the app layer, we may move it into the library layer in the future

*Note Shimmer reserves the right to make any updates which may result in the change of method/enum/class names.*
