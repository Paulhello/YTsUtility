using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace YTsUtility
{
    namespace ImageProcess
    {
        /// <summary>
        /// 图像处理类(暂时只适用于24位深的图像)
        /// </summary>
        public class ImageProcessing
        {
            /// <summary>
            /// 图像源
            /// </summary>
            public Bitmap bitmap { set { bitmap = value;
                                         //rect = new Rectangle(new Point(), bitmap.Size);
                                       } 
                                   get { return bitmap;}
                                 }

            private Rectangle rect;

            /// <summary>
            /// 通过位图构造
            /// </summary>
            /// <param name="bmp"></param>
            public ImageProcessing(Bitmap bmp)
            {
                bitmap = bmp.Clone() as Bitmap;
                rect = new Rectangle(new Point(), bitmap.Size);
            }

            /// <summary>
            /// 通过文件名构造
            /// </summary>
            /// <param name="bmppath">文件路径</param>
            public ImageProcessing(string bmppath)
            {
                bitmap = Bitmap.FromFile(bmppath) as Bitmap;
                rect = new Rectangle(new Point(), bitmap.Size);
            }

            /// <summary>
            /// 灰度化
            /// </summary>
            /// <returns></returns>
            public Bitmap Scalize()
            {
                Bitmap newmap = bitmap.Clone() as Bitmap;
                BitmapData bmpdata = newmap.LockBits(rect, ImageLockMode.ReadWrite, newmap.PixelFormat);
                int bytes = bmpdata.Stride * bmpdata.Height;
                byte[] bdata = new byte[bytes];
                IntPtr ptr = bmpdata.Scan0;
                Marshal.Copy(ptr, bdata, 0, bytes);
                int index = 0,offset=bmpdata.Stride-3*bmpdata.Width;
                byte temp = 0;

                for (int i = 0; i < bmpdata.Height;++i)
                {
                    for (int j = 0; j < bmpdata.Width;++j)
                    {
                        temp = (byte)(bdata[index + 2] * 0.3 + bdata[index + 1] * 0.59 + bdata[index] * 0.11 + 0.5);
                        bdata[index + 2] = bdata[index + 1] = bdata[index] = temp;
                        index += 3;
                    }
                    index += offset;
                }

                Marshal.Copy(bdata, 0, ptr, bytes);
                newmap.UnlockBits(bmpdata);
                return newmap;
            }

            /// <summary>
            /// 根据阈值进行二值化
            /// </summary>
            /// <param name="T0">阈值</param>
            /// <returns>二值化后的图像</returns>
            public Bitmap TwoValue(int T0)
            {
                Bitmap newmap = bitmap.Clone() as Bitmap;
                //Rectangle rect = new Rectangle(new Point(), newmap.Size);
                BitmapData bmpdata = newmap.LockBits(rect, ImageLockMode.ReadWrite, newmap.PixelFormat);
                int bytes = bmpdata.Stride * bmpdata.Height;
                byte[] bdata = new byte[bytes];
                //byte[,] bdata2 = new byte[bmpdata.Stride, bmpdata.Height];
                IntPtr ptr = bmpdata.Scan0;
                Marshal.Copy(ptr, bdata, 0, bytes);
                int index = 0, offset = bmpdata.Stride - 3 * bmpdata.Width;
                byte temp = 0;
                //二值化
                for (int i = 0; i < bmpdata.Height; ++i)
                {
                    for (int j = 0; j < bmpdata.Width; ++j)
                    {
                        temp = (byte)(bdata[index + 2] * 0.3 + bdata[index + 1] * 0.59 + bdata[index] * 0.11 + 0.5);
                        
                        bdata[index + 2] = bdata[index + 1] = bdata[index] = (byte)(temp>T0?255:0);
                        index += 3;
                    }
                    index += offset;
                }

                Marshal.Copy(bdata, 0, ptr, bytes);
                newmap.UnlockBits(bmpdata);
                return newmap;
            }

            /// <summary>
            /// OSTU法获取最佳阈值
            /// </summary>
            /// <returns>最佳阈值</returns>
            public int GetBestTresh()
            {
                var tempimg=Scalize();
                BitmapData bmpdata =tempimg.LockBits(rect, ImageLockMode.ReadOnly, bitmap.PixelFormat);
                int bytes = bmpdata.Stride * bmpdata.Height;
                byte[] bdata = new byte[bytes];
                int[] pixelNum = new int[256];
                IntPtr ptr = bmpdata.Scan0;
                Marshal.Copy(ptr, bdata, 0, bytes);
                int index = 0 , offset=bmpdata.Stride - bmpdata.Width * 3;
                //统计各灰度值个数
                for (int i = 0; i < bmpdata.Height; ++i)
                {
                    for (int j = 0; j < bmpdata.Width; ++j)
                    {
                        ++pixelNum[bdata[index]];
                        index += 3;
                    }
                    index += offset;
                }

                double sum=0, csum=0;
                int n = 0;
                //得到总像素个数与质量矩
                for (int i = 0; i < 256;++i )
                {
                    sum += i * pixelNum[i];
                    n += pixelNum[i];
                }

                int n1 = 0, n2 = 0,threshval = 0;
                double sb, fmax=-1;
                for (int i = 0; i < 256;++i)
                {
                    n1 += pixelNum[i];
                    if (n1 == 0) continue;
                    n2 = n - n1;
                    if (n2==0)break;
                    csum += i * pixelNum[i];
                    double m1 = csum / n1,
                           m2 = (sum - csum) / n2;
                    sb = n1 * n2 * (m1 - m2) * (m1 - m2);
                    if (sb>fmax)
                    {
                        fmax = sb;
                        threshval = i;
                    }
                }

                tempimg.UnlockBits(bmpdata);
                tempimg.Dispose();
                return threshval;
            }

            /// <summary>
            /// 像素各分量个数统计
            /// </summary>
            /// <returns>像素统计表(顺序为RGB)</returns>
            public List<int[]> RGBCount()
            {
                List<int[]> countList = new List<int[]> { new int[256], new int[256], new int[256] };
                BitmapData bmpdata = bitmap.LockBits(rect, ImageLockMode.ReadOnly, bitmap.PixelFormat);
                int bytes = bmpdata.Stride * bmpdata.Height;
                byte[] bdata = new byte[bytes];
                IntPtr ptr = bmpdata.Scan0;
                Marshal.Copy(ptr, bdata, 0, bytes);
                int index = 0, offset = bmpdata.Stride - bmpdata.Width * 3;
                for (int i = 0; i < bmpdata.Height; ++i)
                {
                    for (int j = 0; j < bmpdata.Width; ++j)
                    {
                        ++countList[2][bdata[index]];
                        ++countList[1][bdata[index+1]];
                        ++countList[0][bdata[index+2]];
                        index += 3;
                    }
                    index += offset;
                }
                bitmap.UnlockBits(bmpdata);
                return countList;
            }

            /// <summary>
            /// 像素各分量个数统计的静态方法
            /// </summary>
            /// <param name="bmp">需要统计的图像</param>
            /// <returns>像素统计表(顺序为RGB)</returns>
            public static List<int[]> RGBCount(Bitmap bmp)
            {
                Rectangle rec = new Rectangle(new Point(), bmp.Size);
                List<int[]> countList = new List<int[]> { new int[256], new int[256], new int[256] };
                BitmapData bmpdata = bmp.LockBits(rec, ImageLockMode.ReadOnly, bmp.PixelFormat);
                int bytes = bmpdata.Stride * bmpdata.Height;
                byte[] bdata = new byte[bytes];
                IntPtr ptr = bmpdata.Scan0;
                Marshal.Copy(ptr, bdata, 0, bytes);
                int index = 0, offset = bmpdata.Stride - bmpdata.Width * 3;
                for (int i = 0; i < bmpdata.Height; ++i)
                {
                    for (int j = 0; j < bmpdata.Width; ++j)
                    {
                        ++countList[2][bdata[index]];
                        ++countList[1][bdata[index + 1]];
                        ++countList[0][bdata[index + 2]];
                        index += 3;
                    }
                    index += offset;
                }
                bmp.UnlockBits(bmpdata);
                return countList;
            }

            /// <summary>
            /// 反色处理
            /// </summary>
            /// <returns>反色处理后的图像</returns>
            public Bitmap Invert()
            {
                Bitmap newmap = bitmap.Clone() as Bitmap;
                BitmapData bmpdata = newmap.LockBits(rect, ImageLockMode.ReadWrite, newmap.PixelFormat);
                int bytes = bmpdata.Stride * bmpdata.Height;
                byte[] bdata = new byte[bytes];
                IntPtr ptr = bmpdata.Scan0;
                Marshal.Copy(ptr, bdata, 0, bytes);
                int index = 0, offset = bmpdata.Stride - 3 * bmpdata.Width;

                for (int i = 0; i < bmpdata.Height; ++i)
                {
                    for (int j = 0; j < bmpdata.Width; ++j)
                    {
                        bdata[index + 2] =(byte)~bdata[index + 2];
                        bdata[index + 1] = (byte)~bdata[index + 1];
                        bdata[index] = (byte)~bdata[index]; ;
                        index += 3;
                    }
                    index += offset;
                }

                Marshal.Copy(bdata, 0, ptr, bytes);
                newmap.UnlockBits(bmpdata);
                return newmap;
            }

            /// <summary>
            /// 3*3中值滤波(1028*800,2s)
            /// </summary>
            /// <returns></returns>
            public Bitmap MidFilter()
            {
                Bitmap newmap = bitmap.Clone() as Bitmap;
                BitmapData bmpdata = newmap.LockBits(rect, ImageLockMode.ReadWrite, newmap.PixelFormat);
                int bytes = bmpdata.Stride * bmpdata.Height;
                byte[] bdata = new byte[bytes];
                IntPtr ptr = bmpdata.Scan0;
                Marshal.Copy(ptr, bdata, 0, bytes);
                var bdataEx = ExpandBorder(bdata, bmpdata.Width, bmpdata.Stride, 1);
                int newwid=(bmpdata.Width+2)*3, offset = bmpdata.Stride - 3 * bmpdata.Width;

#region 中值滤波
                int index=0,l,k;
                for (int i = 0; i < bmpdata.Height; ++i)
                {
                    for (int j = 0; j < bmpdata.Width; ++j)
                    {
                        List<byte> blist = new List<byte>();
                        int tempid = 0;
                        for (l = -1; l < 2; ++l)
                            for (k = -1; k < 2; ++k)
                            {
                                tempid = (i + 1 + l) * newwid + (j + 1 + k) * 3;
                                blist.Add(bdataEx[tempid]);
                            }
                        blist.Sort();
                        bdata[index] = blist[4];
                        blist.Clear();

                        for (l = -1; l < 2; ++l)
                            for (k = -1; k < 2; ++k)
                            {
                                tempid = (i + 1 + l) * newwid + (j + 1 + k) * 3+1;
                                blist.Add(bdataEx[tempid]);
                            }
                        blist.Sort();
                        bdata[index+1] = blist[4];
                        blist.Clear();

                        for (l = -1; l < 2; ++l)
                            for (k = -1; k < 2; ++k)
                            {
                                tempid = (i + 1 + l) * newwid + (j + 1 + k) * 3+2;
                                blist.Add(bdataEx[tempid]);
                            }
                        blist.Sort();
                        bdata[index+2] = blist[4];
                        blist.Clear();
                        index += 3;
                    }
                }
#endregion
                
                Marshal.Copy(bdata, 0, ptr, bytes);
                newmap.UnlockBits(bmpdata);
                return newmap;
            }

            /// <summary>
            /// 加权中值滤波
            /// </summary>
            /// <param name="P">权3*3</param>
            /// <param name="z">权分母</param>
            /// <returns></returns>
            public Bitmap MidFilter(double[,]P,double z)
            {
                Bitmap newmap = bitmap.Clone() as Bitmap;
                BitmapData bmpdata = newmap.LockBits(rect, ImageLockMode.ReadWrite, newmap.PixelFormat);
                int bytes = bmpdata.Stride * bmpdata.Height;
                byte[] bdata = new byte[bytes];
                IntPtr ptr = bmpdata.Scan0;
                Marshal.Copy(ptr, bdata, 0, bytes);
                var bdataEx = ExpandBorder(bdata, bmpdata.Width, bmpdata.Stride, 1);
                int newwid = (bmpdata.Width + 2) * 3, offset = bmpdata.Stride - 3 * bmpdata.Width;

                #region 中值滤波
                int index = 0, l, k;
                for (int i = 0; i < bmpdata.Height; ++i)
                {
                    for (int j = 0; j < bmpdata.Width; ++j)
                    {
                        List<double> blist = new List<double>();
                        int tempid = 0;
                        for (l = -1; l < 2; ++l)
                            for (k = -1; k < 2; ++k)
                            {
                                tempid = (i + 1 + l) * newwid + (j + 1 + k) * 3;
                                blist.Add(bdataEx[tempid]*P[l,k]);
                            }
                        bdata[index] = (byte)(blist.Sum()/z);
                        blist.Clear();

                        for (l = -1; l < 2; ++l)
                            for (k = -1; k < 2; ++k)
                            {
                                tempid = (i + 1 + l) * newwid + (j + 1 + k) * 3 + 1;
                                blist.Add(bdataEx[tempid] * P[l, k]);
                            }
                        blist.Sort();
                        bdata[index + 1] = (byte)(blist.Sum() / z);
                        blist.Clear();

                        for (l = -1; l < 2; ++l)
                            for (k = -1; k < 2; ++k)
                            {
                                tempid = (i + 1 + l) * newwid + (j + 1 + k) * 3 + 2;
                                blist.Add(bdataEx[tempid] * P[l, k]);
                            }
                        blist.Sort();
                        bdata[index + 2] = (byte)(blist.Sum() / z);
                        blist.Clear();
                        index += 3;
                    }
                }
                #endregion

                Marshal.Copy(bdata, 0, ptr, bytes);
                newmap.UnlockBits(bmpdata);
                return newmap;
            }

            /// <summary>
            /// 加权中值滤波器1：加强中心，保留和加强水平垂直线
            /// P:
            /// 0 1 0
            /// 1 4 1
            /// 0 1 0
            /// </summary>
            /// <param name="z">权分母，默认为16，影响图像亮度</param>
            /// <returns></returns>
            public Bitmap MidFilter1(double z=16)
            {
                double[,] P = { { 0, 1, 0 }, { 1, 4, 1 }, { 0, 1, 0 } };
                return MidFilter(P, z);
            }

            /// <summary>
            /// 加权中值滤波器2：保留和加强水平垂直线
            /// P:
            /// 0 1 0
            /// 1 0 1
            /// 0 1 0
            /// </summary>
            /// <param name="z">权分母，默认为16，影响图像亮度</param>
            /// <returns></returns>
            public Bitmap MidFilter2(double z = 16)
            {
                double[,] P = { { 0, 1, 0 }, { 1, 0, 1 }, { 0, 1, 0 } };
                return MidFilter(P, z);
            }

            /// <summary>
            /// 加权中值滤波器3：保留对角线
            /// P:
            /// 1 0 1
            /// 0 4 0
            /// 1 0 1
            /// </summary>
            /// <param name="z">权分母，默认为16，影响图像亮度</param>
            /// <returns></returns>
            public Bitmap MidFilter3(double z = 16)
            {
                double[,] P = { { 1, 0, 1 }, { 0, 4, 0 }, { 1, 0, 1 } };
                return MidFilter(P, z);
            }

            /// <summary>
            /// 加权中值滤波器4：均衡效果
            /// P:
            /// 1 1 1
            /// 1 4 1
            /// 1 1 1
            /// </summary>
            /// <param name="z">权分母，默认为16，影响图像亮度</param>
            /// <returns></returns>
            public Bitmap MidFilter4(double z = 16)
            {
                double[,] P = { { 1, 1, 1 }, { 1, 4, 1 }, { 1, 1, 1 } };
                return MidFilter(P, z);
            }

            /// <summary>
            /// 三阶模板卷积
            /// </summary>
            /// <param name="bmp">原图像</param>
            /// <param name="template">模板</param>
            /// <returns></returns>
            private Bitmap TemplateConvolution3(Bitmap bmp, double [,]template)
            {
                Bitmap newmap = bmp.Clone() as Bitmap;
                BitmapData bmpdata = newmap.LockBits(rect, ImageLockMode.ReadWrite, newmap.PixelFormat);
                int bytes = bmpdata.Stride * bmpdata.Height;
                byte[] bdata = new byte[bytes];
                IntPtr ptr = bmpdata.Scan0;
                Marshal.Copy(ptr, bdata, 0, bytes);
                var bdataEx = ExpandBorder(bdata, bmpdata.Width, bmpdata.Stride, 1);
                int newwid = 3*(bmpdata.Width+2), offset = bmpdata.Stride - 3 * bmpdata.Width,index=0;
                double tsum = Methods.Sum2D(template);
                if (tsum == 0) tsum = 1;

                int f1, f2, f3, c1, c2, c3, b1, b2, b3;
                for (int i = 0; i < bmpdata.Height ; ++i)
                {
                    for (int j = 0; j < bmpdata.Width ; ++j)
                    {
                        List<double> blist = new List<double>();
                        f1 = i * newwid + j * 3;
                        f2 = i * newwid + (j+1) * 3;
                        f3 = i * newwid + (j +2) * 3;
                        c1 = (i + 1) * newwid + j * 3 ;
                        c2 = (i + 1) * newwid + (j+1) * 3;
                        c3 = (i + 1) * newwid + (j + 2) * 3;
                        b1 = (i + 2) * newwid + j * 3;
                        b2 = (i + 2) * newwid + (j+1) * 3;
                        b3 = (i + 2) * newwid + (j + 2) * 3;
                        blist.AddRange(new double[]{template[0,0]*bdataEx[f1],template[0,1]*bdataEx[f2],template[0,2]*bdataEx[f3],
                                                    template[1,0]*bdataEx[c1],template[1,1]*bdataEx[c2],template[1,2]*bdataEx[c3],
                                                    template[2,0]*bdataEx[b1],template[2,1]*bdataEx[b2],template[2,2]*bdataEx[b3]});

                        double tempp=blist.Sum()/tsum;
                        if(tempp>255)
                            bdata[index]=(byte)255;
                        else if(tempp<0)
                            bdata[index]=(byte)0;
                        else bdata[index]=(byte)tempp;
                        blist.Clear();

                        f1 = i * newwid + j * 3 +1;
                        f2 = i * newwid + (j + 1) * 3 + 1;
                        f3 = i * newwid + (j + 2) * 3 + 1;
                        c1 = (i + 1) * newwid + j * 3 + 1;
                        c2 = (i + 1) * newwid + (j + 1) * 3 + 1;
                        c3 = (i + 1) * newwid + (j + 2) * 3 + 1;
                        b1 = (i + 2) * newwid + j * 3 + 1;
                        b2 = (i + 2) * newwid + (j + 1) * 3 + 1;
                        b3 = (i + 2) * newwid + (j + 2) * 3 + 1;
                        blist.AddRange(new double[]{template[0,0]*bdataEx[f1],template[0,1]*bdataEx[f2],template[0,2]*bdataEx[f3],
                                                    template[1,0]*bdataEx[c1],template[1,1]*bdataEx[c2],template[1,2]*bdataEx[c3],
                                                    template[2,0]*bdataEx[b1],template[2,1]*bdataEx[b2],template[2,2]*bdataEx[b3]});
                        tempp = blist.Sum() / tsum;
                        if (tempp > 255)
                            bdata[index+1] = (byte)255;
                        else if (tempp < 0)
                            bdata[index+1] = (byte)0;
                        else bdata[index+1] = (byte)tempp;
                        blist.Clear();

                        f1 = i * newwid + j * 3 + 2;
                        f2 = i * newwid + (j + 1) * 3 + 2;
                        f3 = i * newwid + (j + 2) * 3 + 2;
                        c1 = (i + 1) * newwid + j * 3 + 2;
                        c2 = (i + 1) * newwid + (j + 1) * 3 + 2;
                        c3 = (i + 1) * newwid + (j + 2) * 3 + 2;
                        b1 = (i + 2) * newwid + j * 3 + 2;
                        b2 = (i + 2) * newwid + (j + 1) * 3 + 2;
                        b3 = (i + 2) * newwid + (j + 2) * 3 + 2;
                        blist.AddRange(new double[]{template[0,0]*bdataEx[f1],template[0,1]*bdataEx[f2],template[0,2]*bdataEx[f3],
                                                    template[1,0]*bdataEx[c1],template[1,1]*bdataEx[c2],template[1,2]*bdataEx[c3],
                                                    template[2,0]*bdataEx[b1],template[2,1]*bdataEx[b2],template[2,2]*bdataEx[b3]});

                        tempp = blist.Sum() / tsum;
                        if (tempp > 255)
                            bdata[index+2] = (byte)255;
                        else if (tempp < 0)
                            bdata[index+2] = (byte)0;
                        else bdata[index+2] = (byte)tempp;
                        blist.Clear();
                        index+=3;
                    }
                    index+=offset;
                }

                Marshal.Copy(bdata, 0, ptr, bytes);
                newmap.UnlockBits(bmpdata);
                return newmap;
            }

            /// <summary>
            /// 二阶模板卷积
            /// </summary>
            /// <param name="bmp">原图像</param>
            /// <param name="template">二阶模板</param>
            /// <returns></returns>
            private Bitmap TemplateConvolution2(Bitmap bmp, double[,] template)
            {
                Bitmap newmap = bmp.Clone() as Bitmap;
                BitmapData bmpdata = newmap.LockBits(rect, ImageLockMode.ReadWrite, newmap.PixelFormat);
                int bytes = bmpdata.Stride * bmpdata.Height;
                byte[] bdata = new byte[bytes];
                IntPtr ptr = bmpdata.Scan0;
                Marshal.Copy(ptr, bdata, 0, bytes);
                var bdataEx = ExpandBorder(bdata, bmpdata.Width, bmpdata.Stride, 1);
                int newwid = 3 * (bmpdata.Width + 2), offset = bmpdata.Stride - 3 * bmpdata.Width, index = 0;
                double tsum = Methods.Sum2D(template);
                if (tsum == 0) tsum = 1;

                int f1, f2, c1, c2;
                for (int i = 0; i < bmpdata.Height; ++i)
                {
                    for (int j = 0; j < bmpdata.Width; ++j)
                    {
                        List<double> blist = new List<double>();
                        f1 = i * newwid + j * 3;
                        f2 = i * newwid + (j + 1) * 3;
                        c1 = (i + 1) * newwid + j * 3;
                        c2 = (i + 1) * newwid + (j + 1) * 3;
                        blist.AddRange(new double[]{template[0,0]*bdataEx[f1],template[0,1]*bdataEx[f2],
                                                    template[1,0]*bdataEx[c1],template[1,1]*bdataEx[c2]});

                        double tempp = blist.Sum() / tsum;
                        if (tempp > 255)
                            bdata[index] = (byte)255;
                        else if (tempp < 0)
                            bdata[index] = (byte)0;
                        else bdata[index] = (byte)tempp;
                        blist.Clear();

                        f1 = i * newwid + j * 3 + 1;
                        f2 = i * newwid + (j + 1) * 3 + 1;
                        c1 = (i + 1) * newwid + j * 3 + 1;
                        c2 = (i + 1) * newwid + (j + 1) * 3 + 1;
                        blist.AddRange(new double[]{template[0,0]*bdataEx[f1],template[0,1]*bdataEx[f2],
                                                    template[1,0]*bdataEx[c1],template[1,1]*bdataEx[c2]});
                        tempp = blist.Sum() / tsum;
                        if (tempp > 255)
                            bdata[index + 1] = (byte)255;
                        else if (tempp < 0)
                            bdata[index + 1] = (byte)0;
                        else bdata[index + 1] = (byte)tempp;
                        blist.Clear();

                        f1 = i * newwid + j * 3 + 2;
                        f2 = i * newwid + (j + 1) * 3 + 2;
                        c1 = (i + 1) * newwid + j * 3 + 2;
                        c2 = (i + 1) * newwid + (j + 1) * 3 + 2;
                        blist.AddRange(new double[]{template[0,0]*bdataEx[f1],template[0,1]*bdataEx[f2],
                                                    template[1,0]*bdataEx[c1],template[1,1]*bdataEx[c2]});

                        tempp = blist.Sum() / tsum;
                        if (tempp > 255)
                            bdata[index + 2] = (byte)255;
                        else if (tempp < 0)
                            bdata[index + 2] = (byte)0;
                        else bdata[index + 2] = (byte)tempp;
                        blist.Clear();
                        index += 3;
                    }
                    index += offset;
                }

                Marshal.Copy(bdata, 0, ptr, bytes);
                newmap.UnlockBits(bmpdata);
                return newmap;
            }

            /// <summary>
            /// n阶模板卷积
            /// </summary>
            /// <param name="bmp">原图像</param>
            /// <param name="template">n阶模板</param>
            /// <param name="level">模板阶数</param>
            /// <returns></returns>
            private Bitmap TemplateConvolution(Bitmap bmp, double[,] template,int level)
            {
                var tp = template;
                Bitmap newmap = bmp.Clone() as Bitmap;
                BitmapData bmpdata = newmap.LockBits(rect, ImageLockMode.ReadWrite, newmap.PixelFormat);
                int bytes = bmpdata.Stride * bmpdata.Height;
                byte[] bdata = new byte[bytes];
                IntPtr ptr = bmpdata.Scan0;
                Marshal.Copy(ptr, bdata, 0, bytes);
                int dis = level / 2;
                var bdataEx = ExpandBorder(bdata, bmpdata.Width, bmpdata.Stride, dis);
                int newwid = 3 * (bmpdata.Width + dis*2), offset = bmpdata.Stride - 3 * bmpdata.Width, index = 0;
                double tsum = Methods.Sum2D(template);
                if (tsum == 0) tsum = 1;

                int l, k;
                for (int i = 0; i < bmpdata.Height; ++i)
                {
                    for (int j = 0; j < bmpdata.Width; ++j)
                    {
                        List<double> blist = new List<double>();
                        int tempid=0;
                        for(l=-dis;l<dis+1;++l)
                            for(k=-dis;k<dis+1;++k)
                            {
                                tempid=(i + dis + l) * newwid + (j + dis + k) * 3;
                                blist.Add(tp[l + dis, k + dis] * bdataEx[tempid]);
                            }
                        double tempp = blist.Sum() / tsum;
                        if (tempp > 255)
                            bdata[index] = (byte)255;
                        else if (tempp < 0)
                            bdata[index] = (byte)0;
                        else bdata[index] = (byte)tempp;
                        blist.Clear();

                        for (l = -dis; l < dis + 1; ++l)
                            for (k = -dis; k < dis + 1; ++k)
                            {
                                tempid = (i + dis + l) * newwid + (j + dis + k) * 3 + 1;
                                blist.Add(tp[l + dis, k + dis] * bdataEx[tempid]);
                            }
                        tempp = blist.Sum() / tsum;
                        if (tempp > 255)
                            bdata[index + 1] = (byte)255;
                        else if (tempp < 0)
                            bdata[index + 1] = (byte)0;
                        else bdata[index + 1] = (byte)tempp;
                        blist.Clear();

                        for (l = -dis; l < dis + 1; ++l)
                            for (k = -dis; k < dis + 1; ++k)
                            {
                                tempid = (i + dis + l) * newwid + (j + dis + k) * 3 + 2;
                                blist.Add(tp[l + dis, k + dis] * bdataEx[tempid]);
                            }

                        tempp = blist.Sum() / tsum;
                        if (tempp > 255)
                            bdata[index + 2] = (byte)255;
                        else if (tempp < 0)
                            bdata[index + 2] = (byte)0;
                        else bdata[index + 2] = (byte)tempp;
                        blist.Clear();
                        index += 3;
                    }
                    index += offset;
                }

                Marshal.Copy(bdata, 0, ptr, bytes);
                newmap.UnlockBits(bmpdata);
                return newmap;
            }

            /// <summary>
            /// 高斯滤波(1028*800,2.5s)
            /// </summary>
            /// <returns>高斯滤波后的图像</returns>
            public Bitmap GaussFilter()
            {
#region 注释掉
                //Bitmap newmap = bitmap.Clone() as Bitmap;
                //BitmapData bmpdata = newmap.LockBits(rect, ImageLockMode.ReadWrite, newmap.PixelFormat);
                //int bytes = bmpdata.Stride * bmpdata.Height;
                //byte[] bdata = new byte[bytes];
                //IntPtr ptr = bmpdata.Scan0;
                //Marshal.Copy(ptr, bdata, 0, bytes);
                //byte[] newbdata = new byte[bytes];
                //bdata.CopyTo(newbdata, 0);
                //int newwid = bmpdata.Stride, offset = bmpdata.Stride - 3 * bmpdata.Width;

                //int f1, f2, f3, c1, c2, c3, b1, b2, b3;
                //for (int i = 1; i < bmpdata.Height - 1; ++i)
                //{
                //    for (int j = 1; j < bmpdata.Width - 1; ++j)
                //    {
                //        List<int> blist = new List<int>();
                //        f1 = (i - 1) * newwid + j * 3 - 3;
                //        f2 = (i - 1) * newwid + j * 3;
                //        f3 = (i - 1) * newwid + j * 3 + 3;
                //        c1 = i * newwid + j * 3 - 3;
                //        c2 = i * newwid + j * 3;
                //        c3 = i * newwid + j * 3 + 3;
                //        b1 = (i + 1) * newwid + j * 3 - 3;
                //        b2 = (i + 1) * newwid + j * 3;
                //        b3 = (i + 1) * newwid + j * 3 + 3;
                //        blist.AddRange(new int[]{bdata[f1],2*bdata[f2],bdata[f3],
                //                                 2*bdata[c1],4*bdata[c2],2*bdata[c3],
                //                                  bdata[b1],2*bdata[b2],bdata[b3]});

                //        newbdata[c2] = blist.Sum() / 16 > 255 ? (byte)255 : (byte)(blist.Sum() / 16);
                //        blist.Clear();

                //        f1 = (i - 1) * newwid + j * 3 - 2;
                //        f2 = (i - 1) * newwid + j * 3 + 1;
                //        f3 = (i - 1) * newwid + j * 3 + 4;
                //        c1 = i * newwid + j * 3 - 2;
                //        c2 = i * newwid + j * 3 + 1;
                //        c3 = i * newwid + j * 3 + 4;
                //        b1 = (i + 1) * newwid + j * 3 - 2;
                //        b2 = (i + 1) * newwid + j * 3 + 1;
                //        b3 = (i + 1) * newwid + j * 3 + 4;
                //        blist.AddRange(new int[]{bdata[f1],2*bdata[f2],bdata[f3],
                //                                 2*bdata[c1],4*bdata[c2],2*bdata[c3],
                //                                  bdata[b1],2*bdata[b2],bdata[b3]});

                //        newbdata[c2] = blist.Sum() / 16 > 255 ? (byte)255 : (byte)(blist.Sum() / 16);
                //        blist.Clear();

                //        f1 = (i - 1) * newwid + j * 3 - 1;
                //        f2 = (i - 1) * newwid + j * 3 + 2;
                //        f3 = (i - 1) * newwid + j * 3 + 5;
                //        c1 = i * newwid + j * 3 - 1;
                //        c2 = i * newwid + j * 3 + 2;
                //        c3 = i * newwid + j * 3 + 5;
                //        b1 = (i + 1) * newwid + j * 3 - 1;
                //        b2 = (i + 1) * newwid + j * 3 + 2;
                //        b3 = (i + 1) * newwid + j * 3 + 5;
                //        blist.AddRange(new int[]{bdata[f1],2*bdata[f2],bdata[f3],
                //                                 2*bdata[c1],4*bdata[c2],2*bdata[c3],
                //                                  bdata[b1],2*bdata[b2],bdata[b3]});

                //        newbdata[c2] = blist.Sum() / 16 > 255 ? (byte)255 : (byte)(blist.Sum() / 16);
                //        blist.Clear();
                //    }
                //}

                //Marshal.Copy(newbdata, 0, ptr, bytes);
                //newmap.UnlockBits(bmpdata);
                //return newmap;
#endregion
                double[,] temp = { { 1, 2, 1 }, { 2, 4, 2 }, { 1, 2, 1 } };
                return TemplateConvolution(bitmap, temp,3);
            }

            /// <summary>
            /// 图像规格化
            /// </summary>
            /// <param name="M0">期望平均灰度值</param>
            /// <param name="Var_2">期望方差</param>
            /// <returns></returns>
            public Bitmap Normalize(byte M0,double Var_2)
            {
                Bitmap newmap = Scalize();
                BitmapData bmpdata = newmap.LockBits(rect, ImageLockMode.ReadWrite, newmap.PixelFormat);
                int bytes = bmpdata.Stride * bmpdata.Height;
                byte[] bdata = new byte[bytes];
                IntPtr ptr = bmpdata.Scan0;
                Marshal.Copy(ptr, bdata, 0, bytes);
                int index = 0, offset = bmpdata.Stride - 3 * bmpdata.Width;
                double m = 0, var1 = 0;
#region 计算平均灰度值
                for (int i = 0; i < bmpdata.Height; ++i)
                {
                    for (int j = 0; j < bmpdata.Width; ++j)
                    {
                        m += bdata[index];
                        index += 3;
                    }
                    index += offset;
                }
                m /= (bmpdata.Width * bmpdata.Height);//平均灰度值
#endregion
                
#region 计算方差
                index = 0;
                for (int i = 0; i < bmpdata.Height; ++i)
                {
                    for (int j = 0; j < bmpdata.Width; ++j)
                    {
                        var1 += (bdata[index] - m) * (bdata[index] - m);
                        index += 3;
                    }
                    index += offset;
                }
                var1 /= (bmpdata.Width * bmpdata.Height);
#endregion

#region 规定化
                index = 0;
                for (int i = 0; i < bmpdata.Height; ++i)
                {
                    for (int j = 0; j < bmpdata.Width; ++j)
                    {
                        if (bdata[index] > m)
                        {
                            int temp = (int)(M0 + Math.Sqrt(Var_2 * (bdata[index] - m) * (bdata[index] - m) / m) + 0.5);
                            bdata[index] =
                            bdata[index + 1] =
                            bdata[index + 2] = temp > 255 ? (byte)255 : (byte)temp;
                        }
                        else
                        {
                            int temp = (int)(M0 - Math.Sqrt(Var_2 * (bdata[index] - m) * (bdata[index] - m) / m) + 0.5);
                            bdata[index] =
                            bdata[index + 1] =
                            bdata[index + 2] = (byte)temp;
                        }
                        index += 3;
                    }
                    index += offset;
                }
#endregion
                

                Marshal.Copy(bdata, 0, ptr, bytes);
                newmap.UnlockBits(bmpdata);
                return newmap;
            }

            /// <summary>
            /// Gabor滤波（取8个方向的最大值）(1028*800,106s)
            /// </summary>
            /// <param name="f">频率,取0,1,2,3,默认为1</param>
            /// <returns></returns>
            public async Task<Bitmap> GaborFilter(double f=1)
            {
#region 对八个方向进行Gabor滤波(异步进行)
                List<ImageProcessing> imgprList = new List<ImageProcessing>();
                Bitmap[] bmpList = new Bitmap[8];
                List<Task> tasks = new List<Task>();
                for (int i = 0; i < 8; ++i)
                {
                    int temp = i;
                    imgprList.Add(new ImageProcessing(bitmap.Clone() as Bitmap));
                    tasks.Add(Task.Run(() => bmpList[temp] = imgprList[temp].SingleGaborFilter(temp * Math.PI / 8, f)));
                }
                await Task.WhenAll(tasks);
#endregion

#region 取灰度最大值
                List<BitmapData> bmpdataList = new List<BitmapData>();
                List<IntPtr> ptrList = new List<IntPtr>();
                List<byte[]> bdataList = new List<byte[]>();
                int bytes=0;
                for(int i=0;i <8;++i)
                {
                    bmpdataList.Add(bmpList[i].LockBits(rect, ImageLockMode.ReadOnly, bmpList[i].PixelFormat));
                    ptrList.Add(bmpdataList[i].Scan0);
                    bytes = bmpdataList[0].Stride * bmpdataList[0].Height;
                    bdataList.Add(new byte[bytes]);
                    Marshal.Copy(ptrList[i], bdataList[i], 0, bytes);
                }

                Bitmap newmap = bitmap.Clone() as Bitmap;
                BitmapData bmpdata = newmap.LockBits(rect, ImageLockMode.WriteOnly, newmap.PixelFormat);
                IntPtr ptr = bmpdata.Scan0;
                byte[] bdata = new byte[bytes];
                Marshal.Copy(ptr, bdata, 0, bytes);

                int index = 0,
                    offset = bmpdataList[0].Stride - 3 * bmpdataList[0].Width ,
                    hight=bmpdataList[0].Height,
                    width=bmpdataList[0].Width;
                List<byte> tempdata = new List<byte>();

                for (int i = 0; i < hight ; ++i)
                {
                    for (int j = 0; j < width ; ++j)
                    {
                        for (int k = 0; k < 8;++k)
                        {
                            tempdata.Add(bdataList[k][index]);
                        }
                        bdata[index] = bdata[index + 1] = bdata[index + 2] = tempdata.Max();
                        index += 3;
                        tempdata.Clear();
                    }
                    index += offset;
                }
#endregion
                for (int i = 0; i < 8; ++i)
                {
                    bmpList[i].UnlockBits(bmpdataList[i]);
                    bmpList[i].Dispose();
                }

                Marshal.Copy(bdata, 0, ptr, bytes);
                newmap.UnlockBits(bmpdata);

                return newmap;
            }

            /// <summary>
            /// gabor单向滤波
            /// </summary>
            /// <param name="direction">方向</param>
            /// <param name="f">频率</param>
            /// <returns></returns>
            public Bitmap SingleGaborFilter(double direction,double f)
            {
                Bitmap newmap = Scalize();
                BitmapData bmpdata = newmap.LockBits(rect, ImageLockMode.ReadWrite, newmap.PixelFormat);
                int bytes = bmpdata.Stride * bmpdata.Height;
                byte[] bdata = new byte[bytes];
                IntPtr ptr = bmpdata.Scan0;
                Marshal.Copy(ptr, bdata, 0, bytes);
                int index = 0, offset = bmpdata.Stride - 3 * bmpdata.Width;
                var bdataEx = ExpandBorder(bdata, bmpdata.Width, bmpdata.Stride, 5);
                int newwid = bmpdata.Width + 10;

                double cosd = Math.Cos(direction), sind = Math.Sin(direction);
                for (int i = 0; i < bmpdata.Height; ++i)
                {
                    for (int j = 0; j < bmpdata.Width; ++j)
                    {
                        double result=0;
                        for (int k = -5; k < 5;++k )
                        {
                            for(int l=-5;l<5;++l)
                            {
                                double plus = Math.Pow(k * cosd + l * sind, 2) / 9 + Math.Pow(-k * sind + l * cosd, 2);
                                result+=Math.Exp(-0.5*plus)*Math.Cos((k*cosd+l*sind)*f)*bdataEx[(i+5-k)*3*newwid+(j-l+5)*3];
                            }
                        }
                        if (result < 0) result = 0;
                        if (result > 255) result = 255;
                        bdata[index] = bdata[index+1] = bdata[index+2] = (byte)result;
                        index += 3;
                    }
                    index += offset;
                }

                Marshal.Copy(bdata, 0, ptr, bytes);
                newmap.UnlockBits(bmpdata);
                return newmap;
            }

            /// <summary>
            /// 扩展图像数组
            /// </summary>
            /// <param name="bsrc">原数组</param>
            /// <param name="width">图像宽度</param>
            /// <param name="stride">扫描行宽度</param>
            /// <param name="dis">扩展像素个数</param>
            /// <returns>扩展后数组(去除扫描行与图像宽度差异)</returns>
            private byte[]ExpandBorder(byte[] bsrc,int width,int stride,int dis)
            {
                int hight = bsrc.GetLength(0) / stride ,
                    offset=stride-3*width ,
                    index=0,
                    index1=0,
                    newwid=(width + 2*dis)*3;
                byte[] bdes = new byte[newwid*(hight+dis*2)];

                #region 前dis行扩展
                for (int i = 0; i < dis; ++i)
                {
                    index = 0;
                    for (int k = 0; k < dis; ++k)
                    {
                        bdes[index1] = bsrc[index];
                        bdes[index1 + 1] = bsrc[index + 1];
                        bdes[index1 + 2] = bsrc[index + 2];
                        index1 += 3;
                    }

                    for (int j = 0; j < width; ++j)
                    {
                        bdes[index1] = bsrc[index];
                        bdes[index1 + 1] = bsrc[index + 1];
                        bdes[index1 + 2] = bsrc[index + 2];
                        index += 3;
                        index1 += 3;
                    }

                    for (int k = 0; k < dis; ++k)
                    {
                        bdes[index1] = bsrc[index - 3];
                        bdes[index1 + 1] = bsrc[index - 2];
                        bdes[index1 + 2] = bsrc[index - 1];
                        index1 += 3;
                    }
                }
                #endregion

                #region 中间扩展
                index = 0;
                for (int i = 0; i < hight; ++i)
                {
                    for (int k = 0; k < dis; ++k)
                    {
                        bdes[index1] = bsrc[index];
                        bdes[index1 + 1] = bsrc[index + 1];
                        bdes[index1 + 2] = bsrc[index + 2];
                        index1 += 3;
                    }

                    for (int j = 0; j < width; ++j)
                    {
                        bdes[index1] = bsrc[index];
                        bdes[index1 + 1] = bsrc[index + 1];
                        bdes[index1 + 2] = bsrc[index + 2];
                        index += 3;
                        index1 += 3;
                    }

                    for (int k = 0; k < dis; ++k)
                    {
                        bdes[index1] = bsrc[index - 3];
                        bdes[index1 + 1] = bsrc[index - 2];
                        bdes[index1 + 2] = bsrc[index - 1];
                        index1 += 3;
                    }
                    index += offset;
                }
                #endregion

                #region 尾部扩展
                int newindex = index - stride;
                for (int i = 0; i < dis; ++i)
                {
                    index = newindex;
                    for (int k = 0; k < dis; ++k)
                    {
                        bdes[index1] = bsrc[index];
                        bdes[index1 + 1] = bsrc[index + 1];
                        bdes[index1 + 2] = bsrc[index + 2];
                        index1 += 3;
                    }

                    for (int j = 0; j < width; ++j)
                    {
                        bdes[index1] = bsrc[index];
                        bdes[index1 + 1] = bsrc[index + 1];
                        bdes[index1 + 2] = bsrc[index + 2];
                        index += 3;
                        index1 += 3;
                    }

                    for (int k = 0; k < dis; ++k)
                    {
                        bdes[index1] = bsrc[index - 3];
                        bdes[index1 + 1] = bsrc[index - 2];
                        bdes[index1 + 2] = bsrc[index - 1];
                        index1 += 3;
                    }
                }
                #endregion

                return bdes;
            }

            /// <summary>
            /// 图像四向各项异性扩散平滑(1028*800,4s)
            /// </summary>
            /// <param name="n">迭代次数</param>
            /// <param name="k">扩散系数参数</param>
            /// <param name="t">步长</param>
            /// <returns></returns>
            public Bitmap FourAnisotropic(int n=25,double k=0.08,double t=0.16)
            {
                Bitmap newmap = Scalize();
                BitmapData bmpdata = newmap.LockBits(rect, ImageLockMode.ReadWrite, newmap.PixelFormat);
                int bytes = bmpdata.Stride * bmpdata.Height;
                byte[] bdata = new byte[bytes];
                IntPtr ptr = bmpdata.Scan0;
                Marshal.Copy(ptr, bdata, 0, bytes);
                int index = 0, offset = bmpdata.Stride - 3 * bmpdata.Width;
                var bdataEx = ExpandBorder(bdata, bmpdata.Width, bmpdata.Stride, 2);
                int newwid = (bmpdata.Width + 4)*3;
                double[] c = new double[5];

                for (int l = 0; l < n;++l )
                {
                    index = 0;
                    for (int i = 0; i < bmpdata.Height; ++i)
                    {
                        for (int j = 0; j < bmpdata.Width; ++j)
                        {
                            //计算梯度时需要的索引
                            int ij=(i + 2) * newwid + (j + 2) * 3,
                                ij_1=(i + 2) * newwid + (j + 1) * 3,
                                ij_2=(i + 2) * newwid + j * 3,
                                ij1=(i + 2) * newwid + (j + 3) * 3,
                                i_1j=(i + 1) * newwid + (j + 2) * 3,
                                i_1j_1=(i + 1) * newwid + (j + 1) * 3,
                                i_1j1=(i + 1) * newwid + (j + 3) * 3,
                                i_2j=i * newwid + (j + 2) * 3,
                                i1j=(i + 3) * newwid + (j + 2) * 3,
                                i1j_1=(i + 3) * newwid + (j + 1) * 3;

                            c[0]=1/(1+k*((bdataEx[ij]-bdataEx[ij_1])*(bdataEx[ij]-bdataEx[ij_1])+
                                         (bdataEx[ij]-bdataEx[i_1j])*(bdataEx[ij]-bdataEx[i_1j])));
                            c[1]=1/(1+k*((bdataEx[i_1j]-bdataEx[i_1j_1])*(bdataEx[i_1j]-bdataEx[i_1j_1])+
                                         (bdataEx[i_1j]-bdataEx[i_2j])*(bdataEx[i_1j]-bdataEx[i_2j])));
                            c[2]=1/(1+k*((bdataEx[i1j]-bdataEx[i1j_1])*(bdataEx[i1j]-bdataEx[i1j_1])+
                                         (bdataEx[i1j]-bdataEx[ij])*(bdataEx[i1j]-bdataEx[ij])));
                            c[3]=1/(1+k*((bdataEx[ij_1]-bdataEx[ij_2])*(bdataEx[ij_1]-bdataEx[ij_2])+
                                         (bdataEx[ij_1]-bdataEx[i_1j_1])*(bdataEx[ij_1]-bdataEx[i_1j_1])));
                            c[4]=1/(1+k*((bdataEx[ij1]-bdataEx[ij])*(bdataEx[ij1]-bdataEx[ij])+
                                         (bdataEx[ij1]-bdataEx[i_1j1])*(bdataEx[ij1]-bdataEx[i_1j1])));
                            double result=bdataEx[ij]+t*((c[3]+c[0])*bdataEx[ij_1]/2+
                                                         (c[4]+c[0])*bdataEx[ij1]/2+
                                                         (c[2]+c[0])*bdataEx[i1j]/2+
                                                         (c[1]+c[0])*bdataEx[i_1j]/2-
                                                         (c[0]*4+c[1]+c[2]+c[3]+c[4])*bdataEx[ij]/2);
                            bdata[index] = bdata[index + 1] = bdata[index + 2] = (byte)(result+0.5);
                            index += 3;
                        }
                        index += offset;
                    }
                }
                

                Marshal.Copy(bdata, 0, ptr, bytes);
                newmap.UnlockBits(bmpdata);
                return newmap;
            }

            /// <summary>
            /// 图像八向各项异性扩散平滑(1028*800,8s)
            /// </summary>
            /// <param name="n">迭代次数</param>
            /// <param name="k">扩散系数参数</param>
            /// <param name="t">步长</param>
            /// <returns></returns>
            public Bitmap EightAnisotropic(int n = 25, double k = 0.08, double t = 0.16)
            {
                Bitmap newmap = Scalize();
                BitmapData bmpdata = newmap.LockBits(rect, ImageLockMode.ReadWrite, newmap.PixelFormat);
                int bytes = bmpdata.Stride * bmpdata.Height;
                byte[] bdata = new byte[bytes];
                IntPtr ptr = bmpdata.Scan0;
                Marshal.Copy(ptr, bdata, 0, bytes);
                int index = 0, offset = bmpdata.Stride - 3 * bmpdata.Width;
                var bdataEx = ExpandBorder(bdata, bmpdata.Width, bmpdata.Stride, 2);
                int newwid = (bmpdata.Width + 4)*3;
                double[] c = new double[9];

                for (int l = 0; l < n; ++l)
                {
                    index = 0;
                    for (int i = 0; i < bmpdata.Height; ++i)
                    {
                        for (int j = 0; j < bmpdata.Width; ++j)
                        {
                            //计算梯度时需要的索引(可改进)
                            int ij = (i + 2) * newwid + (j + 2) * 3,
                                ij_1 = (i + 2) * newwid + (j + 1) * 3,
                                ij_2 = (i + 2) * newwid + j * 3,
                                ij1 = (i + 2) * newwid + (j + 3) * 3,
                                i_1j = (i + 1) * newwid + (j + 2) * 3,
                                i_1j_1 = (i + 1) * newwid + (j + 1) * 3,
                                i_1j1 = (i + 1) * newwid + (j + 3) * 3,
                                i_2j = i * newwid + (j + 2) * 3,
                                i1j = (i + 3) * newwid + (j + 2) * 3,
                                i1j_1 = (i + 3) * newwid + (j + 1) * 3,
                                i_2j_1 = i * newwid + (j + 1) * 3,
                                i_2j1 = i * newwid + (j + 3) * 3,
                                i_1j_2 = (i + 1) * newwid + j * 3,
                                i1j_2 = (i + 3) * newwid + j * 3,
                                i1j1 = (i + 3) * newwid + (j + 3) * 3;

                            c[0] = 1 / (1 + k * ((bdataEx[ij] - bdataEx[ij_1]) * (bdataEx[ij] - bdataEx[ij_1]) +
                                         (bdataEx[ij] - bdataEx[i_1j]) * (bdataEx[ij] - bdataEx[i_1j])));//i,j
                            c[1] = 1 / (1 + k * ((bdataEx[i_1j] - bdataEx[i_1j_1]) * (bdataEx[i_1j] - bdataEx[i_1j_1]) +
                                         (bdataEx[i_1j] - bdataEx[i_2j]) * (bdataEx[i_1j] - bdataEx[i_2j])));//i-1,j
                            c[2] = 1 / (1 + k * ((bdataEx[i1j] - bdataEx[i1j_1]) * (bdataEx[i1j] - bdataEx[i1j_1]) +
                                         (bdataEx[i1j] - bdataEx[ij]) * (bdataEx[i1j] - bdataEx[ij])));//i+1,j
                            c[3] = 1 / (1 + k * ((bdataEx[ij_1] - bdataEx[ij_2]) * (bdataEx[ij_1] - bdataEx[ij_2]) +
                                         (bdataEx[ij_1] - bdataEx[i_1j_1]) * (bdataEx[ij_1] - bdataEx[i_1j_1])));//i,j-1
                            c[4] = 1 / (1 + k * ((bdataEx[ij1] - bdataEx[ij]) * (bdataEx[ij1] - bdataEx[ij]) +
                                         (bdataEx[ij1] - bdataEx[i_1j1]) * (bdataEx[ij1] - bdataEx[i_1j1])));//i,j+1
                            c[5] = 1 / (1 + k * ((bdataEx[i_1j_1] - bdataEx[i_1j_2]) * (bdataEx[i_1j_1] - bdataEx[i_1j_2]) +
                                        (bdataEx[i_1j_1] - bdataEx[i_2j_1]) * (bdataEx[i_1j_1] - bdataEx[i_2j_1])));//i-1,j-1
                            c[6] = 1 / (1 + k * ((bdataEx[i_1j1] - bdataEx[i_1j]) * (bdataEx[i_1j1] - bdataEx[i_1j]) +
                                        (bdataEx[i_1j1] - bdataEx[i_2j1]) * (bdataEx[i_1j1] - bdataEx[i_2j1])));//i-1,j+1
                            c[7] = 1 / (1 + k * ((bdataEx[i1j_1] - bdataEx[i1j_2]) * (bdataEx[i1j_1] - bdataEx[i1j_2]) +
                                         (bdataEx[i1j_1] - bdataEx[ij_1]) * (bdataEx[i1j_1] - bdataEx[ij_1])));//i+1,j-1
                            c[8] = 1 / (1 + k * ((bdataEx[i1j1] - bdataEx[i1j]) * (bdataEx[i1j1] - bdataEx[i1j]) +
                                         (bdataEx[i1j1] - bdataEx[ij1]) * (bdataEx[i1j1] - bdataEx[ij1])));//i+1,j+1

                            double result = bdataEx[ij] + t * ((c[3] + c[0]) * bdataEx[ij_1] / 2 +
                                                         (c[4] + c[0]) * bdataEx[ij1] / 2 +
                                                         (c[2] + c[0]) * bdataEx[i1j] / 2 +
                                                         (c[1] + c[0]) * bdataEx[i_1j] / 2 +
                                                         (c[5] + c[0]) * bdataEx[i_1j_1] / 2 +
                                                         (c[6] + c[0]) * bdataEx[i_1j1] / 2 +
                                                         (c[7] + c[0]) * bdataEx[i1j_1] / 2 +
                                                         (c[8] + c[0]) * bdataEx[i1j1] / 2  -
                                                         (c[0] * 8 + c[1] + c[2] + c[3] + c[4]+c[5]+c[6]+c[7]+c[8]) * bdataEx[ij] / 2);
                            bdata[index] = bdata[index + 1] = bdata[index + 2] = (byte)(result + 0.5);
                            index += 3;
                        }
                        index += offset;
                    }
                }


                Marshal.Copy(bdata, 0, ptr, bytes);
                newmap.UnlockBits(bmpdata);
                return newmap;
            }

            /// <summary>
            /// 二值图像腐蚀(四个方向)(白色背景)
            /// </summary>
            /// <returns></returns>
            public Bitmap Erode()
            {
                var tempmap = TwoValue(GetBestTresh());
                BitmapData bmpdata = tempmap.LockBits(rect, ImageLockMode.ReadWrite, tempmap.PixelFormat);
                int bytes = bmpdata.Stride * bmpdata.Height;
                byte[] bdata = new byte[bytes];
                IntPtr ptr = bmpdata.Scan0;
                Marshal.Copy(ptr, bdata, 0, bytes);
                int index = 0, offset = bmpdata.Stride - 3 * bmpdata.Width;
                var bdataEx = ExpandBorder(bdata, bmpdata.Width, bmpdata.Stride, 1);
                int newwid = (bmpdata.Width + 2)*3;

                for (int i = 0; i < bmpdata.Height; ++i)
                {
                    for (int j = 0; j < bmpdata.Width; ++j)
                    {
                        int ij=(i+1)*newwid+(j+1)*3,
                            i_1j=i*newwid+(j+1)*3,
                            ij_1=(i+1)*newwid+j*3,
                            ij1=(i+1)*newwid+(j+2)*3,
                            i1j=(i+2)*newwid+(j+1)*3;
                        if (bdataEx[ij] == 0)
                            if (bdataEx[i_1j] == 255 ||
                                bdataEx[ij_1] == 255 ||
                                bdataEx[ij1] == 255 ||
                                bdataEx[i1j] == 255)
                                bdata[index]=bdata[index+1]=bdata[index+2] = 255;
                        index += 3;
                    }
                    index += offset;
                }

                Marshal.Copy(bdata, 0, ptr, bytes);
                tempmap.UnlockBits(bmpdata);
                return tempmap;

            }

            /// <summary>
            /// 二值图像膨胀(四个方向)(白色背景)
            /// </summary>
            /// <returns></returns>
            public Bitmap Dilate()
            {
                var tempmap = TwoValue(GetBestTresh());
                BitmapData bmpdata = tempmap.LockBits(rect, ImageLockMode.ReadWrite, tempmap.PixelFormat);
                int bytes = bmpdata.Stride * bmpdata.Height;
                byte[] bdata = new byte[bytes];
                IntPtr ptr = bmpdata.Scan0;
                Marshal.Copy(ptr, bdata, 0, bytes);
                int index = 0, offset = bmpdata.Stride - 3 * bmpdata.Width;
                var bdataEx = ExpandBorder(bdata, bmpdata.Width, bmpdata.Stride, 1);
                int newwid = (bmpdata.Width + 2)*3;

                for (int i = 0; i < bmpdata.Height; ++i)
                {
                    for (int j = 0; j < bmpdata.Width; ++j)
                    {
                        int ij=(i+1)*newwid+(j+1)*3,
                            i_1j=i*newwid+(j+1)*3,
                            ij_1=(i+1)*newwid+j*3,
                            ij1=(i+1)*newwid+(j+2)*3,
                            i1j=(i+2)*newwid+(j+1)*3;
                        if (bdataEx[ij] == 255)
                            if (bdataEx[i_1j] == 0 ||
                                bdataEx[ij_1] == 0 ||
                                bdataEx[ij1] == 0 ||
                                bdataEx[i1j] == 0)
                                bdata[index]=bdata[index+1]=bdata[index+2] = 0;
                        index += 3;
                    }
                    index += offset;
                }

                Marshal.Copy(bdata, 0, ptr, bytes);
                tempmap.UnlockBits(bmpdata);
                return tempmap;
            }

            /// <summary>
            /// 灰度图像腐蚀(5*5)(白色背景)(1028*800,1.5s)
            /// </summary>
            /// <returns></returns>
            public Bitmap GrayErode()
            {
                var tempmap =Scalize();
                BitmapData bmpdata = tempmap.LockBits(rect, ImageLockMode.ReadWrite, tempmap.PixelFormat);
                int bytes = bmpdata.Stride * bmpdata.Height;
                byte[] bdata = new byte[bytes];
                IntPtr ptr = bmpdata.Scan0;
                Marshal.Copy(ptr, bdata, 0, bytes);
                int index = 0, offset = bmpdata.Stride - 3 * bmpdata.Width;
                var bdataEx = ExpandBorder(bdata, bmpdata.Width, bmpdata.Stride, 2);
                int newwid = (bmpdata.Width + 4)*3;
                List<byte> tempbList = new List<byte>();

                for (int i = 0; i < bmpdata.Height; ++i)
                {
                    for (int j = 0; j < bmpdata.Width; ++j)
                    {
                        int tempid = 0,dis=2,l,k;
                        for (l = -dis; l < dis + 1; ++l)
                            for (k = -dis; k < dis + 1; ++k)
                            {
                                tempid = (i + dis + l) * newwid + (j + dis + k) * 3;
                                tempbList.Add(bdataEx[tempid]);
                            }
                        bdata[index] = bdata[index + 1] = bdata[index + 2] = tempbList.Max();
                        tempbList.Clear();
                        index += 3;
                    }
                    index += offset;
                }

                Marshal.Copy(bdata, 0, ptr, bytes);
                tempmap.UnlockBits(bmpdata);
                return tempmap;
            }

            /// <summary>
            /// 灰度图像膨胀(5*5)(白色背景)(1028*800,1.5s)
            /// </summary>
            /// <returns></returns>
            public Bitmap GrayDilate()
            {
                var tempmap = Scalize();
                BitmapData bmpdata = tempmap.LockBits(rect, ImageLockMode.ReadWrite, tempmap.PixelFormat);
                int bytes = bmpdata.Stride * bmpdata.Height;
                byte[] bdata = new byte[bytes];
                IntPtr ptr = bmpdata.Scan0;
                Marshal.Copy(ptr, bdata, 0, bytes);
                int index = 0, offset = bmpdata.Stride - 3 * bmpdata.Width;
                var bdataEx = ExpandBorder(bdata, bmpdata.Width, bmpdata.Stride, 2);
                int newwid = (bmpdata.Width + 4)*3;
                List<byte> tempbList = new List<byte>();

                for (int i = 0; i < bmpdata.Height; ++i)
                {
                    for (int j = 0; j < bmpdata.Width; ++j)
                    {
                        int tempid = 0, dis = 2, l, k;
                        for (l = -dis; l < dis + 1; ++l)
                            for (k = -dis; k < dis + 1; ++k)
                            {
                                tempid = (i + dis + l) * newwid + (j + dis + k) * 3;
                                tempbList.Add(bdataEx[tempid]);
                            }
                        bdata[index] = bdata[index + 1] = bdata[index + 2] = tempbList.Min();
                        tempbList.Clear();
                        index += 3;
                    }
                    index += offset;
                }

                Marshal.Copy(bdata, 0, ptr, bytes);
                tempmap.UnlockBits(bmpdata);
                return tempmap;
            }

            /// <summary>
            /// Sobel边缘检测(1028*800,3s)
            /// </summary>
            /// <returns></returns>
            public async Task<Bitmap> Sobel()
            {
                double[,] tp1 = { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
                double[,] tp2 = { { 1, 2, 1 }, { 0, 0, 0 }, { -1, -2, -1 } };
                Bitmap newmap=null, tempmap=null,map1=bitmap.Clone() as Bitmap,map2=bitmap.Clone() as Bitmap;
                Task task1=Task.Run(()=> newmap=TemplateConvolution(map1, tp1, 3)),
                     task2=Task.Run(()=>tempmap = TemplateConvolution(map2, tp2, 3));
                await Task.WhenAll(task1, task2);
                BitmapData bmpdata = newmap.LockBits(rect, ImageLockMode.ReadWrite, newmap.PixelFormat),
                           tempbmpdata = tempmap.LockBits(rect, ImageLockMode.ReadWrite, tempmap.PixelFormat);
                int bytes = bmpdata.Stride * bmpdata.Height;
                byte[] bdata = new byte[bytes];
                byte[]tempbdata=new byte[bytes];
                IntPtr ptr = bmpdata.Scan0,
                       tempptr = tempbmpdata.Scan0;
                Marshal.Copy(ptr, bdata, 0, bytes);
                Marshal.Copy(tempptr, tempbdata, 0, bytes);
                int index = 0, offset = bmpdata.Stride - 3 * bmpdata.Width;

                for (int i = 0; i < bmpdata.Height; ++i)
                {
                    for (int j = 0; j < bmpdata.Width; ++j)
                    {
                        if (bdata[index] < tempbdata[index]) bdata[index] = tempbdata[index];
                        if (bdata[index+1] < tempbdata[index+1]) bdata[index+1] = tempbdata[index+1];
                        if (bdata[index+2] < tempbdata[index+2]) bdata[index+2] = tempbdata[index+2];
                        index += 3;
                    }
                    index += offset;
                }

                Marshal.Copy(bdata, 0, ptr, bytes);
                tempmap.UnlockBits(tempbmpdata);
                tempmap.Dispose();
                newmap.UnlockBits(bmpdata);
                return newmap;
            }

            /// <summary>
            /// 水平Sobel增强(1028*800,3s)
            /// </summary>
            /// <returns></returns>
            public Bitmap HSobel()
            {
                double[,] tp = { { 1, 2, 1 }, { 0, 0, 0 }, { -1, -2, -1 } };
                return TemplateConvolution(bitmap, tp,3);
            }

            /// <summary>
            /// 垂直Sobel增强(1028*800,3s)
            /// </summary>
            /// <returns></returns>
            public Bitmap VSobel()
            {
                double[,] tp = { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
                return TemplateConvolution(bitmap, tp, 3);
            }

            /// <summary>
            /// Prewitt边缘检测(1028*800,3s)
            /// </summary>
            /// <returns></returns>
            public async Task<Bitmap> Prewitt()
            {
                double[,] tp1 = { { -1, 0, 1 }, { -1, 0, 1 }, { -1, 0, 1 } };
                double[,] tp2 = { { 1, 1, 1 }, { 0, 0, 0 }, { -1, -1, -1 } };
                Bitmap newmap = null, tempmap = null, map1 = bitmap.Clone() as Bitmap, map2 = bitmap.Clone() as Bitmap;
                Task task1 = Task.Run(() => newmap = TemplateConvolution(map1, tp1, 3)),
                     task2 = Task.Run(() => tempmap = TemplateConvolution(map2, tp2, 3));
                await Task.WhenAll(task1, task2);
                BitmapData bmpdata = newmap.LockBits(rect, ImageLockMode.ReadWrite, newmap.PixelFormat),
                           tempbmpdata = tempmap.LockBits(rect, ImageLockMode.ReadWrite, tempmap.PixelFormat);
                int bytes = bmpdata.Stride * bmpdata.Height;
                byte[] bdata = new byte[bytes];
                byte[] tempbdata = new byte[bytes];
                IntPtr ptr = bmpdata.Scan0,
                       tempptr = tempbmpdata.Scan0;
                Marshal.Copy(ptr, bdata, 0, bytes);
                Marshal.Copy(tempptr, tempbdata, 0, bytes);
                int index = 0, offset = bmpdata.Stride - 3 * bmpdata.Width;

                for (int i = 0; i < bmpdata.Height; ++i)
                {
                    for (int j = 0; j < bmpdata.Width; ++j)
                    {
                        if (bdata[index] < tempbdata[index]) bdata[index] = tempbdata[index];
                        if (bdata[index + 1] < tempbdata[index + 1]) bdata[index + 1] = tempbdata[index + 1];
                        if (bdata[index + 2] < tempbdata[index + 2]) bdata[index + 2] = tempbdata[index + 2];
                        index += 3;
                    }
                    index += offset;
                }

                Marshal.Copy(bdata, 0, ptr, bytes);
                tempmap.UnlockBits(tempbmpdata);
                tempmap.Dispose();
                newmap.UnlockBits(bmpdata);
                return newmap;
            }

            /// <summary>
            /// Robert边缘检测(1028*800,3s)
            /// </summary>
            /// <returns></returns>
            public async Task<Bitmap> Robert()
            {
                double[,] tp1 = { { 1, 0 }, { 0, -1}};
                double[,] tp2 = { { 0, 1 }, { -1, 0 }};
                Bitmap newmap = null, tempmap = null, map1 = bitmap.Clone() as Bitmap, map2 = bitmap.Clone() as Bitmap;
                Task task1 = Task.Run(() => newmap = TemplateConvolution2(map1, tp1)),
                     task2 = Task.Run(() => tempmap = TemplateConvolution2(map2, tp2));
                await Task.WhenAll(task1, task2);
                BitmapData bmpdata = newmap.LockBits(rect, ImageLockMode.ReadWrite, newmap.PixelFormat),
                           tempbmpdata = tempmap.LockBits(rect, ImageLockMode.ReadWrite, tempmap.PixelFormat);
                int bytes = bmpdata.Stride * bmpdata.Height;
                byte[] bdata = new byte[bytes];
                byte[] tempbdata = new byte[bytes];
                IntPtr ptr = bmpdata.Scan0,
                       tempptr = tempbmpdata.Scan0;
                Marshal.Copy(ptr, bdata, 0, bytes);
                Marshal.Copy(tempptr, tempbdata, 0, bytes);
                int index = 0, offset = bmpdata.Stride - 3 * bmpdata.Width;

                for (int i = 0; i < bmpdata.Height; ++i)
                {
                    for (int j = 0; j < bmpdata.Width; ++j)
                    {
                        if (bdata[index] < tempbdata[index]) bdata[index] = tempbdata[index];
                        if (bdata[index + 1] < tempbdata[index + 1]) bdata[index + 1] = tempbdata[index + 1];
                        if (bdata[index + 2] < tempbdata[index + 2]) bdata[index + 2] = tempbdata[index + 2];
                        index += 3;
                    }
                    index += offset;
                }

                Marshal.Copy(bdata, 0, ptr, bytes);
                tempmap.UnlockBits(tempbmpdata);
                tempmap.Dispose();
                newmap.UnlockBits(bmpdata);
                return newmap;
            }

            /// <summary>
            /// Kirsch边缘检测(1028*800,10s)
            /// </summary>
            /// <returns></returns>
            public async Task<Bitmap> Kirsch()
            {
                double[,] tp1 = { { 5, 5, 5 }, { -3, 0, -3 }, { -3, -3, -3 } };
                double[,] tp2 = { { -3, 5, 5 }, {-3, 0, 5 }, { -3, -3, -3 } };
                double[,] tp3 = { { -3,-3, 5 }, {-3, 0, 5 }, { -3, -3, 5 } };
                double[,] tp4 = { { -1, -1, -1 }, { 0, 0, 0 }, { 1, 1, 1 } };
                double[,] tp5 = { { -3, -3, -3 }, { -3, 0, -3 }, { 5, 5, 5 } };
                double[,] tp6 = { { -3, -3, -3 }, { 5, 0, -3 }, { 5, 5, -3 } };
                double[,] tp7 = { { 5, -3, -3 }, { 5, 0, -3 }, { 5, -3, -3 } };
                double[,] tp8 = { { 5, 5, -3 }, { 5, 0, -3 }, { -3, -3, -3 } };
                List<double[,]> tps = new List<double[,]> { tp1, tp2, tp3, tp4, tp5, tp6, tp7, tp8 };
                List<Bitmap> tempmapList = new List<Bitmap>();
                List<Bitmap> mapList = new List<Bitmap>{  bitmap.Clone() as Bitmap,
                                                          bitmap.Clone() as Bitmap,
                                                          bitmap.Clone() as Bitmap,
                                                          bitmap.Clone() as Bitmap,
                                                          bitmap.Clone() as Bitmap,
                                                          bitmap.Clone() as Bitmap,
                                                          bitmap.Clone() as Bitmap,
                                                          bitmap.Clone() as Bitmap};
                List<Task> tasks = new List<Task>();
                Bitmap newmap = bitmap.Clone()as  Bitmap;
                for (int i = 0; i < 8;++i )
                {
                    int tempi=i;
                    tasks.Add(Task.Run(() => tempmapList.Add(TemplateConvolution(mapList[tempi], tps[tempi], 3))));
                }
                await Task.WhenAll(tasks);
                BitmapData bmpdata = newmap.LockBits(rect, ImageLockMode.ReadWrite, newmap.PixelFormat);
                int bytes = bmpdata.Stride * bmpdata.Height;
                byte[] bdata = new byte[bytes];
                IntPtr ptr = bmpdata.Scan0;
                Marshal.Copy(ptr, bdata, 0, bytes);

                List<BitmapData> tempbmpdataList = new List<BitmapData>();
                List<IntPtr> ptrList = new List<IntPtr>();
                List<byte[]> tempbdataList = new List<byte[]>{new byte[bytes],new byte[bytes],
                                                              new byte[bytes],new byte[bytes],
                                                              new byte[bytes],new byte[bytes],
                                                              new byte[bytes],new byte[bytes]};
                for (int i = 0; i < 8;++i )
                {
                    tempbmpdataList.Add(tempmapList[i].LockBits(rect, ImageLockMode.ReadOnly, tempmapList[i].PixelFormat));
                    ptrList.Add(tempbmpdataList[i].Scan0);
                    Marshal.Copy(ptrList[i],tempbdataList[i],0,bytes);
                }

                int index = 0, offset = bmpdata.Stride - 3 * bmpdata.Width;
                List<byte> tempRList = new List<byte>();
                List<byte> tempGList = new List<byte>();
                List<byte> tempBList = new List<byte>();
                for (int i = 0; i < bmpdata.Height; ++i)
                {
                    for (int j = 0; j < bmpdata.Width; ++j)
                    {
                        for (int k = 0; k < 8;++k )
                        {
                            tempBList.Add(tempbdataList[k][index]);
                            tempGList.Add(tempbdataList[k][index+1]);
                            tempRList.Add(tempbdataList[k][index+2]);
                        }
                        bdata[index] = tempBList.Max();
                        bdata[index+1] = tempGList.Max();
                        bdata[index+2] = tempRList.Max();
                        tempBList.Clear();
                        tempGList.Clear();
                        tempRList.Clear();
                        index += 3;
                    }
                    index += offset;
                }

                Marshal.Copy(bdata, 0, ptr, bytes);
                for (int i = 0; i < 8;++i )
                {
                    tempmapList[i].UnlockBits(tempbmpdataList[i]);
                    tempmapList[i].Dispose();
                }
                newmap.UnlockBits(bmpdata);
                return newmap;
            }

            /// <summary>
            /// Laplace滤波器1(1028*800,3s)
            /// 0  -1  0
            /// -1  4 -1
            /// 0  -1  0
            /// </summary>
            /// <returns></returns>
            public Bitmap LaplaceFilter1()
            {
                double[,] tp = { { 0, -1, 0 }, { -1, 4, -1 }, { 0, -1, 0 } };
                return TemplateConvolution(bitmap, tp, 3);
            }

            /// <summary>
            /// Laplace滤波器2(1028*800,3s)
            /// -1  -1 -1
            /// -1   8 -1
            /// -1  -1 -1
            /// </summary>
            /// <returns></returns>
            public Bitmap LaplaceFilter2()
            {
                double[,] tp = { { -1, -1, -1 }, { -1, 8, -1 }, { -1, -1, -1 } };
                return TemplateConvolution(bitmap, tp, 3);
            }

            /// <summary>
            /// Gauss-Laplace滤波器1
            ///  0  0 -1  0  0
            ///  0 -1 -2 -1  0
            /// -1 -2 16 -2 -1
            ///  0 -1 -2 -1  0
            ///  0  0 -1  0  0
            /// </summary>
            /// <returns></returns>
            public Bitmap GaussLaplace1()
            {
                double[,] tp ={{0,0,-1,0,0},
                               {0,-1,-2,-1,0},
                               {-1,-2,16,-2,-1},
                               {0,-1,-2,-1,0},
                               {0,0,-1,0,0}};
                return TemplateConvolution(bitmap, tp, 5);
            }

            /// <summary>
            /// Gauss-Laplace滤波器1
            /// -2 -4 -4 -4 -2
            /// -4  0  8  0 -4
            /// -4  8 16  8 -4
            /// -4  0  8  0 -4
            /// -2 -4 -4 -4 -2
            /// </summary>
            /// <returns></returns>
            public Bitmap GaussLaplace2()
            {
                double[,] tp ={{-2,-4,-4,-4,-2},
                               {-4,0,8,0,-4},
                               {-4,8,16,8,-4},
                               {-4,0,8,0,-4},
                               {-2,-4,-4,-4,-2}};
                return TemplateConvolution(bitmap, tp, 5);
            }

            /*Canny检测
             * 暂未写好
             * public Bitmap Canny()
            {
                Bitmap newmap = bitmap.Clone() as Bitmap;
                BitmapData bmpdata = newmap.LockBits(rect, ImageLockMode.ReadWrite, newmap.PixelFormat);
                int bytes = bmpdata.Stride * bmpdata.Height;
                byte[] bdata = new byte[bytes];
                IntPtr ptr = bmpdata.Scan0;
                Marshal.Copy(ptr, bdata, 0, bytes);
                var bdataEx = ExpandBorder(bdata, bmpdata.Width, bmpdata.Stride, 2);
                int newwid = 3 * (bmpdata.Width + 4), offset = bmpdata.Stride - 3 * bmpdata.Width, index = 0;

                int l, k;
                for (int i = 0; i < bmpdata.Height; ++i)
                {
                    for (int j = 0; j < bmpdata.Width; ++j)
                    {
                        List<double> blist = new List<double>();
                        int tempid = 0;
                        for (l = -dis; l < dis + 1; ++l)
                            for (k = -dis; k < dis + 1; ++k)
                            {
                                tempid = (i + dis + l) * newwid + (j + dis + k) * 3;
                                blist.Add(tp[l + dis, k + dis] * bdataEx[tempid]);
                            }
                        double tempp = blist.Sum() / tsum;
                        if (tempp > 255)
                            bdata[index] = (byte)255;
                        else if (tempp < 0)
                            bdata[index] = (byte)0;
                        else bdata[index] = (byte)tempp;
                        blist.Clear();

                        for (l = -dis; l < dis + 1; ++l)
                            for (k = -dis; k < dis + 1; ++k)
                            {
                                tempid = (i + dis + l) * newwid + (j + dis + k) * 3 + 1;
                                blist.Add(tp[l + dis, k + dis] * bdataEx[tempid]);
                            }
                        tempp = blist.Sum() / tsum;
                        if (tempp > 255)
                            bdata[index + 1] = (byte)255;
                        else if (tempp < 0)
                            bdata[index + 1] = (byte)0;
                        else bdata[index + 1] = (byte)tempp;
                        blist.Clear();

                        for (l = -dis; l < dis + 1; ++l)
                            for (k = -dis; k < dis + 1; ++k)
                            {
                                tempid = (i + dis + l) * newwid + (j + dis + k) * 3 + 2;
                                blist.Add(tp[l + dis, k + dis] * bdataEx[tempid]);
                            }

                        tempp = blist.Sum() / tsum;
                        if (tempp > 255)
                            bdata[index + 2] = (byte)255;
                        else if (tempp < 0)
                            bdata[index + 2] = (byte)0;
                        else bdata[index + 2] = (byte)tempp;
                        blist.Clear();
                        index += 3;
                    }
                    index += offset;
                }

                Marshal.Copy(bdata, 0, ptr, bytes);
                newmap.UnlockBits(bmpdata);
                return newmap;
            }*/

            /// <summary>
            /// Marr边缘检测
            /// </summary>
            /// <returns></returns>
            public Bitmap Marr()
            {
                double[,] tp ={{-2,-4,-4,-4,-2},
                               {-4,0,8,0,-4},
                               {-4,8,24,8,-4},
                               {-4,0,8,0,-4},
                               {-2,-4,-4,-4,-2}};
                return TemplateConvolution(bitmap, tp, 5);
            }

            /// <summary>
            /// 线性变换
            /// </summary>
            /// <returns></returns>
            public Bitmap LineTranse()
            {
                return new Bitmap(bitmap.Width,bitmap.Height);
            }


            /// <summary>
            /// 提供原图像的副本
            /// </summary>
            /// <returns></returns>
            public Bitmap Clone()
            {
                return bitmap.Clone() as Bitmap;
            }
        }
         
    }

}
