/*Rev 0.2
 * 
 * Copyright (c) 2014, Shimmer Research, Ltd.
 * All rights reserved
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are
 * met:

 *     * Redistributions of source code must retain the above copyright
 *       notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above
 *       copyright notice, this list of conditions and the following
 *       disclaimer in the documentation and/or other materials provided
 *       with the distribution.
 *     * Neither the name of Shimmer Research, Ltd. nor the names of its
 *       contributors may be used to endorse or promote products derived
 *       from this software without specific prior written permission.

 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
 * "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
 * LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
 * A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
 * OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
 * LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 * THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
 * OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 * @author Cathy Swanton, Mike Healy, Jong Chern Lim
 * @date   April, 2014
 * 
 * Changes since re0.1 
 * - Added  SerialPort.ReadExisting() to flush input connection
 * 
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ShimmerAPI
{
    [System.Obsolete] //Moving the serial port controls to the highest implementing class so code is more concise and extendible for use with 32 feet/xamarin/etc
    public abstract class Shimmer : ShimmerBluetooth
    {
        protected String ComPort;
        public System.IO.Ports.SerialPort SerialPort = new System.IO.Ports.SerialPort();

        public Shimmer(String devID, String bComPort)
            : base(devID)
        {
            ComPort = bComPort;
        }

        public Shimmer(String devName, String bComPort, double samplingRate, int accelRange, int gsrRange, int setEnabledSensors, bool enableLowPowerAccel, bool enableLowPowerGyro, bool enableLowPowerMag, int gyroRange, int magRange, byte[] exg1configuration, byte[] exg2configuration,bool internalexppower)
            :base(devName, samplingRate, accelRange, gsrRange, setEnabledSensors, enableLowPowerAccel, enableLowPowerGyro, enableLowPowerMag, gyroRange, magRange, exg1configuration, exg2configuration, internalexppower)
        {
            ComPort = bComPort;
        }

        public Shimmer(String devID, String bComPort, double samplingRate, int AccelRange, int GyroRange, int gsrRange, int setEnabledSensors)
            : base(devID, samplingRate, AccelRange, GyroRange, gsrRange, setEnabledSensors)
        {
            ComPort = bComPort;
        }
        protected override bool IsConnectionOpen()
        {
            //SerialPort.PortName = ComPort;
            return SerialPort.IsOpen;
        }

        protected override void CloseConnection()
        {
            SerialPort.Close();
        }
        protected override void FlushConnection()
        {
            SerialPort.DiscardInBuffer();
            SerialPort.DiscardOutBuffer();
        }
        protected override void FlushInputConnection()
        {
            try
            {
                SerialPort.ReadExisting();
                SerialPort.DiscardInBuffer();
            }
            catch
            {
            }
            
        }
        protected override void WriteBytes(byte[] b, int index, int length)
        {
            SerialPort.Write(b, index, length);
        }

        public override string GetShimmerAddress()
        {
            return ComPort;
        }
        protected override int ReadByte()
        {
            return SerialPort.ReadByte();
        }
        protected override void OpenConnection()
        {
                SerialPort.BaudRate = 115200;
                SerialPort.PortName = ComPort;
                SerialPort.ReadTimeout = this.ReadTimeout;
                SerialPort.WriteTimeout = this.WriteTimeout;
                SetState(SHIMMER_STATE_CONNECTING);
                try
                {
                    SerialPort.Open();
                }
                catch
                {
                }
                SerialPort.DiscardInBuffer();
                SerialPort.DiscardOutBuffer();
        }
        public String GetComPort()
        {
            return ComPort;
        }
        public void SetComPort(String comPort)
        {
            ComPort = comPort;
        }

    }
}
