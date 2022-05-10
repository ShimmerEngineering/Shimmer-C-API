# TestConsoleBLE

The TestConsoleBLE is used to connect and stream data from Verisense device(s) via Bluetooth. 
**Please make sure ShimmerAPI and ShimmerBLEAPI are built successfully before run TestConsoleBLE**.

The purpose of this example is to show users how to implement their own Radio e.g. RadioPluginWindows

The TestConsoleBLE provides
- Connect to Verisense device(s)
- Streaming Accel data from Verisense device(s)

HOW TO USE:
- Add the uuid(s) for each of the device(s) that you want to connect to the variable List uuids in the Program.cs
- Launch TestConsoleBLE in Visual Studio (user expected to see these four prompt option):
  - Press 'S' to connect with Bluetooth 
  - Press 'D' to start streaming 
  - Press 'C' to stop the streaming 
  - Press 'V' to disconnect with Bluetooth

Note: if the example works you might have to add/reference the following files manually (e.g.)
C:\Program Files (x86)\Windows Kits\10\UnionMetadata\Windows.winmd
C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework.NETCore\v4.5\System.Runtime.WindowsRuntime.dll

Improvements required: 
Currently connect and disconnect method in RadioPluginWindows uses a arbitary delay to wait for the device to connect/disconnect, it would be better to use a taskcompletionsource with a timeout and wait for the handler BluetoothLEDevice.ConnectionStatusChange to be called.