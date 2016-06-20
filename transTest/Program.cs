using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YTsUtility;
using System.IO;

namespace transTest
{
    class Param
    {
        public double X0;
        public double Y0;
        public double Z0;
        public double lambda = 1;
        public double q0 = 1;
        public double q1;
        public double q2;
        public double q3;
    }
    
    class Program
    {
        static Param CalParam(List<_3D_Point> oldPoints,List<_3D_Point> newPoints)
        {
            Param para = new Param();
            Matrix B = new Matrix(oldPoints.Count * 3, 8);
            Matrix l = new Matrix(B.Row, 1);
            Matrix Bx=new Matrix(1,8);
            Matrix w=new Matrix(1,1);
            Matrix x;
            double X1=100;
            while(Math.Abs(X1-para.X0)>0.00001)
            {
                X1 = para.X0;
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
                for (int i = 0; i < oldPoints.Count; i++)
                {
                    int row0 = 3 * i;
                    double X = oldPoints[i].X,
                           Y = oldPoints[i].Y,
                           Z = oldPoints[i].Z;
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

                    l[row0, 0] = newPoints[i].X - (X0 + lambda * (a1 * X + a2 * Y + a3 * Z));
                    l[row0 + 1, 0] = newPoints[i].Y - (Y0 + lambda * (b1 * X + b2 * Y + b3 * Z));
                    l[row0 + 2, 0] = newPoints[i].Z - (Z0 + lambda * (c1 * X + c2 * Y + c3 * Z));
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
            }
            
            return para;
        }
        static void Main(string[] args)
        {
            List<_3D_Point> oldp = new List<_3D_Point>();
            List<_3D_Point> newp = new List<_3D_Point>();
            string[] sl = File.ReadAllLines("data.txt");
            for (int i = 0; i < sl.Length;i++ )
            {
                var ss = sl[i].Split(' ');
                double x = double.Parse(ss[1]);
                double y = double.Parse(ss[2]);
                double z = double.Parse(ss[3]);
                oldp.Add(new _3D_Point(x, y, z));
                x = double.Parse(ss[4]);
                y = double.Parse(ss[5]);
                z = double.Parse(ss[6]);
                newp.Add(new _3D_Point(x, y, z));
            }
            //0CoordinateT3 ct = new CoordinateT3(oldp, newp);
            //var newpp = ct.Change(oldp);
            //CoordinateTQ cq = new CoordinateTQ(oldp, newp);
            //var nnp = cq.Change(oldp);
           // var pa = CalParam(oldp, newp);
            List<string> slist = new List<string> { "1", "2", "3" };
            slist.ForEach(s => s = "12");
            slist.ForEach(s => Console.WriteLine(s));
        }
    }
}
