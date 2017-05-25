using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VSClient
{
    public partial class Form221 : Form
    {
        public Form221()
        {
            InitializeComponent();

            StartPosition = FormStartPosition.CenterScreen;

            //关闭对文本框的非线程操作检查
            TextBox.CheckForIllegalCrossThreadCalls = false;
        }

        private void Form221_Activated(object sender, EventArgs e)
        {
            label4.Text = Tag as string;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "")
            {
                MessageBox.Show("请输入数据");
                return;
            }
            string str = "删除学生@教师@" + label4.Text.Trim() +"@"+ textBox1.Text.Trim();
            Form1.ClientSendMsg(str);
            textBox1.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "" || textBox2.Text.Trim() == "")
            {
                MessageBox.Show("请输入数据");
                return;
            }
            string str = "注册学生@教师@" + label4.Text.Trim() +"@"+ textBox1.Text.Trim()
                + "@" + textBox2.Text.Trim();
            Form1.ClientSendMsg(str);
            textBox1.Clear();
            textBox2.Clear();
        }
    }
}
