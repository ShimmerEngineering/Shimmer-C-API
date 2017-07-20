# Shimmer-C-API Rev0.7 (BETA)
The C# APi is currently in a BETA development state, users are free to use and provide feedback. For users working on production code we recommend downloading the API from the Shimmer Website http://www.shimmersensing.com/support/wireless-sensor-networks-download/

To use, build the ShimmerAPI project and make sure the examples (e.g. ShimmerCapture) references the build dll.

NOTE: Where require the dlls for ShimmerLibrary, InTheHand, MathNet.Numerics can be found in the libraries folder in the ShimmerAPI project

Changes since Rev0.6
- BTStream Firmware Is No Longer officially supported
- Shimmer, Shimmer32Feet, ShimmerSDBT and ShimmerSDBT32Feet has been deprecated, please see ShimmerLogAndStreamSystemSerialPort and ShimmerLogAndStream32Feet, and ShimmerLogAndStreamXamarin (Within the ShimmerCaptureXamarin Project)
- Major restructuring to code, ensuring platform specific bluetooth controls are strictly seperated between ShimmerLogAndStream and classes which extend it ShimmerLogAndStreamSystemSerialPort/ShimmerLogAndStream32Feet/ShimmerLogAndStreamXamarin
- Updates to API to support the updated Shimmer3 device whose sensors have been updated as follows
      1.	Pressure sensor:		BMP180 → BMP280
      2.	Gyroscope + mag:		MPU9150 → MPU9250
      3.	Low-Noise Accel:		KXRB5-2042 → KXTC9-2050
      4.	Wide-Range Accel:		LSM303DLHC → LSM303AHTR

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
