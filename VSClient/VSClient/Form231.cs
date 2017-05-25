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
    public partial class Form231 : Form
    {
        public Form231()
        {
            InitializeComponent();

            StartPosition = FormStartPosition.CenterScreen;

            //关闭对文本框的非线程操作检查
            TextBox.CheckForIllegalCrossThreadCalls = false;
        }

        private void Form231_Activated(object sender, EventArgs e)
        {
            label4.Text = Tag as string;
        }

        private static string[] strStu;

        public static void getStrStu(string[] str)
        {
            strStu = new string[str.Length];
            for (int i = 0; i < str.Length; i++)
            {
                strStu[i] = str[i].Trim();
            }
        }

        /*public void setdataGridView1(string[] str)
        {
            dataGridView1.Rows.Clear();
            for (int i = 2; i + 2 < str.Length; i += 3)
            {
                dataGridView1.Rows.Add(str[i], str[i + 1], str[i + 2]);
            }
        }*/

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "")
            {
                MessageBox.Show("请输入数据");
                return;
            }
            string str = "删除学生@管理员@" + textBox1.Text.Trim();
            Form1.ClientSendMsg(str);
            textBox1.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "" || textBox2.Text.Trim() == "" ||
                textBox3.Text.Trim() == "")
            {
                MessageBox.Show("请输入数据");
                return;
            }
            string str = "注册学生@管理员@" + textBox1.Text.Trim()
                + "@" + textBox2.Text.Trim() + "@" + textBox3.Text.Trim();
            Form1.ClientSendMsg(str);
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string str = "查询学生@管理员@" + label4.Text.Trim();
            Form1.ClientSendMsg(str);
            button4.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            for (int i = 2; i + 2 < strStu.Length; i += 3)
            {
                dataGridView1.Rows.Add(strStu[i], strStu[i + 1], strStu[i + 2]);
            }
            button4.Enabled = false;
        }
    }
}
