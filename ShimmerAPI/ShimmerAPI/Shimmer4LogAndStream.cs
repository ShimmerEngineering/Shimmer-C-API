using ShimmerAPI.Sensors;
using ShimmerAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShimmerAPI
{
    public abstract class Shimmer4LogAndStream : ShimmerLogAndStream
    {
        LNAccel LowNoiseAccel = new LNAccel();
        public Shimmer4LogAndStream(String devID) : base(devID)
        {

        }
        protected void ParseCalibrationDump(byte[] calibDump)
        {
            var length = (int)calibDump[0] + ((int)calibDump[1] << 8);
            var calibrationBytes = new byte[length + 2];
            System.Array.Copy(calibDump, 0, calibrationBytes, 0, calibrationBytes.Length);
            var infoBytes = ProgrammerUtilities.CopyAndRemoveBytes(ref calibrationBytes, 10);
            while (calibrationBytes.Length > 10)
            {
                var calibrationlength = (int)(calibrationBytes[3]);
                var sensorcalibrationdumplength = calibrationlength + 12; //4 + 8TS
                var sensorcalibrationdump = ProgrammerUtilities.CopyAndRemoveBytes(ref calibrationBytes, sensorcalibrationdumplength);
                System.Console.WriteLine("Sensor Calibration: " + ProgrammerUtilities.ByteArrayToHexString(sensorcalibrationdump));
                LowNoiseAccel.RetrieveKinematicCalibrationParametersFromCalibrationDump(sensorcalibrationdump);

            }
            //var infoBytes = System.Array.Copy();
        }
    }
}
