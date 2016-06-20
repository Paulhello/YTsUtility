namespace ImageTest
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.打开ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.处理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.灰度化ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.二值化ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.原图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.反色ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.中值滤波ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.高斯滤波ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.直方图规定化ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gabor滤波ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.四向扩散ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.八向扩散ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.二值腐蚀ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.二值膨胀ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.处理2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.灰度图像腐蚀ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.灰度图像膨胀ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sobelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.prewittToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.robertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kirschToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.laplace1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.laplace2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lOG1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lOG2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.marrToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.打开ToolStripMenuItem,
            this.处理ToolStripMenuItem,
            this.处理2ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(692, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 25);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(692, 362);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // 打开ToolStripMenuItem
            // 
            this.打开ToolStripMenuItem.Name = "打开ToolStripMenuItem";
            this.打开ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.打开ToolStripMenuItem.Text = "打开";
            this.打开ToolStripMenuItem.Click += new System.EventHandler(this.打开ToolStripMenuItem_Click);
            // 
            // 处理ToolStripMenuItem
            // 
            this.处理ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.原图ToolStripMenuItem,
            this.灰度化ToolStripMenuItem,
            this.二值化ToolStripMenuItem,
            this.反色ToolStripMenuItem,
            this.中值滤波ToolStripMenuItem,
            this.高斯滤波ToolStripMenuItem,
            this.直方图规定化ToolStripMenuItem,
            this.gabor滤波ToolStripMenuItem,
            this.四向扩散ToolStripMenuItem,
            this.八向扩散ToolStripMenuItem,
            this.二值腐蚀ToolStripMenuItem,
            this.二值膨胀ToolStripMenuItem});
            this.处理ToolStripMenuItem.Name = "处理ToolStripMenuItem";
            this.处理ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.处理ToolStripMenuItem.Text = "处理";
            // 
            // 灰度化ToolStripMenuItem
            // 
            this.灰度化ToolStripMenuItem.Name = "灰度化ToolStripMenuItem";
            this.灰度化ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.灰度化ToolStripMenuItem.Text = "灰度化";
            this.灰度化ToolStripMenuItem.Click += new System.EventHandler(this.灰度化ToolStripMenuItem_Click);
            // 
            // 二值化ToolStripMenuItem
            // 
            this.二值化ToolStripMenuItem.Name = "二值化ToolStripMenuItem";
            this.二值化ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.二值化ToolStripMenuItem.Text = "二值化";
            this.二值化ToolStripMenuItem.Click += new System.EventHandler(this.二值化ToolStripMenuItem_Click);
            // 
            // 原图ToolStripMenuItem
            // 
            this.原图ToolStripMenuItem.Name = "原图ToolStripMenuItem";
            this.原图ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.原图ToolStripMenuItem.Text = "原图";
            this.原图ToolStripMenuItem.Click += new System.EventHandler(this.原图ToolStripMenuItem_Click);
            // 
            // 反色ToolStripMenuItem
            // 
            this.反色ToolStripMenuItem.Name = "反色ToolStripMenuItem";
            this.反色ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.反色ToolStripMenuItem.Text = "反色";
            this.反色ToolStripMenuItem.Click += new System.EventHandler(this.反色ToolStripMenuItem_Click);
            // 
            // 中值滤波ToolStripMenuItem
            // 
            this.中值滤波ToolStripMenuItem.Name = "中值滤波ToolStripMenuItem";
            this.中值滤波ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.中值滤波ToolStripMenuItem.Text = "中值滤波";
            this.中值滤波ToolStripMenuItem.Click += new System.EventHandler(this.中值滤波ToolStripMenuItem_Click);
            // 
            // 高斯滤波ToolStripMenuItem
            // 
            this.高斯滤波ToolStripMenuItem.Name = "高斯滤波ToolStripMenuItem";
            this.高斯滤波ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.高斯滤波ToolStripMenuItem.Text = "高斯滤波";
            this.高斯滤波ToolStripMenuItem.Click += new System.EventHandler(this.高斯滤波ToolStripMenuItem_Click);
            // 
            // 直方图规定化ToolStripMenuItem
            // 
            this.直方图规定化ToolStripMenuItem.Name = "直方图规定化ToolStripMenuItem";
            this.直方图规定化ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.直方图规定化ToolStripMenuItem.Text = "直方图规定化";
            this.直方图规定化ToolStripMenuItem.Click += new System.EventHandler(this.直方图规定化ToolStripMenuItem_Click);
            // 
            // gabor滤波ToolStripMenuItem
            // 
            this.gabor滤波ToolStripMenuItem.Name = "gabor滤波ToolStripMenuItem";
            this.gabor滤波ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.gabor滤波ToolStripMenuItem.Text = "Gabor滤波";
            this.gabor滤波ToolStripMenuItem.Click += new System.EventHandler(this.gabor滤波ToolStripMenuItem_Click);
            // 
            // 四向扩散ToolStripMenuItem
            // 
            this.四向扩散ToolStripMenuItem.Name = "四向扩散ToolStripMenuItem";
            this.四向扩散ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.四向扩散ToolStripMenuItem.Text = "四向扩散";
            this.四向扩散ToolStripMenuItem.Click += new System.EventHandler(this.四向扩散ToolStripMenuItem_Click);
            // 
            // 八向扩散ToolStripMenuItem
            // 
            this.八向扩散ToolStripMenuItem.Name = "八向扩散ToolStripMenuItem";
            this.八向扩散ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.八向扩散ToolStripMenuItem.Text = "八向扩散";
            this.八向扩散ToolStripMenuItem.Click += new System.EventHandler(this.八向扩散ToolStripMenuItem_Click);
            // 
            // 二值腐蚀ToolStripMenuItem
            // 
            this.二值腐蚀ToolStripMenuItem.Name = "二值腐蚀ToolStripMenuItem";
            this.二值腐蚀ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.二值腐蚀ToolStripMenuItem.Text = "二值腐蚀";
            this.二值腐蚀ToolStripMenuItem.Click += new System.EventHandler(this.二值腐蚀ToolStripMenuItem_Click);
            // 
            // 二值膨胀ToolStripMenuItem
            // 
            this.二值膨胀ToolStripMenuItem.Name = "二值膨胀ToolStripMenuItem";
            this.二值膨胀ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.二值膨胀ToolStripMenuItem.Text = "二值膨胀";
            this.二值膨胀ToolStripMenuItem.Click += new System.EventHandler(this.二值膨胀ToolStripMenuItem_Click);
            // 
            // 处理2ToolStripMenuItem
            // 
            this.处理2ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.灰度图像腐蚀ToolStripMenuItem,
            this.灰度图像膨胀ToolStripMenuItem,
            this.sobelToolStripMenuItem,
            this.prewittToolStripMenuItem,
            this.robertToolStripMenuItem,
            this.kirschToolStripMenuItem,
            this.laplace1ToolStripMenuItem,
            this.laplace2ToolStripMenuItem,
            this.lOG1ToolStripMenuItem,
            this.lOG2ToolStripMenuItem,
            this.marrToolStripMenuItem});
            this.处理2ToolStripMenuItem.Name = "处理2ToolStripMenuItem";
            this.处理2ToolStripMenuItem.Size = new System.Drawing.Size(51, 21);
            this.处理2ToolStripMenuItem.Text = "处理2";
            // 
            // 灰度图像腐蚀ToolStripMenuItem
            // 
            this.灰度图像腐蚀ToolStripMenuItem.Name = "灰度图像腐蚀ToolStripMenuItem";
            this.灰度图像腐蚀ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.灰度图像腐蚀ToolStripMenuItem.Text = "灰度图像腐蚀";
            this.灰度图像腐蚀ToolStripMenuItem.Click += new System.EventHandler(this.灰度图像腐蚀ToolStripMenuItem_Click);
            // 
            // 灰度图像膨胀ToolStripMenuItem
            // 
            this.灰度图像膨胀ToolStripMenuItem.Name = "灰度图像膨胀ToolStripMenuItem";
            this.灰度图像膨胀ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.灰度图像膨胀ToolStripMenuItem.Text = "灰度图像膨胀";
            this.灰度图像膨胀ToolStripMenuItem.Click += new System.EventHandler(this.灰度图像膨胀ToolStripMenuItem_Click);
            // 
            // sobelToolStripMenuItem
            // 
            this.sobelToolStripMenuItem.Name = "sobelToolStripMenuItem";
            this.sobelToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.sobelToolStripMenuItem.Text = "Sobel";
            this.sobelToolStripMenuItem.Click += new System.EventHandler(this.sobelToolStripMenuItem_Click);
            // 
            // prewittToolStripMenuItem
            // 
            this.prewittToolStripMenuItem.Name = "prewittToolStripMenuItem";
            this.prewittToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.prewittToolStripMenuItem.Text = "Prewitt";
            this.prewittToolStripMenuItem.Click += new System.EventHandler(this.prewittToolStripMenuItem_Click);
            // 
            // robertToolStripMenuItem
            // 
            this.robertToolStripMenuItem.Name = "robertToolStripMenuItem";
            this.robertToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.robertToolStripMenuItem.Text = "Robert";
            this.robertToolStripMenuItem.Click += new System.EventHandler(this.robertToolStripMenuItem_Click);
            // 
            // kirschToolStripMenuItem
            // 
            this.kirschToolStripMenuItem.Name = "kirschToolStripMenuItem";
            this.kirschToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.kirschToolStripMenuItem.Text = "Kirsch";
            this.kirschToolStripMenuItem.Click += new System.EventHandler(this.kirschToolStripMenuItem_Click);
            // 
            // laplace1ToolStripMenuItem
            // 
            this.laplace1ToolStripMenuItem.Name = "laplace1ToolStripMenuItem";
            this.laplace1ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.laplace1ToolStripMenuItem.Text = "Laplace1";
            this.laplace1ToolStripMenuItem.Click += new System.EventHandler(this.laplace1ToolStripMenuItem_Click);
            // 
            // laplace2ToolStripMenuItem
            // 
            this.laplace2ToolStripMenuItem.Name = "laplace2ToolStripMenuItem";
            this.laplace2ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.laplace2ToolStripMenuItem.Text = "Laplace2";
            this.laplace2ToolStripMenuItem.Click += new System.EventHandler(this.laplace2ToolStripMenuItem_Click);
            // 
            // lOG1ToolStripMenuItem
            // 
            this.lOG1ToolStripMenuItem.Name = "lOG1ToolStripMenuItem";
            this.lOG1ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.lOG1ToolStripMenuItem.Text = "LOG1";
            this.lOG1ToolStripMenuItem.Click += new System.EventHandler(this.lOG1ToolStripMenuItem_Click);
            // 
            // lOG2ToolStripMenuItem
            // 
            this.lOG2ToolStripMenuItem.Name = "lOG2ToolStripMenuItem";
            this.lOG2ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.lOG2ToolStripMenuItem.Text = "LOG2";
            this.lOG2ToolStripMenuItem.Click += new System.EventHandler(this.lOG2ToolStripMenuItem_Click);
            // 
            // marrToolStripMenuItem
            // 
            this.marrToolStripMenuItem.Name = "marrToolStripMenuItem";
            this.marrToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.marrToolStripMenuItem.Text = "Marr";
            this.marrToolStripMenuItem.Click += new System.EventHandler(this.marrToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 387);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 打开ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 处理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 灰度化ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 二值化ToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripMenuItem 原图ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 反色ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 中值滤波ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 高斯滤波ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 直方图规定化ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gabor滤波ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 四向扩散ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 八向扩散ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 二值腐蚀ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 二值膨胀ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 处理2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 灰度图像腐蚀ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 灰度图像膨胀ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sobelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem prewittToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem robertToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem kirschToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem laplace1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem laplace2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lOG1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lOG2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem marrToolStripMenuItem;
    }
}

