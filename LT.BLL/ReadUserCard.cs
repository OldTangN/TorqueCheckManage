using LT.Comm;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LT.BLL
{
  public class ReadUserCard
    {
      SerialPort port = new SerialPort();
      private const long priCDelay = 3000000;
      public bool isread = false;
      bool _keepreading;
   
      string readstring = "";
    //  int baudrate = 115200;
      public delegate void DeleteDataBack(Object sender, EventArgs e);
      public event DeleteDataBack HandDataBack;

      public ReadUserCard(SerialPort serialport) 
      {
          try
          {

              port = serialport;           
              port.ReceivedBytesThreshold = 1000;
              port.Open();
              _keepreading = true;
          }
          catch(Exception e)
          {
              //MessageBox.Show(port.PortName + "打开失败！");

              LogUtil.WriteLog(typeof(ReadUserCard), "读卡器端口" + port.PortName + "打开失败！"+ e);
          }
          finally 
          {
              //port.Close();
              //port.Dispose();
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

      public void Read()
      {
          try
          {
              while (_keepreading)
              {
                  string strData;
                  byte[] r_byte = new byte[20];
                  //int r_num = 0;
                  if (port.IsOpen)
                  {
                      // byte[] buffer = new byte[port.ReadBufferSize + 1];
                      try
                      {
                          //int count = port.Read(buffer, 0, port.ReadBufferSize);
                          //string serialIn = System.Text.Encoding.ASCII.GetString(buffer, 0, port.ReadBufferSize);
                          //if (count != 0)
                          //{
                          //    isread = false;
                          //    funMReadOne1();

                          //}


                          port.Read(r_byte, 0, 8);

                          if (r_byte[0] != 0xBB)
                          {
                              strData = "";
                              for (int i = 2; i < 6; i++)
                              {
                                  strData = strData + funBtoHex(r_byte[i]);
                              }
                              SetText(strData);
                              if (HandDataBack != null)
                              {
                                  HandDataBack(this, new EventArgs());
                              }
                              isread = false;
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
          catch { }
      }
      //public  void Read()
      //{
      //    while (_keepreading)
      //    {
      //        if (port.IsOpen)
      //        {
      //            byte[] buffer = new byte[port.ReadBufferSize + 1];
      //            try
      //            {
      //                int count = port.Read(buffer, 0, port.ReadBufferSize);
      //                string serialIn = System.Text.Encoding.ASCII.GetString(buffer, 0, port.ReadBufferSize);
      //                if (count != 0)
      //                {
      //                    isread = false;
      //                    funMReadOne1();

      //                }
      //            }
      //            catch (TimeoutException) { }
      //        }
      //        else
      //        {
      //            TimeSpan waitTime = new TimeSpan(0, 0, 0, 0, 50);
      //            Thread.Sleep(waitTime);
      //        }
      //    }
      //}

      #region
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
              passAB = 0x61;
              strPass = "FFFFFFFFFFFF";
          if (strPass.Length < 12)
          {
              MessageBox.Show("请输入正确位数的密码");
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
          // strRead = "发送：";
          // for (i = 0; i < 12; i++)
          //    strRead = strRead + funBtoHex(outData[i]);
          //SetText(strRead);
          // strRead = "";
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
              Application.DoEvents();
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
                  strRead = "readfail：";
                  isSuc = false;
                  leaveCount = 0;
                  allCount = 4;
                  break;
              case 0xA1:
                  strRead = "readpwdfail：";
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
              Application.DoEvents();
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
              strhex = " 0" + strhex;
          else
              strhex = " " + strhex;
          return strhex;

      }
      #endregion    
      void   SetText(string text)
      {
          readstring = text;
      } 

      public string BackString()
      {

          return readstring;
      }
      public void   setreturnstr(string str)
      {
          isread = false;
       readstring=str;
      }
         public string returnreadstr() 
         {
             if (isread) {
                 return "";
             }
             return readstring;
           
         }
    }
}
