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
    public partial class Form232 : Form
    {
        public Form232()
        {
            InitializeComponent();

            StartPosition = FormStartPosition.CenterScreen;

            //关闭对文本框的非线程操作检查
            TextBox.CheckForIllegalCrossThreadCalls = false;
        }

        /*public void setdataGridView1(string[] str)//查询题库
        {
            dataGridView1.Rows.Clear();
            for (int i = 2; i + 7 < str.Length; i += 8)
            {
                dataGridView1.Rows.Add(str[i], str[i + 1], str[i + 2], str[i + 3], str[i + 4],
                   str[i + 5], str[i + 6], str[i + 7]);
            }
        }*/

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "")
            {
                MessageBox.Show("请输入数据");
                return;
            }
            string str = "删除题库@管理员@" + textBox1.Text.Trim();
            Form1.ClientSendMsg(str);
            textBox1.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "" || textBox2.Text.Trim() == "" ||
                textBox3.Text.Trim() == "" || textBox4.Text.Trim() == "" ||
                textBox5.Text.Trim() == "" || textBox6.Text.Trim() == "" ||
                textBox7.Text.Trim() == "" || textBox8.Text.Trim() == "")
            {
                MessageBox.Show("请输入数据");
                return;
            }
            if (!textBox7.Text.Trim().ToUpper().Equals("A") &&
                !textBox7.Text.Trim().ToUpper().Equals("B") &&
                !textBox7.Text.Trim().ToUpper().Equals("C") &&
                !textBox7.Text.Trim().ToUpper().Equals("D") )
            {
                MessageBox.Show("答案格式应为A,B,C或D");
                return;
            }
            if (!textBox8.Text.Trim().Equals("语文") &&
                !textBox8.Text.Trim().Equals("数学") &&
                !textBox8.Text.Trim().Equals("英语") )
            {
                MessageBox.Show("科目应为语文, 数学或英语");
                return;
            }
            string str = "添加题库@管理员@" + textBox1.Text.Trim()
                + "@" + textBox2.Text.Trim() + "@" + textBox3.Text.Trim() + "@" + textBox4.Text.Trim()
                + "@" + textBox5.Text.Trim() + "@" + textBox6.Text.Trim() + "@" + textBox7.Text.Trim().ToUpper()
                + "@" + textBox8.Text.Trim();
            Form1.ClientSendMsg(str);
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
        }

        private void Form232_Activated(object sender, EventArgs e)
        {
            label2.Text = Tag as string;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string str = "查询题库@管理员@" + label2.Text.Trim();
            Form1.ClientSendMsg(str);
        }
    }
}
