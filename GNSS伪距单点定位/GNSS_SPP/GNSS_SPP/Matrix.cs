using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GNSS_SPP
{
    class Matrix
    {
        public double[,] Array;
        public Matrix(double[,] array)
        {
            int rows = array.GetLength(0);
            int columns = array.GetLength(1);
            Array = new double[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Array[i, j] = array[i, j];
                }
            }
        }
        public Matrix(List<List<double>> array)
        {
            try
            {
                int rows = array.Count();
                int columns = array[0].Count();
                Array = new double[rows, columns];
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        Array[i, j] = array[i][j];
                    }
                }
            }
            catch
            {
                MessageBox.Show("生成二维矩阵失败");
            }
        }
        public int Rows
        {
            get
            {
                return Array.GetLength(0);
            }
        }
        public int Columns
        {
            get
            {
                return Array.GetLength(1);
            }
        }
        public double this[int m, int n]
        {
            get
            {
                if (m < Rows && n < Columns)
                {
                    return Array[m, n];
                }
                else
                {
                    throw new Exception("索引出界");
                }
            }
            set
            {
                Array[m, n] = value;
            }
        }
        #region 矩阵运算
        #region 矩阵加减法运算
        /// <summary>
        /// 二维矩阵的加法运算
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static Matrix operator +(Matrix matrix1, Matrix matrix2)
        {
            if (matrix1.Rows != matrix2.Rows || matrix1.Columns != matrix2.Columns)
            {
                MessageBox.Show("加法行列数不匹配");
                throw new Exception("无效加法计算");
            }
            double[,] array = new double[matrix1.Rows, matrix1.Columns];
            Matrix result = new Matrix(array);
            for (int i = 0; i < result.Rows; i++)
            {
                for (int j = 0; j < result.Columns; j++)
                {
                    result[i, j] = matrix1[i, j] + matrix2[i, j];
                }
            }
            return result;
        }
        /// <summary>
        /// 二维矩阵的减法运算
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static Matrix operator -(Matrix matrix1, Matrix matrix2)
        {
            if (matrix1.Rows != matrix2.Rows || matrix1.Columns != matrix2.Columns)
            {
                MessageBox.Show("矩阵减法行列数不相等");
                throw new Exception("无效减法运算");
            }
            double[,] array = new double[matrix1.Rows, matrix1.Columns];
            Matrix result = new Matrix(array);
            for (int i = 0; i < result.Rows; i++)
            {
                for (int j = 0; j < result.Columns; j++)
                {
                    result[i, j] = matrix1[i, j] - matrix2[i, j];
                }
            }
            return result;
        }
        #endregion

        #region 二维矩阵乘法运算
        /// <summary>
        /// 二维矩阵相乘
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static Matrix operator *(Matrix matrix1, Matrix matrix2)
        {
            double[,] array = new double[matrix1.Rows, matrix2.Columns];
            Matrix result = new Matrix(array);
            double sum;
            try
            {
                if (matrix1.Columns == matrix2.Rows)
                {
                    for (int i = 0; i < result.Rows; i++)
                    {
                        for (int j = 0; j < result.Columns; j++)
                        {
                            sum = 0;
                            for (int z = 0; z < matrix1.Columns; z++)
                            {
                                sum += matrix1[i, z] * matrix2[z, j];
                            }
                            result[i, j] = sum;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("矩阵乘法的两个矩阵行列不匹配!");
                }
            }
            catch
            {
                MessageBox.Show("矩阵乘法运算错误！");
            }
            return result;
        }
        /// <summary>
        /// 二维矩阵右乘一个数
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static Matrix operator *(Matrix matrix1, double num)
        {
            double[,] array = new double[matrix1.Rows, matrix1.Columns];
            Matrix result = new Matrix(array);
            try
            {
                for (int i = 0; i < result.Rows; i++)
                {
                    for (int j = 0; j < result.Columns; j++)
                    {
                        result[i, j] = matrix1[i, j] * num;
                    }
                }
            }
            catch
            {
                MessageBox.Show("矩阵乘法时遇到错误");
            }
            return result;
        }
        /// <summary>
        /// 二维矩阵左乘一个数
        /// </summary>
        /// <param name="num"></param>
        /// <param name="matrix1"></param>
        /// <returns></returns>
        public static Matrix operator *(double num, Matrix matrix1)
        {
            double[,] array = new double[matrix1.Rows, matrix1.Columns];
            Matrix result = new Matrix(array);
            try
            {
                for (int i = 0; i < result.Rows; i++)
                {
                    for (int j = 0; j < result.Columns; j++)
                    {
                        result[i, j] = matrix1[i, j] * num;
                    }
                }
            }
            catch
            {
                MessageBox.Show("矩阵乘法时遇到错误");
            }
            return result;
        }
        #endregion

        /// <summary>
        /// 二维矩阵转置运算
        /// </summary>
        /// <returns></returns>
        public Matrix Transpose()
        {
            double[,] array = new double[Columns, Rows];
            Matrix result = new Matrix(array);
            try
            {
                for (int i = 0; i < result.Rows; i++)
                {
                    for (int j = 0; j < result.Columns; j++)
                    {
                        result[i, j] = Array[j, i];
                    }
                }
            }
            catch
            {
                MessageBox.Show("矩阵转置运算失败");
            }
            return result;
        }

        /// <summary>
        /// 二维矩阵求逆运算
        /// </summary>
        /// <returns></returns>
        public Matrix Inverse()
        {
            double[,] array = new double[Rows, Columns];
            Matrix result = new Matrix(array);
            try
            {
                if (Rows == Columns)
                {
                    if (Determinant(this) != 0)
                    {
                        if (Rows != 1)
                        {
                            result = complement(this).Transpose() * (1 / Determinant(this));
                        }
                        else
                        {
                            result[0, 0] = 1 / Array[0, 0];
                        }
                    }
                    else
                    {
                        MessageBox.Show("矩阵行列式为0");
                    }
                }
                else
                {
                    MessageBox.Show("输入矩阵不是方阵！");
                }
            }
            catch
            {
                MessageBox.Show("矩阵求逆错误！");
            }
            return result;
        }

        /// <summary>
        /// 递归计算二维矩阵的行列式，使用代数余子式方法
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public double Determinant(Matrix matrix)
        {
            double sum = 0;
            try
            {
                if (matrix.Rows == 1)
                {
                    return matrix[0, 0];
                }
                int flag = 1;
                for (int i = 0; i < matrix.Rows; i++)
                {
                    double[,] tmparray = new double[matrix.Rows - 1, matrix.Columns - 1];
                    Matrix tmpMatrix = new Matrix(tmparray);
                    for (int j = 0; j < tmpMatrix.Rows; j++)
                    {
                        for (int k = 0; k < tmpMatrix.Columns; k++)
                        {
                            tmpMatrix[j, k] = matrix[j + 1, k >= i ? k + 1 : k];
                        }
                    }
                    sum += flag * matrix[0, i] * Determinant(tmpMatrix);
                    flag = -1 * flag;
                }
            }
            catch
            {
                MessageBox.Show("求矩阵行列式时错误！");
            }
            return sum;
        }

        /// <summary>
        /// 计算方阵伴随矩阵
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public Matrix complement(Matrix matrix)
        {
            double[,] array = new double[matrix.Rows, matrix.Columns];
            Matrix result = new Matrix(array);
            try
            {
                for (int i = 0; i < result.Rows; i++)
                {
                    for (int j = 0; j < result.Columns; j++)
                    {
                        //获取余子式
                        double[,] _determinant = new double[result.Rows - 1, result.Columns - 1];
                        Matrix determinant = new Matrix(_determinant);
                        for (int k = 0; k < determinant.Rows; k++)
                        {
                            for (int l = 0; l < determinant.Columns; l++)
                            {
                                determinant[k, l] = matrix[k >= i ? k + 1 : k, l >= j ? l + 1 : l];
                            }
                        }
                        result[i, j] = Math.Pow(-1, i + j) * Determinant(determinant);
                    }
                }
            }
            catch
            {
                MessageBox.Show("计算伴随矩阵时错误");
            }
            return result;
        }

        /// <summary>
        /// 获取矩阵字符串
        /// </summary>
        /// <returns></returns>
        public string print()
        {
            string str = null;
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    str += Array[i, j] + " ";
                }
                str += "\r\n";
            }
            return str;
        }

        /// <summary>
        /// 打印矩阵
        /// </summary>
        public void show()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Console.Write("{0} ", Array[i, j]);
                }
                Console.WriteLine();
            }
        }
        #endregion
    }
}
