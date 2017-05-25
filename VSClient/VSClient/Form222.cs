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
    public partial class Form222 : Form
    {
        public Form222()
        {
            InitializeComponent();

            StartPosition = FormStartPosition.CenterScreen;

            //关闭对文本框的非线程操作检查
            TextBox.CheckForIllegalCrossThreadCalls = false;
        }

        private void Form222_Activated(object sender, EventArgs e)
        {
            label2.Text = Tag as string;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "")
            {
                MessageBox.Show("请输入数据");
                return;
            }
            string str = "删除题库@教师@" + label2.Text.Trim() + "@" + textBox1.Text.Trim();
            Form1.ClientSendMsg(str);
            textBox1.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "" || textBox2.Text.Trim() == "" ||
                textBox3.Text.Trim() == "" || textBox4.Text.Trim() == "" ||
                textBox5.Text.Trim() == "" || textBox6.Text.Trim() == "" ||
                textBox7.Text.Trim() == "")
            {
                MessageBox.Show("请输入数据");
                return;
            }
            if (!textBox7.Text.Trim().ToUpper().Equals("A") &&
                !textBox7.Text.Trim().ToUpper().Equals("B") &&
                !textBox7.Text.Trim().ToUpper().Equals("C") &&
                !textBox7.Text.Trim().ToUpper().Equals("D"))
            {
                MessageBox.Show("答案格式应为A,B,C或D");
                return;
            }
            string str = "添加题库@教师@" + label2.Text.Trim() + "@" + textBox1.Text.Trim()
                + "@" + textBox2.Text.Trim() + "@" + textBox3.Text.Trim() + "@" + textBox4.Text.Trim()
                + "@" + textBox5.Text.Trim() + "@" + textBox6.Text.Trim() + "@" + textBox7.Text.Trim().ToUpper();
            Form1.ClientSendMsg(str);
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
        }
    }
}
