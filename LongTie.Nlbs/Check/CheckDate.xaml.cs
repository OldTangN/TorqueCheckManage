using LT.BLL;
using LT.BLL.Check;
using LT.Comm;
using LT.DAL;
using LT.DAL.MySql;
using LT.Model;
using LT.Model.BllModel;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
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

namespace LongTie.Nlbs.Check
{
    /// <summary>
    /// Interaction logic for CheckDate.xaml
    /// </summary>
    public partial class CheckDate : Window
    {
        IWrench Wrench = DataAccess.CreateWrench();
        IWrenchSpecies WrenchSpecies = DataAccess.CreateWrenchSpecies();
        SerialPort rC = new SerialPort();    
        ReadCheckTester rct1 = null;
        ReadCheckTester rct2 = null;
        ReadUserCard ruc = null;
        FilterData filterdata = new FilterData();
        List<TorqueTestModel> ttml = new List<TorqueTestModel>();
        List<errorrangset> erl = new List<errorrangset>();
        systemcheckset _systemcheckset = new systemcheckset();
        Toolinfo _toolinfo = new Toolinfo();
        System.Timers.Timer aTimer = null;
        bool isfinish = false;

        int id = 1;
        int arry = 1;//机会
        int successcount = 0;//次数
        bool procheckdata = true;//上次是否校验成功
        int finisharry = 0;//判断校验值是否校验完成


        List<ShowCheckresult> showcheckset = new List<ShowCheckresult>();

