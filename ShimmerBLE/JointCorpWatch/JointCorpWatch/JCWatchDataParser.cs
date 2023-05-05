using ShimmerAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace JointCorpWatch
{
    public class JCWatchDataParser
    {
        public static Logging LoggingHR = null;
        public static JCWatchEvent DataParsingWithData(byte[] value)
        {
            JCWatchEvent watchEvent = new JCWatchEvent();
            switch (value[0])
            {

                case JCWatchDeviceConstant.CMD_HeartPackageFromDevice:
                    DateTimeOffset now = DateTimeOffset.UtcNow;
                    long unixTimeMilliseconds = now.ToUnixTimeMilliseconds();
                    string path = DependencyService.Get<ILocalFolderService>().GetAppLocalFolder();
                    Debug.WriteLine(unixTimeMilliseconds + ";" + " Heart Rate: " + value[1]);
                    ObjectCluster ojc = new ObjectCluster("", "2025E");
                    ojc.Add("TimeStamp", "CAL", "ms", unixTimeMilliseconds);
                    ojc.Add("Heart Rate", "CAL", "bpm", value[1]);
                    if (LoggingHR == null)
                    {
                        var folder = Path.Combine(DependencyService.Get<ILocalFolderService>().GetAppLocalFolder());
                        if (!Directory.Exists(folder))
                        {
                            Directory.CreateDirectory(folder);
                        }
                        LoggingHR = new Logging(Path.Combine(folder, unixTimeMilliseconds.ToString() + "HeartRate.csv"), ",");

                    }
                    else
                    {
                        if (value[1] == 255)
                        {
                            watchEvent.Message = "HeartRateEnd";
                        }
                        else
                        {
                            LoggingHR.WriteData(ojc);
                        }
                    }                   
                    return watchEvent;
                case JCWatchDeviceConstant.CMD_Get_Address:
                    return JCWatchResolveUtil.GetDeviceAddress(value);
                case JCWatchDeviceConstant.CMD_Get_DeviceInfo:
                case JCWatchDeviceConstant.Openecg:
                case JCWatchDeviceConstant.GetEcgPpgStatus:
                case JCWatchDeviceConstant.CMD_ECGQuality:
                    watchEvent = new JCWatchEvent();
                    watchEvent.Identifier = value[0];
                    watchEvent.End = true;
                    watchEvent.DataType = JCWatchBLEConstant.EcgppG;
                    watchEvent.Data = new Dictionary<string, string>();
                    watchEvent.Data.Add(JCWatchDeviceKey.heartValue, JCWatchResolveUtil.getValue(value[1], 0) + "");
                    watchEvent.Data.Add(JCWatchDeviceKey.hrvValue, JCWatchResolveUtil.getValue(value[2], 0) + "");
                    watchEvent.Data.Add(JCWatchDeviceKey.Quality, JCWatchResolveUtil.getValue(value[3], 0) + "");
                    return watchEvent;
                case JCWatchDeviceConstant.CMD_ECGDATA:
                case JCWatchDeviceConstant.CMD_PPGDATA:
                    List<short> data = new List<short>();
                    for (int i = 0; i < value.Length / 2 - 1; i++)
                    {
                        int ecgValueAction = JCWatchResolveUtil.getValue(value[i * 2 + 1], 1) + JCWatchResolveUtil.getValue(value[i * 2 + 2], 0);
                        if (ecgValueAction >= 32768) ecgValueAction = ecgValueAction - 65536;
                        // short[] ecgData = new short[] { (short)-ecgValueAction };
                        data.Add((short)-ecgValueAction);
                    }

                    watchEvent.Identifier = value[0];
                    watchEvent.Data = new Dictionary<string, string>();
                    watchEvent.Data.Add(JCWatchDeviceKey.ECGValue, string.Join(",", data.ToArray()));
                    // watchEvent2.Data.Add(JCWatchDeviceKey.ECGValue, BitConverter.ToString(value));
                    return watchEvent;
                case JCWatchDeviceConstant.GetECGwaveform:
                    return JCWatchResolveUtil.getEcgHistoryData(value);
            }
            return null;

        }
    }
}
