using ShimmerAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using static ShimmerAPI.ShimmerBluetooth;

namespace ShimmerAPI.Sensors
{
    public class WRAccel : AbstractSensor
    {
        protected int ShimmerHardwareVersion = -1;
        public readonly int CALIBRATION_ID = 2;
        public readonly int SHIMMER_LSM303_ACCEL = 31;
        public readonly int SHIMMER_LIS2DW12_ACCEL_WR = 39;
        public int SENSOR_ID { get; private set; }
        public double[,] AlignmentMatrixAccel2 = new double[3, 3];
        public double[,] SensitivityMatrixAccel2 = new double[3, 3];
        public double[,] OffsetVectorAccel2 = new double[3, 1];
        public Dictionary<int, List<double[,]>> calibDetailsAccel2;

        public WRAccel(int hardwareVersion)
        {
            ShimmerHardwareVersion = hardwareVersion;
            if (ShimmerHardwareVersion == (int)ShimmerBluetooth.ShimmerVersion.SHIMMER3R)
            {
                SENSOR_ID = SHIMMER_LIS2DW12_ACCEL_WR;

                SensitivityMatrixAccel2 = SENSITIVITY_MATRIX_WIDE_RANGE_ACCEL_2G_SHIMMER3R_LIS2DW12;
                AlignmentMatrixAccel2 = ALIGNMENT_MATRIX_WIDE_RANGE_ACCEL_SHIMMER3R_LIS2DW12;
                OffsetVectorAccel2 = OFFSET_VECTOR_ACCEL_WIDE_RANGE_SHIMMER3R_LIS2DW12;
            }
            else
            {
                SENSOR_ID = SHIMMER_LSM303_ACCEL;

                SensitivityMatrixAccel2 = SENSITIVITY_MATRIX_WIDE_RANGE_ACCEL_2G_SHIMMER3_LSM303AH;
                AlignmentMatrixAccel2 = ALIGNMENT_MATRIX_WIDE_RANGE_ACCEL_SHIMMER3_LSM303AH;
                OffsetVectorAccel2 = OFFSET_VECTOR_ACCEL_WIDE_RANGE_SHIMMER3_LSM303AH;
            }
        }

        public Dictionary<int, List<double[,]>> GetCalibDetails()
        {
            if (ShimmerHardwareVersion == (int)ShimmerBluetooth.ShimmerVersion.SHIMMER3R)
            {
                calibDetailsAccel2 = new Dictionary<int, List<double[,]>>()
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
                calibDetailsAccel2 = new Dictionary<int, List<double[,]>>()
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
            return calibDetailsAccel2;
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
                (AlignmentMatrixAccel2, SensitivityMatrixAccel2, OffsetVectorAccel2) = UtilCalibration.RetrieveKinematicCalibrationParametersFromCalibrationDump(sensorcalibrationdump);
                System.Console.WriteLine("WR Accel calibration parameters");
            }

        }
    }
}
