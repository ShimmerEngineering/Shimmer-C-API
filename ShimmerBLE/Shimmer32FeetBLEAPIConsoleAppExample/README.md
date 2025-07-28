Please note the app requires admin previledge, in order to obtain the list of Shimmer Hardware from the Operating System. See:-

```
ShimmerDevices.PortFilterOption portFilterOption;
portFilterOption = ShimmerDevices.PortFilterOption.All;
string[] ShimmerComPorts = ShimmerDevices.GetComPorts(portFilterOption);
```

