using shimmer.Communications;
using ShimmerBLEAPI.Devices;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace shimmer.Communications
{
    public class VerisenseBLEDeviceWindows : VerisenseBLEDevice
    {
        public static string path;
        public VerisenseBLEDeviceWindows(string uuid, string name, string comport, CommunicationType commtype):base(uuid,name)
        {
            ComPort = comport;
            CommType = commtype;
        }

        public VerisenseBLEDeviceWindows(string uuid, string name) : base(uuid, name)
        {

        }

        protected override void InitializeRadio()
        {
            

            if (BLERadio != null)
                BLERadio.CommunicationEvent -= UartRX_ValueUpdated;
            if (CommType == CommunicationType.BLE)
            {
                BLERadio = new RadioPlugin32Feet();
            }
            else if (CommType == CommunicationType.SerialPort)
            {
                BLERadio = new SerialPortByteCommunication();
                ((SerialPortByteCommunication)BLERadio).ComPort = ComPort;
            }
        }

        protected override void createBinFile(bool crcError)
        {
            try
            {
                //var asm = RealmService.GetSensorbyID(Asm_uuid.ToString());
                //var trialSettings = RealmService.LoadTrialSettings();

                //var participantID = asm.ParticipantID;
                String sensorID = Asm_uuid.ToString();
                try
                {
                    sensorID = GetSensorID();
                }
                catch (Exception ex) // if production config wasnt read default to the UUID
                {
                    sensorID = Asm_uuid.ToString();
                    AdvanceLog(ex.Message, "Defaulting to UUID", dataFileName, ASMName);
                }
                binFileFolderDir = string.Format("{0}/{1}/{2}/BinaryFiles", GetTrialName(), GetParticipantID(), sensorID);
                //string path = ApplicationData.Current.LocalFolder.Path;
                if(path == null)
                {
                    path = Directory.GetCurrentDirectory();
                }

                var folder = Path.Combine(path, binFileFolderDir);

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                if (crcError)
                {
                    dataFileName = string.Format("{0}_{1}_{2}.bin", DateTime.Now.ToString("yyMMdd_HHmmss"), PayloadIndex.ToString("00000"), BadCRC);
                }
                else
                {
                    dataFileName = string.Format("{0}_{1}.bin", DateTime.Now.ToString("yyMMdd_HHmmss"), PayloadIndex.ToString("00000"));
                }

                AdvanceLog(LogObject, "BinFileNameCreated", dataFileName, ASMName);
                dataFilePath = Path.Combine(folder, dataFileName);

                AdvanceLog(LogObject, "BinFileCreated", dataFilePath, ASMName);
            }
            catch (Exception ex)
            {
                AdvanceLog(LogObject, "BinFileCreatedException", ex, ASMName);
            }
        }
    }
}
