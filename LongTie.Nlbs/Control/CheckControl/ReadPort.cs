using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;

namespace LongTie.Nlbs.Control.CheckControl
{
    public class ReadPort
    {
        SerialPort _serialport = new SerialPort();
        private bool _keepreading;
        StringBuilder _stringdatd = new StringBuilder();
        private bool _isread = false;
        string _outstring = "";
        public ReadPort(SerialPort serialport)
        {
            _serialport = serialport;
            _serialport.ReceivedBytesThreshold = 1000;

        }

        public delegate void DeleteDataBack(Object sender, EventArgs e);
        public event DeleteDataBack HandleDataBack;
        public virtual void Read()
        {
            byte[] ReceiveBytes = new byte[4096];

            while (_keepreading)
            {
                if (_serialport.IsOpen)
                {
                    byte[] buffer = new byte[_serialport.ReadBufferSize + 1];
                    try
                    {

                        int count = _serialport.Read(buffer, 0, _serialport.ReadBufferSize);
                        string restr = System.Text.Encoding.ASCII.GetString(buffer, 0, count);
                        if (restr.IndexOf('\r') != -1)
                        {

                            _stringdatd.Append(restr);


                            SetText(_stringdatd.ToString());
                            if (HandleDataBack != null)
                            {
                                HandleDataBack(this, new EventArgs());
                            }
                            _stringdatd.Clear();
                            _isread = false;
                        }
                        else
                        {

                            _stringdatd.Append(restr);
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
        private void SetText(string text)
        {
            _outstring = text;
        }
    }
}
