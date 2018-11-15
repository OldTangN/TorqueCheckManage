using LT.BLL;
using LT.BLL.Wrench;
using LT.Comm;
using LT.DAL;
using LT.DAL.MySql;
using LT.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LongTie.Nlbs.Wrench
{
    /// <summary>
    /// Interaction logic for editerWrench.xaml
    /// </summary>
    public partial class editerWrench 
    {
      //  public static editerWrench editerwrench=null;

        wrench _wrench=new wrench ();
        IWrench Wrench = DataAccess.CreateWrench();
        IWrenchSpecies WrenchSpecies = DataAccess.CreateWrenchSpecies();
        IWrenchStatus WrenchStatus = DataAccess.CreateWrenchStatus();
        int pageno = 1;
        int pagesize = 15;
        int totalpage = 0;

        IBorrow Borrow = new MySqlBorrow();
        ICheckTarget CheckTarget = DataAccess.CreateCheckTarget();
        ICheckTargetRecord CheckTargetRecord = DataAccess.CreateCheckTargetRecord();
        bool isadd = true ; 
        userinfo _userinfo=null;

     public  editerWrench(userinfo u)
        {
            InitializeComponent();
          
            _userinfo = u;
        }

     private void getwrenchlist(List<ToolModel> toolmodellist)
     {
         dataGrid1.ItemsSource = null;
         dataGrid1.ItemsSource = toolmodellist;
     }
     private List<ToolModel> Gettoolmodel(List<wrench> wrenchlist)
     {
         List<ToolModel> toolmodellist = new List<ToolModel>();
         try
         {
             if (wrenchlist != null)
             {
                 int tempid = 1;
                 foreach (wrench w in wrenchlist)
                 {
                     wrenchspecies ws = WrenchSpecies.selectByGuid(w.species);
                     wrenchstatus wss = WrenchStatus.selectByguid(w.status);
                     toolmodellist.Add(new ToolModel()
                     {                         
                         comment = w.comment,
                         createDate = w.createDate,
                         factory = w.factory,
                         guid = w.guid,
                         id =tempid++,
                         IP = w.IP,
                         port = w.port,
                         rangeMax =Convert .ToDecimal ( w.rangeMax.ToString ("f1")),
                         rangeMin =Convert .ToDecimal ( w.rangeMin.ToString ("f1")),
                         targetvalue =Convert .ToDecimal ( w.targetvalue.ToString ("f1")),
                         targetvalue1 = Convert.ToDecimal(w.targetvalue1.ToString("f1")),
                         targetvalue2 = Convert.ToDecimal(w.targetvalue2.ToString("f1")),
                         unit = w.unit,
                         species = w.species,
                         speciesName =ws!=null? ws.speciesName:"",
                         status = w.status,
                         statusDM =wss!=null? wss.statusDM:"",
                         statusName =wss!=null? wss.statusName:"",
                         wrenchBarCode = w.wrenchBarCode,
                         wrenchCode = w.wrenchCode,
                         lastrepair=w.lastrepair
                     });
                 }
             }
         }
         catch 
         { }
         return toolmodellist;

     }

        void getspecies()
        {
            try {
                this.cb_species.ItemsSource  = WrenchSpecies.select();
                this.cb_species.DisplayMemberPath = "speciesName";
                this.cb_species.SelectedValuePath = "id";
            }
            catch { }
        }
        void getstatus() 
        {
            try {
                List<wrenchstatus> ws = new List<wrenchstatus>();
                ws = WrenchStatus.selectAll();
                this.cb_status.ItemsSource = ws;
                this.cb_status.DisplayMemberPath = "statusName";
                this.cb_status.SelectedValuePath = "id";
                this.cb_status.SelectedIndex = ws.FindIndex (p=>p.statusDM =="001");

            }
            catch { }
        }
        void showwrench(wrench w)
        {
            try {
                if (w == null)
                    return;
                this.tb_wrenchcode.Text = w.wrenchCode;
                this.tb_wrenchbarcode.Text = w.wrenchBarCode;
                this.tb_targetvalue.Text = w.targetvalue.ToString();
                this.tb_targetvalue1.Text = w.targetvalue1<=0?"":w.targetvalue1 .ToString();
                this.tb_targetvalue2.Text = w.targetvalue2<= 0? "" : w.targetvalue2.ToString();
                this.tb_unite.Text = w.unit;
                this.tb_min.Text = w.rangeMin.ToString();
                this.tb_max.Text = w.rangeMax.ToString();
                this.tb_factory.Text = w.factory == null ? "" : w.factory;
                this.tb_com.Text = w.comment == null ? "" : w.comment;
                this.dp_time.Text = w.createDate.ToString ("yyyy-MM-dd HH:mm:ss");
                this.dp_time_Copy.Text = w.lastrepair.ToString("yyyy-MM-dd");
                foreach (var d in cb_species.Items)
                {
                    if (d != null && (((wrenchspecies)d).guid == w.species))
                    {
                        cb_species.SelectedItem = d;
                        break;
                    }
                }
                foreach (var s in cb_status .Items )
                {
                    if (s != null && (((wrenchstatus)s).guid == w.status))
                    {
                        cb_status.SelectedItem = s; break; 
                    }
                }

            }
            catch { }
        
        }
        wrench  getwrench(wrench w)
        {
            try
            {
                w.wrenchCode = this.tb_wrenchcode.Text.Trim();
                w.wrenchBarCode = this.tb_wrenchbarcode.Text.Trim();
                w.factory = this.tb_factory.Text.Trim();
                w.rangeMax = Convert.ToDecimal(this.tb_max.Text.Trim());
                w.rangeMin = Convert.ToDecimal(this.tb_min.Text.Trim());
                w.comment = this.tb_com.Text.Trim();
                w.targetvalue =Convert.ToDecimal(this.tb_targetvalue.Text.Trim());
                w.targetvalue1 = string.IsNullOrEmpty(this.tb_targetvalue1.Text.Trim()) ? 0 : Convert.ToDecimal(this.tb_targetvalue1.Text.Trim());
                w.targetvalue2 = string.IsNullOrEmpty(this.tb_targetvalue2.Text.Trim()) ? 0 : Convert.ToDecimal(this.tb_targetvalue2.Text.Trim());
                w.species = (cb_species.SelectedItem as wrenchspecies).id.ToString();
                w.status = (cb_status.SelectedItem as wrenchstatus).id.ToString();
                w.unit = "N.m";
                w.createDate = Convert.ToDateTime ( Convert.ToDateTime(this.dp_time.Text.Trim()).ToString ("yyyy-MM-dd HH:mm:ss"));
                w.lastrepair =Convert.ToDateTime (  Convert.ToDateTime(this.dp_time_Copy.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss"));
                return w;
            }
            catch {
                MessageAlert.Alert("请输入正确的数字值！");
                return null;
            }
        }
        bool add(wrench w)
        {
            w.createDate = Convert .ToDateTime ( this.dp_time .Text.Trim ());
            w.lastrepair = Convert.ToDateTime(this.dp_time_Copy.Text.Trim());
            w.guid = Guid.NewGuid().ToString();
            w.isallowcheck = true;
            w.cycletime = 0;
            return Wrench.add(w);
        }
        bool isexit(wrench w)
        {
            if (w == null) return true ;
            if (Wrench.selectByBarcode(w.wrenchBarCode) != null)
            { MessageBox.Show("工具条码号已经存在不能重复！"); return true ; }
            if (Wrench.selectBycode(w.wrenchCode)!=null&&Wrench.selectBycode(w.wrenchCode).Count > 0) 
            { MessageBox.Show("工具编号已经存在不能重复！"); return true ; }
            return false ;
        }
        bool IsRepeat(wrench w)
        {
            if (w == null) return false;
            wrench tempw=Wrench.selectByBarcode(this.tb_wrenchbarcode .Text .Trim ());
            if (tempw!=null&& w.guid !=tempw .guid)
            {
                MessageBox.Show("工具条码号已经存在不能重复！"); 
                return true ; 
            }
            List <wrench > wl=Wrench.selectBycode(w.wrenchCode);
            if (wl!= null && wl.Count > 0)
            {
                foreach (wrench wc in wl)
                {
                    if (wc != null && wc.guid != w.guid)
                    {
                        MessageBox.Show("工具编号已经存在不能重复！");
                        return true;
                    }
                }
            }
              
            return false ;
        }
        bool update(wrench w)
        {
            //if (w.cycletime == null||w.cycletime<=0)
            //    w.cycletime = 0;
            try
            {
                w.cycletime = Convert.ToDecimal(w.cycletime);
            }
            catch
            {
                w.cycletime = 0;
            }
            w.species = (cb_species.SelectedItem as wrenchspecies).guid .ToString();
            w.status = (cb_status.SelectedItem as wrenchstatus).guid.ToString();
            return Wrench.updata(w );
        }
        bool IsEmpty()
        {
            if ((this.cb_species.SelectedIndex < 0) || (string.IsNullOrEmpty(this.tb_factory.Text.Trim()))
                || (string.IsNullOrEmpty(this.tb_max.Text.Trim()))
                || (string.IsNullOrEmpty(this.tb_min.Text.Trim()))
                || (string.IsNullOrEmpty(this.tb_targetvalue.Text.Trim()))
                || (string.IsNullOrEmpty(this.tb_wrenchbarcode.Text.Trim()))
                || (string.IsNullOrEmpty(this.tb_wrenchcode.Text.Trim()))
                || (this.cb_status.SelectedIndex < 0))
            {
                MessageAlert.Alert("*为必填信息！");
                return true;
            }
            decimal max = 0;
            decimal min = 0;
            decimal target = 0;
            try
            {
                 max = Convert.ToDecimal(this.tb_max.Text.Trim());
                 min = Convert.ToDecimal(this.tb_min.Text.Trim());
                 target = Convert.ToDecimal(this.tb_targetvalue.Text.Trim());
            }
            catch {
                MessageAlert.Alert("请输入正确的数值");
                return true;
            }
         if (max <min)
            {
                MessageAlert.Alert("误差上限小于误差上限！");
                return true ;
            }
         if (target <min || target >max )
                {
                MessageAlert .Alert ("设定值不在量程范围之内！");
                   return true  ;
                }
         if (!string.IsNullOrEmpty(this.tb_targetvalue1.Text.Trim()))
         {
             try
             {
                 decimal d = Convert.ToDecimal(this.tb_targetvalue1.Text .Trim ());
                 if (d < min || d > max)
                 {
                     MessageAlert.Alert("设定值1不在量程范围之内！");
                     return true;
                 }
             
             }
             catch 
             {
                 MessageAlert.Alert("输入正确的设定值1！");
                 return true;
             }
         }

         if (!string.IsNullOrEmpty(this.tb_targetvalue2.Text.Trim()))
         {
             try
             {
                 decimal d = Convert.ToDecimal(this.tb_targetvalue2.Text.Trim());
                 if (d < min || d > max)
                 {
                     MessageAlert.Alert("设定值2不在量程范围之内！");
                     return true;
                 }
             }
             catch
             {
                 MessageAlert.Alert("输入正确的设定值2！");
                 return true;
             }
         }
            return false;
        }
        private void bt_sub_Click(object sender, RoutedEventArgs e)
        {
            if (IsEmpty())
                return;
            try
            {
                if (isadd)
                {
                    wrench w = new wrench();
                    w = getwrench(w);
                    if (w == null)
                        return;
                    if (isexit(w))
                        return;
                    if (add(w))
                    {
                        MessageAlert.Alert("操作成功！");
                        getwrenchlist(Gettoolmodel(Wrench.selectPage(pagesize, 1)));
                    }
                    else
                    {
                        MessageAlert.Alert("添加失败！");
                    }
                    
                }
                else
                {
                    wrench w = new wrench();
                    w = _wrench;
                    w = getwrench(w);
                    if (IsRepeat(w))
                        return;
                    if (update(w))
                    {
                        MessageAlert.Alert("操作成功！");
                        getwrenchlist(Gettoolmodel(Wrench.selectPage(pagesize, pageno)));
                    }
                    else
                    {
                        MessageAlert.Alert("跟新失败！");
                    }
                   
                }
                         
                getstatus();
                clear();
            }
            catch 
            {
                MessageAlert.Alert("出现异常请与管理员联系！");
            }
 

        //try {
        //    if (!getwrench())
        //        return;
        //    if (isadd)
        //    {
                
        //        if (add())
        //        {
        //            isadd = true;
        //            _wrench = new wrench();
        //            showwrench();
        //            getwrenchlist(Gettoolmodel(Wrench.select()));
        //            MessageBox.Show("保存成功！");
        //            return;
        //        }
             
                          
        //    }
        //    else {
        //        if (update ()) {
        //            isadd = true;
        //            _wrench = new wrench();
        //            showwrench();
        //            getwrenchlist(Gettoolmodel(Wrench.select()));
        //            MessageBox.Show("更新成功！");
        //            return;
        //        }
        //    }
        //    }
        //    catch { MessageBox.Show("操作失败！ 稍后再试！"); }
        //    MessageBox.Show("操作失败");
           
        }

        private void bt_back_Click(object sender, RoutedEventArgs e)
        {
           
            clear();

        }
        void clear()
        {
            this.tb_factory.Clear();
            this.tb_max.Clear();
            this.tb_min.Clear();
            this.tb_targetvalue.Clear();
            this.tb_wrenchbarcode.Clear();
            this.tb_wrenchcode.Clear();
            isadd = true;
            _wrench = null;
            this.dp_time.Text = DateTime.Now.ToShortDateString ();
            getstatus();
        }

        private void editButtonClick(object sender, RoutedEventArgs e)
        {
            int i_index = dataGrid1.SelectedIndex;
            if (i_index >= 0)
            {
                _wrench = dataGrid1.SelectedItem as wrench ;
                showwrench(_wrench );
                isadd = false; 
            }
        }

        private void bt_in_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            WrenchExcelIn wei = new WrenchExcelIn();
            wei.DaoIn();
            getwrenchlist(Gettoolmodel(Wrench.selectPage(pagesize, pageno)));
            this.Cursor = Cursors.Arrow;
        }

        private void tb_moban_Click(object sender, RoutedEventArgs e)
        {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
            ExcelHelp _excelHelper = new ExcelHelp();
            saveFileDialog.Filter = "Excel (*.XLS)|*.xls";
            if ((bool)(saveFileDialog.ShowDialog()))
            {
                try
                {
                    File.Copy(@"工具模板.xls", saveFileDialog.FileName, true);
                    MessageAlert.Alert("导出成功！");
                }
                catch
                {
                    MessageAlert.Error("导出失败！");
                }
            }
        }

        private void delButtonClick(object sender, RoutedEventArgs e)
        {
            int i_index = dataGrid1.SelectedIndex;
            if (i_index >= 0)
            {
                _wrench = dataGrid1.SelectedItem as wrench;
                if (MessageAlert.Alter("是否删除该条记录"))
                {
                    if (SqlietDelWrench(_wrench))
                    {                       
                        getwrenchlist(Gettoolmodel(Wrench.selectPage(pagesize, pageno)));
                        getstatus();
                        clear();
                    }
                  
                }
            }
        }

        bool SqlietDelWrench(wrench w)
        {
            StringBuilder ws = new StringBuilder("");
            if (_userinfo != null && _userinfo.user != null)
            {
                ws.Append("操作人：" + _userinfo.user.username + "\n");
            }
            ws.Append("wrench:"
                + " 工具编号 " + w.wrenchCode
                + " 工具条码号 " + w.wrenchBarCode
                + " 最小值 " + w.rangeMin
                + " 最大值 " + w.rangeMax
                + " 生产厂家 " + w.factory
                + " 入库日期 " + w.createDate
                + " IP " + w.IP
                + " port " + w.port
                + " 种类 " + w.species
                + " 状态 " + w.status
                + " 最近维护时间 " + w.lastrepair
                + " 周期 " + w.cycletime
                + " 允许校验 " + w.isallowcheck
                + " 目标值 " + w.targetvalue
                + " 单位 " + w.unit
                + " comment " + w.comment
                + " 标识 " + w.guid
                + "\n"
                );
            try
            {
                Wrench.Del(w);
            }
            catch
            {
                MessageAlert.Alert("删除失败！数据库执行错误！");
                return false;
            }
          
            LogUtil.WriteLog(typeof(string), ws.ToString());
            MessageAlert.Alert("删除成功");
            return true;

        }

        bool DelWrench(wrench w)
        {
            if (w == null)
                return false;
            try
            {
                StringBuilder borrowlist = new StringBuilder("");
                StringBuilder checklist = new StringBuilder("");
                StringBuilder ws = new StringBuilder("");
                if (_userinfo != null && _userinfo.user != null)
                {
                    ws.Append("操作人："+_userinfo.user .username +"\n");
                }
                ws.Append("wrench:"
                    + " 工具编号 " + w.wrenchCode
                    + " 工具条码号 " + w.wrenchBarCode
                    + " 最小值 " + w.rangeMin
                    + " 最大值 " + w.rangeMax
                    + " 生产厂家 " + w.factory
                    + " 入库日期 " + w.createDate
                    + " IP " + w.IP
                    + " port " + w.port
                    + " 种类 " + w.species
                    + " 状态 " + w.status
                    + " 最近维护时间 " + w.lastrepair
                    + " 周期 " + w.cycletime
                    + " 允许校验 " + w.isallowcheck
                    + " 目标值 " + w.targetvalue
                    + " 单位 " + w.unit
                    + " comment " + w.comment
                    + " 标识 " + w.guid
                    + "\n"
                    );
                List<borrow> borrowmodel = Borrow.SelectByWrench(w.guid);
                List<torquechecktarget> torquechecktargetlist = CheckTarget.SelectByWrench(w.guid);
                if (borrowmodel != null && borrowmodel.Count > 0)
                {
                    foreach (borrow b in borrowmodel)
                    {
                        borrowlist.Append("Borrow:"
                            + " 工具编号 " + b.WrenchID
                            + " 借用人 " + b.borrowUser
                            + " 借出操作人 " + b.borrowOperator
                            + " 归还人 " + b.returnUser
                            + " 归还操作人 " + b.returnOperator
                            + " 借用日期 " + b.borrowDate
                            + " 归还 " + b.is_return
                            + " 归还日期 " + b.returnDate
                            + " comment " + b.comment
                            + " 标识 " + b.guid
                            + "\n"
                            );
                        Borrow.Del(b);
                    }
                }
                if (torquechecktargetlist != null && torquechecktargetlist.Count > 0)
                {
                    foreach (torquechecktarget t in torquechecktargetlist)
                    {
                        checklist.Append("Torquechecktarget:" + " wrenchID " + t.wrenchID
                            + " 校验时间 " + t.checkDate
                            + " 质检id " + t.qaID
                            + " 操作人 " + t.operatorID
                            + " 目标校验值 " + t.torqueTargetValue
                            + " 最大允许误差 " + t.errorRangeMax
                            + " 最小允许误差 " + t.errorRangeMin
                            + " 是否合格 " + t.is_good
                            + " comment " + t.comment
                            + " 唯一标识 " + t.guid + "\n"
                            );
                        List<torquecheckrecord> checkrecord = CheckTargetRecord.SelectByCheckTargetID(t.guid);
                        if (checkrecord != null && checkrecord.Count > 0)
                        {
                            foreach (torquecheckrecord tq in checkrecord)
                            {
                                checklist.Append("torquecheckrecord:"
                                 + " 目标id " + tq.TorqueCheckTargetID
                                 + " 教育你值 " + tq.analyserValue
                                 + " 校验时间 " + tq.checkTime
                                 + "  是否合格 " + tq.passedFlag
                                 + " 是否有效 " + tq.isEffective
                                 + " comment " + tq.comment
                                 + " 标识 " + tq.guid
                                 + "\n"
                                 );
                                CheckTargetRecord.Del(tq);
                            }

                        }

                        CheckTarget.Del(t);
                    }

                }
                Wrench.Del(w);
                // LogUtil.WriteLog(typeof(string), borrowlist.ToString());
                LogUtil.WriteLog(typeof(string), ws.ToString() + borrowlist.ToString() + checklist.ToString());
                MessageAlert.Alert("删除成功");
                return true;
            }
            catch
            {
                LogUtil.WriteLog(typeof(string), "删除失败！");
                MessageAlert.Alert("删除失败");
                return false;
            }
        }

        private void barkeydown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                tb_wrenchbarcode.SelectAll();
        }

        private void pagesize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            pagesize = Convert.ToInt32((this.cb_pagesize.SelectedItem as ComboBoxItem).Content.ToString ());
            totalpage = (Wrench.SelectCount() % pagesize) > 0 ? Wrench.SelectCount() / pagesize + 1 : Wrench.SelectCount() / pagesize;
            this.lb_totalpage.Content = totalpage.ToString();
            getwrenchlist(Gettoolmodel(Wrench.selectPage(pagesize, 1)));
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            getspecies();
            getstatus();
            getwrenchlist(Gettoolmodel(Wrench.selectPage(pagesize, 1)));
            totalpage = (Wrench.SelectCount() % pagesize) > 0 ? Wrench.SelectCount() / pagesize + 1 : Wrench.SelectCount() / pagesize;
            this.lb_totalpage.Content= totalpage.ToString ();
            this.cb_pagesize.SelectedIndex = 1;
        }

        private void bt_firstpage_Click(object sender, RoutedEventArgs e)
        {
            getwrenchlist(Gettoolmodel(Wrench.selectPage(pagesize, 1)));
            this.tb_pageno.Text = "1";
        }

        private void bt_propage_Click(object sender, RoutedEventArgs e)
        {
            if (pageno >1)
            {
                pageno--;
                getwrenchlist(Gettoolmodel(Wrench.selectPage(pagesize, pageno)));
            }
            else
            {
                getwrenchlist(Gettoolmodel(Wrench.selectPage(pagesize, 1)));
            }
            this.tb_pageno.Text = pageno.ToString() ;
        }

        private void bt_nextpage_Click(object sender, RoutedEventArgs e)
        {
            if (pageno < totalpage)
            {
                pageno++;
                getwrenchlist(Gettoolmodel(Wrench.selectPage(pagesize, pageno)));
            }
            else
            {
                getwrenchlist(Gettoolmodel(Wrench.selectPage(pagesize, totalpage)));
            }
            this.tb_pageno.Text = pageno.ToString();
        }

        private void bt_lastpage_Click(object sender, RoutedEventArgs e)
        {
            getwrenchlist(Gettoolmodel(Wrench.selectPage(pagesize, totalpage)));
            this.tb_pageno.Text = totalpage.ToString();
        }

        private void bt_targetpage_Click(object sender, RoutedEventArgs e)
        {
            Regex reg = new Regex(@"[^0-9]");
            if (!reg.IsMatch(this.tb_pageno.Text.Trim()))
            {
                if(Convert.ToInt32(this.tb_pageno.Text.Trim ())>0&&Convert.ToInt32(this.tb_pageno.Text.Trim())<totalpage)
                    getwrenchlist(Gettoolmodel(Wrench.selectPage(pagesize, Convert.ToInt32(this.tb_pageno.Text.Trim()))));
            }
        }


        private void tb_min_KeyUp(object sender, KeyEventArgs e)
        {
            this.tb_targetvalue1.Text = this.tb_min.Text.Trim();
        }

        private void tb_max_KeyUp(object sender, KeyEventArgs e)
        {
            this.tb_targetvalue2.Text = this.tb_max.Text.Trim();
        }
    }
}
