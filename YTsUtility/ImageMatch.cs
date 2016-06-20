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
    namespace PhotoGramMetry
    {
        /// <summary>
        /// 图像匹配类
        /// </summary>
        public class ImageMatch
        {
            /// <summary>
            /// 影像列表
            /// </summary>
            public List<Bitmap> bmpList {private set; get; }
            
            /// <summary>
            /// 影像列表构造
            /// </summary>
            /// <param name="bList"></param>
            public ImageMatch(List<Bitmap> bList)
            {
                bmpList = bList;
            }

            /// <summary>
            /// 匹配
            /// </summary>
            public void Match()
            {
                if (bmpList == null || bmpList.Count <= 1)
                    return;
                Rectangle rect = new Rectangle(new Point(),bmpList[0].Size);
                for (int k = 0; k < bmpList.Count - 1;k++ )
                {
                    Bitmap b1 = bmpList[k], b2 = bmpList[k + 1];
                    BitmapData bdata1 = new BitmapData(),
                               bdata2 = new BitmapData();
                    b1.LockBits(rect, ImageLockMode.ReadOnly, b1.PixelFormat);
                    b2.LockBits(rect, ImageLockMode.ReadOnly, b2.PixelFormat);
                    IntPtr bp1 = bdata1.Scan0, bp2 = bdata2.Scan0;
                    int bytes = bdata1.Stride * bdata1.Height;
                    byte[] b1bytes=new byte[bytes],b2bytes=new byte[bytes];
                    Marshal.Copy(bp1, b1bytes, 0, bytes);
                    Marshal.Copy(bp2, b2bytes, 0, bytes);
                    
                    b1.UnlockBits(bdata1);
                    b2.UnlockBits(bdata2);
                }
            }
        }
    }

}
