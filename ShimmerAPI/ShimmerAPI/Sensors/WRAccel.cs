using ShimmerAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using static ShimmerAPI.ShimmerBluetooth;

namespace ShimmerAPI.Sensors
{
    public class WRAccel : AbstractSensor
    {
        public readonly int CALIBRATION_ID = 2;
        public readonly int SHIMMER_LSM303_ACCEL = 31;
        public readonly int SHIMMER_LIS2DW12_ACCEL_WR = 39;
        public int ShimmerHardwareVersion { get; private set; }
        public int SENSOR_ID { get; private set; }
        public Dictionary<int, List<double[,]>> CalibDetails { get; private set; }
        public double[,] AlignmentMatrixAccel2 = new double[3, 3];
        public double[,] SensitivityMatrixAccel2 = new double[3, 3];
        public double[,] OffsetVectorAccel2 = new double[3, 1];

        public WRAccel(int hardwareVersion)
        {
            ShimmerHardwareVersion = hardwareVersion;
            CreateDefaultCalibParams();
        }

        public void CreateDefaultCalibParams()
        {
            if (ShimmerHardwareVersion == (int)ShimmerBluetooth.ShimmerVersion.SHIMMER3R)
            {
                SENSOR_ID = SHIMMER_LIS2DW12_ACCEL_WR;
                CalibDetails = new Dictionary<int, List<double[,]>>()
                {
                    {
                        0,
                        new List<double[,]>
                        {
                            ALIGNMENT_MATRIX_WIDE_RANGE_ACCEL_SHIMMER3R_LIS2DW12,
                            SENSITIVITY_MATRIX_WIDE_RANGE_ACCEL_2G_SHIMMER3R_LIS2DW12,
                            OFFSET_VECTOR_ACCEL_WIDE_RANGE_SHIMMER3R_LIS2DW12
                        }
                    },
                    {
                        1,
                        new List<double[,]>
                        {
                            ALIGNMENT_MATRIX_WIDE_RANGE_ACCEL_SHIMMER3R_LIS2DW12,
                            SENSITIVITY_MATRIX_WIDE_RANGE_ACCEL_4G_SHIMMER3R_LIS2DW12,
                            OFFSET_VECTOR_ACCEL_WIDE_RANGE_SHIMMER3R_LIS2DW12
                        }
                    },
                    {
                        2,
                        new List<double[,]>
                        {
                            ALIGNMENT_MATRIX_WIDE_RANGE_ACCEL_SHIMMER3R_LIS2DW12,
                            SENSITIVITY_MATRIX_WIDE_RANGE_ACCEL_8G_SHIMMER3R_LIS2DW12,
                            OFFSET_VECTOR_ACCEL_WIDE_RANGE_SHIMMER3R_LIS2DW12
                        }
                    },
                    {
                        3,
                        new List<double[,]>
                        {
                            ALIGNMENT_MATRIX_WIDE_RANGE_ACCEL_SHIMMER3R_LIS2DW12,
                            SENSITIVITY_MATRIX_WIDE_RANGE_ACCEL_16G_SHIMMER3R_LIS2DW12,
                            OFFSET_VECTOR_ACCEL_WIDE_RANGE_SHIMMER3R_LIS2DW12
                        }
                    }
                };
            }
            else
            {
                SENSOR_ID = SHIMMER_LSM303_ACCEL;
                CalibDetails = new Dictionary<int, List<double[,]>>()
                {
                    {
                        0,
                        new List<double[,]>
                        {
                            ALIGNMENT_MATRIX_WIDE_RANGE_ACCEL_SHIMMER3_LSM303AH,
                            SENSITIVITY_MATRIX_WIDE_RANGE_ACCEL_2G_SHIMMER3_LSM303AH,
                            OFFSET_VECTOR_ACCEL_WIDE_RANGE_SHIMMER3_LSM303AH
                        }
                    },
                    {
                        2,
                        new List<double[,]>
                        {
                            ALIGNMENT_MATRIX_WIDE_RANGE_ACCEL_SHIMMER3_LSM303AH,
                            SENSITIVITY_MATRIX_WIDE_RANGE_ACCEL_4G_SHIMMER3_LSM303AH,
                            OFFSET_VECTOR_ACCEL_WIDE_RANGE_SHIMMER3_LSM303AH
                        }
                    },
                    {
                        3,
                        new List<double[,]>
                        {
                            ALIGNMENT_MATRIX_WIDE_RANGE_ACCEL_SHIMMER3_LSM303AH,
                            SENSITIVITY_MATRIX_WIDE_RANGE_ACCEL_8G_SHIMMER3_LSM303AH,
                            OFFSET_VECTOR_ACCEL_WIDE_RANGE_SHIMMER3_LSM303AH
                        }
                    },
                    {
                        1,
                        new List<double[,]>
                        {
                            ALIGNMENT_MATRIX_WIDE_RANGE_ACCEL_SHIMMER3_LSM303AH,
                            SENSITIVITY_MATRIX_WIDE_RANGE_ACCEL_16G_SHIMMER3_LSM303AH,
                            OFFSET_VECTOR_ACCEL_WIDE_RANGE_SHIMMER3_LSM303AH
                        }
                    }
                };
            }

            if (CalibDetails.TryGetValue(0, out var defaultCalib))
            {
                AlignmentMatrixAccel2 = defaultCalib[0];
                SensitivityMatrixAccel2 = defaultCalib[1];
                OffsetVectorAccel2 = defaultCalib[2];
            }
        }

        public void RetrieveKinematicCalibrationParametersFromCalibrationDump(byte[] sensorcalibrationdump)
        {
            (AlignmentMatrixAccel2, SensitivityMatrixAccel2, OffsetVectorAccel2) = UtilCalibration.RetrieveKinematicCalibrationParametersFromCalibrationDump(sensorcalibrationdump);
            System.Console.WriteLine("WR Accel calibration parameters");
        }
    }
}
