# BLECommunicationConsole

The BLECommunicationConsole provides byte level BLE functionality with the Verisensdevice. 

The BLECommunicationConsole provides
- Connect to Verisense device(s)
- Write and Read Bytes

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
