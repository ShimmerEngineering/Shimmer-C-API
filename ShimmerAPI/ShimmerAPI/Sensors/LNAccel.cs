using ShimmerAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static ShimmerAPI.ShimmerBluetooth;

namespace ShimmerAPI.Sensors
{
    public class LNAccel : AbstractSensor
    {
        public readonly int CALIBRATION_ID = 2;
        public readonly int SHIMMER_ANALOG_ACCEL = 2;
        public readonly int SHIMMER_LSM6DSV_ACCEL_LN = 37;
        public int SENSOR_ID { get; private set; }
        public int ShimmerHardwareVersion { get; private set; }
        public Dictionary<int, List<double[,]>> CalibDetails { get; private set; }

        public double[,] AlignmentMatrixAccel = new double[3, 3];
        public double[,] SensitivityMatrixAccel = new double[3, 3];
        public double[,] OffsetVectorAccel = new double[3, 1];

        public LNAccel(int hardwareVersion)
        {
            ShimmerHardwareVersion = hardwareVersion;
            CreateDefaultCalibParams();
        }

        private void CreateDefaultCalibParams()
        {
            if(ShimmerHardwareVersion == (int)ShimmerBluetooth.ShimmerVersion.SHIMMER3R)
            {
                SENSOR_ID = SHIMMER_LSM6DSV_ACCEL_LN;
                CalibDetails = new Dictionary<int, List<double[,]>>()
                {
                    {
                        0,
                        new List<double[,]>
                        {
                            ALIGNMENT_MATRIX_LOW_NOISE_ACCEL_SHIMMER3R_LSM6DSV,
                            SENSITIVITY_MATRIX_LOW_NOISE_ACCEL_2G_SHIMMER3R_LSM6DSV,
                            OFFSET_VECTOR_ACCEL_LOW_NOISE_SHIMMER3R_LSM6DSV
                        }
                    },
                    {
                        1,
                        new List<double[,]>
                        {
                            ALIGNMENT_MATRIX_LOW_NOISE_ACCEL_SHIMMER3R_LSM6DSV,
                            SENSITIVITY_MATRIX_LOW_NOISE_ACCEL_4G_SHIMMER3R_LSM6DSV,
                            OFFSET_VECTOR_ACCEL_LOW_NOISE_SHIMMER3R_LSM6DSV
                        }
                    },
                    {
                        2,
                        new List<double[,]>
                        {
                            ALIGNMENT_MATRIX_LOW_NOISE_ACCEL_SHIMMER3R_LSM6DSV,
                            SENSITIVITY_MATRIX_LOW_NOISE_ACCEL_8G_SHIMMER3R_LSM6DSV,
                            OFFSET_VECTOR_ACCEL_LOW_NOISE_SHIMMER3R_LSM6DSV
                        }
                    },
                    {
                        3,
                        new List<double[,]>
                        {
                            ALIGNMENT_MATRIX_LOW_NOISE_ACCEL_SHIMMER3R_LSM6DSV,
                            SENSITIVITY_MATRIX_LOW_NOISE_ACCEL_16G_SHIMMER3R_LSM6DSV,
                            OFFSET_VECTOR_ACCEL_LOW_NOISE_SHIMMER3R_LSM6DSV
                        }
                    }
                };
            }
            else
            {
                SENSOR_ID = SHIMMER_ANALOG_ACCEL;
                CalibDetails = new Dictionary<int, List<double[,]>>()
                {
                    {
                        0,
                        new List<double[,]>
                        {
                            ALIGNMENT_MATRIX_LOW_NOISE_ACCEL_SHIMMER3_KXTC9_2050,
                            SENSITIVITY_MATRIX_LOW_NOISE_ACCEL_SHIMMER3_KXTC9_2050,
                            OFFSET_VECTOR_ACCEL_LOW_NOISE_SHIMMER3_KXTC9_2050
                        }
                    }
                };
            }

            if (CalibDetails.TryGetValue(0, out var defaultCalib))
            {
                AlignmentMatrixAccel = defaultCalib[0];
                SensitivityMatrixAccel = defaultCalib[1];
                OffsetVectorAccel = defaultCalib[2];
            }
        }

        public void RetrieveKinematicCalibrationParametersFromCalibrationDump(byte[] sensorcalibrationdump)
        {
            (AlignmentMatrixAccel, SensitivityMatrixAccel, OffsetVectorAccel) = UtilCalibration.RetrieveKinematicCalibrationParametersFromCalibrationDump(sensorcalibrationdump);
            Console.WriteLine("LN Accel calibration parameters retrieved successfully.");
        }
    }
}
