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

# BytesParserApp
This app is to help users diagnose/check/correct/interpret opconfig bytes, status bytes and production config bytes. Note that the header is omitted 5A is omitted. 

## To run in Windows:
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
- WRITE_OPCONFIG
  - must include the operational config bytes separated with dash as argument
- WRITE_DEFAULT_OPCONFIG
  - the available default operational configurations are ACCEL1, ACCEL2_GYRO, GSR_BATT_ACCEL1, GSR_BATT, PPG
- ERASE_DATA
- DISABLE_LOGGING

e.g. 
```
start VerisenseConfigureAndSyncConsole.exe 00000000-0000-0000-0000-d02b463da2bb DATA_SYNC C:\\Users\\UserName\\Desktop trialA participantB
```

## To run in Linux:
- Ensure .NET is installed, otherwise can refer to this [guide](https://docs.microsoft.com/en-us/dotnet/core/install/linux-ubuntu) for how to install .NET
- Download the project
- Open the terminal and navigate to ShimmerBLE/ShimmerBLEAPI directory
- Run (or you can update the PropertyChanged.Fody version in ShimmerBLEAPI.csproj)
```
dotnet add ShimmerBLEAPI.csproj package PropertyChanged.Fody --version 3.4.0
```
- Navigate to ShimmerBLE/ConsoleTools/VerisenseConfigureAndSyncConsole directory
- Run
```
dotnet run  [-uuid] [-options] [args...]
```
where options include
- DATA_SYNC (with the following three optional arguments)
  - bin file path
  - trial name
  - participant id
- WRITE_OPCONFIG
  - must include the operational config bytes separated with dash as argument
- WRITE_DEFAULT_OPCONFIG
  - the available default operational configurations are ACCEL1, ACCEL2_GYRO, GSR_BATT_ACCEL1, GSR_BATT, PPG
- ERASE_DATA
- DISABLE_LOGGING

e.g. 
```
dotnet run 00000000-0000-0000-0000-d02b463da2bb DATA_SYNC
```

## Prerequisites
**Please make sure ShimmerAPI and ShimmerBLEAPI are built successfully before run BLECommunicationConsole**.
