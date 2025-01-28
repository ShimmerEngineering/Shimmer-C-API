using ShimmerAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using static ShimmerAPI.ShimmerBluetooth;

namespace ShimmerAPI.Sensors
{
    public class HighGAccel : AbstractSensor
    {
        protected int ShimmerHardwareVersion = -1;
        public readonly int CALIBRATION_ID = 2;
        public readonly int ALT_ACCEL = 33;
        public readonly int SHIMMER_ADXL371_ACCEL_HIGHG = 40;
        public int SENSOR_ID { get; private set; }
        public double[,] AlignmentMatrixAltAccel = new double[3, 3];
        public double[,] SensitivityMatrixAltAccel = new double[3, 3];
        public double[,] OffsetVectorAltAccel = new double[3, 1];
        public Dictionary<int, List<double[,]>> calibDetailsAltAccel;

        public HighGAccel(int hardwareVersion)
        {
            ShimmerHardwareVersion = hardwareVersion;
            if (ShimmerHardwareVersion == (int)ShimmerBluetooth.ShimmerVersion.SHIMMER3R)
            {
                SENSOR_ID = SHIMMER_ADXL371_ACCEL_HIGHG;

                SensitivityMatrixAltAccel = SENSITIVITY_MATRIX_HIGH_G_ACCEL_200G_SHIMMER3R_ADXL371;
                AlignmentMatrixAltAccel = ALIGNMENT_MATRIX_HIGH_G_ACCEL_SHIMMER3R_ADXL371;
                OffsetVectorAltAccel = OFFSET_VECTOR_ACCEL_HIGH_G_SHIMMER3R_ADXL371;
            }
            else
            {
                SENSOR_ID = ALT_ACCEL;
            }
        }

        public Dictionary<int, List<double[,]>> GetCalibDetails()
        {
            if (ShimmerHardwareVersion == (int)ShimmerBluetooth.ShimmerVersion.SHIMMER3R)
            {
                calibDetailsAltAccel = new Dictionary<int, List<double[,]>>()
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
            return calibDetailsAltAccel;
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
                (AlignmentMatrixAltAccel, SensitivityMatrixAltAccel, OffsetVectorAltAccel) = UtilCalibration.RetrieveKinematicCalibrationParametersFromCalibrationDump(sensorcalibrationdump);
                System.Console.WriteLine("High-G Accel calibration parameters");
            }

        }
    }
}
