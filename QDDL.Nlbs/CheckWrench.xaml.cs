using LongTie.Nlbs.Check;
using LT.Comm;
using LT.Model;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LongTie.Nlbs
{
    /// <summary>
    /// Interaction logic for CheckWrench.xaml
    /// </summary>
    public partial class CheckWrench : Window
    {
        delegate void SetTextCallback(string text);
        SerialPort port;
    //    SerialPort port1;
       // SerialPort port2;
        bool _keepreading;
        Thread _thread;
    //    Thread _quethread1;
   //     Thread _quethread2;
        public string _strid;
        private const long priCDelay = 3000000;
        public CheckWrench()
        {
            InitializeComponent();
            //readcard();
        }

        private void readcard() {
            try
            {
                port = new SerialPort();
                port.PortName = OperationConfig.GetValue("cardcom");
                port.BaudRate = Convert.ToInt32(OperationConfig.GetValue("cardrate"));
                port.DataBits = 8;
                port.Open();
                _keepreading = true;
                _thread = new Thread(ReadCard);
                _thread.Start();
            }
            catch { }
        }
    
          

         #region 读卡信息
        private void ReadCard()
        {
            while (_keepreading)
            {
                if (port.IsOpen)
                {
                    byte[] buffer = new byte[port.ReadBufferSize + 1];
                    try
                    {
                        int count = port.Read(buffer, 0, port.ReadBufferSize);
                        string serialIn = System.Text.Encoding.ASCII.GetString(buffer, 0, port.ReadBufferSize);
                        if (count != 0)
                        {
                            // string str= ThreadFun(buffer, 8);
                            funMReadOne1();

                        }
                    }
                    catch (TimeoutException) { }
                }
                else
                {
                    TimeSpan waitTime = new TimeSpan(0, 0, 0, 0, 50);
                    Thread.Sleep(waitTime);
                }
            }
        }
        private bool funMReadOne1()
        {
            bool isSuc;
            string strRet = "";
            byte passAB;
            string strPass;
            byte b_Block;
            int nowPosit = 0;
            string strMid;
            byte[] b_Pass = new byte[] { 0, 0, 0, 0, 0, 0 };

            b_Block = Convert.ToByte("5", 10);

            passAB = 0x60;

            strPass = "FFFFFFFFFFFf";
            if (strPass.Length < 12)
            {
                MessageBox.Show("磁卡类型有误");
            }
            else
            {
                nowPosit = 0;
                for (int i = 0; i < 12; i += 2)
                {
                    strMid = strPass.Substring(i, 2);
                    b_Pass[nowPosit] = Convert.ToByte(strMid, 16);
                    nowPosit = nowPosit + 1;
                }
                isSuc = funReadOne1(b_Block, passAB, ref b_Pass, out strRet);
                SetText(strRet);
            }
            return true;
        }
        public bool funReadOne1(byte b_Block, byte passAB, ref byte[] b_Pass, out string ret_data)
        {//读一块读卡方式1
            bool isSuc = false;//是否成功 
            bool canRead = true;//可以接收串口数据
            byte[] outData = new byte[12];//发送数据
            byte[] r_data = new byte[20];//接收数据
            string strRead = "";//接收到的数据字符串
            byte b_Xor;
            int i;
            int dataCount = 0;//发送数据个数
            int leaveCount = 0;//剩余数据个数
            int allCount = 0;//需要接收数据个数
            long diffTime = 0;//等待时间
            long t_stars;
            long t_ends;

            while (port.BytesToRead > 0)
            {
                r_data[0] = Convert.ToByte(port.ReadByte());
            }
            isSuc = false;
            outData[0] = 0xAA;
            outData[1] = 0XFF;
            outData[2] = 0X10;
            outData[3] = b_Block;
            outData[4] = passAB;
            for (i = 0; i < 6; i++)
            {
                outData[5 + i] = b_Pass[i];

            }
            b_Xor = 0;

            for (i = 0; i < 11; i++)
            {
                b_Xor = (byte)((int)b_Xor ^ (int)outData[i]);

            }
            outData[11] = b_Xor;
            dataCount = 12;
            if (port.IsOpen) port.Write(outData, 0, dataCount);
            canRead = true;
            t_stars = DateTime.Now.Ticks;
            t_ends = t_stars;
            while (port.BytesToRead < 4)
            {
                t_ends = DateTime.Now.Ticks;
                diffTime = t_ends - t_stars;
                if (diffTime > priCDelay)
                {
                    canRead = false;
                    break;
                }
                // Application.DoEvents();
            }
            if (canRead)//接收前4个字节
            {
                port.Read(r_data, 0, 4);
            }
            else
            {
                ret_data = "";
                return isSuc;
            }
            switch (r_data[2])
            {
                case 0XA0:
                    strRead = "readfail:";
                    isSuc = false;
                    leaveCount = 0;
                    allCount = 4;
                    break;
                case 0xA1:
                    strRead = "readpwdfail:";
                    isSuc = false;
                    leaveCount = 0;
                    allCount = 4;
                    break;
                case 0X10:
                    strRead = "success:";
                    isSuc = true;
                    leaveCount = 16;
                    allCount = 20;
                    break;

            }
            canRead = true;
            t_stars = DateTime.Now.Ticks;
            t_ends = t_stars;
            while (port.BytesToRead < leaveCount)
            {
                t_ends = DateTime.Now.Ticks;
                diffTime = t_ends - t_stars;
                if (diffTime > priCDelay)
                {
                    canRead = false;
                    break;
                }
                // Application.DoEvents();
            }

            if (canRead & leaveCount > 0)
            {
                port.Read(r_data, 4, leaveCount);
            }
            else
            {
                for (i = 0; i < 4; i++)
                {
                    strRead = strRead + funBtoHex(r_data[i]);
                }
                ret_data = strRead;
                return isSuc;
            }
            for (i = 0; i < allCount; i++)
            {
                strRead = strRead + funBtoHex(r_data[i]);
            }
            ret_data = strRead;
            return isSuc;
        }

        private string funBtoHex(byte num)
        {
            string strhex;
            strhex = num.ToString("X");
            if (strhex.Length == 1)
                strhex = "0" + strhex;
            else
                strhex = " " + strhex;
            return strhex;

        }
        private void SetText(string text)
        {
            if (!this.cardid.CheckAccess())
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Dispatcher.Invoke(d, new object[] { text });
            }
            else { this.cardid.Text = text; }

        }

        private string getcardid(string text) {
            string cardid ="";
            string[] strsplit = text.Split(' ');
            if (strsplit[2] == "10") {
                for (int i = 3; i < 7; i++) {
                    cardid += strsplit[i];
                }
            }
            return cardid;
        }

        private void getUser(string cardid) { 
        //获取刷卡人的信息
        }
        private void showUser() { 
        //显示刷卡人信息并设为全局变量
        }
#endregion


        #region  获取校验仪器数据
          private void ReadQue()
        {
            while (_keepreading)
            {
                if (port.IsOpen)
                {
                    byte[] buffer = new byte[port.ReadBufferSize + 1];
                    try
                    {
                        int count = port.Read(buffer, 0, port.ReadBufferSize);
                        string serialIn = System.Text.Encoding.ASCII.GetString(buffer, 0, port.ReadBufferSize);
                        if (count != 0)
                        {
                            string str = serialIn.Split('\r')[0].ToString(); 
                            ///调用方法
                        }
                    }
                    catch (TimeoutException) { }
                }
                else
                {
                    TimeSpan waitTime = new TimeSpan(0, 0, 0, 0, 50);
                    Thread.Sleep(waitTime);
                }
            }
        }
          private void funreadque(string  str) {

          }

        #endregion
        private void bt_start_Click(object sender, RoutedEventArgs e)
        {
            _checkdatashow.Clear();

            for (int j = 0; j < 1; j++)
            {
                _checkdatashow.Add(new CheckDatashow() { id =2, checkvalue =50, isgood = "√", targetvalue =30, rate = 0.04, resultrate =1.3 });
            }
            dt = new GridCheckdata(_checkdatashow);
            Grid.SetRow(dt, 1);
            Grid.SetColumn(dt, 4);
            this.checkdata.Children.Add(dt);
            //port1 = new SerialPort();
            //port1.PortName = "COM3";
            //port1.BaudRate = 9600;
            //port1.DataBits = 8;
            //port1.Open();
            //_keepreading = true;
            //_quethread1 = new Thread(ReadQue);
            //_quethread1.Start();


            //port2 = new SerialPort();
            //port2.PortName = "COM2";
            //port2.BaudRate = 9600;
            //port2.DataBits = 8;
            //port2.Open();
            //_quethread2 = new Thread(ReadQue);
            //_quethread2.Start();


            //List<CheckDatashow> cds = new List<CheckDatashow>() { new CheckDatashow() { id = "1", checkvalue = "20", rate = "4", targetvalue = "20", isgood = "√" }, new CheckDatashow() { id = "1", checkvalue = "20", rate = "4", targetvalue = "20", isgood = "×" } }; 
            //GridCheckdata gck = new GridCheckdata(cds);
            //checkdata.Children.Clear();
            //checkdata.Children.Add(gck);
        }

        int m = 1;
        List<CheckDatashow> _checkdatashow = new List<CheckDatashow>();
        GridCheckdata dt;
        private void bt_finish_Click(object sender, RoutedEventArgs e)
        {
            _checkdatashow.Clear();
            
              for (int j = 0; j < 4; j++) {
                  _checkdatashow.Add(new CheckDatashow() { id =1,checkvalue =50,isgood ="√",targetvalue =30,rate =0.04,resultrate =1.3});
              }
              this.checkdata.RowDefinitions.Add(new RowDefinition());
             dt = new GridCheckdata(_checkdatashow );
            Grid.SetRow(dt, m);
            Grid.SetColumn(dt, 4);

            for (int i = 0; i <=3; i++)
            {
                TextBlock tb = new TextBlock() { Text = string.Format("rb{0}{1}", m, i) };
               
                Grid.SetRow(tb, m );
                Grid.SetColumn(tb, i);

                this.checkdata.Children.Add(tb);
              

            }
            this.checkdata.Children.Add(dt);

            m++;
              
          
              
                
            //}
        }
    }
}
