
using QDDL.Nlbs.Check;
using QDDL.Nlbs.Common;
using QDDL.Nlbs.Help;
using QDDL.Nlbs.Notify;
using QDDL.Nlbs.SystemSet;
using QDDL.Nlbs.User;
using QDDL.Nlbs.Wrench;
using QDDL.BLL;
using QDDL.BLL.Plc;
using QDDL.BLL.ICCard;
using QDDL.Comm;
using QDDL.DAL;
using QDDL.Model;
using QDDL.Model.BllModel;
using Manager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace QDDL.Nlbs
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {

        //  public Login l = null;
        //   public userinfo _userinfo = new userinfo();
        IWrench Wrench = DataAccess.CreateWrench();
        public CheckFinal cf { set; get; } = null;
        // SerialPort rC = new SerialPort();
        SerialPort _t1 = new SerialPort();
        SerialPort _t2 = new SerialPort();
        public SerialPort EncoderPlcPort = new SerialPort();
        public ICardHelper ruc = null;
        public ReadCheckTester rct1 = null;
        public ReadCheckTester rct2 = null;
        TorqueTestModel _tester1 = new TorqueTestModel();
        TorqueTestModel _tester2 = new TorqueTestModel();
        List<TorqueTestModel> ttml = new List<TorqueTestModel>();

        public Thread thead1;
        public Thread thead2;
        System.Timers.Timer aTimer = null;
        private delegate void TimerDispatcherDelegate();
        private bool reallyCloseWindow = false;
        string strerror = "";

        editerWrench ew = null;
        public Main(ICardHelper _ruc)
        {
            this.ruc = _ruc;
            this.taskbarNotifier = new WinTaskbarNotifier();
            InitializeComponent();
            //  this.taskbarNotifier.Show();
            #region
            aTimer = new System.Timers.Timer(100);
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 100;
            aTimer.Enabled = true;

            string msg = "";
            try
            {
                getsystemset();
                //  校验仪
                try
                {

                    rct1 = new ReadCheckTester(_t1);
                    thead1 = new Thread(rct1.Read);
                    thead1.Start();
                    //if (!_t1.IsOpen)
                    //    _t1.Open();
                }
                catch
                {
                    msg += "校验仪1链接失败\r\n";
                    strerror += "校验仪1链接失败";
                }
                try
                {
                    rct2 = new ReadCheckTester(_t2);
                    thead2 = new Thread(rct2.Read);
                    thead2.Start();
                    //if (!_t2.IsOpen)
                    //    _t2.Open();
                }
                catch
                {
                    msg += "校验仪2链接失败\r\n";
                    strerror += "---校验仪2连接失败";
                }



                EncoderPlcPort.PortName = OperationConfig.GetValue("encodercom");
                EncoderPlcPort.BaudRate = 9600;
                EncoderPlcPort.DataBits = 7;
                EncoderPlcPort.StopBits = StopBits.One;
                EncoderPlcPort.Parity = Parity.Even;
                try
                {
                    if (!EncoderPlcPort.IsOpen)
                        EncoderPlcPort.Open();
                }
                catch
                {
                    msg += "编码器链接失败\r\n";
                    strerror += "---编码器连接失败";
                }

                if (msg != "")
                {
                    MessageBox.Show(msg);
                }

                this.porterror.Content = strerror;
            }
            catch
            {

                LogUtil.WriteLog(typeof(Main), "端口打开失败！,设置端口名称不能为空！");
                return;
            }
            #endregion
        }


        #region  taskbar
        private WinTaskbarNotifier taskbarNotifier;
        public WinTaskbarNotifier TaskbarNotifier
        {
            get { return this.taskbarNotifier; }
        }

        List<PowerList> getpower()
        {
            return SerializeXML<PowerList>.Getlist();
        }
        PowerList GetPowerList()
        {
            return getpower().Find(p => p.role == SystData.userInfo.role.roleName);
        }

        private bool show()
        {
            try
            {
                PowerList pl = GetPowerList();
                if (pl == null)
                {
                    MessageAlert.Alert("没有任何功能权限，无法登陆！");
                    return false;
                }
                list(menu);
                foreach (string s in pl.rolepower)
                {
                    foreach (MenuItem m in mll)
                    {
                        if (m.Items.Count > 0)
                        {
                            m.IsEnabled = true;
                        }
                        if (s == m.Header.ToString())
                        {
                            m.IsEnabled = true;
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        //protected override void OnClosing(System.ComponentModel.CancelEventArgs args)
        //{
        //    // In WPF it is a challenge to hide the window's close box in the title bar.
        //    // When the user clicks this, we don't want to exit the app, but rather just
        //    // put it back into hiding.  Unfortunately, this is a challenge too.
        //    // The follow code works around the issue.
        //    // this.taskbarNotifier.Close();

        //    if (!this.reallyCloseWindow)
        //    {
        //        // Don't close, just Hide.
        //        args.Cancel = true;
        //        // Trying to hide
        //        Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, (DispatcherOperationCallback)delegate (object o)
        //        {
        //            this.reallyCloseWindow = true;
        //            this.taskbarNotifier.Close();
        //            Environment.Exit(0);
        //            //this.Hide();
        //            return null;
        //        }, null);
        //    }
        //    else
        //    {
        //        // Actually closing window.

        //        //this.NotifyIcon.Visibility = Visibility.Collapsed;

        //        // Close the taskbar notifier too.
        //        if (this.taskbarNotifier != null)
        //            this.taskbarNotifier.Close();
        //    }
        //}


        private void NotifyIcon_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                // Open the TaskbarNotifier
                this.taskbarNotifier.Notify();
            }
        }

        private void NotifyIconOpen_Click(object sender, RoutedEventArgs e)
        {
            // Open the TaskbarNotifier
            this.taskbarNotifier.Notify();
        }

        //private void NotifyIconConfigure_Click(object sender, RoutedEventArgs e)
        //{
        //    // Show this window
        //    this.Show();
        //    this.Activate();
        //}

        //private void NotifyIconExit_Click(object sender, RoutedEventArgs e)
        //{
        //    // Close this window.
        //    this.reallyCloseWindow = true;
        //    this.taskbarNotifier.Close();
        //    Environment.Exit(0);

        //}
        private void about_Click(object sender, RoutedEventArgs e)
        {
            //this.taskbarNotifier.NotifyContent.Clear();
            //List<department> dl = new List<department>() { new department(){departmentName="nibu",comment="ssss"}};
            //// Add the new title and message to the TaskbarNotifier's content.
            //this.taskbarNotifier.NotifyContent.Add(new NotifyObject(dl));

            // Clear the textboxes.
            //  this.ClearTextBoxes();

            // Tell the TaskbarNotifier to open.
            // this.taskbarNotifier.Notify();
            AboutSoft a = new AboutSoft();
            a.Owner = this;
            a.ShowDialog();
        }
        void OnTimedEvent(object serder, EventArgs e)
        {
            // this.Dispatcher.Invoke(DispatcherPriority.Normal,    new TimerDispatcherDelegate(UpdateUI));
        }
        void UpdateUI()
        {
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.time.Content = "   当前时间： " + time;

            //  Notice(time);
        }
        private void Notice(string time)
        {
            List<systemcheckset> ls = new List<systemcheckset>();
            try
            {
                ls = SerializeXML<systemcheckset>.Getlist();


                if (ls != null && ls.Count > 0)
                {
                    if (time.EndsWith(ls.FirstOrDefault().noticetime))
                    {
                        this.taskbarNotifier.NotifyContent.Clear();
                        this.taskbarNotifier.NotifyContent.Add(new NotifyObject(GetWrenchList(Convert.ToInt32(ls.FirstOrDefault().noticedays))));
                        this.taskbarNotifier.Notify();
                    }
                }
            }
            catch { }
        }
        #endregion
        List<WrenchNotice> GetWrenchList(int days)
        {
            List<WrenchNotice> overdata = new List<WrenchNotice>();
            List<wrench> wrenchs = Wrench.select();
            foreach (wrench w in wrenchs)
            {
                if (w.cycletime > 0)
                {
                    DateTime st = Convert.ToDateTime(w.lastrepair).AddDays(Convert.ToDouble(w.cycletime));
                    DateTime ed = DateTime.Now;
                    if (st < ed)
                    {
                        WrenchNotice wn = new WrenchNotice();
                        //w.cycletime =Convert.ToDecimal ( w.cycletime.ToString("f1"));
                        //w.lastrepair = w.lastrepair.Replace('T',' ');
                        overdata.Add(new WrenchNotice() { wrenchbarcode = w.wrenchBarCode, cycletime = w.cycletime.ToString("f0"), intime = Convert.ToDateTime(w.lastrepair).AddDays(Convert.ToInt32(w.cycletime)).ToString(), lastrepairtime = w.lastrepair.ToString().Replace('T', ' ') });

                    }
                    else
                    {
                        int day = new TimeSpan(ed.Ticks - st.Ticks).Days;
                        if (day <= days)
                        {
                            WrenchNotice wn = new WrenchNotice();
                            //w.cycletime =Convert.ToDecimal ( w.cycletime.ToString("f1"));
                            //w.lastrepair = w.lastrepair.Replace('T',' ');
                            overdata.Add(new WrenchNotice() { wrenchbarcode = w.wrenchBarCode, cycletime = w.cycletime.ToString("f0"), intime = Convert.ToDateTime(w.lastrepair).AddDays(Convert.ToInt32(w.cycletime)).ToString(), lastrepairtime = w.lastrepair.ToString().Replace('T', ' ') });

                        }
                    }

                }
            }
            return overdata;

        }
        void getsystemset()
        {
            string porterror = "";
            ttml = SerializeXML<TorqueTestModel>.Getlist();
            if (ttml.Count > 0)
            {

                _tester1 = ttml.Find(p => p.testername == "校验仪1");
                if (_tester1 != null)
                {
                    try
                    {
                        _t1.PortName = _tester1.portname;
                        _t1.BaudRate = _tester1.baundrate;
                        _t1.DataBits = _tester1.databit;
                        porterror += "校验仪1链接端口为" + _tester1.portname;
                    }
                    catch
                    {
                        porterror += "校验仪1端口配置失败!";
                        //  MessageAlert.Error("校验仪1端口打开失败！");
                    }
                    finally
                    {
                        this.porterror.Content = porterror;
                    }
                }
                _tester2 = ttml.Find(p => p.testername == "校验仪2");
                if (_tester2 != null)
                {
                    try
                    {
                        _t2.PortName = _tester2.portname;
                        _t2.BaudRate = _tester2.baundrate;
                        _t2.DataBits = _tester2.databit;
                        porterror += "     校验仪2链接端口为" + _tester2.portname;
                    }
                    catch
                    {
                        porterror += "校验仪2端口配置失败!";
                        // MessageAlert.Error("校验仪2端口打开失败！");
                    }
                    finally
                    {

                        this.porterror.Content = "提示: " + porterror;
                    }

                }
            }

        }

        private void login_Click(object sender, RoutedEventArgs e)
        {
            //Login login = new Login();
            //Application.Current.MainWindow = login;
            //this.Close();
            //login.Show();
            //if (_userinfo == null || _userinfo.role == null || _userinfo.user == null)
            //{
            //    Login l = new Login(this);
            //    this.main.Children.Clear();
            //    this.main.Children.Add(l);
            //    return;
            //}
            UserloginState uls = new UserloginState();
            this.main.Children.Clear();
            this.main.Children.Add(uls);

        }
        private void updatepwd_Click(object sender, RoutedEventArgs e)
        {
            //if (_userinfo == null || _userinfo.role == null || _userinfo.user == null)
            //{
            //    MessageAlert.Warning("请先登录！");
            //    Login lg = new Login(this);
            //    this.main.Children.Clear();
            //    this.main.Children.Add(lg);
            //    return;
            //}

            ModifyPwd mp = new ModifyPwd(this);
            this.main.Children.Clear();
            this.main.Children.Add(mp);
        }

        private void systemreset_Click(object sender, RoutedEventArgs e)
        {

            Login login = new Login();
            Application.Current.MainWindow = login;
            this.Close();
            login.Show();

            //show();
            //this.user.Content = "当前登录用户: ";
            //Login l = new Login(this);
            //this.main.Children.Clear();
            //this.main.Children.Add(l);
        }
        List<MenuItem> mll = new List<MenuItem>();
        //void show()
        //{
        //    list(this.menu);

        //    foreach (MenuItem m in mll)
        //    {
        //        m.IsEnabled = false;

        //    }

        //}
        void list(Menu m)
        {
            List<MenuItem> ml = new List<MenuItem>();
            foreach (MenuItem mi in m.Items)
            {
                ml = (getlist(mi));
            }
            ml = mll;
        }

        List<MenuItem> getlist(MenuItem ml)
        {
            if (ml.Items.Count > 0)
                mll.Add(ml);
            foreach (var mi in ml.Items)
            {
                MenuItem m = mi as MenuItem;
                if (m == null)
                    continue;

                if (m.Items.Count > 0)
                {
                    getlist(m);
                }
                else { mll.Add(m); }
            }

            return mll;
        }
        private void logout_Click(object sender, RoutedEventArgs e)
        {
            if (MessageAlert.Alter("是否退出系统！"))
            {
                this.reallyCloseWindow = true;
                // this.taskbarNotifier.Close();
                Environment.Exit(0);
            }
        }


        private void adduser_Click(object sender, RoutedEventArgs e)
        {
            //if (eu != null &&!eu._disposed)
            //    eu.Dispose();
            //if (cf != null &&!cf._disposed)
            //    cf.Dispose();

            EditerUser eu = new EditerUser(ruc);
            this.main.Children.Clear();
            this.main.Children.Add(eu);
        }

        private void searchuser_Click(object sender, RoutedEventArgs e)
        {
            UserGrid ug = new UserGrid(ruc);
            this.main.Children.Clear();
            this.main.Children.Add(ug);
        }

        private void addwrench_Click(object sender, RoutedEventArgs e)
        {
            editerWrench ew = new editerWrench();
            this.main.Children.Clear();
            this.main.Children.Add(ew);

        }

        private void searchwrench_Click(object sender, RoutedEventArgs e)
        {
            WrenchList wl = new WrenchList();
            this.main.Children.Clear();
            this.main.Children.Add(wl);
        }

        private void checkset_Click(object sender, RoutedEventArgs e)
        {
            SystemCheckSet scs = new SystemCheckSet();
            // scs.SetSerialPort = this.EncoderPlc;
            this.main.Children.Clear();
            this.main.Children.Add(scs);
        }


        private void checkdata_Click(object sender, RoutedEventArgs e)
        {

            CheckFinal cf = new CheckFinal(ruc, rct1, rct2);
            cf.SetSerialPort = EncoderPlcPort;
            this.main.Children.Clear();
            this.main.Children.Add(cf);


        }

        private void tongxin_Click(object sender, RoutedEventArgs e)
        {
            TesterSet ts = new TesterSet();

            this.main.Children.Clear();
            this.main.Children.Add(ts);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            //  EditerDepartment ed = new EditerDepartment();
            DepartmentEditer ed = new DepartmentEditer();
            this.main.Children.Clear();
            this.main.Children.Add(ed);
        }

        private void rolemenu_Click(object sender, RoutedEventArgs e)
        {
            winEditorRole er = new winEditorRole();
            this.main.Children.Clear();
            this.main.Children.Add(er);
        }

        private void wrenchspecies_Click(object sender, RoutedEventArgs e)
        {
            EditerwrenchSpecies ewx = new EditerwrenchSpecies();
            this.main.Children.Clear();
            this.main.Children.Add(ewx);
        }

        private void checkoutsearch_Click(object sender, RoutedEventArgs e)
        {
            SearchChechResult scr = new SearchChechResult(this);
            this.main.Children.Clear();
            this.main.Children.Add(scr);
        }

        private void addwrenchstatus_Click(object sender, RoutedEventArgs e)
        {
            WrenchSatatus ws = new WrenchSatatus();
            this.main.Children.Clear();
            this.main.Children.Add(ws);
        }

        private void duty_Click(object sender, RoutedEventArgs e)
        {
            EditerDuty ed = new EditerDuty();
            this.main.Children.Clear();
            this.main.Children.Add(ed);
        }

        private void wrenchwaring_Click(object sender, RoutedEventArgs e)
        {
            WrenchRepair wp = new WrenchRepair();
            this.main.Children.Clear();
            this.main.Children.Add(wp);
        }

        private void powermenu_Click(object sender, RoutedEventArgs e)
        {
            PowerAllow pa = new PowerAllow(this);
            this.main.Children.Clear();
            this.main.Children.Add(pa);
        }

        private void Mainwindow_Loaded(object sender, RoutedEventArgs e)
        {
            // this.WindowState = System.Windows.WindowState.Normal;
            //  this.WindowStyle = System.Windows.WindowStyle.None;
            //this.ResizeMode = System.Windows.ResizeMode.NoResize;

            this.Left = 0.0;
            this.Top = 0.0;
            this.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
            show();
            if (this.cf == null)
            {
                this.main.Children.Add(cf = new CheckFinal(ruc, rct1, rct2));
                cf.SetSerialPort = EncoderPlcPort;
            }
            else
                this.main.Children.Add(cf);
            this.user.Content = "当前登录用户:" + SystData.userInfo.user.username;
            //    l = new Login(this);
            //this.main.Children.Clear();
            //this.main.Children.Add(l);
            ew = new editerWrench();
            List<systemcheckset> ls = new List<systemcheckset>();
            try
            {
                ls = SerializeXML<systemcheckset>.Getlist();
                if (ls != null && ls.Count > 0)
                {
                    if (ls.FirstOrDefault().noticeshow)
                    {
                        //this.taskbarNotifier.Show();
                        //this.taskbarNotifier.NotifyContent.Clear();
                        //this.taskbarNotifier.NotifyContent.Add(new NotifyObject(GetWrenchList(Convert.ToInt32(ls.FirstOrDefault().noticedays))));
                        //WinWrenchRepair WinWrenchRepair = new WinWrenchRepair(GetWrenchList(Convert.ToInt32(ls.FirstOrDefault().noticedays)));
                        //WinWrenchRepair.Show();
                    }
                }
            }
            catch { }
        }
        private void tongji_Click(object sender, RoutedEventArgs e)
        {
            CheckCountChart ccc = new CheckCountChart();
            this.main.Children.Clear();
            this.main.Children.Add(ccc);
        }

        private void wrenchalter_Click(object sender, RoutedEventArgs e)
        {
            wrenchalter wa = new wrenchalter();
            this.main.Children.Clear();
            this.main.Children.Add(wa);
        }

        private void checkdataset_Click(object sender, RoutedEventArgs e)
        {
            WrenchErrorrang we = new WrenchErrorrang();
            this.main.Children.Clear();
            this.main.Children.Add(we);
        }

        //private void barcodeprint_Click(object sender, RoutedEventArgs e)
        //{
        //    Process p = new Process();
        //    try
        //    {
        //        p.StartInfo.FileName = OperationConfig.GetValue("printpath");

        //        p.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;//该属性在WinForm中有效                

        //        p.StartInfo.UseShellExecute = true;

        //        p.StartInfo.CreateNoWindow = true;

        //        p.Start();
        //    }
        //    catch
        //    {
        //        LogUtil.WriteLog(typeof(Process), "条码打印启动失败！");
        //    }

        //}


        private void instruction_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AdobeReaderControl pdfCtl = new AdobeReaderControl(@"智能扭矩校验台.pdf");
                System.Windows.Forms.Integration.WindowsFormsHost WindowsFormsHost = new System.Windows.Forms.Integration.WindowsFormsHost();
                WindowsFormsHost.Child = pdfCtl;
                main.Children.Clear();
                main.Children.Add(WindowsFormsHost);
            }
            catch
            {
                MessageAlert.Alert("打开失败！");
                LogUtil.WriteLog(typeof(AdobeReaderControl), "智能扭矩校验台帮助文档打开失败！");
                return;
            }
        }

        private void Mainwindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (reallyCloseWindow)
            {
                if (MessageAlert.Alter("是否退出系统！"))
                {
                    TurnLight(false);
                    this.taskbarNotifier.Close();
                    rct1.ClosePort();
                    rct2.ClosePort();
                    if (ruc.IsOpen())
                        ruc.Close();
                    Environment.Exit(0);
                }
            }
            else
            {
                TurnLight(false);
                this.taskbarNotifier.Close();
                rct1.ClosePort();
                rct2.ClosePort();
                if (ruc.IsOpen())
                    ruc.Close();
            }

        }

        void TurnLight(bool status = true)
        {
            EncoderPlc Plc = new EncoderPlc(EncoderPlcPort);
            //Plc.SmallBlueLight(status);
            //Plc.SmallGreenLight(status);
            //Plc.SmallRedLight(status);
            Plc.BigLightOff();
            Plc.SamllLightOff();
            //Plc.BigBlueLight(status);
            //Plc.BigGreenLight(status);
            //Plc.BigRedLight(status);
            Plc.Close();

        }

        //private void borrow_Click(object sender, RoutedEventArgs e)
        //{
        //    WrenchBorrow wb = new WrenchBorrow(ruc);
        //    this.main.Children.Clear();
        //    this.main.Children.Add(wb);
        //}

        //private void return_Click(object sender, RoutedEventArgs e)
        //{
        //    WrenchReturn wr = new WrenchReturn(ruc);
        //    this.main.Children.Clear();
        //    this.main.Children.Add(wr);
        //}

        private void searchwrenchdata_Click(object sender, RoutedEventArgs e)
        {
            List<systemcheckset> ls = new List<systemcheckset>();
            //try
            //{
            ls = SerializeXML<systemcheckset>.Getlist();
            if (ls != null && ls.Count > 0)
            {
                //        if (ls.FirstOrDefault().noticeshow)
                //        {
                //this.taskbarNotifier.Show();
                //this.taskbarNotifier.NotifyContent.Clear();
                //this.taskbarNotifier.NotifyContent.Add(new NotifyObject(GetWrenchList(Convert.ToInt32(ls.FirstOrDefault().noticedays))));
                WinWrenchRepair WinWrenchRepair = new WinWrenchRepair(GetWrenchList(Convert.ToInt32(ls.FirstOrDefault().noticedays)));
                WinWrenchRepair.Show();
                //        }
            }
            //}
            //catch { } 
        }

        //List<WrenchNotice> GetWrenchList(int days)
        //{
        //    List<WrenchNotice> overdata = new List<WrenchNotice>();
        //    List<wrench> wrenchs = Wrench.select();
        //    foreach (wrench w in wrenchs)
        //    {
        //        if (w.cycletime > 0)
        //        {
        //            DateTime st = Convert.ToDateTime(w.lastrepair).AddDays(Convert.ToDouble(w.cycletime));
        //            DateTime ed = DateTime.Now;
        //            if (st < ed)
        //            {
        //                WrenchNotice wn = new WrenchNotice();
        //                //w.cycletime =Convert.ToDecimal ( w.cycletime.ToString("f1"));
        //                //w.lastrepair = w.lastrepair.Replace('T',' ');
        //                overdata.Add(new WrenchNotice() { wrenchbarcode = w.wrenchBarCode, cycletime = w.cycletime.ToString("f1"), intime = Convert.ToDateTime(w.lastrepair).AddDays(Convert.ToInt32(w.cycletime)).ToString(), lastrepairtime = w.lastrepair.ToString("yyyy-MM-dd") });

        //            }
        //            else
        //            {
        //                int day = new TimeSpan(ed.Ticks - st.Ticks).Days;
        //                if (day <= days)
        //                {
        //                    WrenchNotice wn = new WrenchNotice();
        //                    //w.cycletime =Convert.ToDecimal ( w.cycletime.ToString("f1"));
        //                    //w.lastrepair = w.lastrepair.Replace('T',' ');
        //                    overdata.Add(new WrenchNotice() { wrenchbarcode = w.wrenchBarCode, cycletime = w.cycletime.ToString("f1"), intime = Convert.ToDateTime(w.lastrepair).AddDays(Convert.ToInt32(w.cycletime)).ToString(), lastrepairtime = w.lastrepair.ToString("yyyy-MM-dd") });

        //                }
        //            }

        //        }
        //    }
        //    return overdata;

        //}
        //private void service_Click(object sender, RoutedEventArgs e)
        //{
        //    Service s = new Service();
        //    this.main.Children.Clear();
        //    this.main.Children.Add(s);
        //}

    }
}
