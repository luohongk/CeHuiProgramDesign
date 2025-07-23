using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace 空间后方交汇模拟7._22
{
    internal class Maritx
    {
        public double[,] Cal_Add(double[,] data1, double[,] data2)
        {
            int rows = data1.GetLength(0);
            int cols = data2.GetLength(1);
            var sum = new double[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    sum[i, j] = data1[i, j] + data2[i, j];
                }
            }
            return sum;
        }
        public double[,] Cal_Remove(double[,] data1, double[,] data2)
        {
            int rows = data1.GetLength(0);
            int cols = data2.GetLength(1);
            var sum = new double[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    sum[i, j] = data1[i, j] - data2[i, j];
                }
            }
            return sum;
        }
        public double[,] Cal_T(double[,] data1)
        {
            int rows = data1.GetLength(0);
            int cols = data1.GetLength(1);
            var sum = new double[cols, rows];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    sum[j, i] = data1[i, j];
                }
            }
            return sum;
        }
        public double[,] Cal_Mulitp(double[,] data1, double[,] data2)
        {
            int rows1 = data1.GetLength(0);
            int cols1 = data1.GetLength(1);
            int rows2 = data2.GetLength(0);
            int cols2 = data2.GetLength(1);
            var sum = new double[rows1, cols2];
            for (int i = 0; i < rows1; i++)
            {
                for (int j = 0; j < cols2; j++)
                {
                    double diag = 0;
                    for (int k = 0; k < cols1; k++)
                    {
                        diag += data1[i, k] * data2[k, j];
                    }
                    sum[i, j] = diag;
                }
            }
            return sum;

        }
        /// <summary>
        /// 求逆
        /// </summary>
        /// <param name="data1"></param>
        /// <returns></returns>
        public double[,] Cal_Inverse(double[,]data1)
        {
            int rows1 = data1.GetLength(0);
            int cols1 = data1.GetLength(1);
            int n = rows1;
            var sum=new double[n,2*n];
            for(int i = 0;i<n;i++)
            {
                for(int j = 0;j<n;j++)
                {
                    sum[i,j]= data1[i,j];
                }
                sum[i, n + i] = 1;
            }
            for(int i = 0;i<n;i++)
            {
                double diag = sum[i, i];
                // 归一化当前行
                for (int j = 0; j < 2 * n; j++)
                {
                    sum[i, j] /= diag;
                }
                // 消去其他行
                for (int k = 0; k < n; k++)
                {
                    if (k != i)
                    {
                        double factor = sum[k, i];
                        for (int j = 0; j < 2 * n; j++)
                        {
                            sum[k, j] -= factor * sum[i, j];
                        }
                    }
                }
            }
            // 提取逆矩阵
            double[,] inverse = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    inverse[i, j] = sum[i, j + n];
                }
            }
            return inverse;
        }
    }
    }
