using QDDL.BLL.ICCard;
using QDDL.Comm;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Threading;


namespace QDDL.BLL
{
    public class ReadCheckTester
    {
        SerialPort port = new SerialPort();
        bool _keepreading;
        public bool isread = true;
        //  string readstring = ""; 
        public event EventHandler<CardEventArgs> HandDataBack;
        public ReadCheckTester(SerialPort serialport)
        {
            try
            {
                port = serialport;
                port.Open();
                _keepreading = true;
            }
            catch
            {
                LogUtil.WriteLog(typeof(ReadCheckTester), "校验仪端口" + port.PortName + "打开失败！");

            }
        }
        public bool Openport()
        {
            try
            {
                if (!port.IsOpen)
                    port.Open();
                return true;
            }
            catch { return false; }
        }

        public void ClosePort()
        {
            if (port.IsOpen)
                port.Close();
        }


        // List<string > buffers = new List<string >();
        StringBuilder sb = new StringBuilder();
        public void Read()
        {
            byte[] ReceiveBytes = new byte[4096];

            while (_keepreading)
            {
                if (port.IsOpen)
                {
                    byte[] buffer = new byte[port.ReadBufferSize + 1];
                    try
                    {

                        int count = port.Read(buffer, 0, port.ReadBufferSize);
                        string restr = System.Text.Encoding.ASCII.GetString(buffer, 0, count);

                        if (restr.IndexOf('\r') != -1)
                        {

                            sb.Append(restr);
                            //  SetText(sb.ToString());
                            HandDataBack?.Invoke(this, new CardEventArgs(sb.ToString()));
                            sb.Clear();
                            isread = false;
                        }
                        else
                        {
                            sb.Append(restr);
                        }

                    }


                    catch (TimeoutException) { }
                }
                else
                {
                    TimeSpan waitTime = new TimeSpan(0, 0, 0, 1, 50);
                    Thread.Sleep(waitTime);
                }
            }
        }


        //public string back()
        //{
        //    return readstring;
        //}

        //private void SetText(string text)
        //{
        //    readstring = text;
        //}



        public string ReturnReadData(string data)
        {
            string returndate = "";

            string[] testdata = data.Split(' ');
            switch (testdata.Length)
            {
                case 1:
                    returndate = (testdata[0]);
                    break;
                case 2:
                    returndate = (testdata[0]);
                    break;
                case 3:
                    returndate = (testdata[1]);
                    break;
                default:
                    returndate = "";
                    break;
            }
            return returndate;
        }
    }
}
