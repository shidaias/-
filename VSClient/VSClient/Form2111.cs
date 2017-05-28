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
    public partial class Form2111 : Form
    {
        public Form2111()
        {
            InitializeComponent();
        }

        private static string[,] text = new string[10, 5];
        private static string[] answer = new string[10];
        private static string[] answer0 = new string[10];
        private static int score;

        public static void getText(string[,] str1, string[] str2, string[] str3, int x)
        {
            score = x;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    text[i, j] = str1[i, j];
                }
                answer[i] = str2[i];
                answer0[i] = str3[i];
            }
        }

        private void Form2111_Activated(object sender, EventArgs e)
        {
            label2.Text = Tag as string;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox2.AppendText("你的成绩为：" + score);
            for (int i = 0; i < 10; i++)
            {
                textBox1.AppendText(text[i, 0] + "\r\n");
                textBox1.AppendText("A: " + text[i, 1] + "\tB: " + text[i, 2] + "\r\n");
                textBox1.AppendText("C: " + text[i, 3] + "\tD: " + text[i, 4] + "\r\n");
                textBox1.AppendText("正确答案：" + answer[i] + "\t你的答案：" + answer0[i] + "\r\n\r\n");
            }
        }

        private void Form2111_FormClosed(object sender, FormClosedEventArgs e)
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

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox2.AppendText("你的成绩为：" + score);
            for (int i = 0; i < 10; i++)
            {
                if (!answer[i].Equals(answer0[i]))
                {
                    textBox1.AppendText(text[i, 0] + "\r\n");
                    textBox1.AppendText("A: " + text[i, 1] + "\tB: " + text[i, 2] + "\r\n");
                    textBox1.AppendText("C: " + text[i, 3] + "\tD: " + text[i, 4] + "\r\n");
                    textBox1.AppendText("正确答案：" + answer[i] + "\t你的答案：" + answer0[i] + "\r\n\r\n");
                }
            }
        }
    }
}
