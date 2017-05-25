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
    public partial class Form23 : Form
    {
        public Form23()
        {
            InitializeComponent();

            StartPosition = FormStartPosition.CenterScreen;

            //关闭对文本框的非线程操作检查
            TextBox.CheckForIllegalCrossThreadCalls = false;
        }

        private void Form23_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Form23_Activated(object sender, EventArgs e)
        {
            label2.Text = Tag as string;
        }

        private static string[] strTea;

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
            for (int i = 2; i + 4 < str.Length; i += 5)
            {
                dataGridView1.Rows.Add(str[i], str[i + 1], str[i + 2], str[i + 3], str[i + 4]);
            }
        }*/

        private void button1_Click(object sender, EventArgs e)
        {
            string str = "注销@管理员@" + label2.Text.Trim();
            Form1.ClientSendMsg(str);
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            dataGridView1.Rows.Clear();
            foreach (Form fm in Application.OpenForms)
            {
                if (fm.Name == "Form231" || fm.Name == "Form232")
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

        private void button7_Click(object sender, EventArgs e)
        {
            string str = "查询教师@管理员@" + label2.Text.Trim();
            Form1.ClientSendMsg(str);
            button8.Enabled = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "")
            {
                MessageBox.Show("请输入数据");
                return;
            }
            string str = "删除教师@管理员@" + textBox1.Text.Trim();
            Form1.ClientSendMsg(str);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox6.Text.Trim() == "" || textBox7.Text.Trim() == "" ||
                textBox8.Text.Trim() == "")
            {
                MessageBox.Show("请输入数据");
            }
            else if (!textBox7.Text.Trim().Equals(textBox8.Text.Trim()))
            {
                MessageBox.Show("密码不一致");
            }
            else
            {
                string str = "修改密码@管理员@" + label2.Text.Trim() + "@" + textBox6.Text.Trim()
                    + "@" + textBox7.Text.Trim();
                Form1.ClientSendMsg(str);
                textBox6.Clear();
                textBox7.Clear();
                textBox8.Clear();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "" || textBox2.Text.Trim() == "" ||
                textBox3.Text.Trim() == "" || textBox4.Text.Trim() == "" ||
                textBox5.Text.Trim() == "")
            {
                MessageBox.Show("请输入数据");
            }
            else
            {
                string str = "注册教师@管理员@" + textBox1.Text.Trim() + "@" + textBox2.Text.Trim()
                    + "@" + textBox3.Text.Trim() + "@" + textBox4.Text.Trim() + "@" + textBox5.Text.Trim();
                Form1.ClientSendMsg(str);
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bool isfind = false;
            foreach (Form fm in Application.OpenForms)
            {
                if (fm.Name == "Form231")
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
                Form fm = new Form231();
                fm.Tag = label2.Text.Trim();
                fm.Show();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bool isfind = false;
            foreach (Form fm in Application.OpenForms)
            {
                if (fm.Name == "Form232")
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
                Form fm = new Form232();
                fm.Tag = label2.Text.Trim();
                fm.Show();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            for (int i = 2; i + 4 < strTea.Length; i += 5)
            {
                dataGridView1.Rows.Add(strTea[i], strTea[i + 1], strTea[i + 2], strTea[i + 3], strTea[i + 4]);
            }
            button8.Enabled = false;
        }
    }
}
