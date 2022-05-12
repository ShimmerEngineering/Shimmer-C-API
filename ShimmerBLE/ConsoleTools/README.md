# BLECommunicationConsole

The BLECommunicationConsole is used to connect and stream data from Verisense device(s) via Bluetooth. 

The purpose of this example is to show users how to implement their own Radio e.g. RadioPluginWindows.

The BLECommunicationConsole provides
- Connect to Verisense device(s)
- Streaming Accel data from Verisense device(s)

HOW TO USE:
- Add the uuid(s) for each of the device(s) that you want to connect to the variable List uuids in the Program.cs
- Launch TestConsoleBLE in Visual Studio (user expected to see these four prompt option):
  - Press 'S' to connect with Bluetooth 
  - Press 'D' to start streaming 
  - Press 'C' to stop the streaming 
  - Press 'V' to disconnect with Bluetooth

Improvements required: 
Currently connect and disconnect method in RadioPluginWindows uses a arbitary delay to wait for the device to connect/disconnect, it would be better to use a taskcompletionsource with a timeout and wait for the handler BluetoothLEDevice.ConnectionStatusChange to be called.

# VerisenseConfigureAndSyncConsole

The VerisenseConfigureAndSyncConsole is used to configure the Verisense device and synchronize the data.

The purpose of this example is to allow users to configure and sync their Verisense device.

The VerisenseConfigureAndSyncConsole provides
- Configure the Verisense device
- Run data sync

HOW TO USE:
- Build the project
- Navigate to the exe directory in command prompt
- Run 
```
start VerisenseConfigureAndSyncConsole.exe [-uuid] [-options] [args...]
```
where options include
- DATA_SYNC (with the following three optional arguments)
  - bin file path
  - trial name
  - participant id
- WRITE_OP_CONFIG
  - must include the operational config bytes separated with dash as argument

e.g. 
```
start VerisenseConfigureAndSyncConsole.exe 00000000-0000-0000-0000-d02b463da2bb DATA_SYNC C:\\Users\\UserName\\Desktop trialA participantB
```
## Prerequisites
**Please make sure ShimmerAPI and ShimmerBLEAPI are built successfully before run BLECommunicationConsole**.
