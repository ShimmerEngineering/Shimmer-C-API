using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace JointCorpWatch
{
    public class JCWatchResolveUtil
    {
        public static JCWatchEvent GetDeviceAddress(byte[] value)
        {
            String address = "";
            for (int i = 1; i < 7; i++)
            {
                address= address+BitConverter.ToString(value, i, 1);
                if (i != 6)
                {
                    address = address + ":";
                }
            }
            JCWatchEvent watchEvent = new JCWatchEvent();
            watchEvent.Identifier = value[0];
            watchEvent.Message = address;
            return watchEvent;
        }

        public static JCWatchEvent getEcgData(byte[] value)
        {
            JCWatchEvent watchEvent = new JCWatchEvent();
            watchEvent.Identifier = value[0];
            watchEvent.Data = new Dictionary<string, string>();
            String ecgData = getEcgDataString(value);
            watchEvent.Data.Add(JCWatchDeviceKey.ECGValue, ecgData);

            return watchEvent;
        }

        static long ECGHistoryTotalBytes = 0;
        static long AllECGHistoryInKB = 0;
        static Stopwatch timer = new Stopwatch();
        static Stopwatch timerTotal = new Stopwatch();
        public static JCWatchEvent getEcgHistoryData(byte[] value)
        {
            JCWatchEvent watchEvent = new JCWatchEvent();
            watchEvent.Identifier = value[0];
            watchEvent.DataType = JCWatchBLEConstant.ECGdata;
            watchEvent.End = false;
            watchEvent.Data = new Dictionary<string, string>();

            int length = value.Length;
            ECGHistoryTotalBytes = ECGHistoryTotalBytes + length;
            if (length == 3 || (value[length - 3] == (byte)0x71 && value[length - 2] == (byte)0xff && value[length - 1] == (byte)0xff))
            {
                if (value[1] == 0xFF && value[2] == 0xFF)
                {
                    timer.Stop();
                    long elapsedMS = timer.ElapsedMilliseconds;
                    Debug.WriteLine("Timer Elapsed (MilliSeconds) : " + elapsedMS);
                    Debug.WriteLine("Total Number of Bytes: " + ECGHistoryTotalBytes);
                    long inKB = ECGHistoryTotalBytes / 1024;
                    AllECGHistoryInKB += inKB;
                    double throughput = ((double)inKB) / ((double)elapsedMS / 1000);
                    double totalThroughput = ((double)AllECGHistoryInKB) / ((double)timerTotal.ElapsedMilliseconds / 1000);
                    String throughputS = "Throughput (KB/s): " + throughput + "Total Throughput (KB/s): " + totalThroughput;
                    Debug.WriteLine(throughputS);
                    watchEvent.Data.Add("Duration", elapsedMS.ToString());
                    watchEvent.Message = throughputS;
                    watchEvent.End = true;
                }
                return watchEvent;
            }
            int id = getValue(value[1], 0) + getValue(value[2], 1);
            int offset = 3;
            if (id == 0)
            {//第一条
                timer.Reset();
                timer.Start();
                if (!timerTotal.IsRunning)
                {
                    timerTotal.Start();
                }
                ECGHistoryTotalBytes = 0;
                String date = "20" + bcd2String(value[3]) + "-"
                        + bcd2String(value[4]) + "-" + bcd2String(value[5]) + " "
                        + bcd2String(value[6]) + ":" + bcd2String(value[7]) + ":" + bcd2String(value[8]);
                String hrv = getValue(value[11], 0).ToString();
                String heart = getValue(value[12], 0).ToString();
                Debug.WriteLine(date + "HR: " + heart);
                String moodValue = getValue(value[13], 0).ToString();
                watchEvent.Data.Add(JCWatchDeviceKey.Date, date);
                watchEvent.Data.Add(JCWatchDeviceKey.HRV, hrv);
                watchEvent.Data.Add(JCWatchDeviceKey.HeartRate, heart);
                watchEvent.Data.Add(JCWatchDeviceKey.ECGMoodValue, moodValue);
                /* list.add(hashMap);*/
                offset = 27;

            }
            byte[] tempValue = new byte[length - offset];
            Array.Copy(value, offset, tempValue, 0, tempValue.Length);
            String ecgData = getEcgDataString(tempValue);
            watchEvent.Data.Add(JCWatchDeviceKey.ECGValue, ecgData);
            /*list.add(hashMap);*/
            return watchEvent;
        }

        public static int getValue(byte b, int count)
        {
            return (int)((b & 0xff) * Math.Pow(256, count));
        }

        public static String bcd2String(byte bytes)
        {
            StringBuilder temp = new StringBuilder();
            temp.Append((byte)((bytes & 0xf0) >> 4));
            temp.Append((byte)(bytes & 0x0f));
            return temp.ToString();
        }

        protected static String getEcgDataString(byte[] value)
        {
            StringBuilder stringBuffer = new StringBuilder();
            int length = value.Length / 2 - 1;
            for (int i = 0; i < length; i++)
            {
                int ecgValue = getValue(value[i * 2 + 1], 1) + getValue(value[i * 2 + 2], 0);
                if (ecgValue >= 32768) ecgValue = ecgValue - 65536;
                stringBuffer.Append(ecgValue).Append(",");
            }
            return stringBuffer.ToString();
        }
    }
}
