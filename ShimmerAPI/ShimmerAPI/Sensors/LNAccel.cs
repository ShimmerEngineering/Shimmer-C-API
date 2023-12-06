using ShimmerAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShimmerAPI.Sensors
{
    public class LNAccel
    {
        public readonly int CALIBRATION_ID = 2;
        public double[,] AlignmentMatrixAccel = new double[3, 3];
        public double[,] SensitivityMatrixAccel = new double[3, 3];
        public double[,] OffsetVectorAccel = new double[3, 1];
        public void RetrieveKinematicCalibrationParametersFromCalibrationDump(byte[] sensorcalibrationdump)
        {

            var packetType = ProgrammerUtilities.CopyAndRemoveBytes(ref sensorcalibrationdump, 2);
            var sensorID = ((int)packetType[0]) + ((int)packetType[1] << 8);
            if (sensorID == CALIBRATION_ID)
            {
                var rangebytes = ProgrammerUtilities.CopyAndRemoveBytes(ref sensorcalibrationdump, 1);
                var lengthsensorcal = ProgrammerUtilities.CopyAndRemoveBytes(ref sensorcalibrationdump, 1);
                var ts = ProgrammerUtilities.CopyAndRemoveBytes(ref sensorcalibrationdump, 8);
                (AlignmentMatrixAccel, SensitivityMatrixAccel, OffsetVectorAccel) = UtilCalibration.RetrieveKinematicCalibrationParametersFromCalibrationDump(sensorcalibrationdump);
                System.Console.WriteLine("LN Accel calibration parameters");
            }

        }

    }
}
