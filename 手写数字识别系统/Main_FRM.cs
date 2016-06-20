using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using YTsUtility.ImageProcess;
using System.IO;

namespace 手写数字识别系统
{
    public partial class Main_FRM : Form
    {
        bool drawmode=false;
        List<List<Point>> points = new List<List<Point>>();
        Pen pen = new Pen(Brushes.Black, 6);
        TemplateFormer tf;
        public Main_FRM()
        {
            InitializeComponent();
        }

        private void 文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("F");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MaximumSize = Size;
            if (File.Exists("C:\\ProgramData\\YT\\Template\\tp.bin"))
            {
                tf = new TemplateFormer();
                tf.ReadTemplate();
            }
            else
                tf = new TemplateFormer();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if(points.Count==0)
              pictureBox1.Invalidate();
            drawmode = true;
            points.Add(new List<Point>());
            points[points.Count-1].Add(e.Location);
            //pictureBox1.Invalidate();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (points.Count>0&& points[points.Count - 1].Count > 1&&!drawmode)
            {
                //e.Graphics.FillRectangle(Brushes.White, 0, 0, pictureBox1.Width, pictureBox1.Height);
                Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height,PixelFormat.Format24bppRgb);
                Graphics g = Graphics.FromImage(bmp);
                g.FillRectangle(Brushes.White, 0, 0, bmp.Width, bmp.Height);
                points.ForEach(p => { g.DrawLines(pen, p.ToArray()); e.Graphics.DrawLines(pen, p.ToArray()); });

                bmp.Save("temp.bmp",ImageFormat.Bmp);
                tf.bitmap = bmp;
                pictureBox12.Image = tf.Get_Most_LRTB();
                points.Clear();
            }
            
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (drawmode)
                {
                    points[points.Count - 1].Add(e.Location);
                    points.ForEach(p => pictureBox1.CreateGraphics().DrawLines(pen, p.ToArray()));
                }
            }
            catch
            {
                points.Clear();
                drawmode = false;
                return;
            }
 

        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            drawmode = false;
            timer1.Stop();
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!drawmode)
            { 
                pictureBox1.Invalidate(true);    
                timer1.Stop();
            }

        }

        private void Main_FRM_FormClosing(object sender, FormClosingEventArgs e)
        {
            tf.SaveAsBFile();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            tf.SaveAsBFile();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            toolStripButton3.Checked = false;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            toolStripButton4.Checked = false;
        }
    }
}
