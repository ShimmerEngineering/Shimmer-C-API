using ShimmerAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using static ShimmerAPI.ShimmerBluetooth;

namespace ShimmerAPI.Sensors
{
    public class WRMag : AbstractSensor
    {
        public readonly int CALIBRATION_ID = 2;
        public readonly int ALT_MAG = 34;
        public readonly int SHIMMER_LIS3MDL_MAG = 41;
        public int ShimmerHardwareVersion { get; private set; }
        public int SENSOR_ID { get; private set; }
        public Dictionary<int, List<double[,]>> CalibDetails { get; private set; }
        public double[,] AlignmentMatrixMag2 = new double[3, 3];
        public double[,] SensitivityMatrixMag2 = new double[3, 3];
        public double[,] OffsetVectorMag2 = new double[3, 1];

        public WRMag(int hardwareVersion)
        {
            ShimmerHardwareVersion = hardwareVersion;
            CreateDefaultCalibParams();
        }

        public void CreateDefaultCalibParams()
        {
            if (ShimmerHardwareVersion == (int)ShimmerBluetooth.ShimmerVersion.SHIMMER3R)
            {
                SENSOR_ID = SHIMMER_LIS3MDL_MAG;
                CalibDetails = new Dictionary<int, List<double[,]>>()
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
            else
            {
                SENSOR_ID = ALT_MAG;
            }

            if (CalibDetails.TryGetValue(0, out var defaultCalib))
            {
                AlignmentMatrixMag2 = defaultCalib[0];
                SensitivityMatrixMag2 = defaultCalib[1];
                OffsetVectorMag2 = defaultCalib[2];
            }
        }

        public void RetrieveKinematicCalibrationParametersFromCalibrationDump(byte[] sensorcalibrationdump)
        {
            (AlignmentMatrixMag2, SensitivityMatrixMag2, OffsetVectorMag2) = UtilCalibration.RetrieveKinematicCalibrationParametersFromCalibrationDump(sensorcalibrationdump);
            System.Console.WriteLine("WR Mag calibration parameters");
        }
    }
}
