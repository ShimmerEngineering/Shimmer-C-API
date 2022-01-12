# Shimmer C# BLE API

## Current Limitations
### Verisense Pairing Key
When using the verisense device you will require pairing the device manually, please get in touch with us to obtain said pairing key. On our road map we plan to release a version of the device which uses a default pairing key.

### Android
- requires the use of VS 2019 we've had compatibility issues with VS 2022

### IOS
- requires the use of iOS 15, due to issues setting the MTU
- ~~requires the use of VS 17.0.0, we've had compatibility issues~~ in order to support VS >17.0.0 we've had to circumvent a (problem)[https://developercommunity2.visualstudio.com/t/XamariniOS-getting-FoundationMonoTouch/1610258?space=8] by (updating)[https://github.com/ShimmerEngineering/xamarin-bluetooth-le/tree/shimmer_dev] and compiling new dlls for iOS
- note that the uuid value for a sensor on UWP/Android vs iOS is different, you can typically retrieve the uuid using any common bluetooth scanning app. We recommend either using nrfconnect or https://github.com/xabre/xamarin-bluetooth-le
- note that uuid value for the same sensor on different iOS devices can be different
- to retrieve the uuid on iOS an app such as Blue - Bluetooth & developers can be used to retrieve the uuid as highlighted below 
![image](https://user-images.githubusercontent.com/2862032/149056918-270fe963-42e2-470a-9dd7-3e6b7be7eeb0.png)


### Binary File Parsing
For solutions to parse your binary files please get in touch with us, at the moment the release of a binary file open source solution is not on our roadmap. 

### Examples
While not provided at the moment it is on our roadmap to provide the following examples
- ble manager (e.g. scanning, and ble auto pairing where applicable (uwp and android))
- s3 cloud manager (e.g. to upload the binary files to the cloud)
