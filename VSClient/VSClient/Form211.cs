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
    public partial class Form211 : Form
    {
        public Form211()
        {
            InitializeComponent();

            StartPosition = FormStartPosition.CenterScreen;

            //关闭对文本框的非线程操作检查
            TextBox.CheckForIllegalCrossThreadCalls = false;
        }

        private static string type = "";
        private static string[,] text = new string[10,5];
        private static string[] num = new string[10];
        private static string[] answer = new string[10];
        private static string[] answer0 = new string[10];
        private int i = 0;

        public static void getText(string[] str)
        {
            type = str[1].Trim();
            for (int i = 2, j = 0; i + 6 < str.Length && j < 10; i++, j++)
            {
                num[j] = str[i++].Trim();
                for (int x = 0; x < 5; x++, i++)
                {
                    text[j, x] = str[i].Trim();
                }
                answer[j] = str[i].Trim();
            }
            for (int i = 0; i < answer0.Length; i++)
            {
                answer0[i] = "";
            }
            //MessageBox.Show("获取成功");
        }

        private bool getAnswer()
        {
            if (radioButton1.Checked)
            {
                answer0[i] = "A";
            }
            else if (radioButton2.Checked)
            {
                answer0[i] = "B";
            }
            else if (radioButton3.Checked)
            {
                answer0[i] = "C";
            }
            else if (radioButton4.Checked)
            {
                answer0[i] = "D";
            }
            else
            {
                return false;
            }
            return true;
        }

        private void setText()
        {
            //MessageBox.Show("测试进入");
            if (i == 0)
            {
                button1.Enabled = false;
                button4.Enabled = false;
            }
            else if (i == 9)
            {
                button2.Enabled = false;
                button4.Enabled = true;
            }
            else
            {
                button1.Enabled = true;
                button2.Enabled = true;
                button4.Enabled = false;
            }
            //MessageBox.Show("测试变化");
            if (answer0[i].Equals("A"))
            {
                radioButton1.Checked = true;
            }
            else if (answer0[i].Equals("B"))
            {
                radioButton2.Checked = true;
            }
            else if (answer0[i].Equals("C"))
            {
                radioButton3.Checked = true;
            }
            else if (answer0[i].Equals("D"))
            {
                radioButton4.Checked = true;
            }
            //MessageBox.Show("测试转换");
            label3.Text = (i + 1) + ".";
            label4.Text = text[i, 0];
            label5.Text = text[i, 1];
            label6.Text = text[i, 2];
            label7.Text = text[i, 3];
            label8.Text = text[i, 4];
            //MessageBox.Show("测试出题");
        }

        private void Form211_Activated(object sender, EventArgs e)
        {
            label2.Text = Tag as string;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;
            button3.Enabled = false;
            setText();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;
            getAnswer();
            i--;
            setText();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!getAnswer())
            {
                MessageBox.Show("未选择答案");
                return;
            }
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;
            i++;
            setText();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!getAnswer())
            {
                MessageBox.Show("未选择答案");
                return;
            }
            int score = 0;
            for (int j = 0; j < answer.Length; j++)
            {
                if (answer[j].Equals(answer0[j]))
                {
                    score += 10;
                }
            }
            MessageBox.Show("您的成绩为" + score);
        }

        private void Form211_FormClosed(object sender, FormClosedEventArgs e)
        {
            bool isfind = false;
            Hide();
            foreach (Form fm in Application.OpenForms)
            {
                if (fm.Name == "Form21")
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
                Form fm = new Form21();
                fm.Tag = label2.Text.Trim();
                fm.Show();
            }
        }
    }
}
