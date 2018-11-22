using QDDL.Nlbs.Print; 
using QDDL.BLL;
using QDDL.BLL.Check;
using QDDL.Comm;
using QDDL.DAL;
using QDDL.DAL.MySql;
using QDDL.Model;
using QDDL.Model.BllModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QDDL.Nlbs.Check
{
    /// <summary>
    /// SearchChechResult.xaml 的交互逻辑
    /// </summary>
    public partial class SearchChechResult 
    {
        ICheckTarget CheckTarget = DataAccess.CreateCheckTarget();
        ICheckTargetRecord CheckTargetRecord = DataAccess.CreateCheckTargetRecord();
        IWrench Wrench = DataAccess.CreateWrench();
        IUser User = DataAccess .CreateUser ();
        Main _main = null;
        List<WrenchCheckOut> wrenchchecklist = new List<WrenchCheckOut>();
       // SetPage<WrenchCheckOut> setpage = null;
        CheckResultSetPage CheckResultSetPage = new CheckResultSetPage();
        public SearchChechResult(Main m)
        {
            InitializeComponent();
            _main = m;
           // setpage = new SetPage<WrenchCheckOut>(this.dt_showdate);
           // this.cb_pages.SelectedIndex = 0;
        
            //setpage.TList = wrenchchecklist;
            //setpage.PageSize = Convert.ToInt16(cb_pages.SelectionBoxItem );
            //setpage.PageCount();

            //Dictionary<string, string> dict = new Dictionary<string, string>();
            //string  dt = DateTime.Now.ToShortDateString ();
            //if (this.dp_starttime.SelectedDate != null)           
            //    dict.Add("starttime", Convert.ToDateTime ( dt).ToString ());              
            //if (this.dp_endtime.SelectedDate != null)
            //    dict.Add("endtime", Convert .ToDateTime (dt).AddDays (1).AddSeconds(-1).ToString ());
            //List<torquechecktarget> q = CheckTarget.SelectByContion(dict);       
            //showdata(q);

            this.cb_pages.SelectedIndex = 0;
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string dt = DateTime.Now.ToShortDateString();
            if (this.dp_starttime.SelectedDate != null)
                dict.Add("starttime", Convert.ToDateTime(dt).ToString());
            if (this.dp_endtime.SelectedDate != null)
                dict.Add("endtime", Convert.ToDateTime(dt).AddDays(1).AddSeconds(-1).ToString());
            CheckResultSetPage.Dictionary = dict;
            CheckResultSetPage.PageSize = 15;
            List<torquechecktarget> q = CheckResultSetPage.getTorquechecktarget(0);
            CheckResultSetPage.getTotalPage();
            showdata(q);
        }

        private void bt_search_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(this.tb_wrenchbarcode.Text.Trim()))
            {
                wrench wl = Wrench.selectByBarcode(this.tb_wrenchbarcode.Text.Trim());
                if (wl != null)
                    dict.Add("WrenchID_id", wl.guid);
            }
            if (this.dp_starttime.SelectedDate != null)
                dict.Add("starttime", this.dp_starttime.SelectedDate.ToString());
            if (this.dp_endtime.SelectedDate != null)
            {
                if (this.dp_endtime.SelectedDate.ToString() == this.dp_starttime.SelectedDate.ToString())
                    dict.Add("endtime", this.dp_starttime.SelectedDate.Value.AddDays(1).AddSeconds(-1).ToString());
                else
                {
                    dict.Add("endtime", this.dp_endtime.SelectedDate.ToString());
                }
            }
            CheckResultSetPage.PageSize = Convert.ToInt16(cb_pages.SelectionBoxItem);
            CheckResultSetPage.Dictionary = dict;
            CheckResultSetPage.getTotalPage();
            List<torquechecktarget> q = CheckResultSetPage.getTorquechecktarget(0);

            if (q == null || q.Count <= 0)
            {
                MessageAlert.Alert("查询信息不存在！");
                return;
            }
            showdata(q);
            this.Cursor = Cursors.Arrow;
        }

        void showdata(List<torquechecktarget> q)
        {
       
              
            wrenchchecklist = getshowdata(q);
            binding(wrenchchecklist);

            this.pagecount.Text = CheckResultSetPage.TotalPage.ToString();
            this.tb_mbpage.Text = (CheckResultSetPage.PageNo + 1).ToString();
        }

        private void bt_first_Click(object sender, RoutedEventArgs e)
        {
            //setpage.firstpage();
            //this.tb_mbpage.Text = setpage.pageno.ToString();
            this.Cursor = Cursors.Wait;
            showdata(CheckResultSetPage.targetPage(1));
            this.tb_mbpage.Text = "1";
            this.Cursor = Cursors.Arrow;
        }

        private void bt_pro_Click(object sender, RoutedEventArgs e)
        {
         
            this.Cursor = Cursors.Wait;
            showdata(CheckResultSetPage.proPage());
            this.Cursor = Cursors.Arrow;
        }

        private void bt_next_Click(object sender, RoutedEventArgs e)
        {
         
            this.Cursor = Cursors.Wait;
            showdata(CheckResultSetPage.nextPage());
            this.Cursor = Cursors.Arrow;
        }

        private void bt_end_Click(object sender, RoutedEventArgs e)
        {
       
            this.Cursor = Cursors.Wait;
            if (CheckResultSetPage.TotalPage <= 0)
            {
                this.Cursor = Cursors.Arrow;
                return;
            }
            CheckResultSetPage.PageNo = CheckResultSetPage.TotalPage-1;
            showdata(CheckResultSetPage.getTorquechecktarget(CheckResultSetPage.TotalPage-1));
            this.Cursor = Cursors.Arrow;
        }

        private void bt_go_Click(object sender, RoutedEventArgs e)
        {
         
            this.Cursor = Cursors.Wait;
            if (string.IsNullOrEmpty(this.tb_mbpage.Text))
                return;
            int count = 1;
            try
            {
                count = Convert.ToInt16(this.tb_mbpage.Text.Trim());
            }
            catch
            {
                MessageAlert.Alert("请输入正确的数值！");
                return;
            }
            if (count >0 && count <=CheckResultSetPage.TotalPage)
                showdata(CheckResultSetPage.targetPage(count));
            CheckResultSetPage.PageNo = count-1;
            //this.tb_mbpage.Text = "1";
            this.Cursor = Cursors.Arrow;
        }
        private void cb_pages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(this.tb_wrenchbarcode.Text.Trim()))
            {
                wrench wl = Wrench.selectByBarcode(this.tb_wrenchbarcode.Text.Trim());
                if (wl != null)
                    dict.Add("WrenchID_id", wl.guid);
            }
            if (this.dp_starttime.SelectedDate != null)
                dict.Add("starttime", this.dp_starttime.SelectedDate.ToString());
            if (this.dp_endtime.SelectedDate != null)
            {
                if (this.dp_endtime.SelectedDate.ToString() == this.dp_starttime.SelectedDate.ToString())
                    dict.Add("endtime", this.dp_starttime.SelectedDate.Value.AddDays(1).AddSeconds(-1).ToString());
                else
                {
                    dict.Add("endtime", this.dp_endtime.SelectedDate.ToString());
                }
            }
            CheckResultSetPage.PageSize = Convert.ToInt32((this.cb_pages.SelectedItem as ComboBoxItem).Content.ToString());
            CheckResultSetPage.Dictionary = dict;
            CheckResultSetPage.getTotalPage();
            List<torquechecktarget> q = CheckResultSetPage.getTorquechecktarget(0);
            showdata(q);
           
        }
        List<WrenchCheckOut> getshowdata(List<torquechecktarget> torquetargetlist)
        {
            GetUser gu = new GetUser();
            List<WrenchCheckOut> tempwcolist = new List<WrenchCheckOut>();
            if (torquetargetlist == null)
                return tempwcolist;
            foreach (torquechecktarget t in torquetargetlist) 
            {
                userinfo juser = new userinfo();
                userinfo zuser = new userinfo();
                juser = gu.getuserinfo(gu.getusers(t.operatorID));
                if (juser == null || juser.user == null)
                {
                    users jusers = new users();
                    jusers.username = t.operatorID;
                    juser.user = jusers;
                    department d = new department();
                    d.departmentName = t.operatorID;
                    juser.department = d;
                }
                if (!string.IsNullOrEmpty(t.qaID)) 
                {
                    zuser = gu.getuserinfo (gu.getusers (t.qaID));
                }
                wrench tempw = Wrench.selectByguid(t.wrenchID);

                tempwcolist.Add(new WrenchCheckOut() 
                { 
                wrenchcode =tempw.wrenchCode ,
                wrenchbarcode =tempw.wrenchBarCode ,
                jusername =(juser==null||juser.user ==null)?"":juser.user.username ,
                zusername =(zuser ==null||zuser.user ==null)?"":zuser.user.username ,
                juserinfo =juser,
                zuserinfo =zuser,
                torquetargetvalue =t.torqueTargetValue.ToString (),
                errorrange =(t.torqueTargetValue *(1+Convert.ToDecimal(t.errorRangeMin))).ToString ("f2")+"~"+(t.torqueTargetValue *(1+ Convert.ToDecimal (t.errorRangeMax) )).ToString ("f2"),
                errormax =Convert.ToDecimal (t.errorRangeMax).ToString ("f4"),
                errormin =Convert.ToDecimal (t.errorRangeMin).ToString ("f4"),
                is_good =t.is_good ,
                checkdate =t.checkDate.ToString ().Replace ('T',' '),                
                guid =t.guid 
                });
            }
            return tempwcolist;
        }

        void binding(List<WrenchCheckOut > list) {
            this.dt_showdate.ItemsSource = list;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.AppStarting;
            if (wrenchchecklist == null || wrenchchecklist.Count <= 0)
            {
                MessageAlert.Alert("没有数据不能导出！");
                this.Cursor = Cursors.Arrow;
                return;
            }
            CheckDataOutExcel cdoe = new CheckDataOutExcel();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            CheckDataOutExcelHelp _excelHelper = new CheckDataOutExcelHelp();
            saveFileDialog.Filter = "Excel (*.XLS)|*.xls";
            if ((bool)(saveFileDialog.ShowDialog()))
            {
                try
                {
               
                   // _excelHelper.SaveToExcel(saveFileDialog.FileName, cdoe.ToTable (wrenchchecklist),"智能扭矩校验数据");


                    _excelHelper.SaveToExcel(saveFileDialog.FileName, cdoe.Header(wrenchchecklist), wrenchchecklist, "智能扭矩校验数据");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("导出失败：" + ex.Message);
                    this.Cursor = Cursors.Arrow;
                    return;
                }
                MessageBox.Show("导出成功");
                this.Cursor = Cursors.Arrow;
            }
        }

        private void editeButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.dt_showdate.SelectedIndex < 0)
                return;
            WrenchCheckOut wco = dt_showdate.SelectedItem as WrenchCheckOut;
            CheckdataDetail cd = CheckdataDetail.GetCheckdataDetail(wco );
            cd.Owner = _main;
            cd.Show();
        }

        private void bt_print_Click(object sender, RoutedEventArgs e)
        {
            if (this.dt_showdate.SelectedIndex < 0)
            {
                MessageAlert.Alert("请选择要打印的行！");
                return;
            }
            int cid = 1;
            systemcheckset sysset=null;
           List<systemcheckset> scsl = SerializeXML<systemcheckset>.Getlist();
            if (scsl != null || scsl.Count > 0)
                sysset = scsl.FirstOrDefault();
           WrenchCheckOut wco = dt_showdate.SelectedItem as WrenchCheckOut;
           List < torquecheckrecord > cd = CheckTargetRecord.SelectByCheckTargetID (wco.guid);
            wrench w = Wrench.selectByBarcode(wco.wrenchbarcode);
            List<ShowCheckresult> scr = new List<ShowCheckresult>();
            if (cd != null)
            {
                List<torquecheckrecord> success = cd.FindAll(p => p.isEffective == true);
                foreach (torquecheckrecord t in success)
                {
                    scr.Add(new ShowCheckresult() { id = cid, checkdata = t.analyserValue, setdata = t.torqueTargetValue, normalrang = (t.torqueTargetValue * (1 +Convert.ToDecimal (t.errorRangeMin))).ToString("f2") + "~" + (t.torqueTargetValue * (1 + Convert.ToDecimal (t.errorRangeMax))).ToString("f2"), result = t.passedFlag == true ? "√" : "×", error = t.torqueTargetValue == 0 ? "" : (((t.analyserValue - t.torqueTargetValue) / t.torqueTargetValue) * 100).ToString("f2") + "%" });
                    cid++;
                }

                List<string> successdata = GetCheckTarget(success);

                foreach (string s in successdata)
                {
                    cd.RemoveAll(p => { if (p.torqueTargetValue.ToString("f2") == s) { return true; } else { return false; } });
                }
                List<string> faildataa = GetCheckTarget(cd);
                foreach (string s in faildataa)
                {
                    List<torquecheckrecord> fail = cd.FindAll(p => p.torqueTargetValue.ToString("f2") == s);
                    int m = 0;
                    foreach (torquecheckrecord t in fail)
                    {

                        if (m >= sysset.count)
                            break;
                        scr.Add(new ShowCheckresult() { id = cid, checkdata = t.analyserValue, setdata = t.torqueTargetValue, normalrang = (t.torqueTargetValue * (1 + Convert.ToDecimal (t.errorRangeMin))).ToString("f2") + "~" + (t.torqueTargetValue * (1 + Convert.ToDecimal ( t.errorRangeMax))).ToString("f2"), result = t.passedFlag == true ? "√" : "×", error = t.torqueTargetValue == 0 ? "" : (((t.analyserValue - t.torqueTargetValue) / t.torqueTargetValue) * 100).ToString("f2") + "%" });
                        m++;
                        cid++;
                    }

                }
            }
            #region


            //foreach (torquecheckrecord ck in cd)
            //{
            //    decimal check= Convert.ToDecimal(ck.analyserValue );
            //    decimal setvalue= Convert.ToDecimal(wco.torquetargetvalue);
            //    decimal derror = Convert.ToDecimal(((check - setvalue) / setvalue).ToString("f4"));
            //    scr.Add(new ShowCheckresult() { checkdata =check, setdata =setvalue, normalrang = wco.errorrange,normalmax=wco.errormax ,normalmin =wco.errormin , errorrang =derror,result = ck.passedFlag  ? "√" : "×" });        
            //}
        if(scr ==null ||scr.Count <=0)
        {
            MessageAlert.Alert("选中行没有详细的校验数据！");
            return;

        }

    
        //foreach (torquecheckrecord t in cd)
        //{
        //    if (t.isEffective)
        //        confcount++;
        //}
        //if (confcount <= 0)
        //{
        //    systemcheckset _systemcheckset = new systemcheckset();
        //    try
        //    {
        //        confcount = Convert.ToInt16(_systemcheckset.count);
        //    }
        //    catch { confcount = 0; }
        //}
        //if (confcount <= 0)
        //    confcount = 5;
        HandleData hd = new HandleData(wco.juserinfo.user , wco.zuserinfo, w,wco.is_good , Convert.ToDecimal(wco.torquetargetvalue), Convert.ToDecimal(10), Convert.ToDecimal(10));
        //hd.Checkdatashow = scr;
        //hd.filterdata();
        //List<ShowCheckresult> lssc = hd.Getprint();
            PrintSingleCheckdata psc = PrintSingleCheckdata.GetPrintSingleCheckdata(w,scr,cd.Count ,Convert.ToDecimal (wco.torquetargetvalue) ,wco.juserinfo,wco.zuserinfo ,Convert.ToDateTime (wco.checkdate),wco.is_good );
            psc.Topmost = true;
            psc.Owner = _main;
            psc.Show();
            #endregion

        }

        List<string > GetCheckTarget(List<torquecheckrecord> cd)
        {
            List<string> checktarget = new List<string>();
            foreach (torquecheckrecord t in cd)
            {
                if (!checktarget.Contains(t.torqueTargetValue.ToString("f2")))
                    checktarget.Add(t.torqueTargetValue .ToString ("f2"));
            }
            return checktarget;
        }

   


    }
}
