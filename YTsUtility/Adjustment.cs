using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YTsUtility
{
    /// <summary>
    /// 平差类
    /// </summary>
    public class Adjustment
    {
        /// <summary>
        /// 系数矩阵B
        /// </summary>
        public Matrix B { private set; get; }

        /// <summary>
        /// 权阵P
        /// </summary>
        public Matrix P { private set; get; }

        /// <summary>
        /// l
        /// </summary>
        public Matrix l { private set; get; }

        /// <summary>
        /// 初值L0
        /// </summary>
        public Matrix L0 { private set; get; }

        /// <summary>
        /// 初值X0
        /// </summary>
        public Matrix X0 { set; get; }

        /// <summary>
        /// Qxx^,x的谐因数阵
        /// </summary>
        public Matrix QXX { private set; get; }

        /// <summary>
        /// QLL^,L的协因数阵
        /// </summary>
        public Matrix QLL { private set; get; }

        /// <summary>
        /// X的改正项
        /// </summary>
        public Matrix x { private set; get; }

        /// <summary>
        /// 平差后的X^
        /// </summary>
        public Matrix X { private set; get; }

        /// <summary>
        /// 平差后的L^
        /// </summary>
        public Matrix L { private set; get; }

        /// <summary>
        /// v=Bx-l
        /// </summary>
        public Matrix v{private set;get;}

        /// <summary>
        /// v的协因数阵
        /// </summary>
        public Matrix Qvv{private set;get;}

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="B">x的系数阵</param>
        /// <param name="P">权阵</param>
        /// <param name="l">l</param>
        /// <param name="L0">初值L0</param>
        /// <param name="X0">初值X0</param>
        public Adjustment(Matrix B,Matrix P,Matrix l,Matrix L0,Matrix X0)
        {
            this.B = B;
            this.P = P;
            this.l = l;
            this.L0 = L0;
            this.X0 = X0;
        }

        /// <summary>
        /// 构造实例
        /// </summary>
        /// <param name="B"></param>
        /// <param name="P"></param>
        /// <param name="l"></param>
        /// <param name="X0"></param>
        public Adjustment(Matrix B,Matrix P,Matrix l,Matrix X0)
        {
            this.B = B;
            this.P = P;
            this.l = l;
            this.X0 = X0;
        }
        /// <summary>
        /// 平差计算
        /// <param name="mode">0表示完整计算，其他表示不需观测值平差值的计算</param>
        /// </summary>
        public void Calculate(int mode=0)
        {
            if(mode==0)
            {
                QXX = (B.T()* P * B).Inverse();
                x = QXX * B.T() * P * l;
                v = B * x - l;
                L = L0 + l;
                X = X0 + x;
                QLL = B * QXX * B.T();
                Qvv = P.Inverse() - QLL;
            }
            else
            {
                QXX = (B.T()*P * B).Inverse();
                x = QXX * B.T()*P * l;
                X = X0 + x;
            }
        }

        /// <summary>
        /// 确认是否终止迭代
        /// </summary>
        /// <param name="accurate"></param>
        /// <returns></returns>
        public bool IsTerminating(double accurate)
        {
            for(int i=0;i<x.Row;i++)
                for(int j=0;j<x.Column;j++)
                {
                    if (x[i, j] <= accurate)
                        continue;
                    else
                        return false;
                }
            return true;
        }
    }
}
