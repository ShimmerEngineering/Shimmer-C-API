1)that the appropriate bluetooth address or comport (Shimmer device) needs to be set in Program.cs before compiling/running the example. When Using USB/Comport, first you will need to connect to BLE and enableUSB via the console app. Once connected there will be a new comport showing via Device Manager->Ports (COM & LPT) when you connect the device via USB. To use the console app example, fill in the comport and remove all uuids. The example gives preference to BLE comms, and only tries the comport if no uuids are available to try.
2)an option is provided to obtain the comport details (see ConsoleKey.L)
3)note the app requires admin previledge, in order to obtain the list of Shimmer Hardware from the Operating System. See:-

```
ShimmerDevices.PortFilterOption portFilterOption;
portFilterOption = ShimmerDevices.PortFilterOption.All;
string[] ShimmerComPorts = ShimmerDevices.GetComPorts(portFilterOption);
```

