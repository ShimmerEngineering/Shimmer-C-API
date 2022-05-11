# Shimmer C# BLE API 
The following projects (API) are Alpha Releases, the rest of the projects are Pre-Alpha Releaseas
- ShimmerBLEAPI.Android
- ShimmerBLEAPI.UWP
- ShimmerBLEAPI.iOS
- ShimmerBLEAPI

Please refer to our [support policy](https://shimmersensing.com/support/wireless-sensor-networks-documentation/) on Pre-Alpha/Alpha Releases

## Introduction
This API is to help users get started with using the Verisense device. If you are looking for Shimmer3 support in C# please refer to the [following](https://github.com/ShimmerEngineering/Shimmer-C-API#readme).

## Hardware Supported
- Verisense IMU
- Verisense Pulse+ (_upcoming_)

## Functionality Supported
- Configuring via BLE
- Real Time Sensor Data Streaming via BLE
- Logged Sensor Data Syncing via BLE
_note: syncing is the retrieval of logged sensor data in a binary file fomat. **this binary file can be [parsed using this Java app](https://github.com/ShimmerEngineering/Shimmer-C-API/tree/master/ShimmerBLE/FileParser), however users should be aware that we do not provide file parsing source code in C#**_

## List of Examples
We would recommend reading through the [readme](https://github.com/ShimmerEngineering/Shimmer-C-API/blob/master/ShimmerBLE/VerisenseBLEDemoApp/README.md) of VerisenseBLEDemoApp before proceeding to using any of the examples within this repository.

[BLE.Client (only UWP supported)](https://github.com/ShimmerEngineering/Shimmer-C-API/tree/master/ShimmerBLE/BLE.Client)
_example app to configure, stream sync from a single device via BLE_

[VerisenseBLEDemoApp](https://github.com/ShimmerEngineering/Shimmer-C-API/tree/master/ShimmerBLE/VerisenseBLEDemoApp)
_example app which among other things shows S3 file parsing capabilities_

[PasskeyConfigurationApp](https://github.com/ShimmerEngineering/Shimmer-C-API/tree/master/ShimmerBLE/PasskeyConfigurationApp)
_app to configure the passkey_

[MultiVerisenseExample](https://github.com/ShimmerEngineering/Shimmer-C-API/tree/master/ShimmerBLE/MultiVerisenseExample)
_app showing how to stream from multiple verisense devices_

[Shimmer32FeetBLEAPIConsoleAppExample](https://github.com/ShimmerEngineering/Shimmer-C-API/tree/master/ShimmerBLE/Shimmer32FeetBLEAPIConsoleAppExample)
_app showing how to use 32 feet library_

## Current Limitations
### Verisense Pairing Key
When using the verisense device you will require knowing what the pairing key. Some examples already expect that the verisense devices are already paired. In others where the BLEManager is used, it will provide some support for pairing. Typically you will be allowed to specify whether you want to use the default passkey of 123456 on purchase. Should you have any inquiries contact support. 

### Android
- requires the use of VS 2019 we've had compatibility issues with VS 2022

### IOS
- requires the use of iOS 15, due to issues setting the MTU
- ~~requires the use of VS 17.0.0, we've had compatibility issues~~ in order to support VS >17.0.0 we've had to circumvent a [problem](https://developercommunity2.visualstudio.com/t/XamariniOS-getting-FoundationMonoTouch/1610258?space=8) by [updating](https://github.com/ShimmerEngineering/xamarin-bluetooth-le/tree/shimmer_dev) and compiling new dlls for iOS
- note that the uuid value for a sensor on UWP/Android vs iOS is different, you can typically retrieve the uuid using any common bluetooth scanning app. We recommend either using nrfconnect or https://github.com/xabre/xamarin-bluetooth-le
- note that uuid value for the same sensor on different iOS devices can be different
- to retrieve the uuid on iOS an app such as Blue - Bluetooth & developers can be used to retrieve the uuid as highlighted below 
![image](https://user-images.githubusercontent.com/2862032/149056918-270fe963-42e2-470a-9dd7-3e6b7be7eeb0.png)
- will require manual entry of the pairing key by the user



