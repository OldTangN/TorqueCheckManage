using QDDL.Comm;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;

namespace QDDL.BLL.Plc
{
    public class EncoderPlc
    {

        /// <summary>
        /// 编码器正反互换  
        /// </summary>
        SerialPort _serialport;
        public EncoderPlc(SerialPort serialport)
        {
            this._serialport = serialport;
        }

        string portaddress = "01";
        public string PortAddress
        {
            get { return portaddress; }
            set { portaddress = value; }
        }

        public bool Open()
        {
            try
            {
                if (!_serialport.IsOpen)
                    _serialport.Open();
                return true;
            }
            catch { return false; }
        }
        public void Close()
        {
            if (_serialport.IsOpen)
                _serialport.Close();
        }
        /// <summary>
        /// 收发命令
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        private string CPM2A(byte[] inStr)               //无消息提示发送函数
        {
            if (Open())
            {
                byte[] SendBuffer;
                byte[] ReadBuffer;
                int length = 0;
                SendBuffer = inStr;
                this._serialport.Write(SendBuffer, 0, SendBuffer.Length);
                while (_serialport.BytesToRead < 11)                              //等待接收所有的数据 
                {
                    Thread.Sleep(30);
                    break;
                }
                //do { Thread.Sleep(5); }
                //while (_serialport.BytesToRead < 11);

                Thread.Sleep(20);
                length = _serialport.BytesToRead;
                ReadBuffer = new byte[length];
                int buffercount = _serialport.Read(ReadBuffer, 0, length);
                string instring = Encoding.ASCII.GetString(ReadBuffer, 0, buffercount);
                return instring;
            }
            else
            {
                return "";
            }
        }
        private bool getResult(string readstr)
        {
            try
            {
                string exceptioncode = "";
                if (readstr.Length < 11)
                {
                    if (readstr.Length <= 0)
                        return false;
                    exceptioncode = readstr.Substring(5, 2);
                    switch (exceptioncode)
                    {
                        case "01":
                            //  MessageBox.Show("非法命令码！");

                            LogUtil.WriteLog(typeof(EncoderPlc), "非法命令码");
                            break;
                        case "02":
                            LogUtil.WriteLog(typeof(EncoderPlc), "非法的装置地址");
                            //  MessageBox.Show("非法的装置地址！");
                            break;
                        case "03":
                            LogUtil.WriteLog(typeof(EncoderPlc), "非法装置值！");
                            //  MessageBox.Show("非法装置值！");
                            break;
                        case "07":
                            LogUtil.WriteLog(typeof(EncoderPlc), "非法的消息命令！");
                            // MessageBox.Show("非法的消息命令！");
                            break;
                        default:
                            break;
                    }
                    return false;
                }
                return true;
            }
            catch { return false; }
        }

        string effectcode(string returnstr)
        {
            try
            {
                string addword = "";
                if (returnstr.Length < 11)
                    return null;
                int wordcount = Convert.ToInt32(returnstr.Substring(5, 2));
                addword += returnstr.Substring(7, wordcount * 2);
                return addword;
            }
            catch { return ""; }
        }


        #region 正反转

