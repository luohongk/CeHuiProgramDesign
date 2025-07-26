using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 空间后方交汇模拟7._22
{
    internal class MyAlog
    {
        public List<MyData> Points = new List<MyData>();
        public double fk; // 焦距(mm)
        public double x0; // 像主点x坐标(mm)
        public double y0; // 像主点y坐标(mm)
        public double m0; // 摄影比例尺分母

        // 外方位元素
        public double Xs; // 摄站X坐标(m)
        public double Ys; // 摄站Y坐标(m)
        public double Zs; // 摄站Z坐标(m)
        public double phi; // 俯仰角(弧度)
        public double omega; // 横滚角(弧度)
        public double kappa; // 偏航角(弧度)

        // 旋转矩阵元素
        public double a1, a2, a3;
        public double b1, b2, b3;
        public double c1, c2, c3;

        /// <summary>
        /// 执行空间后方交会计算并返回结果
        /// </summary>
        public string PrintBG()
        {
            // 初始化参数
            InitParameters();
            // 执行迭代计算
            IterateCalculation();

            // 构建结果字符串
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("计算结果:");
            sb.AppendLine($"Xs(m) = {Xs:F2}");
            sb.AppendLine($"Ys(m) = {Ys:F2}");
            sb.AppendLine($"Zs(m) = {Zs:F2}");
            sb.AppendLine("--------R矩阵信息---");
            sb.AppendLine($"{a1:F5} {a2:F5} {a3:F5}");
            sb.AppendLine($"{b1:F5} {b2:F5} {b3:F5}");
            sb.AppendLine($"{c1:F5} {c2:F5} {c3:F5}");
            return sb.ToString();
        }

        /// <summary>
        /// 初始化外方位元素初始值及旋转矩阵
        /// </summary>
        private void InitParameters()
        {
            // 线元素初始值：Xs、Ys为控制点坐标平均值，Zs为摄影高度(m0*fk转换为米)
            Xs = Points.Average(p => p.X);
            Ys = Points.Average(p => p.Y);
            Zs = (m0 * fk) / 1000.0; // 转换mm到m

            // 角元素初始值(近似垂直摄影)
            phi = 0;
            omega = 0;
            kappa = 0;

            // 更新旋转矩阵
            UpdateRotationMatrix();
        }

        /// <summary>
        /// 根据当前角元素更新旋转矩阵
        /// </summary>
        private void UpdateRotationMatrix()
        {
            double cphi = Math.Cos(phi);
            double sphi = Math.Sin(phi);
            double comega = Math.Cos(omega);
            double somega = Math.Sin(omega);
            double ckappa = Math.Cos(kappa);
            double skappa = Math.Sin(kappa);

            // 计算旋转矩阵元素
            a1 = cphi * ckappa - sphi * somega * skappa;
            a2 = -cphi * skappa - sphi * somega * ckappa;
            a3 = -sphi * comega;

            b1 = comega * skappa;
            b2 = comega * ckappa;
            b3 = -somega;

            c1 = sphi * ckappa + cphi * somega * skappa;
            c2 = -sphi * skappa + cphi * somega * ckappa;
            c3 = cphi * comega;
        }

        /// <summary>
        /// 迭代计算直到收敛
        /// </summary>
        private void IterateCalculation()
        {
            Maritx matrixTool = new Maritx();
            double threshold = 1e-5; // 收敛阈值
            int maxIter = 20; // 最大迭代次数
            int iterCount = 0;

            while (iterCount < maxIter)
            {
                // 1. 计算近似影像坐标及残差
                CalculateApproxImageCoords();

                // 2. 构建系数矩阵A和常数项矩阵L
                double[,] A;
                double[,] L;
                BuildALMatrices(out A, out L);

                // 3. 计算法方程系数(ATA)和常数项(ATL)
                double[,] AT = matrixTool.Cal_T(A);
                double[,] ATA = matrixTool.Cal_Mulitp(AT, A);
                double[,] ATL = matrixTool.Cal_Mulitp(AT, L);

                // 4. 求解改正数
                double[,] ATAInv = matrixTool.Cal_Inverse(ATA);
                double[,] corrections = matrixTool.Cal_Mulitp(ATAInv, ATL);

                // 5. 提取改正数
                double dXs = corrections[0, 0];
                double dYs = corrections[1, 0];
                double dZs = corrections[2, 0];
                double dPhi = corrections[3, 0];
                double dOmega = corrections[4, 0];
                double dKappa = corrections[5, 0];

                // 6. 检查收敛性
                double maxCorrection = Math.Max(
                    Math.Max(Math.Abs(dXs), Math.Abs(dYs)),
                    Math.Max(Math.Abs(dZs),
                    Math.Max(Math.Abs(dPhi),
                    Math.Max(Math.Abs(dOmega), Math.Abs(dKappa)))));

                if (maxCorrection < threshold)
                    break;

                // 7. 更新外方位元素
                Xs += dXs;
                Ys += dYs;
                Zs += dZs;
                phi += dPhi;
                omega += dOmega;
                kappa += dKappa;

                // 更新旋转矩阵
                UpdateRotationMatrix();
                iterCount++;
            }
        }

        /// <summary>
        /// 计算各点的近似影像坐标(xap, yap)
        /// </summary>
        private void CalculateApproxImageCoords()
        {
            foreach (var p in Points)
            {
                // 地面点与摄站的坐标差
                double dX = p.X - Xs;
                double dY = p.Y - Ys;
                double dZ = p.Z - Zs;

                // 计算投影坐标
                double X_ = a1 * dX + b1 * dY + c1 * dZ;
                double Y_ = a2 * dX + b2 * dY + c2 * dZ;
                double Z_ = a3 * dX + b3 * dY + c3 * dZ;

                // 近似影像坐标(考虑内方位元素x0=y0=0)
                p.xap = -fk * (X_ / Z_);
                p.yap = -fk * (Y_ / Z_);
            }
        }

        /// <summary>
        /// 构建系数矩阵A和常数项矩阵L
        /// </summary>
        private void BuildALMatrices(out double[,] A, out double[,] L)
        {
            int pointCount = Points.Count;
            A = new double[2 * pointCount, 6]; // 每个点2行，6个未知数
            L = new double[2 * pointCount, 1]; // 残差矩阵

            for (int i = 0; i < pointCount; i++)
            {
                var p = Points[i];
                double X = p.X - Xs; // 地面点与摄站X差
                double Y = p.Y - Ys; // 地面点与摄站Y差
                double Z = p.Z - Zs; // 地面点与摄站Z差

                // 投影坐标
                double X_ = a1 * X + b1 * Y + c1 * Z;
                double Y_ = a2 * X + b2 * Y + c2 * Z;
                double Z_ = a3 * X + b3 * Y + c3 * Z;

                // 影像坐标观测值(mm)
                double x = p.x;
                double y = p.y;

                // 残差L(观测值-近似值)
                L[2 * i, 0] = x - p.xap;
                L[2 * i + 1, 0] = y - p.yap;

                // 计算A矩阵元素(偏导数)
                double cphi = Math.Cos(phi);
                double sphi = Math.Sin(phi);
                double comega = Math.Cos(omega);
                double somega = Math.Sin(omega);
                double ckappa = Math.Cos(kappa);
                double skappa = Math.Sin(kappa);

                // 1. 对Xs、Ys、Zs的偏导数(a11-a13, a21-a23)
                double a11 = (a1 * fk + a3 * x) / Z_;
                double a12 = (b1 * fk + b3 * x) / Z_;
                double a13 = (c1 * fk + c3 * x) / Z_;

                double a21 = (a2 * fk + a3 * y) / Z_;
                double a22 = (b2 * fk + b3 * y) / Z_;
                double a23 = (c2 * fk + c3 * y) / Z_;

                // 2. 对phi的偏导数(a14, a24)
                double da1_dphi = -sphi * ckappa - cphi * somega * skappa;
                double da2_dphi = sphi * skappa - cphi * somega * ckappa;
                double da3_dphi = -cphi * comega;
                double dc1_dphi = cphi * ckappa - sphi * somega * skappa;
                double dc2_dphi = -cphi * skappa - sphi * somega * ckappa;
                double dc3_dphi = -sphi * comega;

                double dX_dphi = da1_dphi * X + dc1_dphi * Z;
                double dZ_dphi = da3_dphi * X + dc3_dphi * Z;
                double a14 = -fk * (dX_dphi * Z_ - X_ * dZ_dphi) / (Z_ * Z_);

                double dY_dphi = da2_dphi * X + dc2_dphi * Z;
                double a24 = -fk * (dY_dphi * Z_ - Y_ * dZ_dphi) / (Z_ * Z_);

                // 3. 对omega的偏导数(a15, a25)
                double da1_domega = -sphi * comega * skappa;
                double da2_domega = -sphi * comega * ckappa;
                double da3_domega = sphi * somega;
                double db1_domega = -somega * skappa;
                double db2_domega = -somega * ckappa;
                double db3_domega = -comega;
                double dc1_domega = cphi * comega * skappa;
                double dc2_domega = cphi * comega * ckappa;
                double dc3_domega = -cphi * somega;

                double dX_domega = da1_domega * X + db1_domega * Y + dc1_domega * Z;
                double dZ_domega = da3_domega * X + db3_domega * Y + dc3_domega * Z;
                double a15 = -fk * (dX_domega * Z_ - X_ * dZ_domega) / (Z_ * Z_);

                double dY_domega = da2_domega * X + db2_domega * Y + dc2_domega * Z;
                double a25 = -fk * (dY_domega * Z_ - Y_ * dZ_domega) / (Z_ * Z_);

                // 4. 对kappa的偏导数(a16, a26)
                double da1_dkappa = -cphi * skappa - sphi * somega * ckappa;
                double da2_dkappa = -cphi * ckappa + sphi * somega * skappa;
                double db1_dkappa = comega * ckappa;
                double db2_dkappa = -comega * skappa;
                double dc1_dkappa = -sphi * skappa + cphi * somega * ckappa;
                double dc2_dkappa = -sphi * ckappa - cphi * somega * skappa;

                double dX_dkappa = da1_dkappa * X + db1_dkappa * Y + dc1_dkappa * Z;
                double a16 = -fk * dX_dkappa / Z_;

                double dY_dkappa = da2_dkappa * X + db2_dkappa * Y + dc2_dkappa * Z;
                double a26 = -fk * dY_dkappa / Z_;

                // 赋值到A矩阵
                A[2 * i, 0] = a11;
                A[2 * i, 1] = a12;
                A[2 * i, 2] = a13;
                A[2 * i, 3] = a14;
                A[2 * i, 4] = a15;
                A[2 * i, 5] = a16;

                A[2 * i + 1, 0] = a21;
                A[2 * i + 1, 1] = a22;
                A[2 * i + 1, 2] = a23;
                A[2 * i + 1, 3] = a24;
                A[2 * i + 1, 4] = a25;
                A[2 * i + 1, 5] = a26;
            }
        }
    }
}