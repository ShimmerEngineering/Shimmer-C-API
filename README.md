# Shimmer-C-API
The Shimmer C# API is used to control and stream data from a Shimmer3 Bluetooth Device running LogAndStream firmware. **First open the ShimmerAPI solution. The examples will be opened along with the ShimmerAPI project. Build the ShimmerAPI first before proceeding to using the examples as the examples references the ShimmerAPI dll**.

This API is broken into the following projects, please refer to the Readme of each project for further info

      1.	ShimmerAPI
                  
      2.	ShimmerCapture
      
      3.	ShimmerComPortParsingExample
      
      4.	ShimmerConsoleAppExample
      
      5.    ShimmerECGConsoleAppExample
      
      6.    ShimmerEXGConsoleAppExample
      
      7.    Shimmer32FeetConsoleAppExample
      
      8.    ShimmerPPGHRGSRConsoleAppExample
      
      9.    ShimmerCaptureXamarin

NOTE: Where required the dll for ShimmerClosedLibrary (algorithms) can be found in the libs folder within the ShimmerAPI project. Where required the following nuget packages are used:-
- MathNet.Numerics which is a ShimmerClosedLibrary dependency
- 32feet.NET which is required when using the 32 feet library (ShimmerLogAndStream32Feet.cs , etc)

NOTE: The ShimmerClosedLibrary provides 
- ECG to Heart Rate/Inter Beat Interval(IBI) algorithm
- PPG to Heart Rate/Inter Beat Interval(IBI) algorithm

User's should note that IBI derived from ECG is more accurate than IBI derived from PPG.

# Rev 1.0.1
- Bugfix for #14 (internal reference: CCF18-036)

# Rev 1.0.0
- Added functionality for FW_IDENTIFIER_SHIMMERECGMD
- Started adding unit test
- Improve GSR algorithm accuracy
- Fix to expansion board ID
- Improve Bluetooth reliability, with previous uncaught exceptions being dealt with
- Update project structure, when users open the ShimmerAPI solution, the other examples will be loaded as well, users will still need to build the ShimmerAPI dll to use the examples

# Rev 0.9
- Removed the double representation of the firmware version (FirmwareVersion) as there is no way to represent 0.1 (_where 0 is FirmwareMajor and 1 is FirmwareMinor_) and 0.10 (_where 0 is FirmwareMajor and 10 is FirmwareMinor_) as a double. FirmwareMajor and FirmwareMinor variables remain. Added two methods compareVersions which allows quick version comparisons

# Rev 0.8
- Updated ECG to HR and PPG to HR algorithms , now both support IBI
- Updated 3D orientation example making it more user friendly
- Added ShimmerConsoleTest which is a recursive test which: connect-stream-stop-disconnect-repeat
- Fix to Low Power Mag not functioning
- Updated the EXG console examples making it clearer and more user friendly
- Added 3 byte time stamp support for internal (development) hardware and firmware

# REV 0.7
- BTStream Firmware Is No Longer officially supported
- Shimmer, Shimmer32Feet, ShimmerSDBT and ShimmerSDBT32Feet has been deprecated, please see ShimmerLogAndStreamSystemSerialPort and ShimmerLogAndStream32Feet, and ShimmerLogAndStreamXamarin (Within the ShimmerCaptureXamarin Project), restructuring was driven by the need to reduce unsustainable code duplicates as well as Xamarin not being able to handle Bluetooth implementations (e.g. system serial port/32 feet)
- Consolidating duplicates of code in ShimmerSDBT and ShimmerSDBT32Feet into ShimmerLogAndStream
- Major restructuring to code, ensuring platform specific bluetooth controls are strictly seperated between ShimmerLogAndStream and classes which extend it ShimmerLogAndStreamSystemSerialPort/ShimmerLogAndStream32Feet/ShimmerLogAndStreamXamarin
- Updates to API to support the updated Shimmer3 device whose sensors have been updated to the following

      1.	Pressure sensor:		BMP180 → BMP280
      
      2.	Gyroscope + mag:		MPU9150 → MPU9250
      
      3.	Low-Noise Accel:		KXRB5-2042 → KXTC9-2050
      
      4.	Wide-Range Accel:		LSM303DLHC → LSM303AHTR

- Added support for Xamarin 
- Fixes when using 32 Feet

# REV 0.6
- fix to calibrated time stamp when using 3 byte raw time stamp (e.g. LogAndStream 0.6)

# REV 0.5
- major updates to allow API to work with LogAndStream 0.6 and BtStream 0.8, 3 byte raw timestamp

# REV 0.4
- minor update to packet loss detection, increasing the limit to 10%
- update to writesamplingrate makes sure internal sensor rates are approximately close/higher than shimmer sampling rate

# REV 0.3.2
- Fix to filter, fix to to exg, gui failing when custom gain is used 
- Currently uses ShimmerClosedLibraryRev0_4

# REV 0.3.1
- Minor fix to ppgtohr reset
- Currently uses ShimmerClosedLibraryRev0_4

# REV 0.3
- Major update to ecgtohr algorithm and filtering algorithm, minor update to ppgtohr algorithm, user should see major improvements in both ecgtohr and ppgtohr algorithms.
- Currently uses ShimmerClosedLibraryRev0_3

# Linux Mono
1) In order to use the API on Linux, please change the .net framework to 4.5. Mono is required and can be installed via the following command 
*sudo apt-get install mono-complete*
2) The following command can be used to build the c# project 
*xbuild ShimmerCapture.sln*
3) To run the application use the following command
*sudo mono ShimmerCapture\ vX.X.exe*
4) Note without the **sudo** command when running the application connecting to the Bluetooth device **will not work**

# Linux Bluetooth Pairing
1) first install Bluetooth Manager : sudo apt-get install blueman
2) next open Bluetooth Manager and search for devices
3) right click on the device you want to use and choose pair, note the Bluetooth pairing key is 1234
4) once paired click setup, and connect to serial port, at this stage you should see the Bluetooth come on permanently on the Shimmer device, also you should see a pop up indicating /dev/rfcommx
5) now you can open the exe and connect to that port /dev/rfcommx

# The Following Applies To All Code Provided in the repository
Copyright (c) 2014, Shimmer Research, Ltd.
 All rights reserved

 Redistribution and use in source and binary forms, with or without
 modification, are permitted provided that the following conditions are
 met:

     * Redistributions of source code must retain the above copyright
       notice, this list of conditions and the following disclaimer.
     * Redistributions in binary form must reproduce the above
       copyright notice, this list of conditions and the following
       disclaimer in the documentation and/or other materials provided
       with the distribution.
     * Neither the name of Shimmer Research, Ltd. nor the names of its
       contributors may be used to endorse or promote products derived
       from this software without specific prior written permission.

 THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
 "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
 LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
 A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
 OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
 SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
 LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
 OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
