using QDDL.Nlbs.Print;
using QDDL.BLL;
using QDDL.BLL.Check;
using QDDL.BLL.ICCard;
using QDDL.BLL.Plc;
using QDDL.Comm;
using QDDL.DAL;
using QDDL.DAL.MySql;
using QDDL.Model;
using QDDL.Model.BllModel;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace QDDL.Nlbs.Check
{
    /// <summary>
    /// Interaction logic for CheckFinal.xaml
    /// </summary>
    public partial class CheckFinal
    {
         
        userinfo _juser { set; get; } = null;
        userinfo _zuer = null;
        Light _light = null;
        LightControl LightControl = null;
        public bool _disposed;
        Toolinfo _toolinfo = null;
        IUser User = DataAccess.CreateUser();
        IDepartment Department = DataAccess.CreateDepartment();
        IWrench Wrench = DataAccess.CreateWrench();
        IUserRole UserRole = DataAccess.CreateUserRole();
        IWrenchSpecies WrenchSpecies = DataAccess.CreateWrenchSpecies();
        GetUser getuser = new GetUser();
        systemcheckset _systemcheckset = new systemcheckset(); 
        List<TorqueTestModel> ttml = new List<TorqueTestModel>();

        SerialPort _serialEncoder = null;
        List<errorrangset> erl = new List<errorrangset>();
        List<ShowCheckresult> showcheckset = new List<ShowCheckresult>();
        ReadCheckTester rct1 = null;
        ReadCheckTester rct2 = null;
        ICardHelper ruc = null;
        EncoderPlc EncoderPlc = null;
        FilterData filterdata = new FilterData();
        public SerialPort SetSerialPort { set { _serialEncoder = value; } }
        //  System.Timers.Timer qTimer = null;           
        bool issave = false;
        int tempbarcodevalue = 0;
        private delegate void TimerDispatcherDelegate();

        public CheckFinal(ICardHelper r, ReadCheckTester r1, ReadCheckTester r2)
        {
            InitializeComponent();
            //读卡
            rct1 = r1;
            rct2 = r2;
            ruc = r;
            ruc.HandDataBack -= BackCardID;
            ruc.HandDataBack += BackCardID;

            rct1.HandDataBack -= Hand1show;
            rct1.HandDataBack += Hand1show;

            rct2.HandDataBack -= Hand2show;
            rct2.HandDataBack += Hand2show;
        }

        #region 重构获取校验仪
        void getTargetTester(decimal targetvalue)
        {
            bool isgettester = false;
            if (targetvalue == 0)
                return;
            List<TorqueTestModel> ttml = new List<TorqueTestModel>();
            ttml = SerializeXML<TorqueTestModel>.Getlist();
            if (ttml == null || ttml.Count <= 0)
                return;
            foreach (TorqueTestModel t in ttml)
            {

                if (targetvalue < t.maxvalue && targetvalue >= t.minvalue)
                {
                    if (t.testername == "校验仪1")
                    {
                        _testertype = true;
                        if (!rct1.Openport())
                        {
                            MessageBox.Show("校验仪1链接失败！");
                            tb_testername.Text = "";
                            return;
                        }
                        tb_testername.Text = "校验仪1";
                        isgettester = true;
                        //  UpdateCheckData(rct1);

                        break;
                    }
                    else
                    {
                        _testertype = false;
                        if (!rct2.Openport())
                        {
                            MessageBox.Show("校验仪2链接失败！");
                            tb_testername.Text = "";
                            return;
                        }
                        tb_testername.Text = "校验仪2";
                        // UpdateCheckData(rct2);
                        isgettester = true;
                        break;
                    }
                }
            }
            if (!isgettester)
            {
                MessageBox.Show("超出校验仪量程无法校验！");
                tb_testername.Text = "";
            }
        }
        #endregion
        #region 重构校验

        int _checktype = 3;//3 三点校验-  1-一点校验
        int _checkcount = 0;//记录校验次数//
        int _checkarr = 0;//记录校验组数
        bool _checking = false;//校验进行中   有扳手信息 有校验信息
        bool _havecheck = false;
        bool _checkover = false; //校验完成 false 未完成
        bool _onearryover = false;//一次校验结果 成功失败
        bool _checkresult = false;//校验结果  最中总结过
        int _checkindex = 0;//校验数据位置
        bool _testertype = false;//校验仪true t1  false t2
        int _listid = 1;
        string _temptbbarcode = "";

        string backdata = "";
        string backcard = "";
        DateTime time = DateTime.Now;

        /// <summary>
        /// 采集小校验数据显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Hand1show(Object sender, CardEventArgs e)
        {
            if (_checking && !_checkover && !_onearryover)
            {
                ReadCheckTester t = (ReadCheckTester)sender;
                if (_testertype)
                {
                    backdata = t.ReturnReadData(e.data);
                    Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.ApplicationIdle, new Action(testerTypemin));
                }
            }
        }
        private void testerTypemin()
        {
            try
            {
                decimal allowmin = Convert.ToDecimal(this.tb_jyw.Text.Trim());
                decimal allowmax = Convert.ToDecimal(this.tb_jywm.Text.Trim());
                if (string.IsNullOrEmpty(backdata))
                    return;
                ShowCheckresult cr = CheckResult(checkDataValidata(Convert.ToDecimal(backdata)), allowmin, allowmax);
                if (cr == null)
                {
                    return;
                }

                if (cr.result == "√")
                {
                    _light.Type = "03";
                    _light.Turn = true;
                    LightControl.ThreadControl(_light);
                    //EncoderPlc.SmallBlueLight(false);
                    //EncoderPlc.SmallGreenLight(true);
                    //EncoderPlc.SmallRedLight(false);
                }
                else
                {
                    _light.Type = "02";
                    _light.Turn = true;
                    LightControl.ThreadControl(_light);
                    //EncoderPlc.SmallBlueLight(false);
                    //EncoderPlc.SmallGreenLight(false);
                    //EncoderPlc.SmallRedLight(true);
                }
                //this.checktype.IsEnabled = false;
                this.onecheck.IsEnabled = false;
                this.threecheck.IsEnabled = false;
                this.editer_check.Visibility = Visibility.Hidden;
                // _checkover = false;
                _havecheck = true;
                AddToList(cr);
                CheckBinddata();
                checkControl();
            }
            catch (Exception e)
            {

            }
        }

        bool Bdirector = true;

        private void Test()
        {
            try
            {
                if (!EncoderPlc.ReadBigBit() && !EncoderPlc.ReadSmallBit())
                {
                    Bdirector = true;
                    return;
                }

                //int tempintime = Convert.ToInt32(EncoderPlc.getIntimeData(), 16);

                ////反转
                //  MessageBox.Show("正向标识：" + EncoderPlc.ReadFinalDirection().ToString() + "反向转标识：" + EncoderPlc.ReadFinalReDirection().ToString());

                // if (EncoderPlc.ReadFinalReDirection())
                // {
                //     Bdirector = true;
                // }
                //else
                // {

                //     Bdirector = false;
                // }



                if (EncoderPlc.ReadFinalDirection())
                {
                    Bdirector = false;
                }
                else
                {

                    Bdirector = true;
                }





                //  MessageBox.Show("最终标识：" + Bdirector.ToString());
                //if (EncoderPlc.Direction() && !EncoderPlc.ReDirection())
                //{
                //    Bdirector = true;
                //} 


                //if (tempintime<= tempbarcodevalue)
                //{
                //    if (Math.Abs(tempintime - tempbarcodevalue) <= 2)
                //    {                       
                //        Bdirector = false;
                //    }
                //    else
                //    {                        
                //        Bdirector = true;
                //    }
                //}

                tempbarcodevalue = Convert.ToInt32(EncoderPlc.getIntimeData(), 16);
                // DateTime t11 = DateTime.Now;
                //Console.WriteLine("t11" + t11.ToString("yyyy-MM-dd hh:mm:ss fff"));
            }
            catch (Exception ex)
            {
                Bdirector = true;
                // MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 大扭矩校验仪显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Hand2show(Object sender, CardEventArgs e)
        {
            DateTime dtnow = DateTime.Now;

            if (_checking && !_checkover && !_onearryover)
            {
                ReadCheckTester t = (ReadCheckTester)sender;
                if (!_testertype)
                {
                    string ct = OperationConfig.GetValue("checknexttime");
                    double temp = 0;
                    try
                    {
                        temp = Convert.ToDouble(ct);
                    }
                    catch
                    {

                    }
                    if ((dtnow - time).TotalSeconds > temp)
                    {

                        time = dtnow;
                        backdata = t.ReturnReadData(e.data);
                        Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(testerType));

                    }
                }
            }
        }

        /// <summary>
        /// 大校验仪
        /// </summary>
        void testerType()
        {
            try
            {
                // 有编码器无编码器
                Test();
                if (Bdirector)
                {

                    decimal allowmin = Convert.ToDecimal(this.tb_jyw.Text.Trim());
                    decimal allowmax = Convert.ToDecimal(this.tb_jywm.Text.Trim());
                    if (string.IsNullOrEmpty(backdata))
                        return;
                    ShowCheckresult cr = CheckResult(checkDataValidata(Convert.ToDecimal(backdata)), allowmin, allowmax);
                    if (cr == null)
                    {
                        return;
                    }
                    if (cr.result == "√")
                    {
                        _light.Type = "05";
                        _light.Turn = true;
                        LightControl.ThreadControl(_light);
                        //EncoderPlc.BigBlueLight(false);
                        //EncoderPlc.BigGreenLight(true);
                        //EncoderPlc.BigRedLight(false);
                    }
                    else
                    {
                        _light.Type = "04";
                        _light.Turn = true;
                        LightControl.ThreadControl(_light);
                        //EncoderPlc.BigBlueLight(false);
                        //EncoderPlc.BigGreenLight(false);
                        //EncoderPlc.BigRedLight(true);
                    }
                    //this.checktype.IsEnabled = false;
                    this.onecheck.IsEnabled = false;
                    this.threecheck.IsEnabled = false;
                    this.editer_check.Visibility = Visibility.Hidden;
                    // _checkover = false;
                    _havecheck = true;
                    AddToList(cr);
                    CheckBinddata();
                    checkControl();
                }
                //Thread.Sleep(500);

                //EncoderPlc.BigBlueLight(false);
                //EncoderPlc.BigGreenLight(false);
                //EncoderPlc.BigRedLight(true);
            }
            catch (Exception e)
            {

            }
        }


        void CheckBinddata()
        {
            this.dg_showcheck.ItemsSource = null;
            this.dg_showcheck.ItemsSource = showcheckset;

            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.ApplicationIdle, new Action(ProcessRows));
        }
        private void ProcessRows()
        {
            try
            {
                //for (int i = 0; i < colorIndex(showcheckset); i++)
                //{
                //    var row = dg_showcheck.ItemContainerGenerator.ContainerFromItem(dg_showcheck.Items[i]) as DataGridRow;
                //    // row.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ff7d40"));
                //    row.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ff6100"));
                //}

                this.dg_showcheck.SelectedIndex = this.dg_showcheck.Items.Count - 1;
                this.dg_showcheck.ScrollIntoView(this.dg_showcheck.SelectedItem);
            }
            catch
            { }
        }
        void AddToList(ShowCheckresult sc)
        {
            if (sc == null)
                return;
            sc.id = _listid;
            showcheckset.Add(sc);
            _listid++;
        }
        /// <summary>
        /// 单循环是否结束
        /// </summary>
        /// <returns></returns>
        bool isLastCheckData()
        {
            if (_systemcheckset != null && _systemcheckset.count != null)
            {
                if (_checkcount >= _systemcheckset.count)
                    return true;
                return false;
            }

            return true;
        }
        /// <summary>
        /// 组数是否结束
        /// </summary>
        /// <returns></returns>
        bool isLastCheckArry()
        {
            if (_systemcheckset != null && _systemcheckset.arry != null)
            {
                if (_checkarr >= _systemcheckset.arry)
                    return true;
                return false;
            }
            return true;
        }
        /// <summary>
        /// 校验是否完成
        /// </summary>
        /// <returns></returns>
        bool isLastData()
        {
            if (_checkindex >= _checktype - 1)
                return true;
            return false;

        }
        void checkControl()
        {
            if (isLastCheckArry())
            {
                if (isLastCheckData())
                {
                    if (isLastData())
                    {
                        //扳手合格，显示结果 校验完成
                        _checkover = true;
                        _checkresult = true;
                        ShowCheckResult(_checkresult);
                        return;
                    }
                    else
                    {
                        ///改变目标值继续
                        changeCheckValue();
                        _checkcount = 0;
                        _checkarr = 0;
                    }
                }
                else
                {
                    //扳手不和个 停止校验
                    _checkresult = false;
                    _checkover = true;
                    ShowCheckResult(_checkresult);
                    return;
                }
            }
            else
            {
                if (isLastCheckData())
                {
                    if (isLastData())
                    {
                        //扳手合格，显示结果
                        _checkover = true;
                        _checkresult = true;
                        ShowCheckResult(_checkresult);
                        return;
                    }
                    else
                    {
                        ///改变目标值继续
                        changeCheckValue();
                        _checkcount = 0;
                        _checkarr = 0;
                    }
                }
            }
        }
        void ShowCheckResult(bool isgood)
        {
            if (isgood)
            {
                this.lb_result.Content = "扳手合格";
            }
            else
            {
                this.lb_result.Content = "扳手不合格";
            }
            MessageAlert.Alert("该扳手校验完成");
            this.bt_queren.Focus();
        }

        void changeCheckValue()
        {
            try
            {
                _onearryover = true;
                MessageAlert.Alert(list_check.Items[_checkindex] + "  完成");
                _checkindex++;
                this.list_check.ScrollIntoView(list_check.Items[_checkindex]);
                this.editer_check.Visibility = Visibility.Visible;
                getTargetTester(Convert.ToDecimal(list_check.Items[_checkindex]));
                checkpoint.Content = "校验点" + (_checkindex + 1).ToString();
                this.editer_check.Focus();
                _onearryover = false;
            }
            catch { }
        }

        ShowCheckresult CheckResult(decimal checkdata, decimal allowmin, decimal allwmax)
        {
            ShowCheckresult sc = new ShowCheckresult();
            try
            {
                if (checkdata == 0)
                    return null;
                if (checkdata < 0)
                {
                    sc.isturn = false;
                    checkdata = Math.Abs(checkdata);
                }
                else { sc.isturn = true; }
                sc.id = 1;
                sc.checkdata = checkdata;
                sc.setdata = Convert.ToDecimal(GetTargetValue());
                decimal min = sc.setdata + (sc.setdata * allowmin / 100);
                decimal max = sc.setdata + (sc.setdata * allwmax / 100);
                sc.normalrang = min.ToString("f2") + "~" + max.ToString("f2");
                sc.errorrang = Convert.ToDecimal(((checkdata - sc.setdata) / sc.setdata).ToString("f4"));
                sc.normalmin = (allowmin / 100).ToString("f2");
                sc.normalmax = (allwmax / 100).ToString("f2");
                sc.error = (sc.errorrang * 100).ToString("f2") + "%";
                if (sc.checkdata <= max && sc.checkdata >= min)
                {
                    sc.result = "√";
                    _checkcount++;
                }
                else
                {
                    sc.result = "×";
                    _checkcount = 0;
                    _checkarr++;
                }
                return sc;
            }
            catch
            {
                return null;
            }
        }

        decimal checkDataValidata(decimal backdata)
        {
            try
            {
                if (_systemcheckset == null || _systemcheckset.throwvalue == null || _systemcheckset.throwvalue <= 0)
                    return backdata;

                decimal targetvalue = Convert.ToDecimal(GetTargetValue());
                decimal throwdatemin = targetvalue * (1 - (1 - ((_systemcheckset.throwvalue ?? 0) / 100)));
                decimal throwdatemax = targetvalue * (1 + (1 - ((_systemcheckset.throwvalue ?? 0) / 100)));
                if (Math.Abs(backdata) > throwdatemax || Math.Abs(backdata) < throwdatemin)
                    return 0;
                return backdata;
            }
            catch { return backdata; }

        }
        double GetTargetValue()
        {
            try
            {
                return Convert.ToDouble(this.list_check.Items[_checkindex]);
            }
            catch
            {
                MessageAlert.Alert("请填写正确的校验值！");
                return 0;
            }
        }
        #endregion

        #region common

        void getsystemset()
        {
            try
            {
                List<systemcheckset> scsl = SerializeXML<systemcheckset>.Getlist();
                erl = SerializeXML<errorrangset>.Getlist();
                ttml = SerializeXML<TorqueTestModel>.Getlist();
                if (scsl.Count > 0)
                {
                    _systemcheckset = scsl.FirstOrDefault();
                }
            }
            catch
            {
                MessageAlert.Alert("校验仪没有任何进行设置！不能校验！");
            }
        }

        //void OnTimedEvent(object serder, EventArgs e)
        //{
        //    this.Dispatcher.Invoke(DispatcherPriority.Normal,
        //        new TimerDispatcherDelegate(UpdateUI));
        //}


        //void UpdateUI()
        //{
        //  //  UpdateCardInfo();
        //  //   UpdateCheckDataUI();
        //    try
        //    {

        //        this.machon.Text = Convert.ToInt64(EncoderPlc.getIntimeData(), 16).ToString();
        //        if (Convert.ToInt32(EncoderPlc.getIntimeData(), 16) >temp)
        //        {
        //            derector = false;

        //        }
        //        if (Convert.ToInt32(EncoderPlc.getIntimeData(), 16) <temp)
        //        {
        //            derector = true;
        //        }
        //        if (Convert.ToInt32(EncoderPlc.getIntimeData(), 16)!=temp)
        //          temp = Convert.ToInt32(EncoderPlc.getIntimeData(), 16);
        //        if (derector)
        //        {
        //            direction.Visibility = Visibility.Visible;
        //            redirection.Visibility = Visibility.Hidden;
        //        }
        //        else
        //        {
        //            direction.Visibility = Visibility.Hidden;
        //            redirection.Visibility = Visibility.Visible;
        //        }
        //       // Console.WriteLine("方向值：" + (derector == true ? "正向" : "反向|||") + Convert.ToInt32(EncoderPlc.getIntimeData(), 16).ToString ());
        //        //if (!derector)
        //        //{
        //        //    redirection.Visibility = Visibility.Visible;
        //        //    direction.Visibility = Visibility.Hidden;
        //        //}
        //        //else
        //        //{
        //        //    redirection.Visibility = Visibility.Hidden;
        //        //    direction.Visibility = Visibility.Visible;
        //        //}
        //    }
        //    catch
        //    {

        //    }

        //}

        #endregion

        #region   获取员工信息 Userinfo

        void BackCardID(object sender, CardEventArgs e)
        {
            //ReadUserCard ruc = (ReadUserCard)sender;
            //backcard = ruc.BackString();
            backcard = e.data;
            Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(UpdateCardInfo));
        }

        void UpdateCardInfo()
        {
            try
            {
                this.Cursor = Cursors.Wait;
                showjuser(_juser);
                showzuser(_zuer);
                if (OperationConfig.GetValue("CardSort") == "USB")
                {
                    Getuserinfo(backcard);
                }
                else
                {
                    filterdata.CardId = backcard;
                    filterdata.resetCard();
                    string cardid = filterdata.getcardid();
                    if (cardid == "")
                        return;
                    Getuserinfo(cardid);
                    filterdata.resetid("");
                }


                this.tb_wrenchbarcode.Focus();
                showjuser(_juser);
                showzuser(_zuer);
                this.Cursor = Cursors.Arrow;
            }
            catch { }

        }

        bool GetJuser(string cardid, string pwd)
        {
            try
            {
                List<users> us = getuser.getusers(cardid, pwd);

                if (us != null && us.Count > 0)
                {
                    userinfo u = getuser.getuserinfo(us);
                    if (u.role.roleDM == "003")
                        _juser = u;
                    else
                    {
                        MessageAlert.Alert("不是校验员信息！无法登陆！");
                        return false;
                    }
                    return true;
                }
                else
                {
                    MessageAlert.Alert("登录信息有误！无法登陆！");
                    return false;
                }

            }
            catch
            {
                MessageAlert.Alert("系统异常！无法登陆！");
                return false;
            }

        }
        bool GetJuser(string cardid)
        {
            userinfo user = new userinfo();
            try
            {
                user = getuser.getuserinfo(getuser.getusers(cardid));
                if (user == null)
                    return false;
                if (user.role == null)
                    return false;
                if (user.role.roleDM == "003")
                { _juser = user; }
                if (user.role.roleDM == "004")
                { _zuer = user; }
                //   Console.WriteLine(user.user .username);
                return true;
            }
            catch { return false; }

        }
        bool GetZuser(string cardid)
        {
            try
            {
                userinfo u = getuser.getuserinfo(getuser.getusers(cardid));
                if (u == null || u.user == null)
                    return false;
                if (u.role == null)
                    return false;
                if (u.user.guid == _juser.user.guid)
                {
                    return false;
                }
                _zuer = u;
                return true;
            }
            catch { return false; }

        }
        bool GetZuser(string cardid, string pwd)
        {
            try
            {
                List<users> us = getuser.getusers(cardid, pwd);

                if (us.Count > 0)
                {
                    userinfo u = getuser.getuserinfo(us);
                    if (u != null && u.role != null && u.role.roleDM == "004")
                        _zuer = u;
                    else
                    {
                        MessageAlert.Alert("不是质检员信息！无法登陆");
                        return false;
                    }
                    return true;
                }
                else
                {
                    MessageAlert.Alert("登录信息有误！无法登陆！");
                    return false;
                }

            }
            catch
            {
                MessageAlert.Alert("系统异常！无法登陆！");
                return false;
            }
        }

        void Getuserinfo(string cardid)
        {
            GetJuser(cardid);
        }
        /// <summary>
        /// 校验员
        /// </summary>
        /// <param name="su"></param>
        void showjuser(userinfo su)
        {
            if (su == null) return;
            if (su.user == null) return;
            this.lb_jname.Text = su.user.username;
            this.lb_jdep.Text = su.department.departmentName;
        }
        /// <summary>
        /// 质检员
        /// </summary>
        /// <param name="su"></param>
        void showzuser(userinfo su)
        {
            if (su == null) return;
            if (su.user == null) return;
            this.tb_zname.Text = su.user.username;
            this.tb_zdep.Text = su.department.departmentName;
        }



        private void bt_jloginout_Click(object sender, RoutedEventArgs e)
        {
            _juser = new userinfo();
            this.lb_jname.Text = "";
            this.lb_jdep.Text = "";
        }

        private void bt_zloginout_Click(object sender, RoutedEventArgs e)
        {
            _zuer = new userinfo();
            this.tb_zname.Text = "";
            this.tb_zdep.Text = "";
        }


        private void bt_jlogin_Click(object sender, RoutedEventArgs e)
        {
            jlogin();
        }
        void jlogin()
        {
            sp_jlogin.Visibility = Visibility.Visible;
            sp_juser.Visibility = Visibility.Hidden;
        }
        private void bt_zlogin_Click(object sender, RoutedEventArgs e)
        {
            zlogin();
        }
        void zlogin()
        {
            sp_zlogin.Visibility = Visibility.Visible;
            sp_zuser.Visibility = Visibility.Hidden;
        }
        private void bt_jlogout_Click(object sender, RoutedEventArgs e)
        {
            jlogout();
        }
        void jlogout()
        {
            sp_jlogin.Visibility = Visibility.Hidden;
            sp_juser.Visibility = Visibility.Visible;
            jcardid.Clear();
            jpwd.Clear();
        }
        private void bt_zlogout_Click(object sender, RoutedEventArgs e)
        {
            zlogout();
        }
        void zlogout()
        {
            sp_zlogin.Visibility = Visibility.Hidden;
            sp_zuser.Visibility = Visibility.Visible;
            zcardid.Clear();
            zpwd.Clear();

        }
        private void jcardid_GotFocus(object sender, RoutedEventArgs e)
        {
            this.jcardid.Clear();
        }

        private void jpwd_GotFocus(object sender, RoutedEventArgs e)
        {
            this.jpwd.Clear();
        }

        private void zcardid_GotFocus(object sender, RoutedEventArgs e)
        {
            this.zcardid.Clear();
        }

        private void zpwd_GotFocus(object sender, RoutedEventArgs e)
        {
            this.zpwd.Clear();
        }

        private void bt_jlogini_Click(object sender, RoutedEventArgs e)
        {
            if (jcardid.Text.Trim() == "")
            { MessageAlert.Warning("请填写校验员卡号"); return; }
            if (jpwd.Password.Trim() == "")
            { MessageAlert.Warning("请输入校验员密码！"); return; }
            if (GetJuser(this.jcardid.Text.Trim(), this.jpwd.Password.Trim()))
            {

                showjuser(_juser);
                sp_jlogin.Visibility = Visibility.Hidden;
                sp_juser.Visibility = Visibility.Visible;
                jcardid.Clear();
                jpwd.Clear();
            }

        }

        private void bt_zlogini_Click(object sender, RoutedEventArgs e)
        {
            if (zcardid.Text.Trim() == "")
            { MessageAlert.Warning("请填写校验员卡号"); return; }
            if (zpwd.Password.Trim() == "")
            { MessageAlert.Warning("请输入校验员密码！"); return; }
            if (GetZuser(this.zcardid.Text.Trim(), this.zpwd.Password.Trim()))
            {
                showzuser(_zuer);
                sp_zlogin.Visibility = Visibility.Hidden;
                sp_zuser.Visibility = Visibility.Visible;
                zcardid.Clear();
                zpwd.Clear();
                //zlogout();
            }
        }
        #endregion

        #region  获取工具信息 wrenchinfo

        private void tb_wrenchbarcode_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void tb_wrenchbarcode_KeyDown(object sender, KeyEventArgs e)
        {
            this.lb_status.Content = "获取工具信息！";
            try
            {
                if (e.Key == Key.Enter)
                {

                    this.bt_wrenchbarcode_Click(this, e);
                }
            }
            catch
            {
                return;
            }
        }



        private void tb_wrenchbarcode_GotFocus(object sender, RoutedEventArgs e)
        {
            this.tb_wrenchbarcode.SelectAll();
            // showwrench(null);

        }
        bool IsValidBarcode(string barcode)
        {
            string regexstr = OperationConfig.GetValue("barcoderegex");
            if (string.IsNullOrEmpty(regexstr))
                return true;
            return Regex.IsMatch(barcode, @regexstr);
        }

        bool isUserExit()
        {
            if (_juser == null || _juser.user == null)
            {
                // bt_jloginout.Focus();
                MessageAlert.Warning("请先登录校验员信息");
                return false;
            }

            if (_zuer == null || _zuer.user == null || _zuer.user.guid == null)
            {
                if (_systemcheckset != null && _systemcheckset.ishavejuser != null && _systemcheckset.ishavejuser == true)
                {
                    //  tb_zname.Focus();
                    MessageAlert.Warning("缺少质检员信息不能校验！");
                    return false;
                }
            }
            return true;

        }


        private void bt_wrenchbarcode_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (this.tb_wrenchbarcode.Text.Trim() == "")
                {
                    _temptbbarcode = "";
                    return;
                }
                if (!IsValidBarcode(this.tb_wrenchbarcode.Text.Trim()))
                {
                    this.tb_wrenchbarcode.Text = _temptbbarcode;
                    return;
                }
                if (_havecheck)
                {
                    this.checkpoint.Focus();
                    if (!MessageAlert.Alter("扳手正在校验，是否继续扫码操作"))
                    {
                        this.tb_wrenchbarcode.Text = _temptbbarcode;
                        return;
                    }

                }
                getsystemset();
                if (!isUserExit())
                    return;
                _temptbbarcode = this.tb_wrenchbarcode.Text.Trim();
                this.tb_wrenchbarcode.SelectAll();
                ClearOut();

                this.editer_check.Focus();
                _toolinfo = new Toolinfo();
                //TurnLight(true);
                _light.Type = "00";
                _light.Turn = false;
                LightControl.ThreadControl(_light);
                _toolinfo.wrench = Wrench.selectByBarcode(this.tb_wrenchbarcode.Text.Trim());
                if (_toolinfo == null || _toolinfo.wrench == null)
                    return;
                _toolinfo.speciesName = WrenchSpecies.selectByGuid(_toolinfo.wrench.species).speciesName;

                if (_toolinfo != null && _toolinfo.wrench != null && _toolinfo.wrench.lastrepair != null)// && _toolinfo.wrench.cycletime != decimal.)
                {
                    if (Convert.ToInt32(_toolinfo.wrench.cycletime) > 0)
                    {

                        DateTime dt1 = DateTime.Now;
                        DateTime dt2 = Convert.ToDateTime(_toolinfo.wrench.lastrepair);
                        TimeSpan ts1 = dt1.Subtract(dt2);
                        if ((Convert.ToInt32(ts1.Days) >= 0) && !_toolinfo.wrench.isallowcheck)
                        {
                            MessageAlert.Warning("扳手超出维修保护期！\n     不能校验！");
                            _toolinfo = null;
                            return;
                        }
                    }
                }
                CheckCountBind(_toolinfo);
                showwrench(_toolinfo);
                _checking = true;
                getTargetTester(Convert.ToDecimal(list_check.Items[_checkindex]));
                list_check.ScrollIntoView(list_check.Items[_checkindex]);
                onecheck_Click(sender, e);
                this.editer_check.Focus();
            }
            catch
            {
                _light.Type = "00";
                _light.Turn = false;
                LightControl.ThreadControl(_light);
                return;
            }
        }

        void CheckCountBind(Toolinfo t)
        {
            if (t != null && t.wrench != null)
            {
                this.list_check.Items.Clear();
                if ((bool)onecheck.IsChecked)
                {
                    this.list_check.Items.Add(t.wrench.targetvalue.ToString("f1"));
                    this._checktype = 1;
                }

                else if ((_toolinfo.wrench.targetvalue1 > 0) && (_toolinfo.wrench.targetvalue2 > 0))
                {
                    this.list_check.Items.Add(_toolinfo.wrench.targetvalue1.ToString("f1"));
                    this.list_check.Items.Add(_toolinfo.wrench.targetvalue.ToString("f1"));
                    this.list_check.Items.Add(_toolinfo.wrench.targetvalue2.ToString("f1"));
                    this._checktype = 3;
                    this.threecheck.IsChecked = true;
                }

            }
        }

        private void onecheck_Click(object sender, RoutedEventArgs e)
        {

            if (_toolinfo == null || _toolinfo.wrench == null)
                return;
            this.list_check.Items.Clear();
            this.list_check.Items.Add(_toolinfo.wrench.targetvalue.ToString("f1"));
            getTargetTester(Convert.ToDecimal(list_check.Items[_checkindex]));
            this._checktype = 1;
            this.onecheck.IsChecked = true;
            this.editer_check.Focus();
        }

        private void threecheck_Click(object sender, RoutedEventArgs e)
        {

            if (_toolinfo == null || _toolinfo.wrench == null)
                return;
            this.list_check.Items.Clear();
            this.list_check.Items.Add(_toolinfo.wrench.targetvalue1.ToString("f1"));
            this.list_check.Items.Add(_toolinfo.wrench.targetvalue.ToString("f1"));
            this.list_check.Items.Add(_toolinfo.wrench.targetvalue2.ToString("f1"));
            this._checktype = 3;
            this.threecheck.IsChecked = true;
            getTargetTester(_toolinfo.wrench.targetvalue1);
            this.editer_check.Focus();
        }

        void tbempty()
        {

            this.tb_bjb.Text = "";
            this.tb_lc.Text = "";
            this.tb_jyw.Text = "";
            this.tb_jywm.Text = "";
            this.tb_testername.Text = "";
            this.lb_result.Content = "";
        }
        void showwrench(Toolinfo t)
        {
            if (t == null || t.wrench == null)
            {
                tbempty();
                return;
            }
            this.tb_bjb.Text = t.wrench.wrenchCode.ToString();
            this.tb_lc.Text = t.wrench.rangeMin.ToString("f1") + "~" + t.wrench.rangeMax.ToString("f1");
            if (erl == null || erl.Count <= 0)
            {
                MessageAlert.Alert("没有该扳手相关的误差设置！\n     无法校验！");
                return;
            }
            List<errorrangset> tm = erl.Where(p => p.speciesID == t.wrench.species).ToList();
            foreach (errorrangset e in tm)
            {
                decimal targetvalue = Convert.ToDecimal(GetTargetValue());
                if (e.rangmax > targetvalue && e.rangmin <= targetvalue)
                {
                    this.tb_jywm.Text = e.errorrangMax.ToString();
                    this.tb_jyw.Text = e.errorrangMin.ToString();
                    getTargetTester(targetvalue);
                }
            }
            if (_systemcheckset != null)
            {
                this.tb_jyc.Text = _systemcheckset.count.ToString();
                this.tb_jya.Text = _systemcheckset.arry.ToString();
            }
            this.lb_status.Content = "扳手信息获取成功！";
        }
        void geterrorrang(Toolinfo t, string targetvalue)
        {
            if (t == null || t.wrench == null)
            {
                this.tb_jyw.Text = "";
                this.tb_jywm.Text = "";
                return;
            }
            List<errorrangset> tm = erl.Where(p => p.speciesID == t.wrench.species).ToList();
            foreach (errorrangset e in tm)
            {
                if (e.rangmax >= Convert.ToDecimal(targetvalue) && e.rangmin <= Convert.ToDecimal(targetvalue))
                {
                    this.tb_jywm.Text = e.errorrangMax.ToString();
                    this.tb_jyw.Text = e.errorrangMin.ToString();
                    break;
                }
                else
                {
                    this.tb_jyw.Text = "";
                    this.tb_jywm.Text = "";
                }
            }
        }
        #endregion

        #region  校验 Checkdata

        // int group = 1;
        // int oncecount = 1;
        // bool iswrenchgood = false;
        // bool isallover = false;
        //// bool isAllover = false;
        // List<string > hascheckdata = new List<string >();

        // void UpdateCheckDataUI()
        // {
        //     decimal tempsetdata = 0;
        //     if (_toolinfo == null || _toolinfo.wrench == null)
        //     {
        //         rct1.isread = true;
        //         rct2.isread = true;
        //         return;
        //     }
        //     tempsetdata = Convert.ToDecimal(GetTargetValue());
        //     GetTargetTester(tempsetdata);
        // }

        // void UpdateCheckData(ReadCheckTester rct)
        // {
        //     try
        //     {
        //        // if (rct.ReturnReadData()>0)
        //         {
        //             if (isallover)
        //             {
        //                 rct.isread = true;
        //                  MessageAlert.Warning("该扳手校验完成！");
        //                 return;
        //             }
        //             if (GetTargetValue() == 0)
        //             {
        //                 rct.isread = true;
        //                 MessageAlert.Warning("请设置目标值！");
        //                 return;
        //             }
        //             if (IsHasCheck(GetTargetValue().ToString ("f2")))
        //             {

        //                 rct.isread = true;
        //                 MessageAlert.Warning("该目标校验值已经校验完成,不能重复校验");
        //                 return;
        //             }
        //             if (IsAllOver(this.cb_checklist.ItemsSource as List<string>))
        //             {
        //                 rct.isread = true;
        //                 MessageAlert.Warning("扳手校验完成！");
        //                 return;
        //             }
        //             if (this.tb_jyw.Text.Trim() == "" || this.tb_jywm.Text.Trim() == "")
        //             {
        //                 rct.isread = true;
        //                 MessageAlert.Warning("该扳手没有相应的校验策略！请联系管理员设置策略！");
        //                 return;
        //             }

        //             if (Convert.ToDecimal(GetTargetValue()) < _toolinfo.wrench.rangeMin || Convert.ToDecimal(GetTargetValue()) > _toolinfo.wrench.rangeMax)
        //             {
        //                 rct.isread = true;
        //                 MessageAlert.Warning("校验设定值不再扳手量程范围内");
        //                 return;
        //             }

        //             if (isfinish)
        //             {
        //                 rct.isread = true;
        //                 // MessageAlert.Warning("该扳手校验完成！");
        //                 return;
        //             }
        //             this.lb_result.Content = "";
        //             this.lb_status.Content = "数据校验";
        //             ShowCheckData(rct);
        //         }
        //     }
        //     catch { }
        // }

        // void ShowCheckData(ReadCheckTester rct)
        // {

        //    //decimal checkdata = Math.Abs (rct.ReturnReadData());
        //    // rct.isread = true;
        //    // if (checkdata <= 0)
        //    //     return;
        //    // ShowCheckresult sc = CheckResult(CheckDataValidate(checkdata));
        //    // AddToList(sc);
        //    // OnceSuccess(sc);          
        //    // CheckBinddata();
        // }

        // #region 获取校验仪
        // void GetTargetTester(decimal targetvalue)
        // {
        //     if (targetvalue == 0)
        //         return;
        //     List<TorqueTestModel> ttml = new List<TorqueTestModel>();
        //     ttml = SerializeXML<TorqueTestModel>.Getlist();
        //     if (ttml == null || ttml.Count <= 0)
        //         return;
        //     foreach (TorqueTestModel t in ttml)
        //     {

        //         if (targetvalue < t.maxvalue && targetvalue >= t.minvalue)
        //         {
        //             if (t.testername == "校验仪1")
        //             {
        //                 tb_testername.Text = "校验仪1";
        //                 UpdateCheckData(rct1);
        //                 break;
        //             }
        //             else
        //             {
        //                 tb_testername.Text = "校验仪2";
        //                 UpdateCheckData(rct2);
        //                 break;
        //             }
        //         }
        //     }
        // }
        // #endregion

        // #region  更换校验值
        // private void cb_checklist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        // {
        //     //if (this.cb_checklist.SelectedIndex >= 0)
        //     //{
        //     //    this.tb_setvalue.Text = this.cb_checklist.SelectedValue.ToString();
        //     //    geterrorrang(_toolinfo);
        //     //    arry = 1;
        //     //    successcount = 1;
        //     //    isfinish = false;
        //     //}
        // }
        // #endregion



        // void OnceSuccess(ShowCheckresult sc)
        // {
        //     systemcheckset sysset = null;
        //     if (sc == null)
        //         return;
        //     List<systemcheckset> scsl = SerializeXML<systemcheckset>.Getlist();
        //     if (scsl != null || scsl.Count > 0)
        //         sysset = scsl.FirstOrDefault();
        //     if (oncecount >= sysset.count)
        //     {
        //         group = sysset.arry + 1 ?? 4;          
        //     }
        //     if (sc.result.Equals("√"))
        //     {
        //         this.lb_status.Content = "当前数据合格";
        //         oncecount++;
        //     }              
        //     else
        //     {
        //         this.lb_status.Content = "当前数据不合格";
        //         oncecount++;
        //         oncecount = 1;
        //         group++;
        //     }
        //     if (group > sysset.arry)
        //     {
        //         if (oncecount >= sysset.count)
        //         {

        //             this.lb_result.Content = "本次校验合格";
        //             iswrenchgood = true;
        //             if (!hascheckdata.Contains(sc.setdata.ToString("f2")))
        //                 hascheckdata.Add(sc.setdata.ToString("f2"));
        //         }
        //         else
        //         {
        //             this.lb_status.Content = "本次校验不合格";
        //            // this.lb_result.Content = "本次校验不合格";
        //             iswrenchgood = false;
        //             List<string> newlist = ((List<string>)this.cb_checklist.ItemsSource);

        //             foreach (string s in newlist)
        //             {
        //                 if (!hascheckdata.Contains(s))
        //                     hascheckdata.Add(s);
        //             }                   
        //         }               
        //         group = 1;
        //         oncecount = 1;
        //        // AddCheckList(GetTargetValue().ToString ("f2"));

        //         ShowAllOver(iswrenchgood ,IsAllOver(this.cb_checklist.ItemsSource as List <string >));
        //         MessageAlert.Alert(GetTargetValue().ToString("f2") + "校验结束！");
        //         isfinish = true;
        //         this.cb_checklist.SelectedIndex++;
        //     }
        // }
        // //void AddToList(ShowCheckresult sc)
        // //{
        // //    if (sc == null)
        // //        return;
        // //    sc.id = id;
        // //    showcheckset.Add(sc);
        // //    id++;
        // //}
        // bool AddCheckList(string  targetvalue)
        // {
        //     hascheckdata.Add(targetvalue);
        //     return true;
        // }
        // bool IsHasCheck(string  targetvalue)
        // {
        //     foreach (string d in hascheckdata)
        //     {
        //         if (d == targetvalue)
        //         {

        //             return true;
        //         }

        //     }
        //     return false;
        // }
        // bool IsAllOver(List<string> dsource)
        // {
        //     //foreach (string de in dsource)
        //     //{               
        //     //    if (!hascheckdata.Contains(de))
        //     //        return false;
        //     //}
        //     if (hascheckdata.Count <dsource.Count)
        //         return false;
        //     isallover = true;
        //     return true;
        // }
        // void ShowAllOver(bool isgood, bool isallover)
        // {
        //     if (!isallover)
        //         return;
        //     if (isgood)
        //     {
        //         this.lb_result.Content = "该扳手合格";
        //     }
        //     else
        //     {
        //         this.lb_result.Content = "该扳手不合格";
        //     }        
        //     MessageAlert.Alert("该扳手校验完成");
        // }
        // decimal CheckDataValidate(decimal checkdata)
        // {
        //     try
        //     {
        //         this.lb_status.Content = "正在校验...";
        //         systemcheckset sysset = null;
        //         List<systemcheckset> scsl = SerializeXML<systemcheckset>.Getlist();
        //         if (scsl != null || scsl.Count > 0)
        //             sysset = scsl.FirstOrDefault();
        //         if (sysset == null || sysset.throwvalue == null || sysset.throwvalue <= 0)
        //             return checkdata;
        //         decimal targetvalue = Convert.ToDecimal(GetTargetValue());
        //         decimal throwdatemin = targetvalue * (1-(1-((sysset.throwvalue ?? 0) / 100)));
        //         decimal throwdatemax = targetvalue * (1+(1 - ((sysset.throwvalue ?? 0) / 100)));
        //         if (Math.Abs(checkdata) > throwdatemax || Math.Abs(checkdata) < throwdatemin)
        //             return 0;
        //         return checkdata;
        //     }
        //     catch { return 0; }

        // }

        // ShowCheckresult CheckResult(decimal checkdata)
        // {
        //     ShowCheckresult sc = new ShowCheckresult();
        //     try
        //     {
        //         if (checkdata == 0)
        //             return null;
        //         if (checkdata < 0)
        //         {
        //             sc.isturn = false;
        //             checkdata = Math.Abs(checkdata);
        //         }
        //         else { sc.isturn = true; }
        //         sc.id = 1;
        //         sc.checkdata = checkdata;
        //         sc.setdata = Convert.ToDecimal(GetTargetValue());
        //         decimal min = sc.setdata + (sc.setdata * (Convert.ToDecimal(this.tb_jyw.Text.Trim())) / 100);
        //         decimal max = sc.setdata + (sc.setdata * (Convert.ToDecimal(this.tb_jywm.Text.Trim())) / 100);
        //         sc.normalrang = min.ToString("f2") + "~" + max.ToString("f2");
        //         sc.errorrang = Convert.ToDecimal(((checkdata - sc.setdata) / sc.setdata).ToString("f4"));
        //         sc.normalmin = (Convert.ToInt32(this.tb_jyw.Text.Trim()) / 100.0).ToString();
        //         sc.normalmax = (Convert.ToInt32(this.tb_jywm.Text.Trim()) / 100.0).ToString();
        //         sc.error = (sc.errorrang * 100).ToString("f2") + "%";
        //         if (sc.checkdata <= max && sc.checkdata >= min)
        //         {
        //             sc.result = "√";
        //         }
        //         else
        //         {
        //             sc.result = "×";
        //         }
        //         return sc;
        //     }
        //     catch
        //     {
        //         return null;
        //     }
        // }



        #endregion


        #region 保存

        private void bt_queren_Click(object sender, RoutedEventArgs e)
        {
            if (_juser == null || _juser.user == null || _toolinfo == null || _toolinfo.wrench == null)
                return;
            if (!issave)
            {
                if (SaveData())
                {
                    this.lb_status.Content = "保存数据完成！";
                    _havecheck = false;
                    this.tb_wrenchbarcode.Focus();
                    this.tb_wrenchbarcode.SelectAll();
                }
            }
            else
            { MessageAlert.Alert("已经保存！"); }

        }

        bool SaveData()
        {
            try
            {
                if (showcheckset == null || showcheckset.Count <= 0)
                {
                    MessageAlert.Alert("没有校验数据");
                    return false;
                }

                this.lb_status.Content = "保存数据！";
                if (!_checkover)
                {
                    if (!MessageAlert.Alter("校验未完成是否保存！"))
                    {

                        return false;
                    }
                    lb_result.Content = "扳手不合格";
                }
                HandleData hd = new HandleData(_juser.user, _zuer, _toolinfo.wrench, _checkresult);
                hd.Checkdatashow = showcheckset;
                if (hd.save())
                {
                    issave = true;
                    MessageAlert.Alert("数据提交成功！");
                    this.lb_status.Content = "保存成功！";
                    return true;
                }
                else
                {
                    MessageAlert.Alert("数据提交失败！");
                    this.lb_status.Content = "保存失败！";
                }
                return false;
            }
            catch
            {
                MessageAlert.Alert("数据提交失败！");
                this.lb_status.Content = "保存失败！";
                return false;
            }
        }


        //bool Savedata()
        //{

        //    if (!isallover)
        //    {
        //        if (showcheckset == null || showcheckset.Count <=0)
        //            return false;
        //        if (MessageAlert.Alter("校验未完成是否保存！"))
        //        {

        //            iswrenchgood = false;
        //            this.lb_result.Content = "该工具不合格";
        //            isallover = true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    try
        //    {
        //        this.lb_status.Content = "保存数据！";
        //        if (_toolinfo == null || _toolinfo.wrench == null)
        //        {
        //            MessageAlert.Warning("当前工具不存在！");
        //            return false;
        //        }
        //        if (_juser == null || _juser.user == null)
        //        {

        //            MessageAlert.Warning("没有校验员信息！无法保存");
        //            return false;
        //        }

        //        if (issave)
        //        {

        //            MessageAlert.Warning("该数据已经保存！！");
        //            return false;
        //        }
        //        if (_zuer == null || _zuer.user == null || _zuer.user.guid == null)
        //        {
        //            if (_systemcheckset != null && _systemcheckset.ishavejuser != null && _systemcheckset.ishavejuser == true)
        //            {
        //                MessageAlert.Warning("质检员信息为空 \n无法保存保存！");
        //                return false;
        //            }
        //        }

        //        HandleData hd = new HandleData(_juser.user, _zuer, _toolinfo.wrench, iswrenchgood );
        //        hd.Checkdatashow = showcheckset;
        //        if (hd.save())
        //        {
        //            issave = true;
        //            MessageAlert.Alert("数据提交成功！");
        //            this.lb_status.Content = "保存成功！";
        //        }
        //        return true;
        //    }
        //    catch
        //    {

        //        MessageAlert.Alert("数据提交失败！");
        //        this.lb_status.Content = "保存失败！";
        //        return false;
        //    }
        //}
        #endregion

        #region 打印
        private void bt_print_Click(object sender, RoutedEventArgs e)
        {
            if (_juser == null || _juser.user == null || _toolinfo == null || _toolinfo.wrench == null || showcheckset == null || showcheckset.Count <= 0)
                return;
            this.lb_status.Content = "保存打印报告！";
            try
            {
                if (!issave)
                {
                    if (!SaveData())
                    {
                        return;
                    }

                }
                decimal value = 0;
                if (this._checktype == 3)
                {
                    value = Convert.ToDecimal(this.list_check.Items[1]);
                }
                if (this._checktype == 1)
                {
                    value = Convert.ToDecimal(this.list_check.Items[0]);
                }
                HandleData hd = new HandleData(_juser.user, _zuer, _toolinfo.wrench, _checkresult, value, Convert.ToDecimal(this.tb_jywm.Text.Trim()), Convert.ToDecimal(this.tb_jyw.Text.Trim()));
                hd.Checkdatashow = showcheckset;
                /// hd.filterdata();
                List<ShowCheckresult> lssc = hd.Getprint();
                PrintSingleCheckdata psc = PrintSingleCheckdata.GetPrintSingleCheckdata(_toolinfo.wrench, lssc, Convert.ToInt16(_systemcheckset.count), value, _juser, _zuer, DateTime.Now, _checkresult);
                psc.Topmost = true;
                psc.Show();
                this.tb_wrenchbarcode.Focus();
                this.tb_wrenchbarcode.SelectAll();
            }
            catch { MessageAlert.Alert("信息不完整打印失败"); return; }
        }

        List<ShowCheckresult> GetPrint(List<ShowCheckresult> showcheckresult, List<string> haschck)
        {
            if (haschck == null || haschck.Count < 0)
                return null;
            if (showcheckresult == null || showcheckresult.Count < 0)
                return null;
            List<ShowCheckresult> ls = new List<ShowCheckresult>();

            return ls;
        }
        #endregion



        #region 清空
        private void bt_reset_Click(object sender, RoutedEventArgs e)
        {

            _checkcount = 0;//记录校验次数//
            _checkarr = 0;//记录校验组数
            _checking = true;//校验进行中   有扳手信息 有校验信息
            _havecheck = false;
            _checkover = false; //校验完成 false 未完成
            _onearryover = false;//一次校验结果 成功失败
            _checkresult = false;//校验结果  最中总结过
            _checkindex = 0;//校验数据位置            
            _listid = 1;
            backdata = "";
            Bdirector = true;
            showcheckset.Clear();
            this.editer_check.Visibility = Visibility.Visible;
            this.onecheck.IsEnabled = true;
            this.threecheck.IsEnabled = true;
            this.dg_showcheck.ItemsSource = null;
            issave = false;
            this.tb_result.Text = "";
            this.lb_result.Content = "";
            this.lb_status.Content = "重新校验！";
            this.list_check.ScrollIntoView(this.list_check.Items[_checkindex]);
            getTargetTester(Convert.ToDecimal(this.list_check.Items[_checkindex]));
            // TurnLight(true);
            _light.Type = "00";
            _light.Turn = false;
            LightControl.ThreadControl(_light);
        }
        public void ClearOut()
        {
            _checktype = 3;//3 三点校验-  1-一点校验
            _checkcount = 0;//记录校验次数//
            _checkarr = 0;//记录校验组数
            _checking = false;//校验进行中   有扳手信息 有校验信息
            _havecheck = false;
            _checkover = false; //校验完成 false 未完成
            _onearryover = false;//一次校验结果 成功失败
            _checkresult = false;//校验结果  最中总结过
            _checkindex = 0;//校验数据位置
            _testertype = false;//校验仪true t1  false t2
            _listid = 1;
            _temptbbarcode = "";
            Bdirector = true;
            backdata = "";
            showcheckset.Clear();
            this.editer_check.Visibility = Visibility.Visible;
            this.onecheck.IsEnabled = true;
            this.threecheck.IsEnabled = true;
            this.dg_showcheck.ItemsSource = null;
            issave = false;
            this.tb_result.Text = "";
            this.lb_result.Content = "";
            //showwrench(null);
            //this.cb_checklist.ItemsSource = null;
            //this.resultfail.Visibility = Visibility.Hidden;
        }
        #endregion

        //private void text_Click(object sender, RoutedEventArgs e)
        //{
        //    decimal d = Convert.ToDecimal(this.textname.Text.Trim());
        //    decimal fd = filterdate(d);
        //   // if (IsHasCheck(Convert.ToDecimal(GetTargetValue()))) 
        //        //fd = 0;
        //    if (fd > 0)
        //    {

        //        OnceSuccess(CheckResult(CheckDataValidate(fd)));
        //        AddToList(CheckResult(CheckDataValidate(fd)));
        //    }
        //    CheckBinddata();
        //}
        //void shocheckresult(ReadCheckTester rct)
        //{
        //    this.lb_status.Content = "正在进行校验......！";
        //    decimal d = getcheckdata(rct);
        //    if (d <= 0)
        //        return;
        //    decimal fd = filterdate(d);
        //    if (fd > 0)
        //    {
        //        if (calculate(fd) != null)
        //            add(calculate(fd));
        //    }
        //    CheckBinddata();
        //}












        #region
        //       /// <summary>
        //       /// 添加到list
        //       /// </summary>
        //       /// <param name="sc"></param>
        //       void add(ShowCheckresult sc)
        //       {
        //           if (sc == null)
        //               return;
        //           if (sc.result.Equals("√"))
        //           {
        //               currentsuccess = true;
        //               this.lb_status.Content = "本次成功！";
        //               successcount++;
        //           }
        //           else
        //           {
        //               currentsuccess = false;
        //               this.lb_status.Content = "本次失败！";
        //               if (successcount < confcount) successcount = 0;
        //           }
        //           if (addcount())
        //           {
        //               sc.id = id;
        //               showcheckset.Add(sc);
        //               id++;
        //               if (sc.result.Equals("×"))
        //                   id = 1;
        //           }
        //           else
        //           {
        //               this.lb_status.Content = "本轮校验结束！";
        //           }
        //           if ((successcount < confcount) && isfinish)
        //           {
        //               this.resultfail.Visibility = Visibility.Visible;
        //               this.lb_status.Content = "本轮校验结束！";
        //           }
        //       }



        //       int colorIndex(List<ShowCheckresult> showchecklist)
        //       {
        //           int i = 0, j = 0;
        //           foreach (ShowCheckresult s in showchecklist)
        //           {
        //               i++;
        //               if (s.result.Equals("×"))
        //                   j = i;
        //           }
        //           return j;
        //       }
        //       /// <summary>
        //       /// 计数是否完成
        //       /// </summary>
        //       /// <returns></returns>
        //       private bool addcount()
        //       {
        //           if (successcount > (confcount))
        //           {
        //               arry = confarry;
        //               isfinish = true;
        //               return false;
        //           }
        //           if (arry >= confarry)
        //           {
        //               isfinish = true;
        //               return false;
        //           }
        //           if (currentsuccess)
        //           {
        //               if (count >= confcount)
        //               {
        //                   count = 0; arry++;
        //               }
        //               count++;
        //           }
        //           else
        //           {
        //               arry++; count = 0;
        //               if (arry >= confarry)
        //               {
        //                   isfinish = true;
        //               }
        //           }

        //           return true;
        //       }

        //       /// <summary>
        //       /// 计算校验值是否正常
        //       /// </summary>
        //       /// <param name="data">校验值</param>
        //       /// <returns></returns>
        //       ShowCheckresult calculate(decimal data)
        //       {
        //           ShowCheckresult sc = new ShowCheckresult();
        //           try
        //           {
        //               if (data <= 0)
        //                   return sc;
        //               sc.id = 1;
        //               sc.checkdata = data;
        //               sc.setdata = Convert.ToDecimal(this.tb_setvalue.Text.Trim());
        //               decimal min = sc.setdata + (sc.setdata * (Convert.ToDecimal(this.tb_jyw.Text.Trim())) / 100);
        //               decimal max = sc.setdata + (sc.setdata * (Convert.ToDecimal(this.tb_jywm.Text.Trim())) / 100);
        //               sc.normalrang = min.ToString("f3") + "~" + max.ToString("f3");
        //               sc.errorrang = Convert.ToDecimal(((data - sc.setdata) / sc.setdata).ToString("f4"));
        //               sc.normalmin = (Convert.ToInt32(this.tb_jyw.Text.Trim()) / 100.0).ToString();
        //               sc.normalmax = (Convert.ToInt32(this.tb_jywm.Text.Trim()) / 100.0).ToString();
        //               sc.error = (sc.errorrang * 100).ToString("f2") + "%";
        //               if (sc.checkdata <= max && sc.checkdata >= min)
        //               {
        //                   sc.result = "√";
        //               }
        //               else
        //               {
        //                   sc.result = "×";
        //               }

        //               return sc;
        //           }
        //           catch
        //           {
        //               return null;
        //           }
        //       }
        //       decimal getcheckdata(ReadCheckTester rct)
        //       {
        //           //string[] tempvalue = rct.ReturnReadString().Split(' ');
        //           rct.isread = true;
        //           decimal temp = 0;
        //           //try
        //           //{
        //           //    temp = Convert.ToDecimal(tempvalue[0]);
        //           //}
        //           //catch
        //           //{
        //           //    temp = Convert.ToDecimal(tempvalue[1]);
        //           //}
        //           return temp;
        //       }
        //       decimal filterdate(decimal chechdata)
        //       {
        //           try
        //           {
        //               if (_systemcheckset.throwvalue == null || _systemcheckset.throwvalue <= 0)
        //               {
        //                   return chechdata;
        //               }

        //               if (Convert.ToDecimal(this.tb_setvalue.Text.Trim()) > 0)
        //               {
        //                   decimal m = Convert.ToDecimal(Math.Abs((Convert.ToDecimal(this.tb_setvalue.Text.Trim()) - chechdata) / Convert.ToDecimal(this.tb_setvalue.Text.Trim())));
        //                   if (_systemcheckset.throwvalue > (Math.Abs((1 - m) * 100)))
        //                   {
        //                       return chechdata = 0;
        //                   }
        //               }
        //               return chechdata;
        //           }
        //           catch { return 0; }
        //       }
























        #endregion
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            EncoderPlc = new EncoderPlc(_serialEncoder);

            LightControl = new Check.LightControl(EncoderPlc);

            _light = new Light() { time = 0, Type = "00", Turn = false };

            LightControl.ThreadControl(_light);

            //  TurnLight(true);

            if (OperationConfig.GetValue("encodesetshow") == "true")
                bt_encodeset.Visibility = Visibility.Visible;
            else
                bt_encodeset.Visibility = Visibility.Hidden;

            try
            {
                _light.time = Convert.ToInt32(OperationConfig.GetValue("lighttime"));
            }
            catch
            {
                _light.time = 500;
            }
            //aTimer = new System.Timers.Timer(800);
            //aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            //aTimer.Interval = 800;
            //aTimer.Enabled = true;
            //this.tb_wrenchbarcode.Focus();
            //temp = 0;
        }
        //void TurnLight(bool status = true)
        //{
        //    EncoderPlc.SmallBlueLight(status);
        //    EncoderPlc.SmallGreenLight(status);
        //    EncoderPlc.SmallRedLight(status);

        //    EncoderPlc.BigBlueLight(status);
        //    EncoderPlc.BigGreenLight(status);
        //    EncoderPlc.BigRedLight(status);

        //}
        //void SmallRedLightTurn(bool on=true)
        //{
        //    TurnLight(false);
        //    EncoderPlc.SmallBlueLight(!on);
        //    EncoderPlc.SmallGreenLight(!on);
        //    EncoderPlc.SmallRedLight(on);
        //}

        //void SmallGreenLightTurn(bool on=true)
        //{
        //    TurnLight(false);
        //    EncoderPlc.SmallBlueLight(!on);
        //    EncoderPlc.SmallGreenLight(on);
        //    EncoderPlc.SmallRedLight(!on);
        //}
        //void BigRedLightTurn(bool on=true)
        //{
        //    TurnLight(false);
        //    EncoderPlc.BigBlueLight(!on);
        //    EncoderPlc.BigGreenLight(!on);
        //    EncoderPlc.BigRedLight(on);
        //}

        //void BigGreenLightTurn(bool on = true)
        //{
        //    TurnLight(false);
        //    EncoderPlc.BigBlueLight(!on);
        //    EncoderPlc.BigGreenLight(on);
        //    EncoderPlc.BigRedLight(!on);
        //}


        private void editer_check_Click(object sender, RoutedEventArgs e)
        {
            WinCheckData wcd = new WinCheckData();
            wcd.ShowDialog();
            if (!string.IsNullOrWhiteSpace(wcd.Setvalue))
            {
                if (_toolinfo == null || _toolinfo.wrench == null)
                    this.list_check.Items.Add(Convert.ToDouble(wcd.Setvalue).ToString("f1"));
                this.list_check.Items[_checkindex] = Convert.ToDouble(wcd.Setvalue).ToString("f1");
                this.list_check.ScrollIntoView(this.list_check.Items[_checkindex]);
                getTargetTester(Convert.ToDecimal(wcd.Setvalue));
                geterrorrang(_toolinfo, wcd.Setvalue);
            }
        }

        private void editer_check_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                editer_check_Click(sender, e);
        }

        private void bt_encodeset_Click(object sender, RoutedEventArgs e)
        {
            WinSetDirectionData wsd = new WinSetDirectionData(EncoderPlc);
            wsd.Topmost = true;
            wsd.Show();
        }



    }
    public class Light
    {
        /// <summary>
        /// 00-- 所有灯关  01-- 所有灯开   02--小量程红开 03--小量程绿关 04--大量程红开 05--大量程绿关
        /// </summary>
        public string Type { get; set; }
        public bool Turn { get; set; }
        public int time { get; set; }
    }
    public class LightControl
    {
        EncoderPlc EncoderPlc = null;
        Thread receiveThread;
        public LightControl(EncoderPlc encoderPlc)
        {
            EncoderPlc = encoderPlc;
        }

        public void ThreadControl(object param)
        {
            if (EncoderPlc.Open())
            {
                receiveThread = new Thread(new ParameterizedThreadStart(SetManyCoint));
                receiveThread.Start(param);
            }
        }
        void SetManyCoint(object param)
        {
            Light light = (Light)param;

            switch (light.Type)
            {
                ///所有灯
                case "00":
                    TurnLight(light.Turn);
                    receiveThread.Abort();
                    break;

                case "02":
                    //TurnSmallLight(false);
                    SmallRedLightTurn();
                    Thread.Sleep(light.time);
                    TurnLight(false);
                    receiveThread.Abort();
                    break;

                case "03":
                    // TurnSmallLight(false);
                    SmallGreenLightTurn();
                    Thread.Sleep(light.time);
                    TurnLight(false);
                    receiveThread.Abort();
                    break;
                case "04":
                    // TurnBigLight(false);
                    BigRedLightTurn();
                    Thread.Sleep(light.time);
                    TurnLight(false);
                    receiveThread.Abort();
                    break;
                case "05":
                    // TurnBigLight(false);
                    BigGreenLightTurn();
                    Thread.Sleep(light.time);
                    TurnLight(false);
                    receiveThread.Abort();
                    break;
                default:
                    TurnLight(false);
                    receiveThread.Abort();
                    break;
            }

        }
        void TurnLight(bool status = true)
        {
            TurnBigLight(status);
            TurnSmallLight(status);
            //EncoderPlc.SamllLight(status);
            //EncoderPlc.BigLight(status);

        }
        void TurnSmallLight(bool status = true)
        {
            if (status)
                EncoderPlc.SamllLightOn();
            else
                EncoderPlc.SamllLightOff();

            //EncoderPlc.SmallBlueLight(status);
            //EncoderPlc.SmallGreenLight(status);
            //EncoderPlc.SmallRedLight(status);
        }
        void TurnBigLight(bool status = true)
        {
            if (status)
                EncoderPlc.BigLightOn();
            else
                EncoderPlc.BigLightOff();
            // EncoderPlc.BigLight(status);
            //EncoderPlc.BigBlueLight(status);
            //EncoderPlc.BigGreenLight(status);
            //EncoderPlc.BigRedLight(status);
        }

        void SmallRedLightTurn()
        {
            EncoderPlc.SmallRedLight();
            //EncoderPlc.SamllLight(!on);
            //EncoderPlc.SmallRedLight(on);
            //EncoderPlc.SmallBlueLight(!on);
            //EncoderPlc.SmallGreenLight(!on);

        }

        void SmallGreenLightTurn()
        {
            EncoderPlc.SmallGreenLight();
            //EncoderPlc.SamllLight(!on);
            //EncoderPlc.SmallGreenLight(on);
            //EncoderPlc.SmallBlueLight(!on);
            //EncoderPlc.SmallGreenLight(on);
            //EncoderPlc.SmallRedLight(!on);
        }
        void BigRedLightTurn()
        {
            EncoderPlc.BigRedLight();
            //EncoderPlc.BigLight(!on);
            //EncoderPlc.BigRedLight(on);
            //EncoderPlc.BigBlueLight(!on);
            //EncoderPlc.BigGreenLight(!on);

        }

        void BigGreenLightTurn()
        {
            EncoderPlc.BigGreenLight();
            //EncoderPlc.BigLight(!on);
            //EncoderPlc.BigGreenLight(on);
            // EncoderPlc.BigBlueLight(!on);

            //   EncoderPlc.BigRedLight(!on);
        }
    }
}
