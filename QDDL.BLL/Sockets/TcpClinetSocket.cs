using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using QDDL.Comm;
using QDDL.BLL.Sockets.Tools;

namespace QDDL.BLL.Sockets
{
    public class TcpClinetSocket
    {
        public byte[] buffer = new byte[1024];
        /// <summary>
        /// 远程服务IP地址
        /// </summary>
        public string RemoteIp { set; get; } = string.Empty;
        /// <summary>
        /// 远程服务IP地址对应端口
        /// </summary>
        public int RemotePort { set; get; } = -1;

        Socket socket { set; get; }

        public bool IsClose { set; get; } = true;

        public event EventHandler<ReciveEventArgs> ReciveHandler;
        /// <summary>
        /// tcp客户端
        /// </summary>
        /// <param name="ip">服务器IP</param>
        /// <param name="port">服务器端口</param>
        public TcpClinetSocket(string ip, int port)
        {
            this.RemoteIp = ip;
            this.RemotePort = port;
        }
        public void ConnectToServer()
        {
            IPAddress ip = IPAddress.Parse(RemoteIp);
            //①创建一个Socket
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipep = new IPEndPoint(ip, RemotePort);
            try
            {
                //②连接到指定服务器的指定端口
                //  socket.BeginConnect(ipep, new AsyncCallback(anyncConnect), socket);
                socket.Connect(RemoteIp, RemotePort);
                IniBeginReceive(socket);
                //③实现异步接受消息的方法 客户端不断监听消息
                //  buffer = new byte[1024];
                //  socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveMessage), socket);
                IsClose = false;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(typeof(TcpClinetSocket), "ConnectToServer" + ex.Message); 
                //  ConnectToServer();
                IsClose = true;
            }
        }
         
        /// <summary>
        /// 初始化异步接收数据
        /// </summary>
        public void IniBeginReceive(Socket skt)
        {
            buffer = new byte[1024];

            try
            {
                skt.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveMessage), socket);
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(typeof(TcpClinetSocket), "ClientInfo.BeginReceive" + ex.Message);  
                IsClose = true;
            }

        }
        /// <summary>
        /// 接收信息
        /// </summary>
        /// <param name="ar"></param>
        public void ReceiveMessage(IAsyncResult ar)
        {
            var sok = ar.AsyncState as Socket;
            var length = 0;
            try
            {
                if (sok.Connected)
                {
                    length = socket.EndReceive(ar);
                    if (length <= 0)
                    {
                        IniBeginReceive(sok);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(typeof(TcpClinetSocket), "ReceiveMessage" + ex.Message);  
                IniBeginReceive(sok);
            }
            if (length != 0)
            {
                //读取出来消息内容 
                byte[] message = new byte[length];
                Array.ConstrainedCopy(buffer, 0, message, 0, length);
                //  Buffer.BlockCopy(buffer, 0, message, 0, length);
                //注册消息事件
                ReciveEventArgs asEventArgs = new ReciveEventArgs(message);
                ReciveHandler(this, asEventArgs);
            }
            IniBeginReceive(sok);
        }

        /// <summary>
        /// 发送数据
        /// <param name="SendBytes">需要发送的数据</param>
        /// </summary>
        public void SendData(byte[] SendBytes)
        {
            try
            {
                if (SendBytes != null && SendBytes.Length > 0)
                {
                    //发送数据
                    socket.Send(SendBytes);
                    //networkStream.Write(SendBytes, 0, SendBytes.Length);
                    //etworkStream.Flush();
                }
            }
            catch (Exception ex)
            {
                //if (tcpClient != null)
                //{
                //    tcpClient.Close();
                //    //关闭连接后马上更新连接状态标志
                //    IsConnected = false;
                //}
                LogUtil.WriteLog(typeof(TcpClinetSocket), "SendData" + ex.Message); 
                IsClose = true;
            }
        }
    }

}
