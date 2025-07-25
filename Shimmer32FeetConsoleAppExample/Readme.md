# Shimmer32FeetConsoleAppExample
This example requires a Shimmer3 device, the low noise accelerometer is enabled. The X axis data is printed to a console.
- **Before using this example create the ShimmerAPI dll by building the ShimmerAPI project**

- **When using this example ensure that the reference to the ShimmerAPI dll is correct**

- Note that the appropriate bluetooth address or comport (Shimmer device) needs to be set in Program.cs before compiling/running the example. When Using USB/Comport, first you will need to connect to BLE and enableUSB via the console app. Once connected there will be a new comport showing via Device Manager when you connect the device via USB. To use the console app example, fill in the comport and remove all uuids. The example gives preference to BLE comms, and only tries the comport if no uuids are available to try.

- This example requires the 32 feet library (InTheHand.Net.Personal) in addition to the ShimmerAPI dll
