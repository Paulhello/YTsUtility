using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace YTsUtility
{
    /// <summary>
    /// 矩阵类
    /// </summary>
    [Serializable]
    public class Matrix
    {
        /// <summary>
        /// 矩阵行数
        /// </summary>
        public int Row { get; private set; }
        /// <summary>
        /// 矩阵列数
        /// </summary>
        public int Column { get; private set; }
        /// <summary>
        /// 矩阵数组，只读
        /// </summary>
        public double[][] M_matrix { get; private set; }

        /// <summary>
        /// 构造一个实例
        /// </summary>
        private Matrix()
        {

        }

        /// <summary>
        /// 构造一个实例
        /// </summary>
        public Matrix(int row, int col)
        {
            Row = row;
            Column = col;
            M_matrix = new double[row][];
            for (int i = 0; i < row; ++i)
            {
                M_matrix[i] = new double[col];
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="m_matrix"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public Matrix(double[][]m_matrix,int row,int col)
        {
            Row = row;
            Column = col;
            M_matrix = new double[row][]; 
            for (int i = 0; i < Row;++i )
            {
                M_matrix[i] = new double[col];
                for (int j = 0; j < Column; ++j)
                {
                    M_matrix[i][j] = m_matrix[i][j];
                }
            }

        }
        /// <summary>
        /// 构造一个实例
        /// </summary>
        /// <param name="m_matrix"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public Matrix(double[,] m_matrix, int row, int col)
        {
            M_matrix = new double[row][];
            for (int i = 0; i < row;++i )
            {
                M_matrix[i] = new double[col];
                for(int j=0;j<col;++j)
                {
                    M_matrix[i][j] = m_matrix[i, j];
                }
            }
                Row = row; Column = col;
        }

        /// <summary>
        /// 构造一个实例
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="value"></param>
        public Matrix(int row, int col, double[] value)
        {
            Row = row; Column = col;
            M_matrix = new double[row][];
            int k = 0;
            for (int i = 0; i < row; ++i)
            {
                M_matrix[i] = new double[col];
                for (int j = 0; j < col; ++j)
                {
                    M_matrix[i][j] = value[k];
                    ++k;
                }
            }
        }

        /// <summary>
        /// 获得子矩阵
        /// </summary>
        /// <param name="rstart">行起点(从零开始计)</param>
        /// <param name="rend">行终点(从零开始计)</param>
        /// <param name="cstart">列起点(从零开始计)</param>
        /// <param name="cend">列终点(从零开始计)</param>
        /// <returns></returns>
        public Matrix SubMatrix(int rstart,int rend,int cstart,int cend)
        {
            Matrix m = new Matrix(rend - rstart + 1, cend - cstart + 1);
            for (int i = rstart; i <= rend;++i )
            {
                for (int j = cstart; j <= cend;++j )
                {
                    m[i, j] = M_matrix[i][j];
                }
            }
            return m;
        }

        /// <summary>
        /// 获得子矩阵
        /// </summary>
        /// <param name="rstart">行起点(从零开始计)</param>
        /// <param name="rend">行终点(从零开始计)</param>
        /// <returns></returns>
        public Matrix SubRMatrix(int rstart,int rend)
        {
            return SubMatrix(rstart, rend, 0, Column - 1);
        }

        /// <summary>
        /// 获得子矩阵
        /// </summary>
        /// <param name="cstart">列起点(从零开始计)</param>
        /// <param name="cend">列终点(从零开始计)</param>
        /// <returns></returns>
        public Matrix SubCMatrix(int cstart,int cend)
        {
            return SubMatrix(0, Row - 1, cstart, cend);
        }

        /// <summary>
        /// 去掉矩阵中的某列
        /// </summary>
        /// <param name="nth"></param>
        public void Abandon_Column(int nth)
        {
            if (nth < Column)
            {
                Matrix m=new Matrix(Row,Column);
                bool flag = false;
                for (int j = 0; j < Column; ++j)
                {
                    if (j == nth)
                    {
                        flag = true;
                        continue;
                    }
                    for (int i = 0; i < Row; ++i)
                    {
                        if (flag)
                        {
                            m[i, j - 1] = M_matrix[i][j];
                        }
                        else
                        {
                            m[i, j] = M_matrix[i][j];
                        }
                    }
                }
                M_matrix = m.M_matrix;
                Column--;
            }

        }

        /// <summary>
        /// 获取从某行起的行主元索引
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        private int GetMax(int start)
        {
            double max=M_matrix[start][start];
            int index=start;
            for(int i=start;i<Row;++i)
                if(Math.Abs(max)<Math.Abs(M_matrix[i][start]))
                {
                    max = M_matrix[i][start];
                    index = i;
                }
            return index;
        }

        /// <summary>
        /// 交换两行
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        private void SwapRow(int first,int second)
        {
            double[] t = M_matrix[first];
            M_matrix[first] = M_matrix[second];
            M_matrix[second] = t;
        }

        /// <summary>
        /// 一行同时乘一个数
        /// </summary>
        /// <param name="row"></param>
        /// <param name="f"></param>
        private void Multiply(int row,double f)
        {
            for(int i=0;i<Column;++i)
            {
                M_matrix[row][i] *= f;
            }
        }

        /// <summary>
        /// 消元A[row]-f*A[main]
        /// </summary>
        /// <param name="row"></param>
        /// <param name="main"></param>
        /// <param name="f"></param>
        private void Elimination(int row,int main,double f)
        {
            for(int i=0;i<Column;++i)
            {
                M_matrix[row][i] -= M_matrix[main][i] * f;
            }
        }

        /// <summary>
        /// 方阵的行列式
        /// </summary>
        /// <returns></returns>
        public double Det()
        {
            Matrix t = this.Clone();
            int count = 0;
            for (int i = 0; i < Row-1; ++i)
            {
                int index = t.GetMax(i);
                if(index!=i)
                {
                    t.SwapRow(i, index);
                    ++count;
                }

                for (int j = i + 1; j < Row; ++j)
                {
                    if (t[i, i] == 0) return 0;
                    t.Elimination(j, i, t[j, i] / t[i, i]);
                }
            }
            double sum = 1;
            for (int i = 0; i < Row;++i )
            {
                sum *= t[i, i];
            }
            return sum*(count%2==0?1:-1);
        }

        /// <summary>
        /// 高斯消去法求逆阵
        /// </summary>
        /// <returns></returns>
        public Matrix Inverse(bool diag=false)
        {
            if (diag)
            {
                Matrix arm=new Matrix(Row,Column);
                for (int i = 0; i < Row; ++i)
                {
                    arm[i, i] = 1 / M_matrix[i][i];

                }
                return arm;
            }
            Matrix t=this.Clone();
            Matrix E = Eye(Row);
            for(int i=0;i<Row;++i)
            {
                int index = t.GetMax(i);
                if(index!=i)
                {
                    t.SwapRow(i, index);
                    E.SwapRow(i, index);
                }

                for(int j=0;j<Row;++j)
                {
                    if (j == i) continue;
                    E.Multiply(i, 1 / t[i, i]);
                    t.Multiply(i, 1 / t[i, i]);
                    E.Elimination(j, i, t[j, i]);
                    t.Elimination(j, i, t[j, i]);
                }
            }
            return E;
        }
        //public Matrix Get_ArmatrixN(bool diag = false)
        //{
        //    Matrix m_matrixt;

        //    else
        //    {
        //        Matrix[] mtx = new Matrix[M_Width];
        //        Matrix[] mty = new Matrix[M_Width];
        //        for (int i = 0; i < M_Width; ++i)
        //        {
        //            double[] t = new double[M_Width];
        //            for (int j = 0; j < M_Hight; ++j)
        //            {
        //                t[j] = M_matrix[i, j];
        //            }
        //            mtx[i] = new Matrix(1, M_Hight, t);
        //            mty[i] = new Matrix(1, M_Hight);
        //        }
        //        mty[0] /= mtx[0].M_matrix[0, 0];
        //        mty[0].M_matrix[0, 0] = 1 / mtx[0].M_matrix[0, 0];
        //        mtx[0] /= (-mtx[0].M_matrix[0, 0]);
        //        mtx[0].M_matrix[0, 0] = 0;
        //        for (int i = 1; i < M_Width; ++i)//迭代
        //        {
        //            for (int k = 0; k < i; ++k)
        //            {
        //                mtx[i] += (mtx[i].M_matrix[0, k] * mtx[k]);
        //                mty[i] += (mtx[i].M_matrix[0, k] * mty[k]);
        //                mtx[i].M_matrix[0, k] = 0;
        //            }
        //            mty[i] /= (-mtx[i].M_matrix[0, i]);
        //            mty[i].M_matrix[0, i] = 1 / mtx[i].M_matrix[0, i];
        //            mtx[i] /= (-mtx[i].M_matrix[0, i]);
        //            mtx[i].M_matrix[0, i] = 0;
        //        }
        //        for (int i = M_Width - 1; i > 0; i--)//回带
        //        {
        //            for (int k = i; k < M_Width; ++k)
        //            {
        //                mty[i - 1] += mtx[i - 1].M_matrix[0, k] * mty[k];
        //            }
        //        }
        //        double[,] value = new double[M_Width, M_Hight];
        //        for (int i = 0; i < M_Width; ++i)//转换成矩阵
        //        {
        //            for (int j = 0; j < M_Hight; ++j)
        //            {
        //                value[i, j] = mty[i].M_matrix[0, j];
        //            }
        //        }
        //        m_matrixt = new Matrix(value, M_Width, M_Hight);
        //        m_armatrix = value;
        //    }


        //    return m_matrixt;
        //}

        /// <summary>
        /// 转置
        /// </summary>
        /// <returns></returns>
        public Matrix T()
        {
            double[,] tm = new double[Column, Row];
            for (int i = 0; i < Row; ++i)
                for (int j = 0; j < Column; ++j)
                    tm[j, i] = M_matrix[i][j];
            return new Matrix(tm, Column, Row);
        }

        /// <summary>
        /// 设置精度
        /// </summary>
        /// <param name="n"></param>
        public void SetAccurate(int n)
        {
            for (int i = 0; i < Row; ++i)
                for (int j = 0; j < Column; ++j)
                    M_matrix[i][j] = Methods.Set_Accurate(M_matrix[i][j], n);
        }

        /// <summary>
        /// 矩阵相加
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            Matrix t = new Matrix(m1.Row, m1.Column);
            for (int i = 0; i < m1.Row; ++i)
                for (int j = 0; j < m1.Column; ++j)
                    t[i,j] = m1[i,j] + m2[i,j];
            return t;
        }

        /// <summary>
        /// 矩阵相减
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            Matrix t = new Matrix(m1.Row, m1.Column);
            for (int i = 0; i < m1.Row; ++i)
                for (int j = 0; j < m1.Column; ++j)
                    t[i,j] = m1[i,j] - m2[i,j];
            return t;
        }

        /// <summary>
        /// 矩阵取反
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static Matrix operator -(Matrix m)
        {
            return m*(-1);
        }

        /// <summary>
        /// 数乘
        /// </summary>
        /// <param name="m"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Matrix operator *(Matrix m, double f)
        {
            Matrix t = new Matrix(m.Row, m.Column);
            for (int i = 0; i < m.Row; ++i)
                for (int j = 0; j < m.Column; ++j)
                    t[i,j] = m[i,j] * f;
            return t;
        }

        /// <summary>
        /// 数乘
        /// </summary>
        /// <param name="f">乘数</param>
        /// <param name="m">矩阵</param>
        /// <returns></returns>
        public static Matrix operator *(double f, Matrix m)
        {
            Matrix t = new Matrix(m.Row, m.Column);
            for (int i = 0; i < m.Row; ++i)
                for (int j = 0; j < m.Column; ++j)
                    t[i, j] = m[i, j] * f;
            return t;
        }

        /// <summary>
        /// 矩阵相乘
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            Matrix t = new Matrix(m1.Row, m2.Column);
            for (int i = 0; i < m1.Row; ++i)
            {
                for (int j = 0; j < m2.Column; ++j)
                    for (int k = 0; k < m2.Row; ++k)
                    {
                        t[i, j] += m1[i, k] * m2[k, j];
                    }
            }
            return t;
        }

        /// <summary>
        /// 矩阵除以一个数
        /// </summary>
        /// <param name="m"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Matrix operator /(Matrix m, double f)
        {
            Matrix t = new Matrix(m.Row, m.Column);
            for (int i = 0; i < m.Row; ++i)
                for (int j = 0; j < m.Column; ++j)
                    t[i, j] = m[i, j] / f;
            return t;
        }

        /// <summary>
        /// 索引器(返回确定坐标的值)
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="col">列</param>
        /// <returns></returns>
        public double this[int row,int col]
        {
            get
            {
                return M_matrix[row][col];
            }
            set
            {
                M_matrix[row][col]=value;
            }
        }

        /// <summary>
        /// 转换成字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Row; ++i)
            {
                for (int j = 0; j < Column; ++j)
                {
                    sb.Append(M_matrix[i][j] + "\t");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        /// <summary>
        /// 转换成特定小数位数的字符串
        /// </summary>
        /// <param name="n">小数位数</param>
        /// <returns></returns>
        public string ToFormatString(int n)
        {
            StringBuilder sb=new StringBuilder();
            for (int i = 0; i < Row; ++i)
            {
                for (int j = 0; j < Column; ++j)
                {
                    sb.Append(M_matrix[i][j].ToFormatString(n) + "\t");
                }
                if(i<Row-1)sb.AppendLine();
            }
            return sb.ToString();
        }

        /// <summary>
        /// 对外提供一个精确副本
        /// </summary>
        /// <returns></returns>
        public Matrix Clone()
        {
            return new Matrix(M_matrix,Row, Column);
        }

        /// <summary>
        /// 将矩阵输出为csv文件
        /// </summary>
        /// <param name="filename"></param>
        public void OutPut(string filename)
        {
            StreamWriter writer = new StreamWriter(filename + ".csv", false, Encoding.UTF8);
            for (int i = 0; i < Row; ++i)
            {
                for (int j = 0; j < Column - 1; ++j)
                {
                    writer.Write(M_matrix[i][j] + ",");
                }
                writer.WriteLine(M_matrix[i][Column - 1]);
            }
            writer.Close();
        }

        /// <summary>
        /// 构造一个单位矩阵
        /// </summary>
        /// <param name="level">矩阵阶数</param>
        /// <returns></returns>
        public static Matrix Eye(int level)
        {
            Matrix mat = new Matrix(level, level);
            for (int i = 0; i < level; ++i)
                mat[i, i] = 1;
            return mat;
        }

        /// <summary>
        /// 生成指定行列的零矩阵
        /// </summary>
        /// <param name="width"></param>
        /// <param name="hight"></param>
        /// <returns></returns>
        public static Matrix Zeros(int width,int hight)
        {
            return new Matrix(width, hight);
        }

        /// <summary>
        /// 构造一个全为num的矩阵
        /// </summary>
        /// <param name="num"></param>
        /// <param name="width"></param>
        /// <param name="hight"></param>
        /// <returns></returns>
        public static Matrix Num(double num,int width,int hight)
        {
            Matrix t = new Matrix(width, hight);
            for (int i = 0; i < width; ++i)
                for (int j = 0; j < hight; ++j)
                    t[i, j] = num;
            return t;
        }

        /// <summary>
        /// 将两个矩阵按行合并(须保证列数相等)
        /// </summary>
        /// <param name="m1">第一个矩阵</param>
        /// <param name="m2">第二个矩阵</param>
        /// <returns></returns>
        public static Matrix RowCombine(Matrix m1,Matrix m2)
        {
            int level=m1.Row+m2.Row;
            Matrix mt = new Matrix(level, m1.Column);
            for (int i = 0; i < m1.Row; ++i)
            {
                for (int j = 0; j < m1.Column; ++j)
                {
                    mt[i, j] = m1[i, j];
                }
            }
            for (int i = m1.Row,k=0; i < level; ++i,++k)
            {
                for (int j = 0; j < m2.Column; ++j)
                {
                    mt[i, j] = m2[k, j];
                }
            }
            return mt;
        }

        /// <summary>
        /// 将两个矩阵按列合并(须保证行数相等)
        /// </summary>
        /// <param name="m1">第一个矩阵</param>
        /// <param name="m2">第二个矩阵</param>
        /// <returns></returns>
        public static Matrix ColumnCombine(Matrix m1, Matrix m2)
        {
            int level = m1.Column + m2.Column;
            Matrix mt = new Matrix(m1.Row,level);
            for (int i = 0; i < m1.Row; ++i)
            {
                for (int j = 0; j < m1.Column; ++j)
                {
                    mt[i, j] = m1[i, j];
                }
            }
            for (int i = 0; i < m2.Row; ++i)
            {
                for (int j = m1.Column,k=0; j < level; ++j,++k)
                {
                    mt[i, j] = m2[i, k];
                }
            }
            return mt;
        }
    }

}