        bool isStartCheck = false;
        bool isFinishCheck = false;
        private delegate void TimerDispatcherDelegate();
        public CheckDate(ReadUserCard r, ReadCheckTester r1, ReadCheckTester r2)
        {
            InitializeComponent();
            rct1 = r1;
            rct2 = r2;
            ruc = r;
            aTimer = new System.Timers.Timer(10);
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 1000;
            aTimer.Enabled = true;
            getsystemset();
        }
        void OnTimedEvent(object serder, EventArgs e)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal,
                new TimerDispatcherDelegate(UpdateUI));
        }

        void UpdateUI()
        {
           // UpdateCardInfo();
            UpdateCheckDataUI();
        }

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
        void UpdateCardInfo()
        {
            try
            {
              //  showjuser(_juser);
               // showzuser(_zuer);

                if (!ruc.isread)
                {
                    filterdata.CardId = ruc.returnreadstr();
                    filterdata.resetCard();
                    ruc.isread = true;
                }
                string cardid = filterdata.getcardid();
                if (cardid == "")
                    return;
               // Getuserinfo(cardid);
                filterdata.resetid("");

            }
            catch { }

        }

        void UpdateCheckDataUI()
        {
            if (_toolinfo == null || _toolinfo.wrench == null)
                return;


            if (!Validate())
                return;
            GeQueue();
            GetTester(GetTargetValue());
            geterrorrang(_toolinfo, erl, Convert .ToDecimal ( GetTargetValue()));


            //try
            //{
            //    tempsetdata = Convert.ToDecimal(this.tb_setvalue.Text.Trim());
            //}
            //catch { this.tb_setvalue.Clear(); MessageAlert.Warning("请填写正确的设定值！"); return; }
            //if (ttml == null || ttml.Count <= 0)
            //    return;
            //foreach (TorqueTestModel t in ttml)
            //{

            //    if (tempsetdata < t.maxvalue && tempsetdata >= t.minvalue)
            //    {
            //        if (t.testername == "校验仪1")
            //        {
            //            tb_testername.Text = "校验仪1";
            //            UpdateCheckData(rct1);
            //            break;
            //        }
            //        else
            //        {
            //            tb_testername.Text = "校验仪2";
            //            UpdateCheckData(rct2);
            //            break;
            //        }
            //    }
            //}

        }

        private void bt_wrenchbarcode_Click(object sender, RoutedEventArgs e)
        {
            if (this.tb_wrenchbarcode.Text.Trim() == "")
                return;
            //this.tb_wrenchbarcode.TabIndex = -1;
            this.tb_wrenchbarcode.SelectAll();
            ClearOut();
            getsystemset();
            if (string.IsNullOrEmpty(this.tb_wrenchbarcode.Text.Trim())) { return; }

            _toolinfo.wrench = Wrench.selectByBarcode(this.tb_wrenchbarcode.Text.Trim());
            if (_toolinfo.wrench != null)
            {
                _toolinfo.speciesName = WrenchSpecies.selectByGuid(_toolinfo.wrench.species).speciesName;

            }
            if (_toolinfo != null && _toolinfo.wrench != null && _toolinfo.wrench.lastrepair != null && _toolinfo.wrench.cycletime != null)
            {

                //if (_zuer == null || _zuer.user == null || _zuer.user.guid == null)
                //{
                //    if (_systemcheckset != null && _systemcheckset.ishavejuser != null && _systemcheckset.ishavejuser == true)
                //    {
                //        MessageAlert.Warning("质检员信息为空 \n不能校验！");
                //        return;
                //    }
                //}
                DateTime dt1 = DateTime.Now;
                DateTime dt2 = Convert.ToDateTime(_toolinfo.wrench.lastrepair);
                TimeSpan ts1 = dt1.Subtract(dt2);
                if (Convert.ToInt32(ts1.Days) >= Convert.ToInt32(_toolinfo.wrench.cycletime))
                {
                    if (!_toolinfo.wrench.isallowcheck)
                    {
                        MessageAlert.Warning("已经超出保护期！需要提交维护！\n该扳手不能校验！");
                        _toolinfo = null;
                        return;
                    }

                }
            }
            showwrench(_toolinfo);
        }

        void showwrench(Toolinfo t)
        {
            if (t.wrench == null)
            {
               // tbempty();
                return;
            }
          //  this.tb_setvalue.Text = t.wrench.targetvalue.ToString("f1");
            //this.tb_bjb.Text = t.wrench.wrenchCode.ToString();
            this.tb_lc.Text = t.wrench.rangeMin.ToString("f1") + "~" + t.wrench.rangeMax.ToString("f1") + "N.m";
            // this.tb_xh.Text = t.wrench.factory.ToString();
            if (erl == null || erl.Count <= 0)
            {
                MessageAlert.Alert("没有任何扳手误差设置！\n     无法校验！");
                return;
            }
            List<errorrangset> tm = erl.Where(p => p.speciesID == t.wrench.species).ToList();
            foreach (errorrangset e in tm)
            {
                if (e.rangmax > t.wrench.targetvalue && e.rangmin <= t.wrench.targetvalue)
                {
                    this.tb_jywm.Text = e.errorrangMax.ToString();
                    this.tb_jyw.Text = e.errorrangMin.ToString();
                }
            }
            this.lb_status.Content = "工具信息获取成功！";
        }

        public void ClearOut()
        {
        
            arry = 1;
            id = 1;
         
            successcount = 0;
            isfinish = false;
            showcheckset.Clear();
         
            this.dg_showcheck.ItemsSource = null;
          
            this.tb_result.Text = "";
            this.resultsuccess.Visibility = Visibility.Hidden;
            this.resultfail.Visibility = Visibility.Hidden;
        }

      void geterrorrang(Toolinfo t, List<errorrangset> erl,decimal targetvalue)
        {
            if (t.wrench == null)
            {
                this.tb_jyw.Text = "";
                this.tb_jywm.Text = "";
                return;
            }
            if (erl == null)
                return;
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

        void GetTester(double  targetvalue)
        {
            if (ttml == null || ttml.Count <= 0)
                return;
            if (targetvalue == 0)
                return;
            decimal d = Convert.ToDecimal(targetvalue);
            foreach (TorqueTestModel t in ttml)
            {

                if (d < t.maxvalue && d >= t.minvalue)
                {
                    if (t.testername == "校验仪1")
                    {
                        tb_testername.Text = "校验仪1";
                        UpdateCheckData(rct1);
                        break;
                    }
                    else
                    {
                        tb_testername.Text = "校验仪2";
                        UpdateCheckData(rct2);
                        break;
                    }
                }
            }
        }
        bool Validate()
        {
           
            if(!string.IsNullOrEmpty (this.tb_setvalue1 .Text .Trim ()))
            {
            try        
            {
                Convert.ToDouble(this.tb_setvalue1 .Text .Trim ());
            }
                catch
            {
                MessageAlert.Warning("请填写正确的预校验值1！"); this.tb_setvalue1.Clear(); this.tb_setvalue1.Focus();
                    return false; 
            }
            }

            if (!string.IsNullOrEmpty(this.tb_setvalue.Text.Trim()))
            {
                try
                {
                    Convert.ToDouble(this.tb_setvalue.Text.Trim());
                }
                catch
                {
                    MessageAlert.Warning("请填写正确的目标校验值！"); this.tb_setvalue.Clear(); this.tb_setvalue.Focus(); 
                    return false;
                }
            }


            if (!string.IsNullOrEmpty(this.tb_setvalue2.Text.Trim()))
            {
                try
                {
                    Convert.ToDouble(this.tb_setvalue2.Text.Trim());
                }
                catch
                {
                    MessageAlert.Warning("请填写正确的目标校验值2！"); this.tb_setvalue2.Clear(); this.tb_setvalue2.Focus();
                    return false;
                }
            }
            return true;
        }

        double GetTargetValue()
        {
            double targetvalue = 0;
            if (!string.IsNullOrEmpty(this.tb_setvalue1.Text.Trim()) && this.tb_setvalue1.Background.Equals(Brushes.Green))
            {
                targetvalue = Convert.ToDouble(this.tb_setvalue1.Text .Trim ());
                finisharry = 0;
            }
            if (!string.IsNullOrEmpty(this.tb_setvalue.Text.Trim()) && this.tb_setvalue.Background.Equals(Brushes.Green))
            {
                targetvalue = Convert.ToDouble(this.tb_setvalue.Text.Trim());
                finisharry = 1;
            }
            if (!string.IsNullOrEmpty(this.tb_setvalue2.Text.Trim()) && this.tb_setvalue2.Background.Equals(Brushes.Green))
            {
                targetvalue = Convert.ToDouble(this.tb_setvalue2.Text.Trim());
                finisharry = 2;
            }
            return targetvalue;
        }

        /// <summary>
        /// 设置开始的校验顺序
        /// </summary>
        void GeQueue()
        {
            if (!isStartCheck)
            {
                if (!string.IsNullOrEmpty(this.tb_setvalue1.Text.Trim()))
                {
                    this.tb_setvalue1.Background = Brushes.Green;
                    this.tb_setvalue.Background = Brushes.White;
                    this.tb_setvalue2.Background = Brushes.White;
                    return;
                }
                else
                {
                    if (!string.IsNullOrEmpty(this.tb_setvalue.Text.Trim()))
                    {
                        this.tb_setvalue1.Background = Brushes.White;
                        this.tb_setvalue.Background = Brushes.Green;
                        this.tb_setvalue2.Background = Brushes.White;
                        return;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(this.tb_setvalue2.Text.Trim()))
                        {
                            this.tb_setvalue1.Background = Brushes.White;
                            this.tb_setvalue.Background = Brushes.White;
                            this.tb_setvalue2.Background = Brushes.Green;
                            return;
                        }
                        else
                        {
                            this.tb_setvalue1.Background = Brushes.White;
                            this.tb_setvalue.Background = Brushes.White;
                            this.tb_setvalue2.Background = Brushes.White;
                            return;
                        }
                    }

                }
            }
        }


        void UpdateCheckData(ReadCheckTester rct)
        {
            try
            {
              //  if (rct.ReturnReadData()>0)
                {
                    if (this.tb_setvalue.Text.Trim() == "" && this.tb_setvalue1.Text.Trim() == "" && this.tb_setvalue2.Text.Trim() == "")
                    {
                        rct.isread = true;
                        MessageAlert.Warning("请设置目标值！");
                        return;
                    }
                    if (this.tb_jyw.Text.Trim() == "" || this.tb_jywm.Text.Trim() == "")
                    {
                        rct.isread = true;
                        MessageAlert.Warning("该扳手没有相应的校验策略！请联系管理员设置策略！");
                        return;
                    }

                    if (Convert.ToDecimal(GetTargetValue()) < _toolinfo.wrench.rangeMin || Convert.ToDecimal(GetTargetValue()) > _toolinfo.wrench.rangeMax)
                    {
                        rct.isread = true;
                        
                        MessageAlert.Warning("校验设定值不再扳手量程范围内");
                        return;
                    }
                    if (isfinish)
                    {
                        rct.isread = true;
                        // MessageAlert.Warning("该扳手校验完成！");
                        return;
                    }
                    isStartCheck = true;
                    if (isFinishCheck == true)
                        return;
                   // ShowCheckData(rct);
                }
            }
            catch { }
        }

      
        //void ShowCheckData(ReadCheckTester rct)
        //{
        //    this.lb_status.Content = "正在进行校验......！";
        //    decimal d =Convert .ToDecimal ( rct.ReturnReadString()) ;


        //    if (d <= 0)
        //        return;
        //    decimal fd = CheckDataValidate(d);
        //    if (fd > 0)
        //        CountPlus(fd);
        //}

        void ShowCheckData()
        {
            this.lb_status.Content = "正在进行校验......！";
            decimal d = Convert.ToDecimal(textname.Text .Trim ());


            if (d <= 0)
                return;
            decimal fd = CheckDataValidate(d);
            if (fd > 0)
                CountPlus(fd);
        }
        decimal CheckDataValidate(decimal checkdata)
        {
            try
            {
                systemcheckset sysset = null;
                List<systemcheckset> scsl = SerializeXML<systemcheckset>.Getlist();
                if (scsl != null || scsl.Count > 0)
                    sysset = scsl.FirstOrDefault();
                if (sysset == null || sysset.throwvalue == null || sysset.throwvalue <= 0)
                    return checkdata;
                decimal targetvalue = Convert.ToDecimal(GetTargetValue());
                decimal throwdatemin = targetvalue * (((sysset.throwvalue ?? 0) / 100));
                decimal throwdatemax = targetvalue * (1 + ((sysset.throwvalue ?? 0) / 100));
                if (targetvalue > throwdatemax || targetvalue < throwdatemin)
                    return 0;
                return checkdata;
            }
            catch { return 0; }

        }

        ShowCheckresult CheckResult(decimal checkdata)
        {
            ShowCheckresult sc = new ShowCheckresult();
            try
            {
                if (checkdata <= 0)
                    return sc;
                sc.id = 1;
                sc.checkdata = checkdata;
                sc.setdata = Convert.ToDecimal(GetTargetValue());
                decimal min = sc.setdata + (sc.setdata * (Convert.ToDecimal(this.tb_jyw.Text.Trim())) / 100);
                decimal max = sc.setdata + (sc.setdata * (Convert.ToDecimal(this.tb_jywm.Text.Trim())) / 100);
                sc.normalrang = min.ToString("f2") + "~" + max.ToString("f2");
                sc.errorrang = Convert.ToDecimal(((checkdata - sc.setdata) / sc.setdata).ToString("f4"));
                sc.normalmin = (Convert.ToInt32(this.tb_jyw.Text.Trim()) / 100.0).ToString();
                sc.normalmax = (Convert.ToInt32(this.tb_jywm.Text.Trim()) / 100.0).ToString();
                sc.error = (sc.errorrang * 100).ToString("f2") + "%";
                if (sc.checkdata <= max && sc.checkdata >= min)
                {
                    sc.result = "√";
                }
                else
                {
                    sc.result = "×";
                }
                return sc;
            }
            catch
            {
                return sc;
            }
        }


     

        //void AddToList(ShowCheckresult sc)
        //{
        //    if (sc == null) return;
        //    if (CountPlus(sc))
        //    {
              
        //        sc.id = id;
        //        showcheckset.Add(sc);
        //        CheckDataBind();
        //        id++;

        //    }
        //}

        bool CountPlus(decimal checkdata)
        {
            systemcheckset sysset = null;
            List<systemcheckset> scsl = SerializeXML<systemcheckset>.Getlist();
            if (scsl != null || scsl.Count > 0)
                sysset = scsl.FirstOrDefault();  
            if (procheckdata)
            {
                successcount++;
            }

            if (successcount > sysset.count)
            {
                arry = sysset.arry ?? 1;
                successcount = 0;
                arry = 4;
            }

            if (arry > sysset.arry)
            {
                finisharry++;
                NextCheck(finisharry);
                successcount = 0;
                arry = 1;
                procheckdata = true;
            }

            if (finisharry > 3)
            {
                isFinishCheck = true;
                return false;
            }

            ShowCheckresult sc = CheckResult(checkdata);
            sc.id = id;
            showcheckset.Add(sc);
            CheckDataBind();
            id++;

            if (sc.result.Equals("√"))
            {
                procheckdata = true;
                
                this.lb_status.Content = "本次不合格！";
            }
            else
            {
                procheckdata = false ;
                successcount = 0;
                arry++;
                this.lb_status.Content = "本次不合格！";
            }
            return true;
        }

        void SetvalueGround()
        {
            this.tb_setvalue1.Background = Brushes.Gray;
            if (!string.IsNullOrEmpty(this.tb_setvalue.Text.Trim()))
            {
                this.tb_setvalue.Background = Brushes.Green;
            }
            else
            {
                Setvalue2Ground();
            }
        }

        void Setvalue2Ground()
        {
            this.tb_setvalue1.Background = Brushes.Gray;
            this.tb_setvalue.Background = Brushes.Gray;
            if (!string.IsNullOrEmpty(this.tb_setvalue2.Text.Trim()))
            {
              
                  this.tb_setvalue2.Background = Brushes.Green;
            }
           else 
            {
                Setvalue3Ground(); return;
            }
        
        }
        void Setvalue3Ground()
        {
            this.tb_setvalue1.Background = Brushes.Gray;
            this.tb_setvalue.Background = Brushes.Gray;
            this.tb_setvalue2.Background = Brushes.Gray;
            isFinishCheck = true;
        }

        void NextCheck(int i)
        {
            switch (i)
            {
                case 1:
                    SetvalueGround();
                    break;
                case 2:
                    Setvalue2Ground();
                    break;
                case 3:
                    Setvalue3Ground();
                    break;
                default:
                    break;

            }
        }

        void CheckDataBind()
        {
            this.dg_showcheck.ItemsSource = null;
            this.dg_showcheck.ItemsSource = showcheckset;
        }

        private void text_Click(object sender, RoutedEventArgs e)
        {
            ShowCheckData();
        }

    }
}

