using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YTsUtility.ImageProcess;


namespace ImageTest
{
    public partial class Form1 : Form
    {
        Bitmap bitmap;
        ImageProcessing imgpr;
        public Form1()
        {
            InitializeComponent();
        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = string.Empty;
            openFileDialog1.Filter = "*.*|*.*";
            openFileDialog1.Title = "打开...";
            if(DialogResult.OK==openFileDialog1.ShowDialog())
            {
                bitmap = Image.FromFile(openFileDialog1.FileName) as Bitmap;
                pictureBox1.Image = bitmap.Clone() as Image;
            }
        }

        private async void 灰度化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image!=null)
            {
                灰度化ToolStripMenuItem.Enabled = false;
                imgpr = new ImageProcessing(pictureBox1.Image as Bitmap);
                await Task.Run(() => pictureBox1.Image = imgpr.Scalize());
                灰度化ToolStripMenuItem.Enabled = true;
            }

        }

        private async void 二值化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                二值化ToolStripMenuItem.Enabled = false;
                imgpr = new ImageProcessing(pictureBox1.Image as Bitmap);
                await Task.Run(() => pictureBox1.Image = imgpr.TwoValue(imgpr.GetBestTresh()));
                二值化ToolStripMenuItem.Enabled = true;
            }
        }

        private void 原图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = bitmap ?? null;
        }

        private async void 反色ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                反色ToolStripMenuItem.Enabled = false;
                imgpr = new ImageProcessing(pictureBox1.Image as Bitmap);
                await Task.Run(() => pictureBox1.Image = imgpr.Invert());
                反色ToolStripMenuItem.Enabled = true;
            }
        }

        private async void 中值滤波ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                中值滤波ToolStripMenuItem.Enabled = false;
                imgpr = new ImageProcessing(pictureBox1.Image as Bitmap);
                await Task.Run(() => pictureBox1.Image = imgpr.MidFilter());
                中值滤波ToolStripMenuItem.Enabled = true;
            }
        }

        private async void 高斯滤波ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                高斯滤波ToolStripMenuItem.Enabled = false;
                imgpr = new ImageProcessing(pictureBox1.Image as Bitmap);
                await Task.Run(() => pictureBox1.Image = imgpr.GaussFilter());
                高斯滤波ToolStripMenuItem.Enabled = true;
            }
        }

        private async void 直方图规定化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                直方图规定化ToolStripMenuItem.Enabled = false;
                imgpr = new ImageProcessing(pictureBox1.Image as Bitmap);
                await Task.Run(() => pictureBox1.Image = imgpr.Normalize(210,600));
                直方图规定化ToolStripMenuItem.Enabled = true;
            }
        }

        private async void gabor滤波ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                gabor滤波ToolStripMenuItem.Enabled = false;
                imgpr = new ImageProcessing(pictureBox1.Image as Bitmap);
                pictureBox1.Image =await imgpr.GaborFilter();
                gabor滤波ToolStripMenuItem.Enabled = true;
#if nDEBUG
                MessageBox.Show("success!");
#endif
            }
        }

        private async  void 四向扩散ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                四向扩散ToolStripMenuItem.Enabled = false;
                imgpr = new ImageProcessing(pictureBox1.Image as Bitmap);
                await Task.Run(() => pictureBox1.Image = imgpr.FourAnisotropic());
                四向扩散ToolStripMenuItem.Enabled = true;
#if nDEBUG
                MessageBox.Show("success!");
#endif
            }
        }

        private async void 八向扩散ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                八向扩散ToolStripMenuItem.Enabled = false;
                imgpr = new ImageProcessing(pictureBox1.Image as Bitmap);
                await Task.Run(() => pictureBox1.Image = imgpr.EightAnisotropic());
                八向扩散ToolStripMenuItem.Enabled = true;
#if nDEBUG
                MessageBox.Show("success!");
#endif
            }
        }

        private async void 二值腐蚀ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                二值腐蚀ToolStripMenuItem.Enabled = false;
                imgpr = new ImageProcessing(pictureBox1.Image as Bitmap);
                await Task.Run(() => pictureBox1.Image = imgpr.Erode());
                二值腐蚀ToolStripMenuItem.Enabled = true;
#if nDEBUG
                MessageBox.Show("success!");
#endif
            }
        }

        private async void 二值膨胀ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                二值膨胀ToolStripMenuItem.Enabled = false;
                imgpr = new ImageProcessing(pictureBox1.Image as Bitmap);
                await Task.Run(() => pictureBox1.Image = imgpr.Dilate());
                二值膨胀ToolStripMenuItem.Enabled = true;
#if nDEBUG
                MessageBox.Show("success!");
#endif
            }
        }

        private async void 灰度图像腐蚀ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                灰度图像腐蚀ToolStripMenuItem.Enabled = false;
                imgpr = new ImageProcessing(pictureBox1.Image as Bitmap);
                await Task.Run(() => pictureBox1.Image = imgpr.GrayErode());
                灰度图像腐蚀ToolStripMenuItem.Enabled = true;
#if nDEBUG
                MessageBox.Show("success!");
#endif
            }
        }

        private void 灰度图像膨胀ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                灰度图像膨胀ToolStripMenuItem.Enabled = false;
                imgpr = new ImageProcessing(pictureBox1.Image as Bitmap);
                Task.Run(() => pictureBox1.Image = imgpr.GrayDilate());
                灰度图像膨胀ToolStripMenuItem.Enabled = true;
#if nDEBUG
                MessageBox.Show("success!");
