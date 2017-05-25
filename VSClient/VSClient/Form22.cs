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
    public partial class Form22 : Form
    {
        public Form22()
        {
            InitializeComponent();

            StartPosition = FormStartPosition.CenterScreen;

            //关闭对文本框的非线程操作检查
            TextBox.CheckForIllegalCrossThreadCalls = false;
        }

        private void Form22_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Form22_Activated(object sender, EventArgs e)
        {
            label2.Text = Tag as string;
        }

        private static string[] strStu;
        private static string[] strText;

        public static void getStrStu(string[] str)
        {
            strStu = new string[str.Length];
            for (int i = 0; i < str.Length; i++)
            {
                strStu[i] = str[i].Trim();
            }
        }

        public static void getStrText(string[] str)
        {
            strText = new string[str.Length];
            for (int i = 0; i < str.Length; i++)
            {
                strText[i] = str[i].Trim();
            }
        }

        /*public void setdataGridView1(string[] str)//查询学生
        {
            dataGridView1.Rows.Clear();
            for (int i = 2; i + 1 < str.Length; i += 2)
            {
                dataGridView1.Rows.Add(str[i], str[i + 1]);
            }
        }

        public void setdataGridView2(string[] str)//查询题库
        {
            dataGridView2.Rows.Clear();
            for (int i = 2; i + 6 < str.Length; i += 7)
            {
                dataGridView2.Rows.Add(str[i], str[i + 1], str[i + 2], str[i + 3], str[i + 4], str[i + 5], str[i + 6]);
            }
        }*/

        private void button3_Click(object sender, EventArgs e)
        {
            string str = "注销@教师@" + label2.Text.Trim();
            Form1.ClientSendMsg(str);
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            foreach (Form fm in Application.OpenForms)
            {
                if (fm.Name == "Form221" || fm.Name == "Form222")
                {
                    fm.Close();
                }
            }
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "" || textBox2.Text.Trim() == "" ||
                textBox3.Text.Trim() == "")
            {
                MessageBox.Show("请输入数据");
            }
            else if (!textBox2.Text.Trim().Equals(textBox3.Text.Trim()))
            {
                MessageBox.Show("密码不一致");
            }
            else
            {
                string str = "修改密码@教师@" + label2.Text.Trim() + "@" + textBox1.Text.Trim()
                    + "@" + textBox2.Text.Trim();
                Form1.ClientSendMsg(str);
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string str = "查询学生@教师@" + label2.Text.Trim();
            Form1.ClientSendMsg(str);
            button7.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string str = "查询题库@教师@" + label2.Text.Trim();
            Form1.ClientSendMsg(str);
            button8.Enabled = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            bool isfind = false;
            foreach (Form fm in Application.OpenForms)
            {
                if (fm.Name == "Form221")
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
                Form fm = new Form221();
                fm.Tag = label2.Text.Trim();
                fm.Show();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            bool isfind = false;
            foreach (Form fm in Application.OpenForms)
            {
                if (fm.Name == "Form222")
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
                Form fm = new Form222();
                fm.Tag = label2.Text.Trim();
                fm.Show();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            for (int i = 2; i + 1 < strStu.Length; i += 2)
            {
                dataGridView1.Rows.Add(strStu[i], strStu[i + 1]);
            }
            button7.Enabled = false;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            for (int i = 2; i + 6 < strText.Length; i += 7)
            {
                dataGridView2.Rows.Add(strText[i], strText[i + 1], strText[i + 2],
                    strText[i + 3], strText[i + 4], strText[i + 5], strText[i + 6]);
            }
            button8.Enabled = false;
        }
    }
}
