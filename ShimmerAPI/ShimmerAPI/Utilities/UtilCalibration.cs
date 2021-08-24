using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShimmerAPI.Utilities
{
    public class UtilCalibration
    {
        public static double NudgeDouble(double valToNudge, double minVal, double maxVal)
        {
            return Math.Max(minVal, Math.Min(maxVal, valToNudge));
        }

        public static double[] CalibrateInertialSensorData(double[] data, double[,] AM, double[,] SM, double[,] OV)
        {
            /*  Based on the theory outlined by Ferraris F, Grimaldi U, and Parvis M.  
               in "Procedure for effortless in-field calibration of three-axis rate gyros and accelerometers" Sens. Mater. 1995; 7: 311-30.            
               C = [R^(-1)] .[K^(-1)] .([U]-[B])
                where.....
                [C] -> [3 x n] Calibrated Data Matrix 
                [U] -> [3 x n] Uncalibrated Data Matrix
                [B] ->  [3 x n] Replicated Sensor Offset Vector Matrix 
                [R^(-1)] -> [3 x 3] Inverse Alignment Matrix
                [K^(-1)] -> [3 x 3] Inverse Sensitivity Matrix
                n = Number of Samples
                */
            double[] tempdata = data;
            double[,] data2d = new double[3, 1];
            data2d[0, 0] = data[0];
            data2d[1, 0] = data[1];
            data2d[2, 0] = data[2];
            data2d = MatrixMultiplication(MatrixMultiplication(MatrixInverse3x3(AM), MatrixInverse3x3(SM)), MatrixMinus(data2d, OV));
            tempdata[0] = data2d[0, 0];
            tempdata[1] = data2d[1, 0];
            tempdata[2] = data2d[2, 0];
            return tempdata;
        }

        protected static double[,] MatrixMinus(double[,] a, double[,] b)
        {

            int aRows = a.GetLength(0),
            aColumns = a.GetLength(1),
            bRows = b.GetLength(0),
            bColumns = b.GetLength(1);
            double[,] resultant = new double[aRows, bColumns];
            for (int i = 0; i < aRows; i++)
            { // aRow
                for (int k = 0; k < aColumns; k++)
                { // aColumn
                    resultant[i, k] = a[i, k] - b[i, k];
                }
            }
            return resultant;
        }

        protected static double[,] MatrixInverse3x3(double[,] data)
        {
            double a, b, c, d, e, f, g, h, i;
            a = data[0, 0];
            b = data[0, 1];
            c = data[0, 2];
            d = data[1, 0];
            e = data[1, 1];
            f = data[1, 2];
            g = data[2, 0];
            h = data[2, 1];
            i = data[2, 2];
            //
            double deter = a * e * i + b * f * g + c * d * h - c * e * g - b * d * i - a * f * h;
            double[,] answer = new double[3, 3];
            answer[0, 0] = (1 / deter) * (e * i - f * h);

            answer[0, 1] = (1 / deter) * (c * h - b * i);
            answer[0, 2] = (1 / deter) * (b * f - c * e);
            answer[1, 0] = (1 / deter) * (f * g - d * i);
            answer[1, 1] = (1 / deter) * (a * i - c * g);
            answer[1, 2] = (1 / deter) * (c * d - a * f);
            answer[2, 0] = (1 / deter) * (d * h - e * g);
            answer[2, 1] = (1 / deter) * (g * b - a * h);
            answer[2, 2] = (1 / deter) * (a * e - b * d);
            return answer;
        }
        protected static double[,] MatrixMultiplication(double[,] a, double[,] b)
        {

            int aRows = a.GetLength(0),
                aColumns = a.GetLength(1),
                 bRows = b.GetLength(0),
                 bColumns = b.GetLength(1);
            double[,] resultant = new double[aRows, bColumns];

            for (int i = 0; i < aRows; i++)
            { // aRow
                for (int j = 0; j < bColumns; j++)
                { // bColumn
                    for (int k = 0; k < aColumns; k++)
                    { // aColumn
                        resultant[i, j] += a[i, k] * b[k, j];
                    }
                }
            }

            return resultant;
        }
    }
}
