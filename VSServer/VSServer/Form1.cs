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
using System.Collections;
using System.Data.SqlClient;

namespace VSServer
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

        string RemoteEndPoint;     //客户端的网络结点

        Thread threadwatch = null;//负责监听客户端的线程
        Socket socketwatch = null;//负责监听客户端的套接字

        //创建一个和客户端通信的套接字
        Dictionary<string, Socket> dic = new Dictionary<string, Socket> { };   //定义一个集合，存储客户端信息

        //连接信息添加
        void OnlineList_Disp(string Info)
        {
            listBoxOnlineList.Items.Add(Info);   //在线列表中显示连接的客户端套接字
        }

        /// <summary>
        /// 监听客户端发来的请求
        /// </summary>
        private void watchConnecting()
        {
            Socket connection = null;
            while (true)  //持续不断监听客户端发来的请求   
            {
                try
                {
                    connection = socketwatch.Accept();
                }
                catch (Exception ex)
                {
                    textBox3.AppendText(ex.Message); //提示套接字监听异常   
                    break;
                }

                /*
                //获取客户端的IP和端口号
                IPAddress clientIP = (connection.RemoteEndPoint as IPEndPoint).Address;
                int clientPort = (connection.RemoteEndPoint as IPEndPoint).Port;

                //让客户显示"连接成功的"的信息
                string sendmsg = "连接服务端成功！\r\n" + "本地IP:" + clientIP + "，本地端口" + clientPort.ToString();
                byte[] arrSendMsg = Encoding.UTF8.GetBytes(sendmsg);
                connection.Send(arrSendMsg);
                */

                string sendmsg = "连接成功";
                byte[] arrSendMsg = Encoding.UTF8.GetBytes(sendmsg);
                connection.Send(arrSendMsg);

                RemoteEndPoint = connection.RemoteEndPoint.ToString(); //客户端网络结点号
                //textBox3.AppendText("成功与" + RemoteEndPoint + "客户端建立连接！\t\n");     //显示与客户端连接情况
                dic.Add(RemoteEndPoint, connection);    //添加客户端信息

                OnlineList_Disp(RemoteEndPoint);    //显示在线客户端

                //IPEndPoint netpoint = new IPEndPoint(clientIP,clientPort);

                IPEndPoint netpoint = connection.RemoteEndPoint as IPEndPoint;

                //创建一个通信线程    
                ParameterizedThreadStart pts = new ParameterizedThreadStart(recv);
                Thread thread = new Thread(pts);
                thread.IsBackground = true;//设置为后台线程，随着主线程退出而退出   
                //启动线程   
                thread.Start(connection);
            }
        }

        /// <summary>
        /// 接收客户端发来的信息
        /// 客户端套接字对象
        /// </summary>  
        private void recv(object socketclientpara)
        {
            Socket socketServer = socketclientpara as Socket;
            while (true)
            {
                //创建一个内存缓冲区 其大小为1024*1024字节  即1M   
                byte[] arrServerRecMsg = new byte[1024 * 1024];
                //将接收到的信息存入到内存缓冲区,并返回其字节数组的长度  
                try
                {
                    int length = socketServer.Receive(arrServerRecMsg);

                    //将机器接受到的字节数组转换为人可以读懂的字符串   
                    string strSRecMsg = Encoding.UTF8.GetString(arrServerRecMsg, 0, length);

                    textBox3.AppendText(strSRecMsg + "\r\n");//测试

                    string[] str = strSRecMsg.Split('@');
                    string x = "";
                    if (str[0].Equals("登录"))
                    {
                        if (str[1].Equals("学生"))
                        {
                            x = "登录@学生@" + StuLogin(str).ToString();
                        }
                        else if (str[1].Equals("教师"))
                        {
                            x = "登录@教师@" + TeaLogin(str).ToString();
                        }
                        else if (str[1].Equals("管理员"))
                        {
                            x = "登录@管理员@" + AdmLogin(str).ToString();
                        }
                    }
                    else if (str[0].Equals("注销"))
                    {
                        Cancel(str);
                    }
                    else if (str[0].Equals("删除学生"))
                    {
                        if (str[1].Equals("教师"))//教师，管理员操作
                        {
                            x = "删除@学生@" + StuDeleteTea(str).ToString();
                        }
                        else if (str[1].Equals("管理员"))//管理员操作
                        {
                            x = "删除@学生@" + StuDeleteAdm(str[2]).ToString();
                        }
                    }
                    else if (str[0].Equals("删除教师"))
                    {
                        x = "删除@教师@" + TeaDelete(str[2]).ToString();
                    }
                    else if (str[0].Equals("注册学生"))//教师，管理员操作
                    {
                        if (str[1].Equals("教师"))//教师操作
                        {
                            x = "注册@学生@" + StuInsertTea(str).ToString();
                        }
                        else if (str[1].Equals("管理员"))//管理员操作
                        {
                            x = "注册@学生@" + StuInsertAdm(str).ToString();
                        }
                    }
                    else if (str[0].Equals("注册教师"))//管理员操作
                    {
                        x = "注册@教师@" + TeaInsert(str).ToString();
                    }
                    else if (str[0].Equals("添加题库"))//题库；教师管，理员操作
                    {
                        if (str[1].Equals("教师"))
                        {
                            x = "添加题库@教师@" + TextAddTea(str).ToString();
                        }
                        else if (str[1].Equals("管理员"))
                        {
                            x = "添加题库@管理员@" + TextAddAdm(str).ToString();
                        }
                    }
                    else if (str[0].Equals("删除题库"))
                    {
                        if (str[1].Equals("教师"))
                        {
                            x = "删除@题库@" + TextDeleteTea(str).ToString();
                        }
                        else if (str[1].Equals("管理员"))
                        {
                            x = "删除@题库@" + TextDeleteAdm(str[2]).ToString();
                        }
                    }
                    else if (str[0].Equals("清空题库"))
                    {
                        x = "删除@题库@" + TextClear();
                    }
                    else if (str[0].Equals("查询题库"))
                    {
                        if (str[1].Equals("教师"))
                        {
                            x = "查询题库@教师" + TextSelectTea(str[2]);   //编号
                        }
                        else if (str[1].Equals("管理员"))
                        {
                            x = "查询题库@管理员" + TextSelectAbm(str[2]);   //编号
                        }
                    }
                    else if (str[0].Equals("查询学生"))
                    {
                        if (str[1].Equals("教师"))
                        {
                            x = "查询学生@教师" + StuSelectTea(str[2]);   //编号
                        }
                        else if (str[1].Equals("管理员"))
                        {
                            x = "查询学生@管理员" + StuSelectAbm(str);
                        }
                        else if (str[1].Equals("学生"))
                        {
                            x = "查询学生@学生" + StuSelectStu(str[2]);
                        }
                    }
                    else if (str[0].Equals("查询教师"))
                    {
                        if (str[1].Equals("学生"))
                        {
                            x = "查询教师@学生" + TeaSelectStu(str[2]);   //学号
                        }
                        else if (str[1].Equals("管理员"))
                        {
                            x = "查询教师@管理员" + TeaSelectAbm(str);
                        }
                    }
                    else if (str[0].Equals("修改密码"))
                    {
                        if (str[1].Equals("学生"))
                        {
                            x = "修改密码@学生@" + StuModify(str).ToString();
                        }
                        else if (str[1].Equals("教师"))
                        {
                            x = "修改密码@教师@" + TeaModify(str).ToString();
                        }
                        else if (str[1].Equals("管理员"))
                        {
                            x = "修改密码@管理员@" + AdmModify(str).ToString(); 
                        }
                    }
                    else if (str[0].Equals("生成考卷"))
                    {
                        x = "生成考卷@" + str[1] + setText(str[1]);
                    }
                    else
                    {
                        x = "会话@无效传输";
                    }

                    textBox3.AppendText(x + "\r\n");//测试

                    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(x);   //将要发送的信息转化为字节数组，因为Socket发送数据时是以字节的形式发送的
                    dic["" + socketServer.RemoteEndPoint].Send(bytes);   //发送数据
                    //将发送的字符串信息附加到文本框txtMsg上   
                    //textBox3.AppendText("客户端:" + socketServer.RemoteEndPoint + ",time:" + GetCurrentTime() + "\r\n" + strSRecMsg + "\r\n\n");
                }
                catch (Exception ex)
                {
                    textBox3.AppendText("客户端" + socketServer.RemoteEndPoint + "已经中断连接" + "\r\n"); //提示套接字监听异常 
                    listBoxOnlineList.Items.Remove(socketServer.RemoteEndPoint.ToString());//从listbox中移除断开连接的客户端
                    socketServer.Close();//关闭之前accept出来的和客户端进行通信的套接字
                    break;
                }
            }
        }

        /// <summary>
        /// 生成考卷
        /// </summary>
        private string setText(string str)
        {
            SqlConnection myCon = new SqlConnection
                    ("Persist Security Info = False; User id = sa; pwd = 123456; database = vstext; server = .");
            myCon.Open();
            string select = "";
            string sqlStr = "select top 10 * from 题库 where 科目 = '" + str +"' ORDER BY NEWID()";
            SqlDataAdapter da = new SqlDataAdapter(sqlStr, myCon);
            DataSet ds = new DataSet();
            da.Fill(ds);
            DataTable dt = ds.Tables[0];
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    select += "@" + dt.Rows[i]["编号"].ToString() + "@" + dt.Rows[i]["题目"].ToString()
                        + "@" + dt.Rows[i]["A"].ToString() + "@" + dt.Rows[i]["B"].ToString() + "@" + dt.Rows[i]["C"].ToString()
                        + "@" + dt.Rows[i]["D"].ToString() + "@" + dt.Rows[i]["答案"].ToString();
                }
            }
            catch
            {
                select = "@False";
            }
            myCon.Close();
            return select;
        }

        /// <summary>
        /// 学生登录验证
        /// <summary>
        private bool StuLogin(string[] str)
        {
            SqlConnection myCon;
            try
            {
                myCon = new SqlConnection
                    ("Persist Security Info = False; User id = sa; pwd = 123456; database = vstext; server = .");
                myCon.Open();
            }
            catch
            {
                textBox3.AppendText("未连接上数据库\r\n");
                return false;
            }
            string sqlStr = "select 学号, 密码 from 学生 where 学号 ='" + str[2] + "'";
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(sqlStr, myCon);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                string pwd = Convert.ToString(dt.Rows[0]["密码"]).Trim();
                myCon.Close();
                if (!str[3].Equals(pwd))
                {
                    return false;
                }
                else
                {
                    textBox3.AppendText("学生:" + str[2] + "   登录成功。\r\n");
                    return true;
                }
            }
            catch
            {
                myCon.Close();
                return false;
            }
        }

        /// <summary>
        /// 教师登录验证
        /// <summary>
        private bool TeaLogin(string[] str)
        {
            SqlConnection myCon;
            try
            {
                myCon = new SqlConnection
                    ("Persist Security Info = False; User id = sa; pwd = 123456; database = vstext; server = .");
                myCon.Open();
            }
            catch
            {
                textBox3.AppendText("未连接上数据库\r\n");
                return false;
            }
            string sqlStr = "select 编号, 密码 from 教师 where 编号 ='" + str[2] + "'";
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(sqlStr, myCon);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                string pwd = Convert.ToString(dt.Rows[0]["密码"]).Trim();
                myCon.Close();
                if (!str[3].Equals(pwd))
                {
                    return false;
                }
                else
                {
                    textBox3.AppendText("教师:" + str[2] + "   登录成功。\r\n");
                    return true;
                }
            }
            catch
            {
                myCon.Close();
                return false;
            }
        }

        /// <summary>
        /// 管理员登录验证
        /// <summary>
        private bool AdmLogin(string[] str)
        {
            SqlConnection myCon;
            try
            {
                myCon = new SqlConnection
                    ("Persist Security Info = False; User id = sa; pwd = 123456; database = vstext; server = .");
                myCon.Open();
            }
            catch
            {
                textBox3.AppendText("未连接上数据库\r\n");
                return false;
            }
            string sqlStr = "select 编号, 密码 from 管理员 where 编号 ='" + str[2] + "'";
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(sqlStr, myCon);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                string pwd = Convert.ToString(dt.Rows[0]["密码"]).Trim();
                myCon.Close();
                if (!str[3].Equals(pwd))
                {
                    return false;
                }
                else
                {
                    textBox3.AppendText("管理员:" + str[2] + "   登录成功。\r\n");
                    return true;
                }
            }
            catch
            {
                myCon.Close();
                return false;
            }
        }

        /// <summary>
        /// 注销登录
        /// </summary>
        private void Cancel(string[] str)
        {
            textBox3.AppendText(str[1] +":"+ str[2] + "   注销。");
        }

        /// <summary>
        /// 学生删除账号 管理员
        /// </summary>
        private bool StuDeleteAdm(string str)
        {
            SqlConnection myCon;
            try
            {
                myCon = new SqlConnection
                    ("Persist Security Info = False; User id = sa; pwd = 123456; database = vstext; server = .");
                myCon.Open();
            }
            catch
            {
                textBox3.AppendText("未连接上数据库\r\n");
                return false;
            }
            string sqlStr = "delete from 学生 where 学号 ='" + str + "'";
            try
            {
                SqlCommand deleteCMD = new SqlCommand(sqlStr, myCon);
                deleteCMD.ExecuteNonQuery();
                myCon.Close();
                return true;
            }
            catch
            {
                myCon.Close();
                return false;
            }
        }

        /// <summary>
        /// 学生删除账号 教师
        /// </summary>
        private bool StuDeleteTea(string[] str)
        {
            SqlConnection myCon;
            try
            {
                myCon = new SqlConnection
                    ("Persist Security Info = False; User id = sa; pwd = 123456; database = vstext; server = .");
                myCon.Open();
            }
            catch
            {
                textBox3.AppendText("未连接上数据库\r\n");
                return false;
            }
            string sqlStr = "select 班级 form 学生 where 学号 = '" + str[3] + "'";
            string sqlStr0 = "select 班级 from 教师 where 编号 = '" + str[2] + "'";
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(sqlStr, myCon);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                string str1 = Convert.ToString(dt.Rows[0]["班级"]).Trim();
                da = new SqlDataAdapter(sqlStr0, myCon);
                ds = new DataSet();
                da.Fill(ds);
                dt = ds.Tables[0];
                string str2 = Convert.ToString(dt.Rows[0]["班级"]).Trim();
                if (!str1.Equals(str2))
                {
                    textBox3.AppendText("正常\r\n");
                    myCon.Close();
                    return false;
                }
            }
            catch
            {
                textBox3.AppendText("异常1\r\n");
                myCon.Close();
                return false;
            }
            sqlStr = "delete from 学生 where 学号 ='" + str[3] + "'";
            try
            {
                SqlCommand deleteCMD = new SqlCommand(sqlStr, myCon);
                deleteCMD.ExecuteNonQuery();
                myCon.Close();
                return true;
            }
            catch
            {
                textBox3.AppendText("异常2\r\n");
                myCon.Close();
                return false;
            }
        }

        /// <summary>
        /// 教师删除账号
        /// </summary>
        private bool TeaDelete(string str)
        {
            SqlConnection myCon;
            try
            {
                myCon = new SqlConnection
                    ("Persist Security Info = False; User id = sa; pwd = 123456; database = vstext; server = .");
                myCon.Open();
            }
            catch
            {
                textBox3.AppendText("未连接上数据库\r\n");
                return false;
            }
            string sqlStr = "delete from 教师 where 编号 ='" + str + "'";
            try
            {
                SqlCommand deleteCMD = new SqlCommand(sqlStr, myCon);
                deleteCMD.ExecuteNonQuery();
                myCon.Close();
                return true;
            }
            catch
            {
                myCon.Close();
                return false;
            }
        }

        /// <summary>
        /// 题库删除 教师
        /// </summary>
        private bool TextDeleteTea(string[] str)
        {
            SqlConnection myCon;
            try
            {
                myCon = new SqlConnection
                    ("Persist Security Info = False; User id = sa; pwd = 123456; database = vstext; server = .");
                myCon.Open();
            }
            catch
            {
                textBox3.AppendText("未连接上数据库\r\n");
                return false;
            }
            string sqlStr = "select 科目 form 题库 where 编号 = '" + str[3] + "'";
            string sqlStr0 = "select 科目 from 教师 where 编号 = '" + str[2] + "'";
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(sqlStr, myCon);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                string str1 = Convert.ToString(dt.Rows[0]["科目"]).Trim();
                da = new SqlDataAdapter(sqlStr0, myCon);
                ds = new DataSet();
                da.Fill(ds);
                dt = ds.Tables[0];
                string str2 = Convert.ToString(dt.Rows[0]["科目"]).Trim();
                if (!str1.Equals(str2))
                {
                    textBox3.AppendText("正常\r\n");
                    myCon.Close();
                    return false;
                }
            }
            catch
            {
                textBox3.AppendText("异常1\r\n");
                myCon.Close();
                return false;
            }
            sqlStr = "delete from 题库 where 编号 ='" + str[3] + "'";
            try
            {
                SqlCommand deleteCMD = new SqlCommand(sqlStr, myCon);
                deleteCMD.ExecuteNonQuery();
                myCon.Close();
                return true;
            }
            catch
            {
                textBox3.AppendText("异常2\r\n");
                myCon.Close();
                return false;
            }
        }

        /// <summary>
        /// 题库删除 管理员
        /// </summary>
        private bool TextDeleteAdm(string str)
        {
            SqlConnection myCon;
            try
            {
                myCon = new SqlConnection
                    ("Persist Security Info = False; User id = sa; pwd = 123456; database = vstext; server = .");
                myCon.Open();
            }
            catch
            {
                textBox3.AppendText("未连接上数据库\r\n");
                return false;
            }
            string sqlStr = "delete from 题库 where 编号 ='" + str + "'";
            try
            {
                SqlCommand deleteCMD = new SqlCommand(sqlStr, myCon);
                deleteCMD.ExecuteNonQuery();
                myCon.Close();
                return true;
            }
            catch
            {
                myCon.Close();
                return false;
            }
        }

        /// <summary>
        /// 清空题库
        /// </summary>
        private bool TextClear()
        {
            SqlConnection myCon;
            try
            {
                myCon = new SqlConnection
                    ("Persist Security Info = False; User id = sa; pwd = 123456; database = vstext; server = .");
                myCon.Open();
            }
            catch
            {
                textBox3.AppendText("未连接上数据库\r\n");
                return false;
            }
            string sqlStr = "truncate table 题库";
            try
            {
                SqlCommand deleteCMD = new SqlCommand(sqlStr, myCon);
                deleteCMD.ExecuteNonQuery();
                myCon.Close();
                return true;
            }
            catch
            {
                myCon.Close();
                return false;
            }
        }

        /// <summary>
        /// 获取当前系统时间的方法
        /// 当前时间
        /// </summary>  
        private DateTime GetCurrentTime()
        {
            DateTime currentTime = new DateTime();
            currentTime = DateTime.Now;
            return currentTime;
        }

        /// <summary>
        /// 学生注册 教师
        /// </summary>
        private bool StuInsertTea(string[] str)
        {
            SqlConnection myCon;
            try
            {
                myCon = new SqlConnection
                    ("Persist Security Info = False; User id = sa; pwd = 123456; database = vstext; server = .");
                myCon.Open();
            }
            catch
            {
                textBox3.AppendText("未连接上数据库\r\n");
                return false;
            }
            string sqlStr = "select 班级 from 教师 where 编号 = '" + str[2] + "'";
            string cnum = "";
            SqlDataAdapter da = new SqlDataAdapter(sqlStr, myCon);
            DataSet ds = new DataSet();
            da.Fill(ds);
            try
            {
                DataTable dt = ds.Tables[0];
                cnum = Convert.ToString(dt.Rows[0]["班级"]).Trim();
            }
            catch
            {
                myCon.Close();
                return false;
            }
            sqlStr = "insert into 学生 (学号, 密码, 姓名, 班级) values ('" + str[3] + "','"
                + 123456 + "','" + str[4] + "','" + cnum.Trim() + "')";
            try
            {
                SqlCommand addCMD = new SqlCommand(sqlStr, myCon);
                addCMD.ExecuteNonQuery();
                myCon.Close();
                return true;
            }
            catch
            {
                myCon.Close();
                return false;
            }
        }

        /// <summary>
        /// 学生注册 管理员
        /// </summary>
        private bool StuInsertAdm(string[] str)
        {
            SqlConnection myCon;
            try
            {
                myCon = new SqlConnection
                    ("Persist Security Info = False; User id = sa; pwd = 123456; database = vstext; server = .");
                myCon.Open();
            }
            catch
            {
                textBox3.AppendText("未连接上数据库\r\n");
                return false;
            }
            string sqlStr = "insert into 学生 (学号, 密码, 姓名, 班级) values ('" + str[2] + "','"
                + 123456 + "','" + str[3] + "','" + str[4] + "')";
            try
            {
                SqlCommand addCMD = new SqlCommand(sqlStr, myCon);
                addCMD.ExecuteNonQuery();
                myCon.Close();
                return true;
            }
            catch
            {
                myCon.Close();
                return false;
            }
        }

        /// <summary>
        /// 教师注册
        /// </summary>
        private bool TeaInsert(string[] str)
        {
            SqlConnection myCon;
            try
            {
                myCon = new SqlConnection
                    ("Persist Security Info = False; User id = sa; pwd = 123456; database = vstext; server = .");
                myCon.Open();
            }
            catch
            {
                textBox3.AppendText("未连接上数据库\r\n");
                return false;
            }
            string sqlStr = "insert into 教师 (编号, 密码, 姓名, 班级, 科目, 电话) values ('" + str[2] + "','"
                + 123456 + "','" + str[3] + "','" + str[4] + "','" + str[5] + "','" + str[6] + "')";
            try
            {
                SqlCommand addCMD = new SqlCommand(sqlStr, myCon);
                addCMD.ExecuteNonQuery();
                myCon.Close();
                return true;
            }
            catch
            {
                myCon.Close();
                return false;
            }
        }

        /// <summary>
        /// 学生修改密码
        /// </summary>
        private bool StuModify(string[] str)
        {
            SqlConnection myCon;
            try
            {
                myCon = new SqlConnection
                    ("Persist Security Info = False; User id = sa; pwd = 123456; database = vstext; server = .");
                myCon.Open();
            }
            catch
            {
                textBox3.AppendText("未连接上数据库\r\n");
                return false;
            }
            string sqlStr = "select 密码 from 学生 where 学号 ='" + str[2] + "'";
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(sqlStr, myCon);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                string pwd = Convert.ToString(dt.Rows[0]["密码"]).Trim();
                if (!str[3].Equals(pwd))
                {
                    return false;
                }
            }
            catch
            {
                myCon.Close();
                return false;
            }
            sqlStr = "update 学生 set 密码 = '"+ str[4] +"' where 学号 = '"+ str[2] +"'";
            try
            {
                SqlCommand addCMD = new SqlCommand(sqlStr, myCon);
                addCMD.ExecuteNonQuery();
                myCon.Close();
                return true;
            }
            catch
            {
                myCon.Close();
                return false;
            }
        }

        /// <summary>
        /// 教师修改密码
        /// </summary>
        private bool TeaModify(string[] str)
        {
            SqlConnection myCon;
            try
            {
                myCon = new SqlConnection
                    ("Persist Security Info = False; User id = sa; pwd = 123456; database = vstext; server = .");
                myCon.Open();
            }
            catch
            {
                textBox3.AppendText("未连接上数据库\r\n");
                return false;
            }
            string sqlStr = "select 编号, 密码 from 教师 where 编号 ='" + str[2] + "'";
            SqlDataAdapter da = new SqlDataAdapter(sqlStr, myCon);
            DataSet ds = new DataSet();
            da.Fill(ds);
            try
            {
                DataTable dt = ds.Tables[0];
                string pwd = Convert.ToString(dt.Rows[0]["密码"]).Trim();
                if (!str[3].Equals(pwd))
                {
                    return false;
                }
            }
            catch
            {
                myCon.Close();
                return false;
            }
            sqlStr = "update 教师 set 密码 = '" + str[4] + "' where 编号 = '" + str[2] + "'";
            try
            {
                SqlCommand addCMD = new SqlCommand(sqlStr, myCon);
                addCMD.ExecuteNonQuery();
                myCon.Close();
                return true;
            }
            catch
            {
                myCon.Close();
                return false;
            }
        }

        /// <summary>
        /// 管理员修改密码
        /// </summary>
        private bool AdmModify(string[] str)
        {
            SqlConnection myCon;
            try
            {
                myCon = new SqlConnection
                    ("Persist Security Info = False; User id = sa; pwd = 123456; database = vstext; server = .");
                myCon.Open();
            }
            catch
            {
                textBox3.AppendText("未连接上数据库\r\n");
                return false;
            }
            string sqlStr = "select 编号, 密码 from 管理员 where 编号 ='" + str[2] + "'";
            SqlDataAdapter da = new SqlDataAdapter(sqlStr, myCon);
            DataSet ds = new DataSet();
            da.Fill(ds);
            try
            {
                DataTable dt = ds.Tables[0];
                string pwd = Convert.ToString(dt.Rows[0]["密码"]).Trim();
                if (!str[3].Equals(pwd))
                {
                    return false;
                }
            }
            catch
            {
                textBox3.AppendText("异常1\r\n");
                myCon.Close();
                return false;
            }
            sqlStr = "update 管理员 set 密码 = '" + str[4] + "' where 编号 = '" + str[2] + "'";
            try
            {
                SqlCommand addCMD = new SqlCommand(sqlStr, myCon);
                addCMD.ExecuteNonQuery();
                myCon.Close();
                return true;
            }
            catch
            {
                textBox3.AppendText("异常2\r\n");
                myCon.Close();
                return false;
            }
        }

        /// <summary>
        /// 查询学生 学生
        /// </summary>
        private string StuSelectStu(string str)
        {
            SqlConnection myCon;
            try
            {
                myCon = new SqlConnection
                    ("Persist Security Info = False; User id = sa; pwd = 123456; database = vstext; server = .");
                myCon.Open();
            }
            catch
            {
                textBox3.AppendText("未连接上数据库\r\n");
                return "@False";
            }
            string select = "";
            string sqlStr = "select 学号, 姓名 from 学生 where 班级 in ( select 班级 from 学生 where 学号 = '" + str + "')";
            SqlDataAdapter da = new SqlDataAdapter(sqlStr, myCon);
            DataSet ds = new DataSet();
            da.Fill(ds);
            DataTable dt = ds.Tables[0];
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    select += "@" + dt.Rows[i]["学号"].ToString() + "@" + dt.Rows[i]["姓名"].ToString();
                }
            }
            catch
            {
                select = "@False";
            }
            myCon.Close();
            return select;
        }

        /// <summary>
        /// 查询学生 教师
        /// </summary>
        private string StuSelectTea(string str)
        {
            SqlConnection myCon;
            try
            {
                myCon = new SqlConnection
                    ("Persist Security Info = False; User id = sa; pwd = 123456; database = vstext; server = .");
                myCon.Open();
            }
            catch
            {
                textBox3.AppendText("未连接上数据库\r\n");
                return "@False";
            }
            string select = "";
            string sqlStr = "select 学号, 姓名 from 学生 where 班级 in ( select 班级 from 教师 where 编号 = '" + str + "')";
            SqlDataAdapter da = new SqlDataAdapter(sqlStr, myCon);
            DataSet ds = new DataSet();
            da.Fill(ds);
            DataTable dt = ds.Tables[0];
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    select += "@"+ dt.Rows[i]["学号"].ToString() +"@"+ dt.Rows[i]["姓名"].ToString();
                }
            }
            catch
            {
                select = "@False";
            }
            myCon.Close();
            return select;
        }

        /// <summary>
        /// 查询学生 管理员
        /// </summary>
        private string StuSelectAbm(string[] str)
        {
            SqlConnection myCon;
            try
            {
                myCon = new SqlConnection
                    ("Persist Security Info = False; User id = sa; pwd = 123456; database = vstext; server = .");
                myCon.Open();
            }
            catch
            {
                textBox3.AppendText("未连接上数据库\r\n");
                return "@False";
            }
            string select = "";
            string sqlStr = "select 学号, 姓名, 班级 from 学生";
            SqlDataAdapter da = new SqlDataAdapter(sqlStr, myCon);
            DataSet ds = new DataSet();
            da.Fill(ds);
            DataTable dt = ds.Tables[0];
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    select += "@"+ dt.Rows[i]["学号"].ToString() +"@"+ dt.Rows[i]["姓名"].ToString()
                        +"@"+ dt.Rows[i]["班级"].ToString();
                }
            }
            catch
            {
                select = "@False";
            }
            myCon.Close();
            return select;
        }

        /// <summary>
        /// 查询教师 学生
        /// </summary>
        private string TeaSelectStu(string str)
        {
            SqlConnection myCon;
            try
            {
                myCon = new SqlConnection
                    ("Persist Security Info = False; User id = sa; pwd = 123456; database = vstext; server = .");
                myCon.Open();
            }
            catch
            {
                textBox3.AppendText("未连接上数据库\r\n");
                return "@False";
            }
            string select = "";
            string sqlStr = "select 科目, 姓名, 电话 from 教师 where 班级 in ( select 班级 from 学生 where 学号 = '" + str + "')";
            SqlDataAdapter da = new SqlDataAdapter(sqlStr, myCon);
            DataSet ds = new DataSet();
            da.Fill(ds);
            DataTable dt = ds.Tables[0];
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    select += "@" + dt.Rows[i]["科目"].ToString() + "@" + dt.Rows[i]["姓名"].ToString()
                        + "@" + dt.Rows[i]["电话"].ToString();
                }
            }
            catch
            {
                select = "@False";
            }
            myCon.Close();
            return select;
        }

        /// <summary>
        /// 查询教师 管理员
        /// </summary>
        private string TeaSelectAbm(string[] str)
        {
            SqlConnection myCon;
            try
            {
                myCon = new SqlConnection
                    ("Persist Security Info = False; User id = sa; pwd = 123456; database = vstext; server = .");
                myCon.Open();
            }
            catch
            {
                textBox3.AppendText("未连接上数据库\r\n");
                return "@False";
            }
            string select = "";
            string sqlStr = "select 编号, 姓名, 科目, 班级, 电话 from 教师";
            SqlDataAdapter da = new SqlDataAdapter(sqlStr, myCon);
            DataSet ds = new DataSet();
            da.Fill(ds);
            DataTable dt = ds.Tables[0];
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    select += "@" + dt.Rows[i]["编号"].ToString() + "@" + dt.Rows[i]["姓名"].ToString()
                        + "@" + dt.Rows[i]["班级"].ToString() + "@" + dt.Rows[i]["科目"].ToString()
                        + "@" + dt.Rows[i]["电话"].ToString();
                }
            }
            catch
            {
                select = "@False";
            }
            myCon.Close();
            return select;
        }

        /// <summary>
        /// 查询题库 教师
        /// </summary>
        private string TextSelectTea(string str)
        {
            SqlConnection myCon;
            try
            {
                myCon = new SqlConnection
                    ("Persist Security Info = False; User id = sa; pwd = 123456; database = vstext; server = .");
                myCon.Open();
            }
            catch
            {
                textBox3.AppendText("未连接上数据库\r\n");
                return "@False";
            }
            string select = "";
            string sqlStr = "select 编号, 题目, A, B, C, D, 答案 from 题库 where 科目 in ( select 科目 from 教师 where 编号 = '" + str + "')";
            SqlDataAdapter da = new SqlDataAdapter(sqlStr, myCon);
            DataSet ds = new DataSet();
            da.Fill(ds);
            DataTable dt = ds.Tables[0];
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    select += "@" + dt.Rows[i]["编号"].ToString() + "@" + dt.Rows[i]["题目"].ToString()
                        + "@" + dt.Rows[i]["A"].ToString() + "@" + dt.Rows[i]["B"].ToString() + "@" + dt.Rows[i]["C"].ToString()
                        + "@" + dt.Rows[i]["D"].ToString() + "@" + dt.Rows[i]["答案"].ToString();
                }
            }
            catch
            {
                select = "@False";
            }
            myCon.Close();
            return select;
        }

        /// <summary>
        /// 查询题库 管理员
        /// </summary>
        private string TextSelectAbm(string str)
        {
            SqlConnection myCon;
            try
            {
                myCon = new SqlConnection
                    ("Persist Security Info = False; User id = sa; pwd = 123456; database = vstext; server = .");
                myCon.Open();
            }
            catch
            {
                textBox3.AppendText("未连接上数据库\r\n");
                return "@False";
            }
            string select = "";
            string sqlStr = "select 编号, 题目, A, B, C, D, 答案, 科目 from 题库";
            SqlDataAdapter da = new SqlDataAdapter(sqlStr, myCon);
            DataSet ds = new DataSet();
            da.Fill(ds);
            DataTable dt = ds.Tables[0];
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    select += "@" + dt.Rows[i]["编号"].ToString() + "@" + dt.Rows[i]["题目"].ToString()
                        + "@" + dt.Rows[i]["A"].ToString() + "@" + dt.Rows[i]["B"].ToString() + "@" + dt.Rows[i]["C"].ToString()
                        + "@" + dt.Rows[i]["D"].ToString() + "@" + dt.Rows[i]["答案"].ToString() + "@" + dt.Rows[i]["科目"].ToString();
                }
            }
            catch
            {
                select = "@False";
            }
            myCon.Close();
            return select;
        }

        /// <summary>
        /// 添加题库 教师
        /// </summary>
        private bool TextAddTea(string[] str)
        {
            SqlConnection myCon;
            try
            {
                myCon = new SqlConnection
                    ("Persist Security Info = False; User id = sa; pwd = 123456; database = vstext; server = .");
                myCon.Open();
            }
            catch
            {
                textBox3.AppendText("未连接上数据库\r\n");
                return false;
            }
            string sqlStr = "select 科目 from 教师 where 编号 = '" + str[2] + "'";
            string project = "";
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(sqlStr, myCon);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                project = Convert.ToString(dt.Rows[0]["科目"]).Trim();
            }
            catch
            {
                textBox3.AppendText("异常1\r\n");
                myCon.Close();
                return false;
            }
            sqlStr = "insert into 题库 (编号, 题目, A, B, C, D, 答案, 科目) values ('" + str[2] + "','"
                + str[3] + "','" + str[4] + "','" + str[5] + "','" + str[6] + "','" + str[7] + "','" + str[8] + "','" + project + "')";
            try
            {
                SqlCommand addCMD = new SqlCommand(sqlStr, myCon);
                addCMD.ExecuteNonQuery();
                myCon.Close();
                return true;
            }
            catch
            {
                textBox3.AppendText("异常2\r\n");
                myCon.Close();
                return false;
            }
        }

        /// <summary>
        /// 添加题库 管理员
        /// </summary>
        private bool TextAddAdm(string[] str)
        {
            SqlConnection myCon;
            try
            {
                myCon = new SqlConnection
                    ("Persist Security Info = False; User id = sa; pwd = 123456; database = vstext; server = .");
                myCon.Open();
            }
            catch
            {
                textBox3.AppendText("未连接上数据库\r\n");
                return false;
            }
            string sqlStr = "insert into 题库 (编号, 题目, A, B, C, D, 答案, 科目) values ('" + str[2] + "','"
                + str[3] + "','" + str[4] + "','" + str[5] + "','" + str[6] + "','" + str[7] + "','" + str[8] + "','" + str[9] + "')";
            try
            {
                SqlCommand addCMD = new SqlCommand(sqlStr, myCon);
                addCMD.ExecuteNonQuery();
                myCon.Close();
                return true;
            }
            catch
            {
                myCon.Close();
                return false;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            //定义一个套接字用于监听客户端发来的消息，包含三个参数（IP4寻址协议，流式连接，Tcp协议）
            socketwatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //服务端发送信息需要一个IP地址和端口号
            IPAddress address = IPAddress.Parse(textBox1.Text.Trim());//获取文本框输入的IP地址

            //将IP地址和端口号绑定到网络节点point上
            IPEndPoint point = new IPEndPoint(address, int.Parse(textBox2.Text.Trim()));//获取文本框上输入的端口号
            //此端口专门用来监听的

            //监听绑定的网络节点
            socketwatch.Bind(point);

            //将套接字的监听队列长度限制为20
            socketwatch.Listen(20);

            //创建一个监听线程
            threadwatch = new Thread(watchConnecting);

            //将窗体线程设置为与后台同步，随着主线程结束而结束
            threadwatch.IsBackground = true;

            //启动线程   
            threadwatch.Start();

            //启动线程后 textBox3文本框显示相应提示
            textBox3.AppendText("开始监听客户端传来的信息!" + "\r\n");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string sendMsg = "会话@"+ textBox4.Text.Trim();  //要发送的信息
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(sendMsg);   //将要发送的信息转化为字节数组，因为Socket发送数据时是以字节的形式发送的

            if (listBoxOnlineList.SelectedIndex == -1)
            {
                MessageBox.Show("请选择要发送的客户端！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                string selectClient = listBoxOnlineList.Text;  //选择要发送的客户端
                dic[selectClient].Send(bytes);   //发送数据
                textBox4.Clear();
                textBox3.AppendText(label4.Text + GetCurrentTime() + "\r\n" + sendMsg + "\r\n");
            }
        }
    }
}
