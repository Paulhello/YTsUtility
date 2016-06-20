using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YTsUtility
{
    /// <summary>
    /// 三维坐标变换参数
    /// </summary>
    public class TransParam
    {
        /// <summary>
        /// 平移参数X0
        /// </summary>
        public double X0;
        /// <summary>
        /// 平移参数Y0
        /// </summary>
        public double Y0;
        /// <summary>
        /// 平移参数Z0
        /// </summary>
        public double Z0;
        /// <summary>
        /// 比例系数λ
        /// </summary>
        public double lambda = 1;
        /// <summary>
        /// q0
        /// </summary>
        public double q0 = 1;
        /// <summary>
        /// q1
        /// </summary>
        public double q1;
        /// <summary>
        /// q2
        /// </summary>
        public double q2;
        /// <summary>
        /// q3
        /// </summary>
        public double q3;
    }
    /// <summary>
    /// 三维坐标变换(四元数)
    /// </summary>
    public class CoordinateTQ
    {
        /// <summary>
        /// 变换参数
        /// </summary>
        public TransParam para { private set; get; }

        /// <summary>
        /// 通过变换参数构造
        /// </summary>
        /// <param name="p">变换参数</param>
        public CoordinateTQ(TransParam p)
        {
            para = p;
        }
        /// <summary>
        /// 通过新旧坐标表构造，三个点以上
        /// </summary>
        /// <param name="pold">旧坐标表</param>
        /// <param name="pnew">新坐标表</param>
        public CoordinateTQ(List<_3D_Point> pold, List<_3D_Point> pnew)
        {
            para = new TransParam();
            Matrix B = new Matrix(pold.Count * 3, 8);
            Matrix l = new Matrix(B.Row, 1);
            Matrix Bx = new Matrix(1, 8);
            Matrix w = new Matrix(1, 1);
            Matrix x;
            do
            {
                double q0 = para.q0,
                       q1 = para.q1,
                       q2 = para.q2,
                       q3 = para.q3,
                       lambda = para.lambda,
                       X0 = para.X0,
                       Y0 = para.Y0,
                       Z0 = para.Z0,
                       a1 = q0 * q0 + q1 * q1 - q2 * q2 - q3 * q3,
                       a2 = 2 * (q1 * q2 - q0 * q3),
                       a3 = 2 * (q1 * q3 + q0 * q2),
                       b1 = 2 * (q1 * q2 + q0 * q3),
                       b2 = q0 * q0 - q1 * q1 + q2 * q2 - q3 * q3,
                       b3 = 2 * (q2 * q3 - q0 * q1),
                       c1 = 2 * (q1 * q3 - q0 * q2),
                       c2 = 2 * (q2 * q3 + q0 * q1),
                       c3 = q0 * q0 - q1 * q1 - q2 * q2 + q3 * q3;
                Bx[0, 0] = q0;
                Bx[0, 1] = q1;
                Bx[0, 2] = q2;
                Bx[0, 3] = q3;
                w[0, 0] = (1 - (q0 * q0 + q1 * q1 + q2 * q2 + q3 * q3)) / 2;
                for (int i = 0; i < pold.Count; i++)
                {
                    int row0 = 3 * i;
                    double X = pold[i].X,
                           Y = pold[i].Y,
                           Z = pold[i].Z;
                    B[row0, 0] = 2 * lambda * (q0 * X - q3 * Y + q2 * Z);
                    B[row0, 1] = 2 * lambda * (q1 * X + q2 * Y + q3 * Z);
                    B[row0, 2] = 2 * lambda * (-q2 * X + q1 * Y + q0 * Z);
                    B[row0, 3] = 2 * lambda * (-q3 * X - q0 * Y + q1 * Z);
                    B[row0, 4] = a1 * X + a2 * Y + a3 * Z;
                    B[row0, 5] = 1;

                    B[row0 + 1, 0] = 2 * lambda * (q3 * X + q0 * Y - q1 * Z);
                    B[row0 + 1, 1] = 2 * lambda * (q2 * X - q1 * Y - q0 * Z);
                    B[row0 + 1, 2] = 2 * lambda * (q1 * X + q2 * Y + q3 * Z);
                    B[row0 + 1, 3] = 2 * lambda * (q0 * X - q3 * Y + q2 * Z);
                    B[row0 + 1, 4] = b1 * X + b2 * Y + b3 * Z;
                    B[row0 + 1, 6] = 1;

                    B[row0 + 2, 0] = 2 * lambda * (-q2 * X + q1 * Y + q0 * Z);
                    B[row0 + 2, 1] = 2 * lambda * (q3 * X + q0 * Y - q1 * Z);
                    B[row0 + 2, 2] = 2 * lambda * (-q0 * X + q3 * Y - q2 * Z);
                    B[row0 + 2, 3] = 2 * lambda * (q1 * X + q2 * Y + q3 * Z);
                    B[row0 + 2, 4] = c1 * X + c2 * Y + c3 * Z;
                    B[row0 + 2, 7] = 1;

                    l[row0, 0] = pnew[i].X - (X0 + lambda * (a1 * X + a2 * Y + a3 * Z));
                    l[row0 + 1, 0] = pnew[i].Y - (Y0 + lambda * (b1 * X + b2 * Y + b3 * Z));
                    l[row0 + 2, 0] = pnew[i].Z - (Z0 + lambda * (c1 * X + c2 * Y + c3 * Z));
                }
                var N1 = Matrix.ColumnCombine(B.T() * B, Bx.T());
                var N2 = Matrix.ColumnCombine(Bx, Matrix.Zeros(Bx.Row, Bx.Row));
                var N = Matrix.RowCombine(N1, N2);
                var W = Matrix.RowCombine(B.T() * l, w);
                var xx = N.Inverse() * W;
                x = xx.SubRMatrix(0, B.Column - 1);
                para.q0 += x[0, 0];
                para.q1 += x[1, 0];
                para.q2 += x[2, 0];
                para.q3 += x[3, 0];
                para.lambda += x[4, 0];
                para.X0 += x[5, 0];
                para.Y0 += x[6, 0];
                para.Z0 += x[7, 0];
            } while (!Terminate(x));
        }
        /// <summary>
        /// 坐标变换
        /// </summary>
        /// <param name="p">原坐标</param>
        /// <returns>新坐标</returns>
        public _3D_Point Change(_3D_Point p)
        {
            _3D_Point dxyz=new _3D_Point(para.X0,para.Y0,para.Z0);
            Matrix newp = dxyz.ToColumnMatrix() + para.lambda * GetR() * p.ToColumnMatrix();
            return new _3D_Point(newp[0,0],newp[1,0],newp[2,0]);
        }

        private Matrix GetR()
        {
            Matrix m=new Matrix(3,3);
            double q0 = para.q0,
                   q1 = para.q1,
                   q2 = para.q2,
                   q3 = para.q3;
            m[0,0] = q0 * q0 + q1 * q1 - q2 * q2 - q3 * q3;
            m[0,1] = 2 * (q1 * q2 - q0 * q3);
            m[0,2] = 2 * (q1 * q3 + q0 * q2);
            m[1,0] = 2 * (q1 * q2 + q0 * q3);
            m[1,1] = q0 * q0 - q1 * q1 + q2 * q2 - q3 * q3;
            m[1,2] = 2 * (q2 * q3 - q0 * q1);
            m[2,0] = 2 * (q1 * q3 - q0 * q2);
            m[2, 1] = 2 * (q2 * q3 + q0 * q1);
            m[2,2] = q0 * q0 - q1 * q1 - q2 * q2 + q3 * q3;
            return m;
        }
        /// <summary>
        /// 批量坐标变换
        /// </summary>
        /// <param name="ps">旧坐标表</param>
        /// <returns>新坐标表</returns>
        public List<_3D_Point> Change(List<_3D_Point> ps)
        {
            List<_3D_Point> pl = new List<_3D_Point>();
            ps.ForEach(p => pl.Add(Change(p)));
            return pl;
        }

        


        private bool Terminate(Matrix x)
        {
            for (int i = 0; i < x.Row; i++)
            {
                if (Math.Abs( x[i, 0]) > 0.00001)
                    return false;
            }
            return true;
        }

    }
}
