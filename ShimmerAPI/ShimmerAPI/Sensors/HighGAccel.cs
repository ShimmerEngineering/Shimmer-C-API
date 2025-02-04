using ShimmerAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using static ShimmerAPI.ShimmerBluetooth;

namespace ShimmerAPI.Sensors
{
    public class HighGAccel : AbstractSensor
    {
        public readonly int CALIBRATION_ID = 2;
        public readonly int ALT_ACCEL = 33;
        public readonly int SHIMMER_ADXL371_ACCEL_HIGHG = 40;
        public int SENSOR_ID { get; private set; }
        public int ShimmerHardwareVersion { get; private set; }
        public Dictionary<int, List<double[,]>> CalibDetails { get; private set; }
        public double[,] AlignmentMatrixAltAccel = new double[3, 3];
        public double[,] SensitivityMatrixAltAccel = new double[3, 3];
        public double[,] OffsetVectorAltAccel = new double[3, 1];

        public HighGAccel(int hardwareVersion)
        {
            ShimmerHardwareVersion = hardwareVersion;
            CreateDefaultCalibParams();
        }

        public void CreateDefaultCalibParams()
        {
            if (ShimmerHardwareVersion == (int)ShimmerBluetooth.ShimmerVersion.SHIMMER3R)
            {
                SENSOR_ID = SHIMMER_ADXL371_ACCEL_HIGHG;
                CalibDetails = new Dictionary<int, List<double[,]>>()
                {
                    {
                        0,
                        new List<double[,]>
                        {
                            ALIGNMENT_MATRIX_HIGH_G_ACCEL_SHIMMER3R_ADXL371,
                            SENSITIVITY_MATRIX_HIGH_G_ACCEL_200G_SHIMMER3R_ADXL371,
                            OFFSET_VECTOR_ACCEL_HIGH_G_SHIMMER3R_ADXL371
                        }
                    }
                };
            }
            else
            {
                SENSOR_ID = ALT_ACCEL;
            }

            if (CalibDetails.TryGetValue(0, out var defaultCalib))
            {
                AlignmentMatrixAltAccel = defaultCalib[0];
                SensitivityMatrixAltAccel = defaultCalib[1];
                OffsetVectorAltAccel = defaultCalib[2];
            }
        }

        public void RetrieveKinematicCalibrationParametersFromCalibrationDump(byte[] sensorcalibrationdump)
        {
            (AlignmentMatrixAltAccel, SensitivityMatrixAltAccel, OffsetVectorAltAccel) = UtilCalibration.RetrieveKinematicCalibrationParametersFromCalibrationDump(sensorcalibrationdump);
            System.Console.WriteLine("High-G Accel calibration parameters");
        }
    }
}
