using LT.Comm;
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


namespace LT.BLL
{
  public   class ReadCheckTester
    {
      SerialPort port = new SerialPort();
      bool _keepreading;
      public   bool isread = true ;
       string readstring = "";
    //  int  baudrate = 9600;

      public delegate void DeleteDataBack(Object sender,EventArgs e);
      public event DeleteDataBack HandDataBack;
  //    DateTime dt1 = null;

  //    System.Timers.Timer aTimer = null;
      private delegate void TimerDispatcherDelegate();
      public ReadCheckTester(SerialPort serialport)
      {
          try
          {
              port = serialport;
              port.Open();
              _keepreading = true;

              //aTimer = new System.Timers.Timer(1000);
              //aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
              //aTimer.Interval = 3000;
              //aTimer.Enabled = true;
          }
          catch {
              
              //MessageBox.Show("校验仪端口"+port.PortName +"打开失败！");
              LogUtil.WriteLog(typeof (ReadCheckTester ), "校验仪端口" + port.PortName + "打开失败！");
          
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
         byte [] ReceiveBytes=new byte[4096] ;
     
          while (_keepreading)
          {
              if (port.IsOpen)
              {
                  byte[] buffer = new byte[port.ReadBufferSize + 1];
                  try
                  {
                      
                      int count = port.Read(buffer, 0, port.ReadBufferSize);
                      string restr = System.Text.Encoding.ASCII.GetString(buffer, 0, count);
                                                        
                          if (restr.IndexOf ('\r')!=-1)
                      {
                         
                          sb.Append(restr);

                       
                          SetText(sb.ToString());
                          if (HandDataBack != null)
                          {
                              HandDataBack(this, new EventArgs());
                          }

                        
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
                  TimeSpan waitTime = new TimeSpan(0, 0, 0, 0, 50);
                  Thread.Sleep(waitTime);
              }
          }
      }


      public string back()
      {
          return readstring;
      }

      private void SetText(string text)
      {


          readstring = text;
      }


      //public string  ReturnReadString() {


      //    if (isread)
      //        return "";

      //    ////返回值
      //    //string[] tempvalue = readstring.Split(' ');

      //    return readstring;
      //   // return "30.2";
      //}



      //void OnTimedEvent(object serder, EventArgs e)
      //{

      //    System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal,
      //        new TimerDispatcherDelegate(UpdateUI));
      //}
      //Random a = new Random((int)DateTime.Now.Ticks);

      //void UpdateUI()
      //{
      //    readstring = a.Next(4).ToString();
      //    isread = false;
      //}

      public string ReturnReadData()
      {
          string  returndate = "";

          string[] testdata = readstring.Split(' ');
          switch (testdata.Length)
          { 
              case 1:
                  returndate =(testdata[0]);
                  break;
              case 2:
                  returndate = (testdata[0]);
                  break;
              case 3:
                  returndate = (testdata[1]);
                  break;
              default :
                  returndate = "";
                  break;
          }         
          return returndate;
      }






      //decimal getcheckdata()
      //{
      //    string[] tempvalue = rct.ReturnReadString().Split(' ');
      //    rct.isread = true;
      //    decimal temp = 0;
      //    try
      //    {
      //        temp = Convert.ToDecimal(tempvalue[0]);
      //    }
      //    catch
      //    {
      //        temp = Convert.ToDecimal(tempvalue[1]);
      //    }
      //    return temp;
      //}

      //public void Dispose()
      //{
      //    Dispose(true);
      //    GC.SuppressFinalize(this);
      //}
      //public virtual void Dispose(bool disposing)
      //{
      //    if (!_dispose)
      //    {
      //        if (disposing)
      //        {
      //            if (port != null) { port.Dispose(); }
      //            _keepreading = false;
      //        }
      //    }
      //    _dispose = true;
      //}
    }
}
