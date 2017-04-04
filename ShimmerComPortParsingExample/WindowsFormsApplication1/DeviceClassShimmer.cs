/*
 * C# Shimmer Device Class
 * 
 * The purpose of this class is to return a list of Shimmer devices (either
 * removable drives or COM ports). To be used in conjunction with 
 * "DeviceClass.cs" (specifically needed for obtaining removable media 
 * information). 
 * 
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
 * @author Mark Nolan
 * @date   11th August, 2014
 * 
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Data.OleDb;
using System.IO;
using System.Xml;
using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices; // For SetupDiGetDevice()
using Microsoft.Win32;

public class ShimmerDevices
{
    public enum PortFilterOption
    { 
        All, 
        ShimmerAll, 
        ShimmerBSL, 
        ShimmerUART,
        ShimmerAllDocks,
        ShimmerBT
    }

    private enum BluetoothFriendlyNamesColIndx
    {
        macid,
        friendlyname,
        comport,
        columncount
    }


    // Usuage: string[] ShimmerComPorts = ShimmerDevices.GetComPorts(portFilterOption);
    // where portFilterOption can be:
    //  ShimmerDevices.PortFilterOption.All -> all COM ports with any found device friendly names
    //  ShimmerDevices.PortFilterOption.ShimmerAll -> Any Shimmer COM ports found device friendly names
    //  ShimmerDevices.PortFilterOption.ShimmerBSL -> Any Shimmer Dock BSL COM ports
    //  ShimmerDevices.PortFilterOption.ShimmerUART -> Any Shimmer Dock UART COM ports
    //  ShimmerDevices.PortFilterOption.ShimmerAllDocks -> All BSL and UART COM ports
    //  ShimmerDevices.PortFilterOption.ShimmerBT -> All detect Bluetooth COM ports
    //          
    public static string[] GetComPorts(PortFilterOption portFilterOption)
    {
        string[] ComPortList = SerialPort.GetPortNames(); // get list of all com ports
        Array.Sort(ComPortList);
        string sInstanceName = string.Empty;
        string sPortName = string.Empty;
        string sManufacturer = string.Empty;

        string FTDI_VID = "VID_0403";
        string SHIMMER_DOCK_PID = "PID_6010";   // FT2232
        string SMART_DOCK_PID = "PID_6011";     // FT4232
        string PnpDeviceID = string.Empty;
        string lastuniqueID = string.Empty;
        int DockCounter = 0;

        string SmartDockPnpDeviceID = string.Empty;
        string SmartDockLastuniqueID = string.Empty;
        int SmartDockDockCounter = 0;


        bool OSSupported = false;
        if ((OSVersionInfo.Name == "Windows 7") || (OSVersionInfo.Name == "Windows 8"))
        {
            OSSupported = true;
            try
            {
                ManagementObjectSearcher searcherCOM = new ManagementObjectSearcher("root\\WMI", "SELECT * FROM MSSerial_PortName");
                foreach (ManagementObject queryObjCOM in searcherCOM.Get())
                {
                    sInstanceName = queryObjCOM["InstanceName"].ToString();
                    if (sInstanceName.IndexOf(FTDI_VID + "+" + SHIMMER_DOCK_PID) > -1) // FIDT chip used in Shimmer Dock
                    //if ((sInstanceName.IndexOf(FTDI_VID) > -1)&&(sInstanceName.IndexOf(SHIMMER_DOCK_PID) > -1)) // FIDT chip used in Shimmer Dock
                    {
                        // Convert MSSerial_PortName "FTDIBUS\\VID_0403+PID_6010+FTX0UDRGA\\0000_0" to PnpDeviceID "USB\\VID_0403&PID_6010\\FTX0UDRG"
                        string uniqueID = sInstanceName.Split(new string[] { SHIMMER_DOCK_PID + "+" }, StringSplitOptions.None)[1];
                        string uniqueIDStripped = uniqueID.Split(new string[] { @"A\", @"B\" }, StringSplitOptions.None)[0];
                        //string uniqueIDStripped = "";
                        //if (uniqueID.IndexOf(@"A\") > -1)
                        //{
                        //    uniqueIDStripped = uniqueID.Split(new string[] { @"A\"}, StringSplitOptions.None)[0];
                        //}
                        //else if (uniqueID.IndexOf(@"B\") > -1)
                        //{
                        //    uniqueIDStripped = uniqueID.Split(new string[] { @"B\" }, StringSplitOptions.None)[0];
                        //}

                        PnpDeviceID = sInstanceName.Substring(0, sInstanceName.Length - 2);
                        PnpDeviceID = @"USB\" + FTDI_VID + "&" + SHIMMER_DOCK_PID + @"\" + uniqueIDStripped;

                        using (Device device = Device.Get(PnpDeviceID))
                        {
                            sPortName = queryObjCOM["PortName"].ToString();
//                            if (device.BusReportedDeviceDesc.IndexOf("SHIMMER", StringComparison.OrdinalIgnoreCase) > -1) // Check that bus reported name contains "SHIMMER"
                            if (device.BusReportedDeviceDesc.IndexOf("sh", StringComparison.OrdinalIgnoreCase) > -1) // Check that bus reported name contains "SHIMMER"
                            {
                                // Dock counter to help distinguish different docks
                                if (lastuniqueID != uniqueIDStripped)
                                {
                                    DockCounter += 1;
                                }
                                lastuniqueID = uniqueIDStripped;

                                string ShimmerComType = string.Empty;
                                if (uniqueID.IndexOf(@"A\") > -1)
                                {
                                    ShimmerComType = "Shimmer BSL";
                                }
                                else if (uniqueID.IndexOf(@"B\") > -1)
                                {
                                    ShimmerComType = "Shimmer UART";
                                }

                                // Single line replace below is not working because, for example if COM8 is BSL, it will also replace any COM8x entries
                                //ComPortList = ComPortList.Select(val => val.Replace(sPortName, sPortName + " - " + System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(device.BusReportedDeviceDesc.ToLower()) + " " + DockCounter + " - " + ShimmerComType)).ToArray();
                                for (int x = 0; x < ComPortList.GetLength(0) ; x++)
                                {
                                    if (ComPortList[x]==sPortName)
                                    {
                                        //ComPortList[x] = sPortName + " - " + System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(device.BusReportedDeviceDesc.ToLower()) + " " + DockCounter + " - " + ShimmerComType;
                                        ComPortList[x] = sPortName + " - Shimmer Dock " + DockCounter + " - " + ShimmerComType;
                                        break;

                                    }
                                }
                            }
                        }
                    }
                    //TODO: handle Smart Dock
                    else if (sInstanceName.IndexOf(FTDI_VID + "+" + SMART_DOCK_PID) > -1) // FIDT chip used in Smart Dock
                    {
                        // Convert MSSerial_PortName "FTDIBUS\\VID_0403+PID_6010+FTX0UDRGA\\0000_0" to PnpDeviceID "USB\\VID_0403&PID_6010\\FTX0UDRG"
                        string uniqueID = sInstanceName.Split(new string[] { SMART_DOCK_PID + "+" }, StringSplitOptions.None)[1];
                        string uniqueIDStripped = uniqueID.Split(new string[] { @"A\", @"B\", @"C\", @"D\" }, StringSplitOptions.None)[0];

                        SmartDockPnpDeviceID = sInstanceName.Substring(0, sInstanceName.Length - 2);
                        SmartDockPnpDeviceID = @"USB\" + FTDI_VID + "&" + SMART_DOCK_PID + @"\" + uniqueIDStripped;

                        using (Device device = Device.Get(SmartDockPnpDeviceID))
                        {
                            sPortName = queryObjCOM["PortName"].ToString();
//                            if (device.BusReportedDeviceDesc.IndexOf("SHIMMER", StringComparison.OrdinalIgnoreCase) > -1) // Check that bus reported name contains "SHIMMER"
                            if (device.BusReportedDeviceDesc.IndexOf("sh", StringComparison.OrdinalIgnoreCase) > -1) // Check that bus reported name contains "SHIMMER"
                            {
                                // Dock counter to help distinguish different docks
                                if (SmartDockLastuniqueID != uniqueIDStripped)
                                {
                                    SmartDockDockCounter += 1;
                                }
                                SmartDockLastuniqueID = uniqueIDStripped;

                                string ShimmerComType = string.Empty;
                                if (uniqueID.IndexOf(@"A\") > -1)
                                {
                                    ShimmerComType = "Dock BSL";
                                }
                                else if (uniqueID.IndexOf(@"B\") > -1)
                                {
                                    ShimmerComType = "Dock UART";
                                }
                                else if (uniqueID.IndexOf(@"C\") > -1)
                                {
                                    ShimmerComType = "Shimmer BSL";
                                }
                                else if (uniqueID.IndexOf(@"D\") > -1)
                                {
                                    ShimmerComType = "Shimmer UART";
                                }

                                // Single line replace below is not working because, for example if COM8 is BSL, it will also replace any COM8x entries
                                //ComPortList = ComPortList.Select(val => val.Replace(sPortName, sPortName + " - " + System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(device.BusReportedDeviceDesc.ToLower()) + " " + DockCounter + " - " + ShimmerComType)).ToArray();
                                for (int x = 0; x < ComPortList.GetLength(0); x++)
                                {
                                    if (ComPortList[x] == sPortName)
                                    {
                                        //ComPortList[x] = sPortName + " - " + System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(device.BusReportedDeviceDesc.ToLower()) + " " + SmartDockDockCounter + " - " + ShimmerComType;
                                        ComPortList[x] = sPortName + " - Consensys Base " + SmartDockDockCounter + " - " + ShimmerComType;
                                        break;

                                    }
                                }
                            }
                        }

                    }
                }
            }
            catch
            {

            }
        }

        RegistryKey WinStackKey = null;
        RegistryKey ToshibaStackDataKey = null;
        RegistryKey ToshibaStackCOMMKey = null;

        try
        {
            string[,] BluetoothFriendlyNames = null;

            if(OSVersionInfo.Name == "Windows 7")
            {
                WinStackKey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters\LocalServices\{00001101-0000-1000-8000-00805f9b34fb}\", false);
                ToshibaStackDataKey = Registry.Users.OpenSubKey(@"S-1-5-21-1685272303-2415925560-2401851396-1000\Software\Toshiba\BluetoothStack\V1.0\EZC\DATA\", false);
                ToshibaStackCOMMKey = Registry.Users.OpenSubKey(@"S-1-5-21-1685272303-2415925560-2401851396-1000\Software\Toshiba\BluetoothStack\V1.0\Mng\AutoConnectCOMM\", false);

                // Search for Bluetooth device names as they appear in the "Devices and Printers" section of control Panel (e.g, Shimmer-B694)
                // Alternative to reading from registry could be "BluetoothDeviceInfo" but haven't investigated thus far
                int WinStackKeyCount = 0;
                try
                {
                    if (WinStackKey != null)
                    {
                        WinStackKeyCount = WinStackKey.SubKeyCount;
                    }
                }
                catch
                {
                }

                int ToshibaStackDataKeyCount = 0;
                int ToshibaStackCOMMKeyCount = 0;
                int ToshibaStackDataKeyDevicesCount = 0;
                try
                {
                    if (ToshibaStackDataKey != null)
                    {
                        ToshibaStackDataKeyDevicesCount = ToshibaStackDataKey.SubKeyCount;
                    }
                    if (ToshibaStackCOMMKey != null)
                    {
                        ToshibaStackCOMMKeyCount = ToshibaStackCOMMKey.SubKeyCount;
                    }
                    if ((ToshibaStackDataKeyDevicesCount != 0) && (ToshibaStackCOMMKeyCount != 0))
                    {
                        ToshibaStackDataKeyCount = ToshibaStackCOMMKeyCount;
                    }
                }
                catch
                {
                }

                // Initialise Array
                BluetoothFriendlyNames = new string[ToshibaStackDataKeyCount + WinStackKeyCount, (int)BluetoothFriendlyNamesColIndx.columncount];

                // Populate array with Windows Bluetooth stack entries
                int ii = 0;
                if (WinStackKey != null)
                {
                    foreach (string Keyname in WinStackKey.GetSubKeyNames())
                    {
                        RegistryKey key = WinStackKey.OpenSubKey(Keyname);
                        if (key != null)
                        {
                            BluetoothFriendlyNames[ii, (int)BluetoothFriendlyNamesColIndx.friendlyname] = key.GetValue("ServiceName").ToString();
                            var macid = (byte[])key.GetValue("AssocBdAddr");
                            Array.Reverse(macid);
                            var valueAsString = BitConverter.ToString(macid);
                            valueAsString = valueAsString.Replace("-", "");
                            valueAsString = valueAsString.Substring(valueAsString.Length - 12, 12);
                            BluetoothFriendlyNames[ii, (int)BluetoothFriendlyNamesColIndx.macid] = valueAsString;
                            ii += 1;
                        }
                    }
                }

                // Populate array with Toshiba Bluetooth stack entries
                if (ToshibaStackCOMMKey != null)
                {
                    // Compare the two Toshiba Bluetooth Stack registry locations to build a list of MACIDs, COM ports and Friendly device names
                    foreach (string KeynameCOMM in ToshibaStackCOMMKey.GetSubKeyNames())
                    {
                        RegistryKey keyCOMM = ToshibaStackCOMMKey.OpenSubKey(KeynameCOMM);
                        if (keyCOMM != null)
                        {
                            var macidCOMM = (byte[])keyCOMM.GetValue("BdAddr");
                            var macidCOMMAsString = BitConverter.ToString(macidCOMM);
                            macidCOMMAsString = macidCOMMAsString.Replace("-", "");
                            macidCOMMAsString = macidCOMMAsString.Substring(macidCOMMAsString.Length - 12, 12);

                            foreach (string KeynameDATA in ToshibaStackDataKey.GetSubKeyNames())
                            {
                                RegistryKey keyData = ToshibaStackDataKey.OpenSubKey(KeynameDATA);
                                if (keyData != null)
                                {
                                    var macidDATA = (byte[])keyData.GetValue("BDADDR");
                                    var macidDATAvalueAsString = BitConverter.ToString(macidDATA);
                                    macidDATAvalueAsString = macidDATAvalueAsString.Replace("-", "");
                                    macidDATAvalueAsString = macidDATAvalueAsString.Substring(macidDATAvalueAsString.Length - 12, 12);
                                    if (macidCOMMAsString == macidDATAvalueAsString)
                                    {
                                        BluetoothFriendlyNames[ii, (int)BluetoothFriendlyNamesColIndx.macid] = macidDATAvalueAsString;
                                        BluetoothFriendlyNames[ii, (int)BluetoothFriendlyNamesColIndx.friendlyname] = keyData.GetValue("ENTRYNAME2").ToString();
                                        BluetoothFriendlyNames[ii, (int)BluetoothFriendlyNamesColIndx.comport] = KeynameCOMM.ToString();
                                        ii += 1;
                                    }
                                }
                            }
                        }
                    }
                }
            } // end of Windows 7 section
            else if (OSVersionInfo.Name == "Windows 8")
            {
                WinStackKey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\ControlSet001\Enum\BTHENUM\", false);
                // Search for Bluetooth device names as they appear in the "Devices and Printers" section of control Panel (e.g, Shimmer-B694)
                // Alternative to reading from registry could be "BluetoothDeviceInfo" but haven't investigated thus far
                int WinStackKeyCount = 0;
                int ToshibaStackDataKeyCount = 0;
                try
                {
                    if (WinStackKey != null)
                    {
                        foreach (string Keyname in WinStackKey.GetSubKeyNames())
                        {
                            RegistryKey key = WinStackKey.OpenSubKey(Keyname);
                            if (key != null)
                            {
                                if (key.ToString().Contains("Dev_"))
                                {
                                    WinStackKeyCount += 1;
                                }
                            }
                        }
                    }

                    //TODO: Support Toshiba Bluetooth stack in Windows 8
                    ToshibaStackDataKeyCount = 0;

                    // Initialise Array
                    BluetoothFriendlyNames = new string[ToshibaStackDataKeyCount + WinStackKeyCount, (int)BluetoothFriendlyNamesColIndx.columncount];

                    // Populate array with Windows Bluetooth stack entries
                    int ii = 0;
                    if (WinStackKey != null)
                    {
                        foreach (string Keyname in WinStackKey.GetSubKeyNames())
                        {
                            RegistryKey key = WinStackKey.OpenSubKey(Keyname);
                            if (key != null)
                            {
                                if (key.ToString().Contains("Dev_"))
                                {
                                    foreach (string SubKeyname in key.GetSubKeyNames())
                                    {
                                        RegistryKey subkey = key.OpenSubKey(SubKeyname);
                                        BluetoothFriendlyNames[ii, (int)BluetoothFriendlyNamesColIndx.friendlyname] = subkey.GetValue("FriendlyName").ToString();
                                        BluetoothFriendlyNames[ii, (int)BluetoothFriendlyNamesColIndx.macid] = key.ToString().Split(new string[] { "Dev_" }, StringSplitOptions.None)[1];
                                        ii += 1;
                                    }
                                }
                            }
                        }
                    }
                }
                catch
                {
                }
            } // end of Windows 8 section

            if(BluetoothFriendlyNames.GetLength(0) > 0) // if friendly names found then replace entries in com port list
            {
                // Bluetooth related serial ports
                ManagementObjectSearcher searcherBTCOM = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PnPEntity WHERE ConfigManagerErrorCode = 0");
                foreach (ManagementObject queryObjBTCOM in searcherBTCOM.Get())
                {
                    sInstanceName = queryObjBTCOM["PNPDeviceID"].ToString();
                    if (sInstanceName == "HTREE\\ROOT\\0") // Specific to Windows 8 
                    {
                        sInstanceName = null;
                    }
                    else
                    {
                        sPortName = queryObjBTCOM["Caption"].ToString();
                    }

                    if ((sInstanceName != null) && (sPortName != null))
                    {
                        if (sInstanceName.IndexOf("BTHENUM") > -1) // Windows Bluetooth Stack
                        {
                            //DeviceID => COM port
                            //PNPDeviceID => BTHENUM\{00001101-0000-1000-8000-00805F9B34FB}_LOCALMFG&000A\9&1DFB4CCA&0&00066646B694_C00000000
                            if (sPortName.Contains("(COM"))
                            {
                                //sManufacturer = queryObjBTCOM["Manufacturer"].ToString();
                                string MacID = sInstanceName.Substring(sInstanceName.LastIndexOf("_") - 4, 4);
                                sPortName = sPortName.Split(new string[] { "(", ")" }, StringSplitOptions.None)[1];
                                int ComPortListCurrentIndex = Array.IndexOf(ComPortList,sPortName);
                                if (ComPortListCurrentIndex > -1)
                                {
                                    if (MacID == "0000")
                                    {
                                        ComPortList[ComPortListCurrentIndex] = sPortName + " - BT";
//                                        ComPortList = ComPortList.Select(val => val.Replace(sPortName "\n", sPortName + " - BT") + "\n").ToArray();
                                    }
                                    else
                                    {
                                        bool FriendlyNameFound = false;
                                        int ArrayIndex = 0;
                                        // check if each of the values found in registry is a match to the PNPDeviceID (code could be shortened)
                                        for (ArrayIndex = 0; ArrayIndex < BluetoothFriendlyNames.GetLength(0); ArrayIndex++)
                                        {
                                            if (sInstanceName.Contains(BluetoothFriendlyNames[ArrayIndex, (int)BluetoothFriendlyNamesColIndx.macid]))
                                            {
                                                FriendlyNameFound = true;
                                                break;
                                            }
                                        }

                                        if (FriendlyNameFound)// If name found in registry
                                        {
                                            ComPortList[ComPortListCurrentIndex] = sPortName + " - BT - " + BluetoothFriendlyNames[ArrayIndex, (int)BluetoothFriendlyNamesColIndx.friendlyname];
//                                            ComPortList = ComPortList.Select(val => val.Replace(sPortName + "\n", sPortName + " - BT - " + BluetoothFriendlyNames[ArrayIndex, (int)BluetoothFriendlyNamesColIndx.friendlyname]) + "\n").ToArray();
                                        }
                                        else // else just enter last four characters of MAC ID
                                        {
                                            ComPortList[ComPortListCurrentIndex] = sPortName + " - BT - ID: " + MacID;
//                                            ComPortList = ComPortList.Select(val => val.Replace(sPortName + "\n", sPortName + " - BT - ID: " + MacID) + "\n").ToArray();
                                        }
                                    }
                                }
                            }
                        }
                        else if (sInstanceName.IndexOf("BLUETOOTH") > -1) //Toshiba Bluetooth Stack
                        {
                            sManufacturer = queryObjBTCOM["Manufacturer"].ToString();
                            if (sManufacturer == "TOSHIBA") // Double check whether manufacturer is Toshiba
                            {
                                if (sPortName.Contains("(COM"))
                                {
                                    sPortName = sPortName.Split(new string[] { "(", ")" }, StringSplitOptions.None)[1];
                                    int ComPortListCurrentIndex = Array.IndexOf(ComPortList, sPortName);
                                    if (ComPortListCurrentIndex > -1)
                                    {

                                        bool FriendlyNameFound = false;
                                        int ArrayIndex = 0;
                                        // check if each of the values found in registry is a match to the PNPDeviceID (code could be shortened)
                                        for (ArrayIndex = 0; ArrayIndex < BluetoothFriendlyNames.GetLength(0); ArrayIndex++)
                                        {
                                            if (sPortName == BluetoothFriendlyNames[ArrayIndex, 2])
                                            {
                                                FriendlyNameFound = true;
                                                break;
                                            }
                                        }

                                        if (FriendlyNameFound) // If name found in registry
                                        {
                                            ComPortList[ComPortListCurrentIndex] = sPortName + " - BT - " + BluetoothFriendlyNames[ArrayIndex, 1];
//                                            ComPortList = ComPortList.Select(val => val.Replace(sPortName + "\n", sPortName + " - BT - " + BluetoothFriendlyNames[ArrayIndex, 1]) + "\n").ToArray();
                                        }
                                        else // else just add "BT" to name
                                        {
                                            ComPortList[ComPortListCurrentIndex] = sPortName + " - BT";
//                                            ComPortList = ComPortList.Select(val => val.Replace(sPortName + "\n", sPortName + " - BT" + "\n")).ToArray();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (ManagementException e)
        {
            MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
        }
        finally
        {
            //Tidy up
            if (WinStackKey != null)
            {
                WinStackKey.Close();
            }
            if (ToshibaStackDataKey != null)
            {
                ToshibaStackDataKey.Close();
            }
            if (ToshibaStackCOMMKey != null)
            {
                ToshibaStackCOMMKey.Close();
            }
        }

        // if OS is not support then send back list of all COM ports
        if(OSSupported.Equals(true))
        {
            // Filter out COM ports based on setting passed into the function
            if (portFilterOption.Equals(PortFilterOption.ShimmerUART))
            {
                ComPortList = ComPortList.Where(val => val.Contains("UART")).ToArray();
            }
            else if (portFilterOption.Equals(PortFilterOption.ShimmerBSL))
            {
                ComPortList = ComPortList.Where(val => val.Contains("BSL")).ToArray();
            }
            else if (portFilterOption.Equals(PortFilterOption.ShimmerAll))
            {
                ComPortList = ComPortList.Where(val => val.Contains("Shimmer")).ToArray();
            }
            else if (portFilterOption.Equals(PortFilterOption.ShimmerAllDocks))
            {
                ComPortList = ComPortList.Where(val => (val.Contains("Dock") || val.Contains("Base"))).ToArray();
            }
            else if (portFilterOption.Equals(PortFilterOption.ShimmerBT))
            {
                ComPortList = ComPortList.Where(val => val.Contains("BT")).ToArray();
            }
            //else if (portFilterOption.Equals(PortFilterOption.All))
            //{

            //}
        }


        // this is here as a precautionary. Multiple newline characters were being added to each COM port name. Root cause should be fixed but this is just incase
        for (int i = 0; i < ComPortList.Length;i++ )
        {
            if(ComPortList[i].Contains("\n"))
            {
                ComPortList[i] = ComPortList[i].Split(new string[] { "\n" }, StringSplitOptions.None)[0];
            }
        }

        return ComPortList;
    }

    // Usuage: string[] ShimmerDrives = ShimmerDevices.GetShimmerDrives();
    // Returns a list of removable media drive letters where the device model
    // contains the text "Shimmer".
    //   
    public static string[] GetShimmerDrives()
    {
        var listShimmerDrives = new List<string>();
        int counter = 0;

        try
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_DiskDrive");
            foreach (ManagementObject queryObj in searcher.Get())
            {
                foreach (ManagementObject b in queryObj.GetRelated("Win32_DiskPartition"))
                {
                    if ((queryObj["Model"]).ToString().IndexOf("Shimmer") > -1)
                    {
                        foreach (ManagementBaseObject c in b.GetRelated("Win32_LogicalDisk"))
                        {
                            listShimmerDrives.Add((c["Name"]).ToString() + " (" + (queryObj["Model"]).ToString() + ")");
                            counter = counter + 1;
                        }
                    }
                }
            }
        }
        catch (ManagementException e)
        {
            //MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
        }

        string[] ShimmerDriveArray = listShimmerDrives.ToArray();

        return ShimmerDriveArray;
    }
}