#endif
            }
        }

        private async void sobelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                sobelToolStripMenuItem.Enabled = false;
                imgpr = new ImageProcessing(pictureBox1.Image as Bitmap);
                pictureBox1.Image =await imgpr.Sobel();
                sobelToolStripMenuItem.Enabled = true;
#if nDEBUG
                MessageBox.Show("success!");
#endif
            }
        }

        private async void prewittToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                prewittToolStripMenuItem.Enabled = false;
                imgpr = new ImageProcessing(pictureBox1.Image as Bitmap);
                pictureBox1.Image = await imgpr.Prewitt();
                prewittToolStripMenuItem.Enabled = true;
#if nDEBUG
                MessageBox.Show("success!");
#endif
            }
        }

        private async void robertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                robertToolStripMenuItem.Enabled = false;
                imgpr = new ImageProcessing(pictureBox1.Image as Bitmap);
                pictureBox1.Image = await imgpr.Robert();
                robertToolStripMenuItem.Enabled = true;
#if nDEBUG
                MessageBox.Show("success!");
#endif
            }
        }

        private async void kirschToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                kirschToolStripMenuItem.Enabled = false;
                imgpr = new ImageProcessing(pictureBox1.Image as Bitmap);
                pictureBox1.Image = await imgpr.Kirsch();
                kirschToolStripMenuItem.Enabled = true;
#if nDEBUG
                MessageBox.Show("success!");
#endif
            }
        }

        private async void laplace1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                laplace1ToolStripMenuItem.Enabled = false;
                imgpr = new ImageProcessing(pictureBox1.Image as Bitmap);
                await Task.Run(()=>pictureBox1.Image=imgpr.LaplaceFilter1());
                laplace1ToolStripMenuItem.Enabled = true;
#if nDEBUG
                MessageBox.Show("success!");
#endif
            }
        }

        private async void laplace2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                laplace2ToolStripMenuItem.Enabled = false;
                imgpr = new ImageProcessing(pictureBox1.Image as Bitmap);
                await Task.Run(() => pictureBox1.Image = imgpr.LaplaceFilter2());
                laplace2ToolStripMenuItem.Enabled = true;
#if nDEBUG
                MessageBox.Show("success!");
#endif
            }
        }

        private async void lOG1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                lOG1ToolStripMenuItem.Enabled = false;
                imgpr = new ImageProcessing(pictureBox1.Image as Bitmap);
                await Task.Run(() => pictureBox1.Image = imgpr.GaussLaplace1());
                lOG1ToolStripMenuItem.Enabled = true;
#if nDEBUG
                MessageBox.Show("success!");
#endif
            }
        }

        private async void lOG2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                lOG2ToolStripMenuItem.Enabled = false;
                imgpr = new ImageProcessing(pictureBox1.Image as Bitmap);
                await Task.Run(() => pictureBox1.Image = imgpr.GaussLaplace2());
                lOG2ToolStripMenuItem.Enabled = true;
#if nDEBUG
                MessageBox.Show("success!");
#endif
            }
        }

        private async void marrToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                marrToolStripMenuItem.Enabled = false;
                imgpr = new ImageProcessing(pictureBox1.Image as Bitmap);
                await Task.Run(() => pictureBox1.Image = imgpr.Marr());
                marrToolStripMenuItem.Enabled = true;
#if nDEBUG
                MessageBox.Show("success!");
#endif
            }
        }
    }
}
/*
 * ij = (i + 2) * 3 * newwid + (j + 2) * 3,
                            ij_1 = (i + 2) * 3 * newwid + (j + 1) * 3,
                            ij_2 = (i + 2) * 3 * newwid + j * 3,
                            ij1 = (i + 2) * 3 * newwid + (j + 3) * 3,
                            ij2 = (i + 2) * 3 * newwid + (j + 4) * 3,
                            i_1j = (i + 1) * 3 * newwid + (j + 2) * 3,
                            i_1j_1 = (i + 1) * 3 * newwid + (j + 1) * 3,
                            i_1j_2 = (i + 1) * 3 * newwid + j * 3,
                            i_1j1 = (i + 1) * 3 * newwid + (j + 3) * 3,
                            i_1j2 = (i + 1) * 3 * newwid + (j + 4) * 3,
                            i_2j = i * 3 * newwid + (j + 2) * 3,
                            i_2j_1 = i * 3 * newwid + (j + 1) * 3,
                            i_2j_2 = i * 3 * newwid + j * 3,
                            i_2j1 = i * 3 * newwid + (j + 3) * 3,
                            i_2j2 = i * 3 * newwid + (j + 4) * 3,
                            i1j = (i + 3) * 3 * newwid + (j + 2) * 3,
                            i1j_1 = (i + 3) * 3 * newwid + (j + 1) * 3,
                            i1j_2 = (i + 3) * 3 * newwid + j * 3,
                            i1j1 = (i + 3) * 3 * newwid + (j + 3) * 3,
                            i1j2 = (i + 3) * 3 * newwid + (j + 4) * 3,
                            i2j = (i + 4) * 3 * newwid + (j + 2) * 3,
                            i2j_1 = (i + 4) * 3 * newwid + (j + 1) * 3,
                            i2j_2 = (i + 4) * 3 * newwid + j * 3,
                            i2j1 = (i + 4) * 3 * newwid + (j + 3) * 3,
                            i2j2 = (i + 4) * 3 * newwid + (j + 4) * 3};
 */
