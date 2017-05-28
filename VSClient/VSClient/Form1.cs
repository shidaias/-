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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            StartPosition = FormStartPosition.CenterScreen;

            //关闭对文本框的非线程操作检查
            TextBox.CheckForIllegalCrossThreadCalls = false;
        }

        int flag = 0;
        static Thread threadclient = null;
        static Socket socketclient = null;
        static List<IPEndPoint> mlist = new List<IPEndPoint>();

        //<span style = "font-family:KaiTi_GB2312;font-size:18px;" >
        #region datagrideview数据绑定委托  
        public delegate void InvokeHandler();
        #endregion</span> 

        /// <summary>
        /// 接收服务端发来的信息
        /// </summary> 
        protected void recv()//
        {
            int x = 0;
            while (true)//持续监听服务端发来的消息
            {
                try
                {
                    //定义一个1M的内存缓冲区，用于临时性存储接收到的消息
                    byte[] arrRecvmsg = new byte[1024 * 1024];

                    //将客户端套接字接收到的数据存入内存缓冲区，并获取长度
                    int length = socketclient.Receive(arrRecvmsg);

                    //将套接字获取到的字符数组转换为人可以看懂的字符串
                    string strRevMsg = Encoding.UTF8.GetString(arrRecvmsg, 0, length);
                    if (x == 0 && strRevMsg.Equals("连接成功"))
                    {
                        MessageBox.Show(strRevMsg);
                        x = 1;
                    }
                    if (x == 1)
                    {
                        string[] str = strRevMsg.Split('@');
                        if (str[0].Equals("登录"))
                        {
                            Login(str);
                        }
                        else if (str[0].Equals("会话"))
                        {
                            MessageBox.Show(str[1]);
                        }
                        else if (str[0].Equals("修改密码"))
                        {
                            Modify(str[2]);
                        }
                        else if (str[0].Equals("注册"))
                        {
                            Register(str[2]);
                        }
                        else if (str[0].Equals("删除"))
                        {
                            Delete(str[2]);
                        }
                        else if (str[0].Equals("查询学生"))
                        {
                            StuSelect(str);
                        }
                        else if (str[0].Equals("查询教师"))
                        {
                            TeaSelect(str);
                        }
                        else if (str[0].Equals("查询题库"))
                        {
                            TextSelect(str);
                        }
                        else if (str[0].Equals("添加题库"))
                        {
                            TextAdd(str[2]);
                        }
                        else if (str[0].Equals("生成考卷"))
                        {
                            TextSet(str);
                        }
                        else if (str[0].Equals("生成答卷"))
                        {
                            TextAdd(str[1]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("远程服务器已经中断连接");
                    this.button1.Enabled = true;
                    break;
                }
            }
        }

        /// <summary>
        /// 生成考卷触发
        /// </summary>
        private void TextSet(string[] str)
        {
            if (str.Length != 72 )
            {
                MessageBox.Show("获取试题出错");
                return;
            }
            Form211.getText(str);
        }

        /// <summary>
        /// 登录触发
        /// </summary>
        private void Login(string[] str)
        {
            if (str[2].Equals("True"))
            {
                MessageBox.Show("验证成功");
                button5.Enabled = true;
                button2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                if (str[1].Equals("学生"))
                {
                    flag = 1;
                }
                else if(str[1].Equals("教师"))
                {
                    flag = 2;
                }
                else if(str[1].Equals("管理员"))
                {
                    flag = 3;
                }
            }
            else if (str[2].Equals("False"))
            {
                MessageBox.Show("学号/编号或密码错误");
            }
            else
            {
                MessageBox.Show("传输出错");
            }
        }

        /// <summary>
        /// 修改密码触发
        /// </summary>
        private void Modify(string str)
        {
            if (str.Equals("True"))
            {
                MessageBox.Show("修改密码成功");
            }
            else if (str.Equals("False"))
            {
                MessageBox.Show("修改密码失败");
            }
            else
            {
                MessageBox.Show("传输出错");
            }
        }

        /// <summary>
        /// 注册触发
        /// </summary>
        private void Register(string str)
        {
            if (str.Equals("True"))
            {
                MessageBox.Show("添加用户成功");
            }
            else if (str.Equals("False"))
            {
                MessageBox.Show("添加用户失败");
            }
            else
            {
                MessageBox.Show("传输出错");
            }
        }

        /// <summary>
        /// 删除用户触发
        /// </summary>
        private void Delete(string str)
        {
            if (str.Equals("True"))
            {
                MessageBox.Show("删除用户成功");
            }
            else if (str.Equals("False"))
            {
                MessageBox.Show("删除用户失败");
            }
            else
            {
                MessageBox.Show("传输出错");
            }
        }

        /// <summary>
        /// 查询学生
        /// </summary>
        private void StuSelect(string[] str)
        {
            if (str[1].Equals("教师"))
            {
                Form22.getStrStu(str);
                MessageBox.Show("查询成功");
                /*Form22.dataGridView1.Rows.Clear();
                for (int i = 2; i + 1 < str.Length; i += 2)
                {
                    Form22.dataGridView1.Rows.Add(str[i], str[i + 1]);
                }*/
            }
            else if (str[1].Equals("管理员"))
            {
                Form231.getStrStu(str);
                MessageBox.Show("查询成功");
                /*Form231.dataGridView1.Rows.Clear();
                for (int i = 2; i + 2 < str.Length; i += 3)
                {
                    Form231.dataGridView1.Rows.Add(str[i], str[i + 1], str[i + 2]);
                }*/
            }
            else if (str[1].Equals("学生"))
            {
                Form21.getStrStu(str);
                MessageBox.Show("查询成功");
            }
            else
            {
                MessageBox.Show("查询失败");
            }
        }

        /// <summary>
        /// 查询教师
        /// </summary>
        private void TeaSelect(string[] str)
        {
            if (str[1].Equals("学生"))
            {
                Form21.getStrTea(str);
                MessageBox.Show("查询成功");
                /*Form21.dataGridView1.Rows.Clear();
                for (int i = 2; i + 2 < str.Length; i += 3)
                {
                    Form21.dataGridView1.Rows.Add(str[i], str[i + 1], str[i + 2]);
                }*/
            }
            else if (str[1].Equals("管理员"))
            {
                Form23.getStrTea(str);
                MessageBox.Show("查询成功");
                /*Form23.dataGridView1.Rows.Clear();
                for (int i = 2; i + 4 < str.Length; i += 5)
                {
                    Form23.dataGridView1.Rows.Add(str[i], str[i + 1], str[i + 2], str[i + 3], str[i + 4]);
                }*/
            }
            else
            {
                MessageBox.Show("查询失败");
            }
        }

        /// <summary>
        /// 查询题库
        /// </summary>
        private void TextSelect(string[] str)
        {
            if (str[1].Equals("教师"))
            {
                Form22.getStrText(str);
                MessageBox.Show("查询成功");
                /*Form22.dataGridView2.Rows.Clear();
                for (int i = 2; i + 6 < str.Length; i += 7)
                {
                    Form22.dataGridView2.Rows.Add(str[i], str[i + 1], str[i + 2], str[i + 3], str[i + 4], str[i + 5], str[i + 6]);
                }*/
            }
            else if (str[1].Equals("管理员"))
            {
                Form232.getStrText(str);
                MessageBox.Show("查询成功");
                /*Form232.dataGridView1.Rows.Clear();
                for (int i = 2; i + 7 < str.Length; i += 8)
                {
                    Form232.dataGridView1.Rows.Add(str[i], str[i + 1], str[i + 2], str[i + 3], str[i + 4],
                       str[i + 5], str[i + 6], str[i + 7]);
                }*/
            }
            else
            {
                MessageBox.Show("查询失败");
            }
        }

        /// <summary>
        /// 添加题库，答卷触发
        /// </summary>
        private void TextAdd(string str)
        {
            if (str.Equals("True"))
            {
                MessageBox.Show("添加成功");
            }
            else if (str.Equals("False"))
            {
                MessageBox.Show("添加失败");
            }
            else
            {
                MessageBox.Show("传输出错");
            }
        }

        /// <summary>
        /// 获取当前系统时间的方法
        /// 当前时间
        /// </summary> 
        protected DateTime GetCurrentTime()
        {
            DateTime currentTime = new DateTime();
            currentTime = DateTime.Now;
            return currentTime;
        }

        /// <summary>
        /// 发送字符信息到服务端的方法
        /// </summary>
        public static void ClientSendMsg(string sendMsg)
        {
            //将输入的内容字符串转换为机器可以识别的字节数组   
            byte[] arrClientSendMsg = Encoding.UTF8.GetBytes(sendMsg);
            //调用客户端套接字发送字节数组   
            socketclient.Send(arrClientSendMsg);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //SocketException exception;
            this.button1.Enabled = false;

            //定义一个套接字监听
            socketclient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //获取文本框中的IP地址
            IPAddress address = IPAddress.Parse(textBox1.Text.Trim());

            //将获取的IP地址和端口号绑定在网络节点上
            IPEndPoint point = new IPEndPoint(address, int.Parse(textBox2.Text.Trim()));

            try
            {
                //客户端套接字连接到网络节点上，用的是Connect
                socketclient.Connect(point);
            }
            catch (Exception)
            {
                MessageBox.Show("连接失败\r\n");
                this.button1.Enabled = true;
                return;
            }

            threadclient = new Thread(recv);

            threadclient.IsBackground = true;

            threadclient.Start();
        }

        private bool check()
        {
            if (textBox3.Text.Trim() == "" || textBox4.Text.Trim() == "")
            {
                return false;
            }
            return true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!check())
            {
                MessageBox.Show("请输入数据");
                return;
            }
            string str = "登录@学生@" + textBox3.Text.Trim() + "@" + textBox4.Text.Trim();
            ClientSendMsg(str);
            textBox4.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!check())
            {
                MessageBox.Show("请输入数据");
                return;
            }
            string str = "登录@管理员@" + textBox3.Text.Trim() + "@" + textBox4.Text.Trim();
            ClientSendMsg(str);
            textBox4.Clear();
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!check())
            {
                MessageBox.Show("请输入数据");
                return;
            }
            string str = "登录@教师@" + textBox3.Text.Trim() + "@" + textBox4.Text.Trim();
            ClientSendMsg(str);
            textBox4.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button5.Enabled = false;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            if (flag == 1)
            {
                flag = 0;
                bool isfind = false;
                Hide();
                foreach (Form fm in Application.OpenForms)
                {
                    if (fm.Name == "Form21")
                    {
                        fm.WindowState = FormWindowState.Normal;
                        fm.Tag = textBox3.Text.Trim();
                        fm.Show();
                        fm.Activate();
                        return;
                    }
                }
                if (!isfind)
                {
                    Form fm = new Form21();
                    fm.Tag = textBox3.Text.Trim();
                    fm.Show();
                }
            }
            else if (flag == 2)
            {
                flag = 0;
                bool isfind = false;
                Hide();
                foreach (Form fm in Application.OpenForms)
                {
                    if (fm.Name == "Form22")
                    {
                        fm.WindowState = FormWindowState.Normal;
                        fm.Tag = textBox3.Text.Trim();
                        fm.Show();
                        fm.Activate();
                        return;
                    }
                }
                if (!isfind)
                {
                    Form fm = new Form22();
                    fm.Tag = textBox3.Text.Trim();
                    fm.Show();
                }
            }
            else if (flag == 3)
            {
                flag = 0;
                bool isfind = false;
                Hide();
                foreach (Form fm in Application.OpenForms)
                {
                    if (fm.Name == "Form23")
                    {
                        fm.WindowState = FormWindowState.Normal;
                        fm.Tag = textBox3.Text.Trim();
                        fm.Show();
                        fm.Activate();
                        return;
                    }
                }
                if (!isfind)
                {
                    Form fm = new Form23();
                    fm.Tag = textBox3.Text.Trim();
                    fm.Show();
                }
            }
        }
    }
}
