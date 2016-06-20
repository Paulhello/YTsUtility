using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace YTsUtility
{
    namespace ImageProcess
    {
        /// <summary>
        /// 模板识别类
        /// </summary>
        public class TemplateFormer
        {
            private Bitmap bmp;
            private Rectangle lrtbRect;
            /// <summary>
            /// 需要识别的图像
            /// </summary>
            public Bitmap bitmap
            {
                set
                {
                    bmp = value;
                    rect = new Rectangle(new Point(), bmp.Size);
                }
                get { return bmp; }
            }

            private Pattern[] records = new Pattern[10];
            private Rectangle rect;

            /// <summary>
            /// 提取有效区域
            /// </summary>
            /// <returns></returns>
            public Bitmap Get_Most_LRTB()
            {
                int left = 0, right = 0, top = 0, bottom = 0;
                Color col;
                //BitmapData bmpdata = bitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                //int bytes = bmpdata.Stride * bmpdata.Height , index=0;
                //byte[] bmpbytes = new byte[bytes];
                //IntPtr ptr = bmpdata.Scan0;
                //Marshal.Copy(ptr, bmpbytes, 0, bytes);
                #region 找到最左边的点
                for (int i = 0; i < bitmap.Width; ++i)
                {
                    for (int j = 0; j < bitmap.Height; ++j)
                    {
              
                        //index = (i * bmpdata.Height + j) * 3;
                        //if (bmpbytes[index] != 0 || bmpbytes[index + 1] != 0 || bmpbytes[index + 2] != 0)
                        //    continue;
                        col=bmp.GetPixel(i,j);
                        if (col.R!=0||col.G!=0||col.B!=0)
                        {
                            continue;
                        }
                        else
                        {
                            left = i;
                            goto label1;
                        }
                    }
                }
                #endregion

                #region 找到最右边的点
                label1:
                for (int i = bitmap.Width - 1; i >= 0; --i)
                {
                    for (int j = 0; j < bitmap.Height; ++j)
                    {
                        //index = (i * bmpdata.Height + j) * 3;
                        //if (bmpbytes[index] != 0 || bmpbytes[index + 1] != 0 || bmpbytes[index + 2] != 0)
                        //    continue;
                        col = bmp.GetPixel(i, j);
                        if (col.R != 0 || col.G != 0 || col.B != 0)
                        {
                            continue;
                        }
                        else
                        {
                            right = i;
                            goto label2;
                        }
                    }
                }
                #endregion

                #region 找到最上面的点
                label2:
                for (int i = 0; i < bitmap.Height; ++i)
                {
                    for (int j = 0; j < bitmap.Width; ++j)
                    {
                    //    index = i * bmpdata.Stride + j * 3;
                    //    if (bmpbytes[index] != 0 || bmpbytes[index + 1] != 0 || bmpbytes[index + 2] != 0)
                    //        continue;
                        col=bmp.GetPixel(j,i);
                        if (col.R!=0||col.G!=0||col.B!=0)
                        {
                            continue;
                        }
                        else
                        {
                            top = i;
                            goto label3;
                        }
                    }
                }
                #endregion

                #region 找到最下面的点
                label3:
                for (int i = bitmap.Height - 1; i >= 0; --i)
                {
                    for (int j = 0; j < bitmap.Width; ++j)
                    {
                        //index = i * bmpdata.Stride + j * 3;
                        //if (bmpbytes[index] != 0 || bmpbytes[index + 1] != 0 || bmpbytes[index + 2] != 0)
                        //    continue;
                        col=bmp.GetPixel(j,i);
                        if (col.R!=0||col.G!=0||col.B!=0)
                        {
                            continue;
                        }
                        else
                        {
                            bottom = i;
                            goto label4;
                        }
                    }
                }
                #endregion

                label4:
                int width = right - left,
                    hight = bottom - top;
                width %= 8;
                width /= 2;
                if (width>0)
                {
                    left -= width;
                    right += width;
                }//将边缘对称于矩形的左右部分
                hight %= 8;
                hight /= 2;
                if (hight>0)
                {
                    top -= hight;
                    bottom += hight;
                }//将边缘对称于矩形的上下部分

                width = right - left;
                if (width<80)
                {
                    width = (80 - width) / 2;
                    left -= width;
                    right += width;
                }//最小宽度为80
                
                width = right - left;
                hight = bottom - top;
                //bitmap.UnlockBits(bmpdata);
                var newbitmap =bitmap.Clone(lrtbRect=new Rectangle(left, top, width, hight),PixelFormat.Format24bppRgb);
                return newbitmap;
            }

            /// <summary>
            /// 获得8*8特征值
            /// </summary>
            /// <returns></returns>
            private int[,] Get_DEight_Array()
            {
                var map = Get_Most_LRTB();
                int w = map.Width % 8,
                    neww = map.Width + 8 - w,
                    maxBlackNum = 0,
                    h = map.Height % 8,
                    newh = map.Height + 8 - h,
                    left = 0,
                    right = 0,
                    top = 0,
                    bottom = 0;
                int[,] doubleEight = new int[8, 8],
                       relsult = new int[8, 8];
                Rectangle rec;
                h = newh / 8; 
                w = neww / 8;
                var newmap = bmp.Clone(new Rectangle(lrtbRect.Left,lrtbRect.Top, neww, newh), bmp.PixelFormat);
                for (int i = 0; i < 8;++i )
                {
                    for (int j = 0; j < 8;++j )
                    {
                        left = w * i;
                        right = left + w - 1;
                        top = h * j;
                        bottom = top + h - 1;
                        rec = new Rectangle(left, top, right - left, bottom - top);
                        doubleEight[i, j] = GetBlackCount(newmap, rec);
                        if (doubleEight[i, j] > maxBlackNum)
                            maxBlackNum = doubleEight[i, j];
                    }
                }

                for (int i = 0; i < 8; ++i)
                {
                    for (int j = 0; j < 8; ++j)
                    {
                        if (doubleEight[i, j] / (double)maxBlackNum > 0.2)
                            relsult[i, j] = 1;
                        else
                            relsult[i, j] = 0;
                    }
                }
                return relsult;
            }

            /// <summary>
            /// 统计一个区域的黑点数目
            /// </summary>
            /// <param name="bp"></param>
            /// <param name="rec"></param>
            /// <returns></returns>
            private int GetBlackCount(Bitmap bp,Rectangle rec)
            {
                var newmap = bp.Clone(rec, bp.PixelFormat);
                int count = 0;
                Color col;
                for (int i = 0; i < newmap.Width;++i )
                {
                    for (int j = 0; j < newmap.Height;++j )
                    {
                        col=newmap.GetPixel(i,j);
                        if (col.R==0&&col.G==0&&col.B==0)
                        {
                            ++count;
                        }
                    }
                }
                return count;
            }

            /// <summary>
            /// 保存当前模板
            /// </summary>
            /// <param name="num"></param>
            public void SaveCurrentPattern(int num)
            {
                var feature = Get_DEight_Array();
                int[,] temp = new int[8, 8];
                for (int i = 0; i < 8;++i )
                {
                    for (int j = 0; j < 8;++j )
                    {
                        if (feature[j, i] == 1)
                        {
                            temp[i, j] = 1;
                        }
                        else
                            temp[i, j] = 0;
                    }
                }
                records[num] = new Pattern();
                records[num].Num = num;
                records[num].FeatureDetail.Add(temp);
            }

            /// <summary>
            /// 保存到文本文件
            /// </summary>
            /// <param name="filepath"></param>
            public void SaveToFile(string filepath)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < 10;++i )
                {
                    if(records[i]!=null)
                    {
                        sb.AppendLine(records[i].Num.ToString());
                        sb.AppendLine(records[i].FeatureDetail.Count.ToString());
                        records[i].FeatureDetail.ForEach(f =>
                        {
                            for (int j = 0; j < 8; ++j)
                            {
                                for (int k = 0; k < 8; ++k)
                                {
                                    sb.Append(f[j, k] + " ");
                                }
                                if (j != 7)
                                    sb.AppendLine();
                            }
                        }
                            );
                    }
                    File.WriteAllText(filepath, sb.ToString(), Encoding.Default);
                    }
                    
            }

            /// <summary>
            /// 保存到二进制文件
            /// </summary>
            public void SaveAsBFile()
            {
                if (!Directory.Exists("C:\\ProgramData\\YT\\Template"))
                {
                    Directory.CreateDirectory("C:\\ProgramData\\YT\\Template");
                }
                if (File.Exists("C:\\ProgramData\\YT\\Template\\tp.bin"))
                {
                    File.Delete("C:\\ProgramData\\YT\\Template\\tp.bin");
                }
                records.Serialize("C:\\ProgramData\\YT\\Template\\tp.bin");
            }

            /// <summary>
            /// 读取模板文件
            /// </summary>
            public void ReadTemplate()
            {
                if (File.Exists("C:\\ProgramData\\YT\\Template\\tp.bin"))
                {
                    records=Methods.Deserialize<Pattern[]>("C:\\ProgramData\\YT\\Template\\tp.bin");
                }
                
            }
        }

        /// <summary>
        /// 模板类
        /// </summary>
        [Serializable]
        public class Pattern
        {
            /// <summary>
            /// 模板个数
            /// </summary>
            public int Num;

            /// <summary>
            /// 模板个数
            /// </summary>
            public int Count { get { return FeatureDetail.Count; } }
            /// <summary>
            /// 模板细节
            /// </summary>
            public List<int[,]> FeatureDetail=new List<int[,]>();
        }
    }

}
