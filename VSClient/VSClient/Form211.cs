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
            type = str[1];
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
                button2.Enabled = true;
            }
            else if (i == 9)
            {
                button1.Enabled = true;
                button2.Enabled = false;
            }
            else
            {
                button1.Enabled = true;
                button2.Enabled = true;
            }
            if (i == 9 || answer0[9] != "")
            {
                button4.Enabled = true;
            }
            else
            {
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

            string str = "生成答卷@" + label2.Text + "@" + type;
            for (int i = 0; i < num.Length; i++)
            {
                str += "@" + num[i] + "@" + answer0[i];
            }
            str += "@" + score;
            Form1.ClientSendMsg(str);

            //恢复现场
            i = 0;
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = true;
            button4.Enabled = false;
            label3.Text = "题数";
            label4.Text = "                             请认真作答";
            label5.Text = "选项A";
            label6.Text = "选项B";
            label7.Text = "选项C";
            label8.Text = "选项D";


            bool isfind = false;
            Hide();
            foreach (Form fm in Application.OpenForms)
            {
                if (fm.Name == "Form2111")
                {
                    fm.Tag = label2.Text.Trim();
                    fm.WindowState = FormWindowState.Normal;
                    fm.Show();
                    fm.Activate();
                    return;
                }
            }
            if (!isfind)
            {
                Form fm = new Form2111();
                fm.Tag = label2.Text.Trim();
                fm.Show();
            }
            Form2111.getText(text, answer, answer0, score);
            //MessageBox.Show("您的成绩为" + score);
        }

        private void Form211_FormClosed(object sender, FormClosedEventArgs e)
        {
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
        }
    }
}
