using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using YTsUtility;

namespace YTsUtility.User
{
    public partial class UserLogIn : Form
    {
        public bool status { set; get; }
        public UserLogIn()
        {
            InitializeComponent();
            status = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(!File.Exists("user.dat"))
            {
                MessageBox.Show("未在本机上注册!");
                return;
            }
            UserInfo ui = UserInfo.ReadInfo("user.dat");
            if(ui.isRight(textBox1.Text,textBox2.Text))
            {
                status = true;
                MessageBox.Show("登陆成功!");
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("用户名或密码错误!");
                return;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            UserReg ur = new UserReg();
            ur.ShowDialog();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            UserChange uc = new UserChange();
            uc.ShowDialog();
        }
    }
}
