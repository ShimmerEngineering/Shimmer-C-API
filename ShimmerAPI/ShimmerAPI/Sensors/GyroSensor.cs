using ShimmerAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using static ShimmerAPI.ShimmerBluetooth;

namespace ShimmerAPI.Sensors
{
    public class GyroSensor : AbstractSensor
    {
        public readonly int CALIBRATION_ID = 2;
        public readonly int SHIMMER_MPU9X50_GYRO = 30;
        public readonly int SHIMMER_LSM6DSV_GYRO = 38;
        public int SENSOR_ID { get; private set; }
        public int ShimmerHardwareVersion { get; private set; }
        public Dictionary<int, List<double[,]>> CalibDetails { get; private set; }

        public double[,] AlignmentMatrixGyro = new double[3, 3];
        public double[,] SensitivityMatrixGyro = new double[3, 3];
        public double[,] OffsetVectorGyro = new double[3, 1];

        public GyroSensor(int hardwareVersion)
        {
            ShimmerHardwareVersion = hardwareVersion;
            CreateDefaultCalibParams();
        }

        public void CreateDefaultCalibParams()
        {
            if (ShimmerHardwareVersion == (int)ShimmerBluetooth.ShimmerVersion.SHIMMER3R)
            {
                SENSOR_ID = SHIMMER_LSM6DSV_GYRO;
                CalibDetails = new Dictionary<int, List<double[,]>>()
                {
                    {
                        0,
                        new List<double[,]>
                        {
                            ALIGNMENT_MATRIX_GYRO_SHIMMER3R_LSM6DSV,
                            SENSITIVITIY_MATRIX_GYRO_125DPS_SHIMMER3R_LSM6DSV,
                            OFFSET_VECTOR_GYRO_SHIMMER3R_LSM6DSV
                        }
                    },
                    {
                        1,
                        new List<double[,]>
                        {
                            ALIGNMENT_MATRIX_GYRO_SHIMMER3R_LSM6DSV,
                            SENSITIVITIY_MATRIX_GYRO_250DPS_SHIMMER3R_LSM6DSV,
                            OFFSET_VECTOR_GYRO_SHIMMER3R_LSM6DSV
                        }
                    },
                    {
                        2,
                        new List<double[,]>
                        {
                            ALIGNMENT_MATRIX_GYRO_SHIMMER3R_LSM6DSV,
                            SENSITIVITIY_MATRIX_GYRO_500DPS_SHIMMER3R_LSM6DSV,
                            OFFSET_VECTOR_GYRO_SHIMMER3R_LSM6DSV
                        }
                    },
                    {
                        3,
                        new List<double[,]>
                        {
                            ALIGNMENT_MATRIX_GYRO_SHIMMER3R_LSM6DSV,
                            SENSITIVITIY_MATRIX_GYRO_1000DPS_SHIMMER3R_LSM6DSV,
                            OFFSET_VECTOR_GYRO_SHIMMER3R_LSM6DSV
                        }
                    },
                    {
                        4,
                        new List<double[,]>
                        {
                            ALIGNMENT_MATRIX_GYRO_SHIMMER3R_LSM6DSV,
                            SENSITIVITIY_MATRIX_GYRO_2000DPS_SHIMMER3R_LSM6DSV,
                            OFFSET_VECTOR_GYRO_SHIMMER3R_LSM6DSV
                        }
                    },
                    {
                        5,
                        new List<double[,]>
                        {
                            ALIGNMENT_MATRIX_GYRO_SHIMMER3R_LSM6DSV,
                            SENSITIVITIY_MATRIX_GYRO_4000DPS_SHIMMER3R_LSM6DSV,
                            OFFSET_VECTOR_GYRO_SHIMMER3R_LSM6DSV
                        }
                    }
                };
            }
            else
            {
                SENSOR_ID = SHIMMER_MPU9X50_GYRO;
                CalibDetails = new Dictionary<int, List<double[,]>>()
                {
                    {
                        0,
                        new List<double[,]>
                        {
                            ALIGNMENT_MATRIX_GYRO_SHIMMER3,
                            SENSITIVITIY_MATRIX_GYRO_250DPS_SHIMMER3,
                            OFFSET_VECTOR_GYRO_SHIMMER3
                        }
                    },
                    {
                        1,
                        new List<double[,]>
                        {
                            ALIGNMENT_MATRIX_GYRO_SHIMMER3,
                            SENSITIVITIY_MATRIX_GYRO_500DPS_SHIMMER3,
                            OFFSET_VECTOR_GYRO_SHIMMER3
                        }
                    },
                    {
                        2,
                        new List<double[,]>
                        {
                            ALIGNMENT_MATRIX_GYRO_SHIMMER3,
                            SENSITIVITIY_MATRIX_GYRO_1000DPS_SHIMMER3,
                            OFFSET_VECTOR_GYRO_SHIMMER3
                        }
                    },
                    {
                        3,
                        new List<double[,]>
                        {
                            ALIGNMENT_MATRIX_GYRO_SHIMMER3,
                            SENSITIVITIY_MATRIX_GYRO_2000DPS_SHIMMER3,
                            OFFSET_VECTOR_GYRO_SHIMMER3
                        }
                    }
                };
            }

            if (CalibDetails.TryGetValue(0, out var defaultCalib))
            {
                AlignmentMatrixGyro = defaultCalib[0];
                SensitivityMatrixGyro = defaultCalib[1];
                OffsetVectorGyro = defaultCalib[2];
            }
        }

        public void RetrieveKinematicCalibrationParametersFromCalibrationDump(byte[] sensorcalibrationdump)
        {
            (AlignmentMatrixGyro, SensitivityMatrixGyro, OffsetVectorGyro) = UtilCalibration.RetrieveKinematicCalibrationParametersFromCalibrationDump(sensorcalibrationdump);
            System.Console.WriteLine("Gyro calibration parameters");
        }
    }
}