        /// <summary>
        /// 正转
        /// 代表编码器正传 0802H
        /// </summary>
        /// <returns></returns>
        public bool Direction()
        {
            int bitx = 0;                     //字中查询的位的位置
            int datax = 0;                      //返回的数值
            string AddrBit = "00";
            string ReadData = "";
            string sendstr = "";
            string readstr = "";
            string command = "0001";
            string functioncode = "01";
            string address = "0802";
            sendstr = PortAddress + functioncode + address.ToUpper() + command.ToUpper();
            byte[] sendbyte = HexUtil.SendStrMake(sendstr);
            readstr = CPM2A(sendbyte);
            if (getResult(readstr))
            {
                ReadData = readstr.Substring(7, 2);
                bitx = Convert.ToInt16(AddrBit);
                datax = Convert.ToInt16(ReadData, 16);
                datax = (datax >> bitx) & 0x01;///获取最后一位数

                if (datax == 1)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 读取最终反转传方向
        /// </summary>
        /// <returns></returns>
        public bool ReadFinalDirection()
        {
            int bitx = 0;                     //字中查询的位的位置
            int datax = 0;                      //返回的数值
            string AddrBit = "00";
            string ReadData = "";
            string sendstr = "";
            string readstr = "";
            string command = "0001";
            string functioncode = "01";
            string address = "0803";
            sendstr = PortAddress + functioncode + address.ToUpper() + command.ToUpper();
            byte[] sendbyte = HexUtil.SendStrMake(sendstr);
            readstr = CPM2A(sendbyte);
            if (getResult(readstr))
            {
                ReadData = readstr.Substring(7, 2);
                bitx = Convert.ToInt16(AddrBit);
                datax = Convert.ToInt16(ReadData, 16);
                datax = (datax >> bitx) & 0x01;///获取最后一位数

                if (datax == 1)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 代表编码器反方向
        /// </summary>
        /// <returns></returns>
        public bool ReDirection()
        {
            int bitx = 0;                     //字中查询的位的位置
            int datax = 0;                      //返回的数值
            string AddrBit = "00";
            string ReadData = "";

            string sendstr = "";
            string readstr = "";
            string command = "0001";
            string functioncode = "01";
            string address = "0801";

            sendstr = PortAddress + functioncode + address.ToUpper() + command.ToUpper();
            byte[] sendbyte = HexUtil.SendStrMake(sendstr);
            readstr = CPM2A(sendbyte);
            if (getResult(readstr))
            {
                ReadData = readstr.Substring(7, 2);
                bitx = Convert.ToInt16(AddrBit);
                datax = Convert.ToInt16(ReadData, 16);
                datax = (datax >> bitx) & 0x01;///获取最后一位数

                if (datax == 1)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 读取最终正转结果
        /// </summary>
        /// <returns></returns>
        public bool ReadFinalReDirection()
        {
            int bitx = 0;                     //字中查询的位的位置
            int datax = 0;                      //返回的数值
            string AddrBit = "00";
            string ReadData = "";

            string sendstr = "";
            string readstr = "";
            string command = "0001";
            string functioncode = "01";
            string address = "0804";

            sendstr = PortAddress + functioncode + address.ToUpper() + command.ToUpper();
            byte[] sendbyte = HexUtil.SendStrMake(sendstr);
            readstr = CPM2A(sendbyte);
            // MessageBox.Show("发送sendstr:" + sendstr + "读取readstr：" + readstr);
            if (getResult(readstr))
            {
                ReadData = readstr.Substring(7, 2);
                bitx = Convert.ToInt16(AddrBit);
                datax = Convert.ToInt16(ReadData, 16);
                datax = (datax >> bitx) & 0x01;///获取最后一位数

                if (datax == 1)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 读取正转间隙
        /// </summary>
        /// <returns>返回十六进制</returns>
        public string getDirectionData()
        {
            string sendstr = "";
            string readstr = "";
            string command = "0001";
            string functioncode = "03";
            string address = "11F9";

            sendstr = PortAddress + functioncode + address.ToUpper() + command.ToUpper();
            byte[] sendbyte = HexUtil.SendStrMake(sendstr);
            readstr = CPM2A(sendbyte);
            if (getResult(readstr))
            {
                return effectcode(readstr);
            }
            return "";
        }

        /// <summary>
        /// 设置正转间隙D505
        /// </summary>
        /// <param name="longtime"></param>
        /// <returns></returns>
        public bool setDirectorData(string data)
        {
            string sendstr = "";
            string readstr = "";
            string command = data;
            sendstr = PortAddress + "06" + "11F9" + command.ToUpper();
            byte[] sendbyte = HexUtil.SendStrMake(sendstr);
            readstr = CPM2A(sendbyte);
            return getResult(readstr);
        }

        /// <summary>
        /// 读取反转间隙
        /// </summary>
        /// <returns>返回十六进制</returns>
        public string getReDirectionData()
        {
            string sendstr = "";
            string readstr = "";
            string command = "0001";
            string functioncode = "03";
            string address = "11F4";

            sendstr = PortAddress + functioncode + address.ToUpper() + command.ToUpper();
            byte[] sendbyte = HexUtil.SendStrMake(sendstr);
            readstr = CPM2A(sendbyte);
            if (getResult(readstr))
            {
                return effectcode(readstr);
            }
            return "";
        }

        /// <summary>
        /// 设置反转间隙D500
        /// </summary>
        /// <param name="longtime"></param>
        /// <returns></returns>
        public bool setReDirectorData(string data)
        {
            string sendstr = "";
            string readstr = "";
            string command = data;
            sendstr = PortAddress + "06" + "11F4" + command.ToUpper();
            byte[] sendbyte = HexUtil.SendStrMake(sendstr);
            readstr = CPM2A(sendbyte);
            return getResult(readstr);
        }


        #endregion


        #region 按钮指示灯
        /// <summary>
        /// 读取实时值
        /// </summary>
        /// <returns>返回十六进制</returns>
        public string getIntimeData()
        {
            string sendstr = "";
            string readstr = "";
            string command = "0001";
            string functioncode = "03";
            string address = "1064";

            sendstr = PortAddress + functioncode + address.ToUpper() + command.ToUpper();
            byte[] sendbyte = HexUtil.SendStrMake(sendstr);
            readstr = CPM2A(sendbyte);
            if (getResult(readstr))
            {
                return effectcode(readstr);
            }
            return "";
        }

        /// <summary>
        /// 小比--快速模式指示灯
        /// </summary>
        /// <returns></returns>
        public bool ReadSmallBit()
        {
            int bitx = 0;                     //字中查询的位的位置
            int datax = 0;                      //返回的数值
            string AddrBit = "00";
            string ReadData = "";

            string sendstr = "";
            string readstr = "";
            string command = "0001";
            string functioncode = "01";
            string address = "0500";

            sendstr = PortAddress + functioncode + address.ToUpper() + command.ToUpper();
            byte[] sendbyte = HexUtil.SendStrMake(sendstr);
            readstr = CPM2A(sendbyte);
            if (getResult(readstr))
            {
                ReadData = readstr.Substring(7, 2);
                bitx = Convert.ToInt16(AddrBit);
                datax = Convert.ToInt16(ReadData, 16);
                datax = (datax >> bitx) & 0x01;///获取最后一位数

                if (datax == 1)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 大比--省力模式指示灯
        /// </summary>
        /// <returns></returns>
        public bool ReadBigBit()
        {
            int bitx = 0;                     //字中查询的位的位置
            int datax = 0;                      //返回的数值
            string AddrBit = "00";
            string ReadData = "";

            string sendstr = "";
            string readstr = "";
            string command = "0001";
            string functioncode = "01";
            string address = "0501";

            sendstr = PortAddress + functioncode + address.ToUpper() + command.ToUpper();
            byte[] sendbyte = HexUtil.SendStrMake(sendstr);
            readstr = CPM2A(sendbyte);
            if (getResult(readstr))
            {
                ReadData = readstr.Substring(7, 2);
                bitx = Convert.ToInt16(AddrBit);
                datax = Convert.ToInt16(ReadData, 16);
                datax = (datax >> bitx) & 0x01;///获取最后一位数

                if (datax == 1)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 校验结果指示灯

        /// <summary>
        ///小量程 蓝基色开关
        /// </summary>
        /// <param name="onOrOff">开/ 关</param>
        /// <returns></returns>
        public bool SmallBlueLight()
        {

            string command = "0400";
            string address = "0821000302";
            return Fun0FWrite(command, address);
        }
        /// <summary>
        /// 小量程红基色
        /// </summary>
        /// <param name="onOrOff"></param>
        /// <returns></returns>
        public bool SmallRedLight()
        {
            string command = "0100";
            string address = "0821000302";
            return Fun0FWrite(command, address);
        }

        /// <summary>
        /// 小量程绿灯
        /// </summary>
        /// <param name="onOrOff"></param>
        /// <returns></returns>
        public bool SmallGreenLight(bool onOrOff = true)
        {
            string command = "0200";
            string address = "0821000302";
            return Fun0FWrite(command, address);
        }


        /// <summary>
        /// 小全亮
        /// </summary>
        /// <param name="onOrOff"></param>
        /// <returns></returns>
        public bool SamllLightOn()
        {
            string command = "0700";
            string address = "0821000302";
            return Fun0FWrite(command, address);
        }

        public bool SamllLightOff()
        {
            string command = "0000";
            string address = "0821000302";
            return Fun0FWrite(command, address);
        }



        /// <summary>
        ///大量程 蓝基色卡关
        /// </summary>
        /// <param name="onOrOff">开/ 关</param>
        /// <returns></returns>
        public bool BigBlueLight(bool onOrOff = true)
        {
            string command = "0400";
            string address = "081E000302";
            return Fun0FWrite(command, address);

        }
        /// <summary>
        /// 大量程红基色
        /// </summary>
        /// <param name="onOrOff"></param>
        /// <returns></returns>
        public bool BigRedLight()
        {
            string command = "0100";
            string address = "081E000302";
            return Fun0FWrite(command, address);

        }

        /// <summary>
        /// 大量程绿灯
        /// </summary>
        /// <param name="onOrOff"></param>
        /// <returns></returns>
        public bool BigGreenLight(bool onOrOff = true)
        {
            string command = "0200";
            string address = "081E000302";
            return Fun0FWrite(command, address);
        }
        /// <summary>
        /// 大全亮
        /// </summary>
        /// <param name="onOrOff"></param>
        /// <returns></returns>
        public bool BigLightOn()
        {
            string command = "0700";
            string address = "081E000302";
            return Fun0FWrite(command, address);
        }
        /// <summary>
        /// 全灭
        /// </summary>
        /// <returns></returns>
        public bool BigLightOff()
        {
            string command = "0000";
            string address = "081E000302";
            return Fun0FWrite(command, address);
        }

        #endregion

        #region 灯全灭


        #endregion

        #region 小灯




        #endregion


        ///// <summary>
        ///// 蓝绿灭  红亮
        ///// </summary>
        ///// <param name="onOrOff"></param>
        ///// <returns></returns>
        //public bool BigRedLight()
        //{
        //    string sendstr = "";
        //    string readstr = "";
        //    string address = "";
        //    string command = "";
        //    address = "0820082102";
        //    command = "0000";
        //    sendstr = PortAddress + "0F" + address + command;
        //    byte[] sendbyte = HexUtil.SendStrMake(sendstr);
        //    readstr = CPM2A(sendbyte);
        //    return getResult(readstr);
        //}
        ///// <summary>
        ///// 绿灯亮  红蓝灭
        ///// </summary>
        ///// <returns></returns>
        //public bool BigGreenLight()
        //{
        //    string sendstr = "";
        //    string readstr = "";
        //    string address = "";
        //    string command = "";
        //    address = "081F082102";
        //    command = "0000";
        //    sendstr = PortAddress + "0F" + address + command;
        //    byte[] sendbyte = HexUtil.SendStrMake(sendstr);
        //    readstr = CPM2A(sendbyte);
        //    return getResult(readstr);
        //}
        #region 大灯


        #endregion
        #region Common
        bool MRegisterWrite(string command, string address)
        {
            string sendstr = "";
            string readstr = "";
            sendstr = PortAddress + "05" + address + command;
            byte[] sendbyte = HexUtil.SendStrMake(sendstr);
            readstr = CPM2A(sendbyte);
            return getResult(readstr);
        }
        bool Fun0FWrite(string command, string address)
        {
            string sendstr = "";
            string readstr = "";
            sendstr = PortAddress + "0F" + address + command;
            byte[] sendbyte = HexUtil.SendStrMake(sendstr);
            readstr = CPM2A(sendbyte);
            getResult(readstr);
            return true;
        }
        #endregion
    }
}


