using ShimmerAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using static ShimmerAPI.ShimmerBluetooth;

namespace ShimmerAPI.Sensors
{
    public class WRMag : AbstractSensor
    {
        protected int ShimmerHardwareVersion = -1;
        public readonly int CALIBRATION_ID = 2;
        public readonly int SHIMMER_LIS3MDL_MAG = 41;
        public int SENSOR_ID { get; private set; }
        public double[,] AlignmentMatrixMag2 = new double[3, 3];
        public double[,] SensitivityMatrixMag2 = new double[3, 3];
        public double[,] OffsetVectorMag2 = new double[3, 1];
        public Dictionary<int, List<double[,]>> calibDetailsMag;

        public WRMag(int hardwareVersion)
        {
            ShimmerHardwareVersion = hardwareVersion;
            if (ShimmerHardwareVersion == (int)ShimmerBluetooth.ShimmerVersion.SHIMMER3R)
            {
                SENSOR_ID = SHIMMER_LIS3MDL_MAG;
                SensitivityMatrixMag2 = SENSITIVITY_MATRIX_MAG_50GA_SHIMMER3R_LIS2MDL;
                AlignmentMatrixMag2 = ALIGNMENT_MATRIX_MAG_SHIMMER3R_LIS2MDL;
                OffsetVectorMag2 = OFFSET_VECTOR_MAG_SHIMMER3R_LIS2MDL;
            }
        }

        public Dictionary<int, List<double[,]>> GetCalibDetails()
        {
            if (ShimmerHardwareVersion == (int)ShimmerBluetooth.ShimmerVersion.SHIMMER3R)
            {
                calibDetailsMag = new Dictionary<int, List<double[,]>>()
                {
                    {
                        0,
                        new List<double[,]>
                        {
                            ALIGNMENT_MATRIX_MAG_SHIMMER3R_LIS2MDL,
                            SENSITIVITY_MATRIX_MAG_50GA_SHIMMER3R_LIS2MDL,
                            OFFSET_VECTOR_MAG_SHIMMER3R_LIS2MDL
                        }
                    }
                };
            }
            return calibDetailsMag;
        }

        public void RetrieveKinematicCalibrationParametersFromCalibrationDump(byte[] sensorcalibrationdump)
        {

            var packetType = ProgrammerUtilities.CopyAndRemoveBytes(ref sensorcalibrationdump, 2);
            var sensorID = ((int)packetType[0]) + ((int)packetType[1] << 8);
            if (sensorID == SENSOR_ID)
            {
                var rangebytes = ProgrammerUtilities.CopyAndRemoveBytes(ref sensorcalibrationdump, 1);
                var lengthsensorcal = ProgrammerUtilities.CopyAndRemoveBytes(ref sensorcalibrationdump, 1);
                var ts = ProgrammerUtilities.CopyAndRemoveBytes(ref sensorcalibrationdump, 8);
                (AlignmentMatrixMag2, SensitivityMatrixMag2, OffsetVectorMag2) = UtilCalibration.RetrieveKinematicCalibrationParametersFromCalibrationDump(sensorcalibrationdump);
                System.Console.WriteLine("WR Mag calibration parameters");
            }

        }
    }
}
