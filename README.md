# Shimmer-C-API
The Shimmer C# API is used to control and stream data from a Shimmer3 Bluetooth Device running LogAndStream firmware.
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

NOTE: Where required the dlls for ShimmerClosedLibrary (algorithms), InTheHand, MathNet.Numerics can be found in the libs folder in the ShimmerAPI project

# REV 0.7
- BTStream Firmware Is No Longer officially supported
- Shimmer, Shimmer32Feet, ShimmerSDBT and ShimmerSDBT32Feet has been deprecated, please see ShimmerLogAndStreamSystemSerialPort and ShimmerLogAndStream32Feet, and ShimmerLogAndStreamXamarin (Within the ShimmerCaptureXamarin Project), restructuring was driven by Xamarin not being able to handle Bluetooth implementations (e.g. system serial port/32 feet)
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
