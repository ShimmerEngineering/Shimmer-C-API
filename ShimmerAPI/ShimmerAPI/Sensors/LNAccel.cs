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
        protected int ShimmerHardwareVersion = -1;
        public readonly int CALIBRATION_ID = 2;
        public int SENSOR_ID { get; private set; }
        public double[,] AlignmentMatrixAccel = new double[3, 3];
        public double[,] SensitivityMatrixAccel = new double[3, 3];
        public double[,] OffsetVectorAccel = new double[3, 1];
        public Dictionary<int, List<double[,]>> calibDetailsAccelLN;

        public LNAccel(int hardwareVersion)
        {
            ShimmerHardwareVersion = hardwareVersion;
            if (ShimmerHardwareVersion == 10)
            {
                SENSOR_ID = 37;

                SensitivityMatrixAccel = SENSITIVITY_MATRIX_LOW_NOISE_ACCEL_2G_SHIMMER3R_LSM6DSV;
                AlignmentMatrixAccel = ALIGNMENT_MATRIX_LOW_NOISE_ACCEL_SHIMMER3R_LSM6DSV;
                OffsetVectorAccel = OFFSET_VECTOR_ACCEL_LOW_NOISE_SHIMMER3R_LSM6DSV;
            }
            else
            {
                SENSOR_ID = 2;

                SensitivityMatrixAccel = SENSITIVITY_MATRIX_LOW_NOISE_ACCEL_SHIMMER3_KXTC9_2050;
                AlignmentMatrixAccel = ALIGNMENT_MATRIX_LOW_NOISE_ACCEL_SHIMMER3_KXTC9_2050;
                OffsetVectorAccel = OFFSET_VECTOR_ACCEL_LOW_NOISE_SHIMMER3_KXTC9_2050;
            }
        }

        public Dictionary<int, List<double[,]>> GetCalibDetails()
        {
            if(ShimmerHardwareVersion == 10)
            { 
                calibDetailsAccelLN = new Dictionary<int, List<double[,]>>()
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
                calibDetailsAccelLN = new Dictionary<int, List<double[,]>>()
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

            return calibDetailsAccelLN;
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
                (AlignmentMatrixAccel, SensitivityMatrixAccel, OffsetVectorAccel) = UtilCalibration.RetrieveKinematicCalibrationParametersFromCalibrationDump(sensorcalibrationdump);
                Console.WriteLine("LN Accel calibration parameters retrieved successfully.");
            }
            else
            {
                Console.WriteLine("Invalid calibration ID.");
            }
        }
    }
}
