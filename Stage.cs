using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dip3
{
    abstract class Stage
    {
        protected double Interpolation(double x, List<Data> arr)
        {
            int i = 0;
            while (x > arr[i].GetData().Item1 && i < arr.Count - 2) i++;
            return (arr[i + 1].GetData().Item2 - arr[i].GetData().Item2) / (arr[i + 1].GetData().Item1 - arr[i].GetData().Item1) * (x - arr[i].GetData().Item1) + arr[i].GetData().Item2;
        }

        protected double BilinearInterpolation(double x, double y, List<Data> data)
        {

            var a = data.GroupBy(q => q.GetData().Item1).Select(p => p.Key).ToArray();
            double[][] arr = new double[2][];
            arr[0] = data.GroupBy(q => q.GetData().Item1).Select(p => p.Key).ToArray();
            arr[1] = data.GroupBy(q => q.GetParams()[0]).Select(p => p.Key).ToArray();

            double[] z2 = data.Select(a1 => a1.GetData().Item2).ToArray();
            double[,] z = new double[arr[0].Length, arr[1].Length];

            for (int k1 = 0; k1 < arr[1].Length; k1++)
                for (int k2 = 0; k2 < arr[0].Length; k2++)
                    z[k2, k1] = z2[k2 + k1 * arr[0].Length];

           // double[,] z = { { 0.5985, 0.6476, 0.6857, 0.7122 }, { 0.4855, 0.5270, 0.5590, 0.5810 }, { 0.4327, 0.4470, 0.4580, 0.4670 } };


            int i = 0;
            int j = 0;
            while (x > arr[0][i] && i < arr[0].Length - 2) i++;
            while (y > arr[1][j] && j < arr[1].Length - 2) j++;

            double fr1 = (arr[0][i + 1] - x) / (arr[0][i + 1] - arr[0][i]) * z[i, j] + (x - arr[0][i]) / (arr[0][i + 1] - arr[0][i]) * z[i + 1, j];
            double fr2 = (arr[0][i + 1] - x) / (arr[0][i + 1] - arr[0][i]) * z[i, j + 1] + (x - arr[0][i]) / (arr[0][i + 1] - arr[0][i]) * z[i + 1, j + 1];
            double fp = (arr[1][j + 1] - y) / (arr[1][j + 1] - arr[1][j]) * fr1 + (y - arr[1][j]) / (arr[1][j + 1] - arr[1][j]) * fr2;

            return fp;
        }

    }
}
