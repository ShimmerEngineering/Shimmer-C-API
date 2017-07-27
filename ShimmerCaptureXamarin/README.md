# ShimmerCaptureXamarin
This is a work in progress, any feedback is appreciated. 
This code serves as an example on how to extend the ShimmerLogAndStream class (used for streaming between the API and a Shimmer Device running LogAndStream Firmware).
- To use example update the bluetooth address in MainActivity.cs

# Instuctions for Compiling Library (ShimmerAPI.dll) for Xamarin

Note the xamarin compiler is not able to cope with OS specific Bluetooth controls via Serial Port or 32Feet/etc. In order to use the code with the Xamarin example provided the DLL has to be created without the inclusion of the following files:-
- Shimmer.cs
- Shimmer32Feet.cs
- ShimmerLogAndStream32Feet.cs
- ShimmerLogAndStreamSystemSerialPort.cs
- ShimmerSDBT.cs
- ShimmerSDBT32Feet.cs

This can be done by setting the build action of the files above to none (e.g. Select Multiple Files ->Properties -> Build Action)
