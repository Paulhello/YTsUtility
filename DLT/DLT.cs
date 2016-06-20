using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using YTsUtility;

namespace DLT
{
    //内方位元素
    class InElement
    {
	    public _2D_Point p0=new _2D_Point();
        public double fx = 0;
        public double fy = 0;
    };

    //外方位元素
    class OutElement
    {
        public _3D_Point ps=new _3D_Point();
        public double phi = 0;
        public double omege = 0;
        public double kappa = 0;
    };

    //系统误差和其他参数
    class Other
    {
        public double dbeta = 0;
        public double ds = 0;
        public double k1 = 0;
    };

    public delegate double MathCal(double a);
    public class DLT
    {
        CData data=new CData();//数据
        List<InElement> inE=new List<InElement>();//内方位元素
        List<OutElement> outE=new List<OutElement>();//外方位元素
        List<Other> other=new List<Other>();//其他参数
        List<Matrix> Ls=new List<Matrix>();//L系数
        MathCal abs = Math.Abs,
                asin = Math.Asin,
                sqrt = Math.Sqrt,
                tan = Math.Tan,
                atan = Math.Atan,
                cos=Math.Cos;

        double pow2(double x)
        {
            return x * x;
        }
        public DLT(CData data)
        {
            this.data = data;
            for (int i = 0; i < data.Count;i++ )
            {
                inE.Add(new InElement());
                outE.Add(new OutElement());
                other.Add(new Other());
            }
        }
        public void Adjustment()//DLT算法
        {
            int count=data.oCount;
	        Matrix B=new Matrix(count * 2, 11);
	        Matrix l=new Matrix(count * 2, 1);
	
	        //求L系数初值
	        for (int j = 0; j < data.Count;j++)
	        {
		        for (int i = 0; i < data.oCount;i++)
		        {
			        double x = data.oIPoints[j][i].X,
				        y = data.oIPoints[j][i].Y,
				        X = data.oPoints[i].X,
				        Y = data.oPoints[i].Y,
				        Z = data.oPoints[i].Z;
			        B[2 * i,0] = X;
			        B[2 * i,1] = Y;
			        B[2 * i,2] = Z;
			        B[2 * i,3] = 1;
			        B[2 * i,8] = -x*X;
			        B[2 * i,9] = -x*Y;
			        B[2 * i,10] = -x*Z;
			        B[2 * i + 1,4] = X;
			        B[2 * i + 1,5] = Y;
			        B[2 * i + 1,6] = Z;
			        B[2 * i + 1,7] = 1;
			        B[2 * i + 1,8] = -y*X;
			        B[2 * i + 1,9] = -y*Y;
			        B[2 * i + 1,10] = -y*Z;
			        l[2 * i,0] = x;
			        l[2 * i + 1,0] =  y;
		        }
		        int xCount = 12;//未知数个数
		        Matrix L0 = ((B.T()*B).Inverse()*(B.T()*l));
		        Matrix L=new Matrix(xCount, 1);
		        for (int i = 0; i < xCount-1; i++)
			        L[i,0] = L0[i,0];
		        Matrix M=new Matrix(count * 2, xCount);
		        Matrix W=new Matrix(count * 2, 1);
		        double f = 9999;

		        //迭代求解L系数
		        while (abs(f - inE[j].fx) >= 0.01)
		        {
			        f = inE[j].fx;
			        double x0 = (L[0,0] * L[8,0] + L[1,0] * L[9,0] + L[2,0] * L[10,0]) /
				        (pow2(L[8,0]) + pow2(L[9,0]) + pow2(L[10,0])),
				        y0 = (L[4,0] * L[8,0] + L[5,0] * L[9,0] + L[6,0] * L[10,0]) /
				        (pow2(L[8,0]) + pow2(L[9,0]) + pow2(L[10,0]));
			        for (int i = 0; i < data.oCount; i++)
			        {
				        double x = data.oIPoints[j][i].X,
					           y = data.oIPoints[j][i].Y,
					           X = data.oPoints[i].X,
				               Y = data.oPoints[i].Y,
					           Z = data.oPoints[i].Z;
				        double A = X*L[8,0] +Y*L[9,0] +Z*L[10,0] + 1;
				        double r_2 = (x - x0)*(x - x0) + (y - y0)*(y - y0);
				        M[2 * i,0] = X / A;
				        M[2 * i,1] = Y / A;
				        M[2 * i,2] = Z / A;
				        M[2 * i,3] = 1 / A;
				        M[2 * i,8] = -X*x / A;
				        M[2 * i,9] = -Y*x / A;
				        M[2 * i,10] = -Z*x / A;
				        M[2 * i,11] = -(x - x0)*r_2;
				        M[2 * i + 1,4] = X / A;
				        M[2 * i + 1,5] = Y / A;
				        M[2 * i + 1,6] = Z / A;
				        M[2 * i + 1,7] = 1 / A;
				        M[2 * i + 1,8] = -X*y / A;
				        M[2 * i + 1,9] = -Y*y / A;
				        M[2 * i + 1,10] = -Z*y / A;
				        M[2 * i + 1,11] = -(y - y0)*r_2;
				        W[2 * i,0] = x / A;
				        W[2 * i + 1,0] = y / A;
			        }
			        Matrix nL = (M.T()*M).Inverse()*M.T()*W;
			        double dbeta, ds, fx, fy,Xs,Ys,Zs,a3,b3,c3,b1,b2;
			        double gama3 = 1 / sqrt(pow2(nL[8,0]) + pow2(nL[9,0]) + pow2(nL[10,0]));
			        x0 = (nL[0,0] * nL[8,0] + nL[1,0] * nL[9,0] + nL[2,0] * nL[10,0]) /
				        (pow2(nL[8,0]) + pow2(nL[9,0]) + pow2(nL[10,0]));
			        y0 = (nL[4,0] * nL[8,0] + nL[5,0] * nL[9,0] + nL[6,0] * nL[10,0]) /
				        (pow2(nL[8,0]) + pow2(nL[9,0]) + pow2(nL[10,0]));
			        double At = gama3*gama3*(pow2(nL[0,0]) + pow2(nL[1,0]) + pow2(nL[2,0])) - x0*x0,
				        Bt = gama3*gama3*(pow2(nL[4,0]) + pow2(nL[5,0]) + pow2(nL[6,0])) - y0*y0,
				        Ct = gama3*gama3*(nL[0,0] * nL[4,0] + nL[1,0] * nL[5,0] + nL[2,0] * nL[6,0]) - x0*y0;
			        if (Ct >= 0)
			        {
				        dbeta = -asin(sqrt(Ct*Ct / At / Bt));
			        }
			        else
			        {
				        dbeta = asin(sqrt(Ct*Ct / At / Bt));
			        }
			        ds = sqrt(At / Bt) - 1;
			        fx = sqrt((At*Bt - Ct*Ct) / Bt);
			        fy = sqrt((At*Bt - Ct*Ct) / At);
			        Matrix t=new Matrix(3, 3),b=new Matrix(3,1);
			        t[0,0] = L[0,0];
			        t[0,1] = L[1,0];
			        t[0,2] = L[2,0];
			        t[1,0] = L[4,0];
			        t[1,1] = L[5,0];
			        t[1,2] = L[6,0];
			        t[2,0] = L[8,0];
			        t[2,1] = L[9,0];
			        t[2,2] = L[10,0];
			        b[0,0] = L[3,0];
			        b[1,0] = L[7,0];
			        b[2,0] = -1;
			        Matrix xyz = t.Inverse()*b;
			        Xs = xyz[0,0];
			        Ys = xyz[1,0];
			        Zs = xyz[2,0];
			        a3 = gama3*L[8,0];
			        b3 = gama3*L[9,0];
			        c3 = gama3*L[10,0];
			        b2 = (-gama3*L[5,0] + b3*y0)*(1 + ds)*cos(dbeta) / fx;
			        b1 = (-gama3*L[1,0] + b2*fx*tan(dbeta) + b3*x0) / fx;
			        outE[j].ps.X=Xs;
			        outE[j].ps.Y=Ys;
			        outE[j].ps.Z=Zs;
			        outE[j].phi = atan(-a3 / c3);
			        outE[j].omege = asin(-b3);
			        outE[j].kappa = atan(b1 / b2);
			        inE[j].p0.X=x0;
			        inE[j].p0.Y=y0;
			        inE[j].fx = fx;
			        inE[j].fy = fy;
			        other[j].dbeta = dbeta;
			        other[j].ds = ds;
			        other[j].k1 = nL[xCount - 1,0];
			        L = nL;
		        }
		        Ls.Add(L);
	        }		

	        //像点改正
	        for (int i = 0; i < data.Count;i++)
	        {
		        for (int j = 0; j < data.pCount;j++)
		        {
			        double x = data.iPoints[i][j].X,
				           y = data.iPoints[i][j].Y,
				           x0 = inE[i].p0.X,
				           y0 = inE[i].p0.Y,
				           k1 = other[i].k1,
				           r_2=(x-x0)*(x-x0)+(y-y0)*(y-y0);
			        data.iPoints[i][j].X=(x+k1*(x-x0)*r_2);
			        data.iPoints[i][j].Y=(y + k1*(y - y0)*r_2);
		        }
	        }

	        //前方交会
	        for (int i = 0; i < data.pCount;i++)
	        {
		        Matrix Bc=new Matrix(2 * data.Count, 3);
		        Matrix lc=new Matrix(2 * data.Count, 1);
		        for (int j = 0; j < data.Count;j++)
		        {
			        double x = data.iPoints[j][i].X,
				           y = data.iPoints[j][i].Y;
			        Bc[2 * j,0] = Ls[j][0,0] - x*Ls[j][8,0];
			        Bc[2 * j,1] = Ls[j][1,0] - x*Ls[j][9,0];
			        Bc[2 * j,2] = Ls[j][2,0] - x*Ls[j][10,0];
			        Bc[2 * j + 1,0] = Ls[j][4,0] - y*Ls[j][8,0];
			        Bc[2 * j + 1,1] = Ls[j][5,0] - y*Ls[j][9,0];
			        Bc[2 * j + 1,2] = Ls[j][6,0] - y*Ls[j][10,0];
			        lc[2 * j,0] = Ls[j][3,0] - x;
			        lc[2 * j + 1,0] = Ls[j][7,0] - y;
		        }
		        Matrix t = -(Bc.T()*Bc).Inverse()*Bc.T()*lc;
		        data.allPoints.Add(new _3D_Point(t[0,0], t[1,0], t[2,0]));
	        }
        }
        public void OutPut(string path)//输出解算结果到文件
        {
            File.WriteAllText(path, OutToString(), Encoding.Default);
        }
        public string OutToString()// 将结果输出成字符串
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("L系数:");
            sb.AppendLine("L1\tL2\tL3\tL4\tL5\tL6\tL7\tL8\tL9\tL10\tL11");
            for (int i = 0; i < data.Count; i++)
            {
                sb.AppendLine( "第" +(1+i) + "张像片:");
                for (int j = 0; j < 11; j++)
                {
                    sb.Append( Ls[i][j,0] + "\t");
                }
                sb.AppendLine();
            }
            sb.AppendLine("内方位元素:" );
            sb.AppendLine( "x0\ty0\tfx\tfy" );
            for (int i = 0; i < data.Count; i++)
            {
                sb.AppendLine( "第" + (1+i) + "张像片:" );
                sb.Append( inE[i].p0.X + "\t"
                            + inE[i].p0.Y + "\t"
                            + inE[i].fx + "\t"
                            + inE[i].fy);
                sb.AppendLine();
            }
            sb.AppendLine( "外方位元素:");
            sb.AppendLine( "Xs\tYs\tZs\tphi\tomega\tkappa" );
            for (int i = 0; i < data.Count; i++)
            {
                sb.AppendLine( "第" + (1+i) + "张像片:" );
                sb.Append( outE[i].ps.X + "\t"
                   + outE[i].ps.Y + "\t"
                   + outE[i].ps.Z + "\t"
                   + outE[i].phi + "\t"
                   + outE[i].omege + "\t"
                   + outE[i].kappa);
                sb.AppendLine();
            }
            sb.AppendLine( "系统误差和畸变参数:" );
            sb.AppendLine("dbeta\tds\tk1");
            for (int i = 0; i < data.Count; i++)
            {
                sb.AppendLine( "第" + (1+i) + "张像片:" );
                sb.Append(inE[i].p0.X.ToString("0.0000") + "\t"
                    + inE[i].p0.Y.ToString("0.0000") + "\t"
                    + inE[i].fx.ToString("0.0000") + "\t"
                    + inE[i].fy.ToString("0.0000") + "\t");
                sb.AppendLine();
            }
            sb.AppendLine("加密点坐标:");
            sb.AppendLine("点名\tX\tY\tZ" );
            for (int i = 0; i < data.pCount; i++)
            {
                sb.AppendLine(data.pINames[i] + "\t"
                    + data.allPoints[i].X.ToString("0.0000") + "\t"
                    + data.allPoints[i].Y.ToString("0.0000") + "\t"
                    + data.allPoints[i].Z.ToString("0.0000"));
            }
            return sb.ToString();
        }
    }

}
