using QDDL.Nlbs.Control.EncoderControl;
using QDDL.Comm;
using QDDL.Model;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Windows;

namespace QDDL.Nlbs.SystemSet
{
    /// <summary>
    /// Interaction logic for SystemCheckSet.xaml
    /// </summary>
    public partial class SystemCheckSet 
    {
        SerialPort _serialport = null;
        private delegate void TimerDispatcherDelegate();
        public SerialPort SetSerialPort { set { _serialport = value; } }
        EncoderConvert EncoderConvert = new EncoderConvert();
        public SystemCheckSet()
        {
            InitializeComponent();
            checksetshow();
            //aTimer = new System.Timers.Timer(30000);
            //aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            //aTimer.Interval = 3000;
            //aTimer.Enabled = true;
        }

        //void OnTimedEvent(object serder, EventArgs e)
        //{
        //    this.Dispatcher.Invoke(DispatcherPriority.Normal,
        //        new TimerDispatcherDelegate(UpdateUI));
        //}

        //void UpdateUI()
        //{
        //    try
        //    {
        //        uint result = Convert.ToUInt32(EncoderPlc.getIntimeData(),16);
        //        Console.WriteLine("值："+result);
        //        this.machon.Text = Convert.ToInt64(EncoderPlc.getIntimeData(),16).ToString ();
        //        if (EncoderPlc.Direction())
        //        {
        //            direction_yes.Visibility = Visibility.Visible;
        //            direction_no.Visibility = Visibility.Hidden;
        //        }
        //        else
        //        {
        //            direction_yes.Visibility = Visibility.Hidden;
        //            direction_no.Visibility = Visibility.Visible;
        //        }
        //        if (EncoderPlc.ReDirection())
        //        {
        //            redirection_yes.Visibility = Visibility.Visible;
        //            redirection_no.Visibility = Visibility.Hidden;
        //        }
        //        else
        //        {
        //            redirection_yes.Visibility = Visibility.Hidden;
        //            redirection_no.Visibility = Visibility.Visible;
        //        }
        //    }
        //    catch(Exception ex)
        //    { 
        //    }
        //}
        void checksetshow()
        {
            // bool sl = SerializeXML<systemcheckset>.exit();

            List<systemcheckset> ls = new List<systemcheckset>();
            try
            {
                ls = SerializeXML<systemcheckset>.Getlist();
       

            if (ls.Count > 0)
            {
                systemcheckset s = new systemcheckset();
                s = ls.FirstOrDefault();
                this.tb_jarry.Text = s.arry ==null?"":s.arry.ToString();
                this.tb_jcount.Text = s.count ==null?"":s.count.ToString();              
                this.tb_throw.Text = s.throwvalue.ToString();
                radiobutton((s.ishavejuser==null||s.ishavejuser==false )?false:true);
                string[] time = s.noticetime.Split(':');
                if (time.Length >= 3)
                {
                    this.tb_hour.Text = Convert .ToInt32 (time[0]).ToString("D2");
                    this.tb_min.Text = Convert .ToInt32 ( time[1]).ToString("D2");
                    this.tb_second.Text =Convert .ToInt32 (time[2]).ToString("D2");
                }
                this.tb_day.Text = s.noticedays.ToString();
                if (s.noticeshow)
                    this.rb_show.IsChecked = true;
                else
                    this.rb_notshow.IsChecked = true;
            }
            }
            catch
            {
                // MessageAlert.Alert("没有相应的检验参数设置"); return;
            }
        }
   

      
        void savecheckset() {
            List<systemcheckset> ls = new List<systemcheckset>();
            try {
                systemcheckset s = new systemcheckset();
                s.throwvalue = Convert.ToDecimal (this.tb_throw.Text.Trim());
               // s.boundaryvalue = Convert.ToDecimal(this.tb_jmax .Text .Trim  ());
                s.count = Convert.ToInt32(this .tb_jcount .Text .Trim ());
                s.arry = Convert.ToInt32(this .tb_jarry .Text .Trim ());
                s.ishavejuser = (this.rb_true.IsChecked == null || this.rb_true.IsChecked == false) ? false : true;
                s.noticeshow=(this.rb_show.IsChecked == null || this.rb_show.IsChecked == false) ? false : true;
                s.noticetime =Convert .ToInt32 ( this.tb_hour.Text.Trim()).ToString ("D2") + ":" +Convert .ToInt32 (this.tb_min.Text.Trim()).ToString ("D2") + ":" +Convert .ToInt32 ( this.tb_second.Text.Trim()).ToString ("D2");
                s.noticedays = this.tb_day.Text.Trim();
                ls.Add(s);
                if (SerializeXML<systemcheckset>.exit())
                    SerializeXML<systemcheckset>.del();
                SerializeXML<systemcheckset>.SaveList(ls);
                MessageAlert.Alert("设置成功！");
            }

            catch (Exception ex) { MessageAlert.Error("出错！"+ex); }
        }

