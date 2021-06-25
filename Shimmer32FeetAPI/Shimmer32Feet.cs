/*Rev 0.1
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
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InTheHand.Net;
using InTheHand.Net.Sockets;
using System.IO;

namespace ShimmerAPI
{
    [System.Obsolete]
    public abstract class Shimmer32Feet:ShimmerBluetooth
    {
        
        protected String ComPort;
        Guid g = new Guid("00001101-0000-1000-8000-00805F9B34FB");
        BluetoothEndPoint btEndpoint;
        BluetoothClient btClient = new BluetoothClient();
        BluetoothAddress addr;
        Stream peerStream;
        
        public Shimmer32Feet(String devID, String bluetoothAddress)
            : base(devID)
        {
            SetAddress(bluetoothAddress);
        }

        public Shimmer32Feet(String devName, String bluetoothAddress, double samplingRate, int accelRange, int gsrRange, int setEnabledSensors, bool enableLowPowerAccel, bool enableLowPowerGyro, bool enableLowPowerMag, int gyroRange, int magRange, byte[] exg1configuration, byte[] exg2configuration, bool internalexppower)
            :base(devName, samplingRate, accelRange, gsrRange, setEnabledSensors, enableLowPowerAccel, enableLowPowerGyro, enableLowPowerMag, gyroRange, magRange, exg1configuration, exg2configuration, internalexppower)
        {
            SetAddress(bluetoothAddress);
        }

        public Shimmer32Feet(String devID, String bluetoothAddress, double samplingRate, int AccelRange, int GyroRange, int gsrRange, int setEnabledSensors)
            : base(devID, samplingRate, AccelRange, GyroRange, gsrRange, setEnabledSensors)
        {
            SetAddress(bluetoothAddress);
        }

        protected override bool IsConnectionOpen(){
            return btClient.Connected;
        }

        protected override void CloseConnection()
        {
            btClient.Close();
        }
        protected override void FlushConnection()
        {
            peerStream.Flush();
        }
        protected override void FlushInputConnection()
        {
            peerStream.Flush();
        }
        protected override void WriteBytes(byte[] b, int index, int length)
        {
            peerStream.Write(b, index, length);
        }


        public override string GetShimmerAddress()
        {
            return ComPort;
        }
        protected override int ReadByte()
        {
            return peerStream.ReadByte();
        }
        protected override void OpenConnection()
        {
            btEndpoint = new BluetoothEndPoint(addr, g);
            btClient = new BluetoothClient();
            btClient.Connect(btEndpoint);
            peerStream = btClient.GetStream();
        }
        public String GetComPort()
        {
            return ComPort;
        }
        public void SetComPort(String comPort)
        {
            ComPort = comPort;
        //    addr = BluetoothAddress.Parse("00:06:66:66:8B:FF");
            
        }
        public void SetAddress(String add)
        {
            addr = BluetoothAddress.Parse(add);
        }


    }
}
