# MultiVerisenseExample
The MultiVerisenseExample is used to connect and stream data from Verisense device(s) via Bluetooth. 

## Prerequisites
Your Verisense device is already paired with the OS
**Please make sure ShimmerAPI and ShimmerBLEAPI are built successfully before running MultiVerisenseExample**.

MultiVerisenseExample is broken into the following projects, please refer to the Readme of each project for further info

      1.	MultiVerisenseExample.Android
                  
      2.	MultiVerisenseExample.iOS

      3.	MultiVerisenseExample.UWP



NOTE: The MultiVerisenseExample provides 
- Connect to Verisense device(s)
- Streaming Accel data from Verisense device(s)

HOW TO USE:
  - Add the uuid(s) for each of the device(s) that you want to connect to the variable List<string> uuids in MainPage.xaml.cs in the MultiVerisenseExample project
  - Launch MultiVerisenseExample in Visual Studio (user expected to see these four buttons which are Connect Devices, Start Streaming, Stop Streaming and Disconnect Devices)
  - Press Connect Devices button and observe the output in Visual Studio Output
  - Press Start Streaming button and observe the output in Visual Studio Output
  - After a few minutes or how long user want it to stream, press Stop Streaming button and observe the output in Visual Studio Output
  - Press Disconnect Devices button and observe the output in Visual Studio Output