        bool IsValidate()
        {
            if (!IsNumeric(this.tb_jcount.Text.Trim()))
            {
                MessageAlert.Alert("校验次数不是正整数！");
                this.tb_jcount.Focus();
                return false ; 
            }
            if (!IsNumeric(this.tb_jarry.Text.Trim()))
            {
                MessageAlert.Alert("校验组数不是正整数！");
                this.tb_jarry.Focus();
                return false; 
            }
            try
            {
                Convert.ToDecimal(this.tb_throw.Text.Trim());
            }
            catch
            {
            MessageAlert .Alert ("校验抖动临界值必须数值！");
            this.tb_throw.Focus();
            return false; 
            }

            if (!(IsNumeric(this.tb_hour.Text.Trim()) && (Convert.ToInt32(this.tb_hour.Text.Trim()) < 24)))
            {
                MessageAlert.Alert("请填写正确的提示时间");
                this.tb_hour.Focus();
                return false; 

            }
            if (!(IsNumeric(this.tb_min.Text.Trim()) && (Convert.ToInt32(this.tb_min.Text.Trim()) < 60)))
            {
                MessageAlert.Alert("请填写正确的提示时间");
                this.tb_min.Focus();
                return false; 

            }
            if (!(IsNumeric(this.tb_second.Text.Trim()) && (Convert.ToInt32(this.tb_second.Text.Trim()) < 60)))
            {
                MessageAlert.Alert("请填写正确的提示时间");
                this.tb_second.Focus();
                return false; 
            }
            return true;
        }
        static bool IsNumeric(string str)
        {
            System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^[0-9]\d*$");
            return reg1.IsMatch(str);
        } 
   

        private void bt_set_Click(object sender, RoutedEventArgs e)
        {
           if(IsValidate())
            savecheckset();
        }
        void radiobutton(bool b)
        {
            this.rb_true.IsChecked = b;
            this.rb_false.IsChecked = !b;
        }
        private void rb_true_Click(object sender, RoutedEventArgs e)
        {
            radiobutton(true);
        }

        private void rb_false_Click(object sender, RoutedEventArgs e)
        {
            radiobutton(false);
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            //EncoderPlc = new EncoderPlc(_serialport);
            //this.tb_direction.Text =EncoderConvert.StringToDegree
            //(EncoderPlc.getReDirectionData());
            //this.tb_redirection.Text = EncoderConvert.StringToDegree(EncoderPlc .getDirectionData());
        }

        //private void bt_plcsubmit_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        if (Validate())
        //        {
        //            EncoderPlc.setDirectorData(this.tb_redirection.Text.Trim());
        //            EncoderPlc.setReDirectorData(this.tb_direction.Text.Trim());
        //            MessageAlert.Alert("设置成功！");
        //        }
        //    }
        //    catch (Exception ex)
        //    { 
            
        //    }
           
        //}
        //bool Validate()
        //{
        //    try
        //    {
        //        if (!_serialport.IsOpen)
        //            _serialport.Open();
        //    }
        //    catch
        //    {
        //        MessageAlert.Alert("编码器链接串口失败，无法设置");
        //        return false;
        //    }

        //    if (string.IsNullOrWhiteSpace(this.tb_redirection.Text.Trim()) || string.IsNullOrWhiteSpace(this.tb_direction.Text.Trim()))
        //    {
        //        MessageAlert.Alert("正反向间隙输入值不能为空");
        //        return false;
        //    }

        //    try
        //    {

        //        Convert.ToInt32 (this.tb_direction .Text .Trim ());
        //        Convert.ToInt32(this.tb_redirection.Text.Trim());
        //        return true;
        //    }
        //    catch
        //    {
        //        MessageAlert.Alert("请输入正整数的正反向间隙值");
        //        return false;
        //    }
        //}
    }
}
