using ShimmerAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using static ShimmerAPI.ShimmerBluetooth;

namespace ShimmerAPI.Sensors
{
    public class MagSensor : AbstractSensor
    {
        public readonly int CALIBRATION_ID = 2;
        public readonly int SHIMMER_LSM303_MAG = 32;
        public readonly int SHIMMER_LIS2MDL_MAG = 42;
        public int SENSOR_ID { get; private set; }
        public int ShimmerHardwareVersion { get; private set; }
        public Dictionary<int, List<double[,]>> CalibDetails { get; private set; }
        public double[,] AlignmentMatrixMag = new double[3, 3];
        public double[,] SensitivityMatrixMag = new double[3, 3];
        public double[,] OffsetVectorMag = new double[3, 1];

        public MagSensor(int hardwareVersion)
        {
            ShimmerHardwareVersion = hardwareVersion;
            CreateDefaultCalibParams();
        }

        public void CreateDefaultCalibParams()
        {
            if (ShimmerHardwareVersion == (int)ShimmerBluetooth.ShimmerVersion.SHIMMER3R)
            {
                SENSOR_ID = SHIMMER_LIS2MDL_MAG;
                CalibDetails = new Dictionary<int, List<double[,]>>()
                {
                    {
                        0,
                        new List<double[,]>
                        {
                            ALIGNMENT_MATRIX_MAG_SHIMMER3R_LIS3MDL,
                            SENSITIVITY_MATRIX_MAG_4GA_SHIMMER3R_LIS3MDL,
                            OFFSET_VECTOR_MAG_SHIMMER3R_LIS3MDL
                        }
                    },
                {
                        1,
                        new List<double[,]>
                        {
                            ALIGNMENT_MATRIX_MAG_SHIMMER3R_LIS3MDL,
                            SENSITIVITY_MATRIX_MAG_8GA_SHIMMER3R_LIS3MDL,
                            OFFSET_VECTOR_MAG_SHIMMER3R_LIS3MDL
                        }
                    },
                {
                        2,
                        new List<double[,]>
                        {
                            ALIGNMENT_MATRIX_MAG_SHIMMER3R_LIS3MDL,
                            SENSITIVITY_MATRIX_MAG_12GA_SHIMMER3R_LIS3MDL,
                            OFFSET_VECTOR_MAG_SHIMMER3R_LIS3MDL
                        }
                    },
                {
                        3,
                        new List<double[,]>
                        {
                            ALIGNMENT_MATRIX_MAG_SHIMMER3R_LIS3MDL,
                            SENSITIVITY_MATRIX_MAG_16GA_SHIMMER3R_LIS3MDL,
                            OFFSET_VECTOR_MAG_SHIMMER3R_LIS3MDL
                        }
                    },
                };
            }
            else
            {
                SENSOR_ID = SHIMMER_LSM303_MAG;
                CalibDetails = new Dictionary<int, List<double[,]>>()
                {
                    {
                        0,
                        new List<double[,]>
                        {
                            ALIGNMENT_MATRIX_MAG_SHIMMER3_LSM303AH,
                            SENSITIVITY_MATRIX_MAG_50GA_SHIMMER3_LSM303AH,
                            OFFSET_VECTOR_MAG_SHIMMER3_LSM303AH
                        }
                    }
                };
            }

            if (CalibDetails.TryGetValue(0, out var defaultCalib))
            {
                AlignmentMatrixMag = defaultCalib[0];
                SensitivityMatrixMag = defaultCalib[1];
                OffsetVectorMag = defaultCalib[2];
            }

        }

        public void RetrieveKinematicCalibrationParametersFromCalibrationDump(byte[] sensorcalibrationdump)
        {
            (AlignmentMatrixMag, SensitivityMatrixMag, OffsetVectorMag) = UtilCalibration.RetrieveKinematicCalibrationParametersFromCalibrationDump(sensorcalibrationdump);
            System.Console.WriteLine("Mag calibration parameters");
        }
    }
}
