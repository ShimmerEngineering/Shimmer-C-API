using ShimmerAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static ShimmerAPI.ShimmerBluetooth;

namespace ShimmerAPI.Sensors
{
    public class LNAccel
    {
        protected int ShimmerHardwareVersion = -1;
        public readonly int CALIBRATION_ID = 2;
        public readonly int SENSOR_ID = 37;
        public double[,] AlignmentMatrixAccel = new double[3, 3];
        public double[,] SensitivityMatrixAccel = new double[3, 3];
        public double[,] OffsetVectorAccel = new double[3, 1];

        public static readonly double[,] SENSITIVITY_MATRIX_LOW_NOISE_ACCEL_2G_SHIMMER3R_LSM6DSV = new double[3, 3] { { 1672, 0, 0 }, { 0, 1672, 0 }, { 0, 0, 1672 } };
        public static readonly double[,] SENSITIVITY_MATRIX_LOW_NOISE_ACCEL_4G_SHIMMER3R_LSM6DSV = new double[3, 3] { { 836, 0, 0 }, { 0, 836, 0 }, { 0, 0, 836 } };
        public static readonly double[,] SENSITIVITY_MATRIX_LOW_NOISE_ACCEL_8G_SHIMMER3R_LSM6DSV = new double[3, 3] { { 418, 0, 0 }, { 0, 418, 0 }, { 0, 0, 418 } };
        public static readonly double[,] SENSITIVITY_MATRIX_LOW_NOISE_ACCEL_16G_SHIMMER3R_LSM6DSV = new double[3, 3] { { 209, 0, 0 }, { 0, 209, 0 }, { 0, 0, 209 } };
        public static readonly double[,] ALIGNMENT_MATRIX_LOW_NOISE_ACCEL_SHIMMER3R_LSM6DSV = new double[3, 3] { { -1, 0, 0 }, { 0, 1, 0 }, { 0, 0, -1 } };
        public static readonly double[,] OFFSET_VECTOR_ACCEL_LOW_NOISE_SHIMMER3R_LSM6DSV = new double[3, 1] { { 0 }, { 0 }, { 0 } };

        public static List<int> ListofLNAccelRange = new List<int> { 0, 1, 2, 3 };

        public static List<double[,]> sensitivityMatrixMap = new List<double[,]>
        {
            SENSITIVITY_MATRIX_LOW_NOISE_ACCEL_2G_SHIMMER3R_LSM6DSV,
            SENSITIVITY_MATRIX_LOW_NOISE_ACCEL_4G_SHIMMER3R_LSM6DSV,
            SENSITIVITY_MATRIX_LOW_NOISE_ACCEL_8G_SHIMMER3R_LSM6DSV,
            SENSITIVITY_MATRIX_LOW_NOISE_ACCEL_16G_SHIMMER3R_LSM6DSV
        };

        public static List<double[,]> alignmentMatrixMap = new List<double[,]>
        {
            ALIGNMENT_MATRIX_LOW_NOISE_ACCEL_SHIMMER3R_LSM6DSV
        };

        public static List<double[,]> offsetVectorMap = new List<double[,]>
        {
            OFFSET_VECTOR_ACCEL_LOW_NOISE_SHIMMER3R_LSM6DSV
        };

        public static Dictionary<int, List<double[,]>> calibDetailsAccelLN = new Dictionary<int, List<double[,]>>()
        {
            {
                ListofLNAccelRange[0],
                new List<double[,]>
                {
                    alignmentMatrixMap[0],
                    sensitivityMatrixMap[0],
                    offsetVectorMap[0]
                }
            },
            {
                ListofLNAccelRange[1],
                new List<double[,]>
                {
                    alignmentMatrixMap[0],
                    sensitivityMatrixMap[1],
                    offsetVectorMap[0]
                }
            },
            {
                ListofLNAccelRange[2],
                new List<double[,]>
                {
                    alignmentMatrixMap[0],
                    sensitivityMatrixMap[2],
                    offsetVectorMap[0]
                }
            },
            {
                ListofLNAccelRange[3],
                new List<double[,]>
                {
                    alignmentMatrixMap[0],
                    sensitivityMatrixMap[3],
                    offsetVectorMap[0]
                }
            }
        };

        public LNAccel(int hardwareVersion)
        {
            ShimmerHardwareVersion = hardwareVersion;
        }

        public Dictionary<int, List<double[,]>> GetCalibDetails()
        {
            return calibDetailsAccelLN;
        }

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
                Console.WriteLine("LN Accel calibration parameters retrieved successfully.");
            }
            else
            {
                Console.WriteLine("Invalid calibration ID.");
            }
        }
    }
}
