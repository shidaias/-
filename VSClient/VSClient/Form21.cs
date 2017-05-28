using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace VSClient
{
    public partial class Form21 : Form
    {
        public Form21()
        {
            InitializeComponent();

            StartPosition = FormStartPosition.CenterScreen;

            //关闭对文本框的非线程操作检查
            TextBox.CheckForIllegalCrossThreadCalls = false;

            //查询老师信息
        }

        private void Form21_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Form21_Activated(object sender, EventArgs e)
        {
            label2.Text = Tag as string;
        }

        private static string[] strTea;
        private static string[] strStu;

        public static void getStrStu(string[] str)
        {
            strStu = new string[str.Length];
            for (int i = 0; i < str.Length; i++)
            {
                strStu[i] = str[i].Trim();
            }
        }
        public static void getStrTea(string[] str)
        {
            strTea = new string[str.Length];
            for (int i = 0; i < str.Length; i++)
            {
                strTea[i] = str[i].Trim();
            }
        }

        /*public void setdataGridView1(string[] str)//查询教师
        {
            dataGridView1.Rows.Clear();
            for (int i = 2; i + 2 < str.Length; i += 3)
            {
                dataGridView1.Rows.Add(str[i], str[i + 1], str[i + 2]);
            }
        }*/

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "" || textBox2.Text.Trim() == "" ||
                textBox3.Text.Trim() == "" )
            {
                MessageBox.Show("请输入数据");
            }
            else if (!textBox2.Text.Trim().Equals(textBox3.Text.Trim()))
            {
                MessageBox.Show("密码不一致");
            }
            else
            {
                string str = "修改密码@学生@"+ label2.Text.Trim() +"@"+ textBox1.Text.Trim() 
                    +"@"+ textBox2.Text.Trim();
                Form1.ClientSendMsg(str);
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string str = "注销@学生@" + label2.Text.Trim();
            Form1.ClientSendMsg(str);
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            dataGridView1.Rows.Clear();
            bool isfind = false;
            Hide();
            foreach (Form fm in Application.OpenForms)
            {
                if (fm.Name == "Form1")
                {
                    fm.WindowState = FormWindowState.Normal;
                    fm.Show();
                    fm.Activate();
                    return;
                }
            }
            if (!isfind)
            {
                Form fm = new Form1();
                fm.Show();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string str = "查询教师@学生@" + label2.Text.Trim();
            Form1.ClientSendMsg(str);
            button5.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string str = "生成考卷@";
            if (radioButton1.Checked)
            {
                str += "语文";
            }
            else if (radioButton2.Checked)
            {
                str += "数学";
            }
            else if (radioButton3.Checked)
            {
                str += "英语";
            }
            Form1.ClientSendMsg(str);
            bool isfind = false;
            Hide();
            foreach (Form fm in Application.OpenForms)
            {
                if (fm.Name == "Form211")
                {

                    fm.WindowState = FormWindowState.Normal;
                    fm.Tag = label2.Text.Trim();
                    fm.Show();
                    fm.Activate();
                    return;
                }
            }
            if (!isfind)
            {
                Form fm = new Form211();
                fm.Tag = textBox3.Text.Trim();
                fm.Show();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            for (int i = 2; i + 2 < strTea.Length; i += 3)
            {
                dataGridView1.Rows.Add(strTea[i], strTea[i + 1], strTea[i + 2]);
            }
            button5.Enabled = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string str = "查询学生@学生@" + label2.Text.Trim();
            Form1.ClientSendMsg(str);
            button7.Enabled = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            for (int i = 2; i + 1 < strStu.Length; i += 2)
            {
                dataGridView2.Rows.Add(strStu[i], strStu[i + 1]);
            }
            button7.Enabled = false;
        }       
    }
}
