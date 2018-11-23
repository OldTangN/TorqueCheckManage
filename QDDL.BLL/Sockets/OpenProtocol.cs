using Newtonsoft.Json;
using QDDL.BLL.Sockets.Tools;
using QDDL.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace QDDL.BLL.Sockets
{
    /// <summary>
    /// 开放协议
    /// </summary>
    public class OpenProtocol : IProtocol
    {
        private DesoutterData SendedData { set; get; }
        private int PsetID { set; get; }

        /// <summary>
        /// TCP客户端
        /// </summary>
        public TcpClinetSocket client { set; get; }

        public Timer timer { set; get; }

        public event EventHandler<ReciveEventArgs> ReciveHandler;

        public event EventHandler<ReciveEventArgs> SendHandler;

        public event EventHandler<ReciveEventArgs> RequestHandler;

        public void Start(string ip, int port)
        {
            client = new TcpClinetSocket(ip, port);
            client.ReciveHandler += Client_ReciveHandler;
            try
            {
                client.ConnectToServer();
                //client.IniBeginReceive();
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(typeof(OpenProtocol), "连接异常" + ex.Message); 
            }

            timer = new Timer(new TimerCallback(timer_Elapsed), null, 5000, 5000);

        }

        public void ReStart()
        {
            try
            {
                client.ConnectToServer();
                //   client.IniBeginReceive();
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(typeof(OpenProtocol), "连接异常" + ex.Message);
            }
        }
        public void SendCommand(ToolWrench toolwrench)
        {
            this.PsetID = toolwrench.PsetId;

            //dData = new List<DesoutterData>()
            //{
            //    new DesoutterData(20,1,0,""),
            //    new DesoutterData(23, 18, 0, PsetID.ToString().PadLeft(3, '0')),
            //    new DesoutterData(20, 60, 1, "")
            //  //  n,ew DesoutterData(20,62,1,"")
            //};

            ////开始连接
            //DesoutterProtocol desoutterProtocol = new DesoutterProtocol(20, 1, 0, "");
            //byte[] bb = desoutterProtocol.GetProtocol();
            //Thread.Sleep(500);
            //SendData(new DesoutterData(20, 43, 1, "")); 
            Thread.Sleep(500);
            SendedData = new DesoutterData(20, 1, 0, "");
            SendData(SendedData);

            Thread.Sleep(500);
            SendData(new DesoutterData(20, 43, 1, ""));

            Thread.Sleep(500);
            SendedData = new DesoutterData(23, 18, 0, PsetID.ToString().PadLeft(3, '0'));
            SendData(SendedData);

            Thread.Sleep(500);
            SendedData = new DesoutterData(20, 60, 1, "");
            SendData(SendedData);
        }
        public void SendLock()
        {
            SendData(new DesoutterData(20, 42, 1, ""));
        }
        private void SendData(DesoutterData PsetData, bool isHandler = true)
        {
            byte[] senddata = PsetData.GetDesoutterData();

            if (isHandler)
            {
                //发送事件通知
                ReciveEventArgs asEventArgs = new ReciveEventArgs(senddata);
                SendHandler(this, asEventArgs);
            }
            client.SendData(senddata);
        }
        public void Client_ReciveHandler(object sender, ReciveEventArgs e)
        {
            if (e.data.Length > 0)
            {
                ReciveEventArgs asEventArgs = new ReciveEventArgs(e.data);
                ReciveHandler(sender, asEventArgs);

                byteToObj(e.data);
            }
        }

        private void byteToObj(byte[] data)
        {

            DesoutterData ddata = new DesoutterData(); //返回数据解密
            try
            {
                ddata.SetDesoutterEntity(data);

                RdataSend(ddata);

                int dDataLen = Convert.ToInt32(ddata.Length) + 1;
                if (data.Length > dDataLen)
                {
                    byte[] otherData = data.ToList().Skip(dDataLen).ToArray();
                    byteToObj(otherData);
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(typeof(OpenProtocol), "byteToObj" + ex.Message); 
            }
        }

        private void RdataSend(DesoutterData ddata)
        {
            switch (ddata.MID)
            {
                //case "0002": 
                //    {
                //        Thread.Sleep(500);
                //        SendedData = new DesoutterData(23, 18, 0, PsetID.ToString().PadLeft(3, '0'));
                //        SendData(SendedData);
                //    }
                //    break;
                //case "0004":
                //    {
                //        if (ddata.DataField.Contains("0001"))
                //        {
                //            Thread.Sleep(500);
                //            SendedData = new DesoutterData(23, 18, 0, PsetID.ToString().PadLeft(3, '0'));
                //            SendData(SendedData);
                //        } 
                //    }
                //    break;
                //case "0005":
                //    {
                //        if (ddata.DataField == "0018")
                //        {
                //            Thread.Sleep(500);
                //            SendedData = new DesoutterData(20, 60, 1, "");
                //            SendData(SendedData);
                //        }
                //    }

                //    break;

                case "0061":
                    {
                        // send = true;
                        byte[] resutData = Encoding.ASCII.GetBytes(ddata.DataField);
                        byte TighteningStatus = resutData[106 - 21 + 2];//数组第106位-107位为 09 标志，108位为状态

                        byte[] Torque = new byte[6];
                        Buffer.BlockCopy(resutData, 141 - 21, Torque, 0, 6);

                        byte[] Angle = new byte[5];
                        Buffer.BlockCopy(resutData, 170 - 21, Angle, 0, 5);

                        double RealTor = Convert.ToInt32(Encoding.ASCII.GetString(Torque)) / 100.00;
                        int realAng = Convert.ToInt32(Encoding.ASCII.GetString(Angle));

                        TestResult testResult = new TestResult()
                        {
                            RealTar = RealTor.ToString(),
                            RealAgi = realAng.ToString(),
                            AddDate = DateTime.Now
                        };
                        if (TighteningStatus == 0x31)
                        {
                            testResult.IsOk = true;
                        }
                        else
                        {
                            testResult.IsOk = false;
                        }
                        string requestData = JsonConvert.SerializeObject(testResult);
                        ReciveEventArgs asEventArgs = new ReciveEventArgs(Encoding.ASCII.GetBytes(requestData));
                        RequestHandler(this, asEventArgs);
                        SendData(new DesoutterData(20, 62, 1, ""));


                        //Thread.Sleep(500);
                        //SendData(new DesoutterData(20, 42, 1, ""));
                    }
                    break;
                default:

                    //SendData(SendedData);
                    break;
            }

        }
        public void timer_Elapsed(object obj)
        {
            var keepAliveData = new DesoutterData(20, 9999, 0, "");
            SendData(keepAliveData);

        }
        /// <summary>
        /// return client.isclose()
        /// </summary>
        /// <returns></returns>
        public bool GetClientStart()
        {
            return client.IsClose;
        }
    }
    public class DesoutterData
    {

        #region 内部变量

        /// <summary>
        /// The length is the length of the header plus the data field excluding the NUL termination.  
        /// The header always includes information about the length of the message. The length is represented by four ASCII digits (‘0’…’9’) specifying a range of 0000 to 9999. 
        /// </summary>
        public string Length { set; get; }
        /// <summary>
        /// The MID is four bytes long and is specified by four ASCII digits (‘0’…’9’). The MID describes how to interpret the message. 
        /// </summary>
        public string MID { set; get; }

        /// <summary>
        /// The revision of the MID is specified by three ASCII digits (‘0’…’9’).
        /// The MID revision is unique per MID and is used in case several versions are available for the same MID. Using the revision number the integrator can subscribe or ask for different versions of the same MID. By default the MID revision number is three spaces (revision 1 of the MID). So, if the integrator is interested in the initial revision (revision 1) of the MID, it can send three spaces as MID revision or 001. 
        /// </summary>
        public string Revision { set; get; }

        /// <summary>
        /// ONLY FOR SUBSCRIPTION MIDs. The No Ack Flag is used when setting a subscription. If the No Ack flag is not set in a subscription it means that the subscriber will acknowledge each “push” message sent by the controller (reliable mode).  If set, the controller will only push out the information required without waiting for a receive acknowledgement from the subscriber (unreliable mode). 
        /// </summary>
        private byte NoAckFlag { set; get; } = 0x20;
        /// <summary>
        /// The station the message is addressed to in the case of controller with multi-station configuration. The station ID is 2 byte long and is specified by two ASCII digits (‘0’…’9’). One space is considered as station 1 (default value). Only available if not marked with N/A. 
        /// </summary>
        private string StationID { set; get; } = "  ";
        /// <summary>
        /// The spindle the message is addressed to in the case several spindles are connected to the same controller. The spindle ID is 2 bytes long and is specified by two ASCII digits (‘0’…’9’). Two spaces are considered as spindle 1 (default value). Only available if not marked with N/A. OBS! Is allways 0 for FORD OBS!  
        /// </summary>
        private string SpindleID { set; get; } = "  ";
        /// <summary>
        /// Reserved space in the header for future use. 
        /// </summary>
        private string Spare { set; get; } = "    ";

        /// <summary>
        /// Parameter ID (00...99), length two bytes. The parameter ID is padded on the left with the ASCII characters ‘0’. 
        /// Parameter value is defined by parameter selection (fixed number of bytes). ASCII digits (‘0’…’9’) or ASCII characters between 0x20 and 0x7F Hex.  
        /// </summary>
        public string DataField { set; get; }

        private byte MessageEnd { set; get; } = 0x00;

        #endregion

        public bool IsSend { set; get; } = false;

        public DesoutterData()
        {
        }
        public DesoutterData(int _length, int _mid, int _Revision, string _DataField)
        {
            Length = _length.ToString().PadLeft(4, '0');
            MID = _mid.ToString().PadLeft(4, '0');
            Revision = _Revision == 0 ? "   " : _Revision.ToString().PadLeft(3, '0');
            DataField = _DataField;
        }
        public byte[] GetDesoutterData()
        {
            List<byte> protocol = new List<byte>();

            byte[] lg = Encoding.ASCII.GetBytes(Length);
            protocol.AddRange(lg);

            byte[] mid = Encoding.ASCII.GetBytes(MID);
            protocol.AddRange(mid);

            byte[] revi = Encoding.ASCII.GetBytes(Revision);
            protocol.AddRange(revi);

            protocol.Add(NoAckFlag);

            byte[] sid = Encoding.ASCII.GetBytes(StationID);
            protocol.AddRange(sid);

            byte[] pid = Encoding.ASCII.GetBytes(SpindleID);
            protocol.AddRange(pid);

            byte[] spare = Encoding.ASCII.GetBytes(Spare);
            protocol.AddRange(spare);

            byte[] df = Encoding.ASCII.GetBytes(DataField);
            protocol.AddRange(df);

            protocol.Add(MessageEnd);
            return protocol.ToArray();
        }

        public void SetDesoutterEntity(byte[] data)
        {
            try
            {

                Length = Encoding.ASCII.GetString(data, 0, 4);
                MID = Encoding.ASCII.GetString(data, 4, 4);
                Revision = Encoding.ASCII.GetString(data, 8, 3);
                NoAckFlag = data[11];
                StationID = Encoding.ASCII.GetString(data, 12, 2);
                SpindleID = Encoding.ASCII.GetString(data, 14, 2);
                Spare = Encoding.ASCII.GetString(data, 16, 4);
                if (data.Length > 21)
                {
                    DataField = Encoding.ASCII.GetString(data, 20, data.Length - 21);
                }

            }
            catch (Exception ex)
            {

            }
        }

    }
}
