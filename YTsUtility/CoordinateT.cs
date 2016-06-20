using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YTsUtility
{
    
    /// <summary>
    /// 二维坐标变换类
    /// </summary>
    [Serializable]
    public class CoordinateT
    {
        DMath sin = Math.Sin,
              cos = Math.Cos,
              tan = Math.Tan;
        /// <summary>
        /// 比例系数，使用时为(1+m)
        /// </summary>
        public double m {private set;get;}
        /// <summary>
        /// 旋转角
        /// </summary>
        public double xita {private set;get;}
        /// <summary>
        /// Δx
        /// </summary>
        public double dx {private set;get;}
        /// <summary>
        /// Δy
        /// </summary>
        public double dy {private set;get;}

        /// <summary>
        /// 单位权中误差
        /// </summary>
        public double xigema { private set; get; }
        /// <summary>
        /// 直接通过变换4参数构造
        /// </summary>
        /// <param name="m">比例因子</param>
        /// <param name="xita">旋转角</param>
        /// <param name="dx">x0</param>
        /// <param name="dy">y0</param>
        public CoordinateT(double m,double xita,double dx,double dy)
        {
            this.m = m;
            this.xita = xita;
            this.dx = dx;
            this.dy = dy;
        }
        
        /// <summary>
        /// 坐标变换
        /// </summary>
        /// <param name="p">需要变换的点</param>
        /// <returns>变换后的点</returns>
        public _2D_Point Change(_2D_Point p)
        {
            double x=dx+(1+m)*(p.X*cos(xita)+p.Y*sin(xita)),
                   y=dy+(1+m)*(-p.X*sin(xita)+p.Y*cos(xita));
            return new _2D_Point(x, y);
        }

        /// <summary>
        /// 坐标变换
        /// </summary>
        /// <param name="ps">需要变换的点表</param>
        /// <returns>变换后的点表</returns>
        public List<_2D_Point> Change(List<_2D_Point> ps)
        {
            List<_2D_Point> pl = new List<_2D_Point>();
            ps.ForEach(p => pl.Add(Change(p)));
            return pl;
        }

        List<_2D_Point> pold, pnew;
        
        /// <summary>
        /// 通过新旧坐标表构造，至少两个点
        /// </summary>
        /// <param name="pold">旧坐标表</param>
        /// <param name="pnew">新坐标表</param>
        /// <param name="mode">默认为0(迭代求解)，其他时候(直接求解)</param>
        public CoordinateT(List<_2D_Point> pold,List<_2D_Point> pnew,int mode=0)
        {
            this.pold = pold;
            this.pnew = pnew;
            Matrix x;
            if(mode==0)
            {
                Matrix v=new Matrix(pnew.Count*2,1);
                do
                {
                    Matrix B = GetB(),
                           l = Getl();
                    x = (B.T() * B).Inverse() * B.T() * l;
                    Update(x);
                    v = v + B * x - l;
                } while (!Terminate(x));
                xigema = Math.Sqrt((v.T() * v / (pnew.Count * 2 - 4))[0, 0]);
            }
            else
            {
                Matrix B = new Matrix(pnew.Count * 2, 4);
                Matrix l = new Matrix(pnew.Count * 2, 1);
                for (int i = 0; i < pnew.Count; i++)
                {
                    B[2 * i, 0] = 1;
                    B[2 * i, 1] = 0;
                    B[2 * i, 2] = pold[i].X;
                    B[2 * i, 3] = pold[i].Y;
                    B[2 * i + 1, 0] = 0;
                    B[2 * i + 1, 1] = 1;
                    B[2 * i + 1, 2] = pold[i].Y;
                    B[2 * i + 1, 3] = -pold[i].X;
                    l[2*i, 0] = pnew[i].X;
                    l[2 * i + 1,0] = pnew[i].Y;
                }
                x = (B.T() * B).Inverse() * B.T() * l;
                dx = x[0, 0];
                dy = x[1, 0];
                xita = Math.Atan(x[3, 0] / x[2, 0]);
                m = x[2, 0] / cos(xita) - 1;
                Matrix X=new Matrix(4,1);
                Matrix v = B * x - l;
                xigema = Math.Sqrt((v.T() * v / (pnew.Count * 2 - 4))[0, 0]);
            }
        }

        #region 反求坐标变换参数需调用的方法
        private Matrix GetB()
        {
            Matrix B = new Matrix(pold.Count * 2, 4);
            for(int i=0;i<pold.Count;i++)
            {
                B[2 * i, 0] = 1;
                B[2 * i, 2] = pold[i].X * cos(xita) + pold[i].Y * sin(xita);
                B[2 * i, 3] = (1 + m) * (-pold[i].X * sin(xita) + pold[i].Y * cos(xita));
                B[2 * i + 1, 1] = 1;
                B[2 * i + 1, 2] = -pold[i].X * sin(xita) + pold[i].Y * cos(xita);
                B[2 * i + 1, 3] = (1 + m) * (-pold[i].X * cos(xita) - pold[i].Y * sin(xita));
            }
            return B;
        }

        private Matrix Getl()
        {
            Matrix l = new Matrix(pold.Count * 2, 1);
            for (int i = 0; i < pold.Count; i++)
            {
                l[2 * i, 0] = pnew[i].X - (dx + (1 + m) * (pold[i].X * cos(xita) + pold[i].Y * sin(xita)));
                l[2 * i + 1, 0] = pnew[i].Y - (dy + (1 + m) * (-pold[i].X * sin(xita) + pold[i].Y * cos(xita)));
            }
            return l;
        }

        private void Update(Matrix x)
        {
            dx += x[0, 0];
            dy += x[1, 0];
            m += x[2, 0];
            xita += x[3, 0];
        }

        private bool Terminate(Matrix x)
        {
            for (int i = 0; i < x.Row; i++)
            {
                if (Math.Abs( x[i, 0]) > 0.0001)
                    return false;
            }
            return true;
        }

        #endregion
    }

    /// <summary>
    /// 三维坐标变换类（R=Rx*Ry*Rz）
    /// </summary>
    public class CoordinateT3
    {
        DMath sin = Math.Sin,
              cos = Math.Cos,
              tan = Math.Tan;
        /// <summary>
        /// 平移参数X0
        /// </summary>
        public double X0 { private set;get;}
        /// <summary>
        /// 平移参数Y0
        /// </summary>
        public double Y0 { private set;get;}
        /// <summary>
        /// 平移参数Z0
        /// </summary>
        public double Z0 { private set;get;}
        /// <summary>
        /// 比例系数M,使用(1+M)
        /// </summary>
        public double M { private set;get;}
        /// <summary>
        /// 绕X轴转角
        /// </summary>
        public double Ax { private set;get;}
        /// <summary>
        /// 绕Y轴转角
        /// </summary>
        public double Ay { private set; get; }
        /// <summary>
        /// 绕Z轴转角
        /// </summary>
        public double Az { private set; get; }

        /// <summary>
        /// 单位权中误差
        /// </summary>
        public double xigema { private set; get; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dx">dx</param>
        /// <param name="dy">dy</param>
        /// <param name="dz">dz</param>
        /// <param name="m">比例系数,(1+m)</param>
        /// <param name="ex">x转角</param>
        /// <param name="ey">y转角</param>
        /// <param name="ez">z转角</param>
        public CoordinateT3(double dx,double dy,double dz,double m,double ex,double ey,double ez)
        {
            this.X0 = dx;
            this.Y0 = dy;
            this.Z0 = dz;
            this.Ax = ex;
            this.Ay = ey;
            this.Az = ez;
            this.M = m;
        }

        private Matrix GetR()
        {
            Matrix R = new Matrix(3, 3);
            R[0, 0] = cos(Ay)*cos(Az);
            R[0, 1] = cos(Ay)*sin(Az);
            R[0, 2] = sin(Ay);
            R[1, 0] = -cos(Ax)*sin(Az)-cos(Az)*sin(Ax)*sin(Ay);
            R[1, 1] = cos(Ax)*cos(Az)-sin(Ax)*sin(Ay)*sin(Az);
            R[1, 2] = cos(Ay)*sin(Ax);
            R[2, 0] = sin(Ax)*sin(Az)-cos(Ax)*cos(Az)*sin(Ay);
            R[2, 1] = -cos(Az)*sin(Ax)-cos(Ax)*sin(Ay)*sin(Az);
            R[2, 2] = cos(Ax)*cos(Ay);
            return R;
        }

        /// <summary>
        /// 坐标变换
        /// </summary>
        /// <param name="p">原坐标</param>
        /// <returns>新坐标</returns>
        public _3D_Point Change(_3D_Point p)
        {
            _3D_Point dxyz=new _3D_Point(X0,Y0,Z0);
            Matrix newp = dxyz.ToColumnMatrix() + (1 + M) * GetR() * p.ToColumnMatrix();
            return new _3D_Point(newp[0,0],newp[1,0],newp[2,0]);
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

        List<_3D_Point> pold, pnew;
        
        /// <summary>
        /// 通过新旧坐标表构造，三个点以上
        /// </summary>
        /// <param name="pold">旧坐标表</param>
        /// <param name="pnew">新坐标表</param>
        public CoordinateT3(List<_3D_Point> pold,List<_3D_Point> pnew)
        {
            this.pold = pold;
            this.pnew = pnew;
            Matrix x;
            Matrix v=new Matrix(pnew.Count*3,1);
            Matrix B = GetB(),
                   l = Getl();
            x = (B.T() * B).Inverse() * B.T() * l;
            Update(x);
            v = B * x - l;
            xigema = Math.Sqrt((v.T() * v / (pnew.Count * 3 - 4))[0, 0]);
        }

        #region 反求坐标变换参数需调用的方法
        private Matrix GetB()
        {
            Matrix B = new Matrix(pold.Count * 3, 7);
            for(int i=0;i<pold.Count;i++)
            {
                B[3 * i, 0] = 1;
                B[3 * i, 4] = pold[i].Z;
                B[3 * i, 5] = pold[i].Y;
                B[3 * i, 6] = pold[i].X;
                B[3 * i + 1, 1] = 1;
                B[3 * i + 1, 3] = pold[i].Z;
                B[3 * i + 1, 5] = -pold[i].X;
                B[3 * i + 1, 6] = pold[i].Y;
                B[3 * i + 2, 2] = 1;
                B[3 * i + 2, 3] = -pold[i].Y;
                B[3 * i + 2, 4] = -pold[i].X;
                B[3 * i + 2, 6] = pold[i].Z;
            }
            return B;
        }

        private Matrix Getl()
        {
            Matrix l = new Matrix(pold.Count * 3, 1);
            for (int i = 0; i < pold.Count; i++)
            {
                l[3 * i, 0] = pnew[i].X - pold[i].X;
                l[3 * i + 1, 0] = pnew[i].Y - pold[i].Y;
                l[3 * i + 2, 0] = pnew[i].Z - pold[i].Z;
            }
            return l;
        }

        private void Update(Matrix x)
        {
            X0 += x[0, 0];
            Y0 += x[1, 0];
            Z0 += x[2, 0];
            Ax += x[3, 0];
            Ay += x[4, 0];
            Az += x[5, 0];
            M += x[6, 0];
            
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

        #endregion
    }
}
