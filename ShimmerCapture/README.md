# Shimmer-C-API

# REV0_3
- Major update to ecgtohr algorithm and filtering algorithm, user should see major improvements in both ecgtohr and ppgtohr algorithms.
- Currently uses ShimmerClosedLibraryRev0_2
- updated bytestream sync method , now checks timestamps as well
- update readbytes method, now looks out for ack when start streaming command is sent, avoids rare occasions this is missed


BUILDING NOTES
- If new release create a branch with the release number
- Switch output type to Windows Application
- Change Assembly name accordingly ShimmerCapture v0.X
- Update rev in assemblyinfo.cs (follow rev above)
- Update ShimmerClosedLibrary, remove old dll from libs folder and replace with new, update the reference

When releasing a new version the linux - executable has to be compiled in on Shimmer Live machine. To do this.

1) install vmware player

2) download the latest version of the vmware machine 

S:\archive\Shimmer User Resources\Shimmer2r\SUR Content\Firmware Development\VMWare

3) copy the folder to your linux machine, remove all files from the release and debug folder first

4) in the linux machine open a terminal and go to the folder which has the solution file *.sln

5) enter xbuild

6) the executable should now be in the release folder

7) Note, double check your GUI elements, as the mono compiler seems to use different dimensions for panels 

8) To run the application, navigate to the executable file location and type 'mono ShimmerConnect_V0.13.exe' 


For windows

1) Download project from git
2) Build solution and take executable and dlls from Release folder


When releasing
1) zip folder and put in the SUR folder