1) The appropriate bluetooth address or comport (Shimmer device) needs to be set in Program.cs before compiling/running the example. When Using USB/Comport, first you will need to connect to BLE and enableUSB via the console app. Once connected there will be a new comport showing via Device Manager->Ports (COM & LPT) when you connect the device via USB (_ensure you are using a data capable USB cable, also note in point 2 below you can retrieve the info via this console app_). To use the comport/USB in this console app example, fill in the comport and remove all uuids (_e.g. below_). The example gives preference to BLE comms, and only tries the comport if no uuids are available to try.
```
        static List<string> uuids = new List<string>()
        {

        };

        static List<string> comPorts = new List<string>()
        {
            "COM41"
        };
            
```

2) An option is provided to obtain the comport details (see ConsoleKey.L)

3) Note the app requires admin previledge, in order to obtain the list of Shimmer Hardware from the Operating System. See:-

```
ShimmerDevices.PortFilterOption portFilterOption;
portFilterOption = ShimmerDevices.PortFilterOption.All;
string[] ShimmerComPorts = ShimmerDevices.GetComPorts(portFilterOption);
```
4) There is functionality included to automatically list the Verisense device (Com Port) when it is USB connected, should USB be enabled.
