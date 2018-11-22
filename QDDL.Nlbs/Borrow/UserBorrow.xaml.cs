 
using QDDL.BLL;
using QDDL.BLL.Borrow;
using QDDL.Comm;
using QDDL.DAL;
using QDDL.DAL.MySql;
using QDDL.Model;
using QDDL.Model.BllModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Xps.Packaging;

namespace QDDL.Nlbs.Borrow
{
    /// <summary>
    /// Interaction logic for UserBorrow.xaml
    /// </summary>
    public partial class UserBorrow : Window
    {
     //   private bool _firstActivated = true;
        IUser User = DataAccess.CreateUser();
        IWrench Wrench = DataAccess.CreateWrench();
        IBorrow Borrow = new MySqlBorrow();
       List<BorrowHistory> _borrowhistory=new List<BorrowHistory> ();
        public UserBorrow(List<BorrowHistory> borrowhistory)
        {
            InitializeComponent();
            _borrowhistory = borrowhistory;
            DgBind(_borrowhistory);
        }
        void DgBind(List<BorrowHistory> borrowhistory)
        {
            this.gd_borrow.ItemsSource = null;
            this.gd_borrow.ItemsSource = borrowhistory;
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.ApplicationIdle, new Action(ProcessRows)); 
        }

        private void bt_search_Click(object sender, RoutedEventArgs e)
        {
            //if (string.IsNullOrEmpty(this.contion.Text.Trim()))
            //{
            //    MessageAlert.Alert("请填写查询条件!");
            //    return;
            //}
         
            bool? isreturn=null;
            if(this.cb_isreturn.SelectedIndex==1)
                isreturn =true ;
            if(this.cb_isreturn .SelectedIndex ==2)
                isreturn =false ;
          List <borrow >bl=  Getborrowlist(this.starttime.ToString(), Convert .ToDateTime ( this.endtime.SelectedDate).AddDays (1).AddSeconds(-1).ToString(), GetContion(this.contion.Text .Trim ()), isreturn);
          List<BorrowHistory> bhl=null;
            if (bl != null && bl.Count > 0)
          {
              WrenchBorrowHistory wbh = new WrenchBorrowHistory();
             bhl = wbh.GetByUser(bl);            
          }
            DgBind(bhl);
        }
        string GetContion(string s)
        {
            List<users> u = User.SelectNameOrCardid(s);
            if (u == null || u.Count <= 0)
            {
                List<wrench> w = Wrench.SelectBarorcode(s);
                if (w == null || w.Count <= 0)
                    return null;
                return w.FirstOrDefault().guid;
            }
            else
                return u.FirstOrDefault().guid;
        }

        List<borrow> Getborrowlist(string stime,string etime,string condition,bool? isreturn)
        {
            if (condition == null)
            {
                if (isreturn != null)
                {
                    return Borrow.SelectWrenchOrBUser(stime, etime, isreturn ?? false);
                }
                else
                    return Borrow.SelectWrenchOrBUser(stime, etime);
            }
            else
            {
                if (isreturn == null)
                {
                    return Borrow.SelectWrenchOrBUser(stime, etime, condition);
                }
                else
                {
                    return Borrow.SelectWrenchOrBUser(stime, etime, condition, isreturn);
                }  
            }
         
        }

        private void ProcessRows()
        {
            try
            {
                List <BorrowHistory> bl=(List <BorrowHistory >)gd_borrow .ItemsSource ;
                if (bl == null || bl.Count <= 0)
                    return;
                for (int i = 0; i < bl.Count; i++)
                {
                    if (!bl[i].isreturn)
                    {
                        var row = gd_borrow.ItemContainerGenerator.ContainerFromItem(gd_borrow.Items[i]) as DataGridRow;
                        // row.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ff7d40"));

                        row.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ff6100"));
                    }
                }
                //this.gd_borrow.SelectedIndex = this.gd_borrow.Items.Count - 1;
                //this.gd_borrow.ScrollIntoView(this.gd_borrow.SelectedItem);
            }
            catch
            { }
        }

        private void contion_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.bt_search_Click(this,e);
            }
        }

        private void bt_reportprint_Click(object sender, RoutedEventArgs e)
        {
            List<BorrowHistory> bl = (List<BorrowHistory>)this.gd_borrow.ItemsSource;
            if (bl != null)
            {
                ReportWin rw = new ReportWin(bl);
                rw.ShowDialog();
            }
            else { MessageAlert.Alert("没有数据！"); }
        }

        private void bt_dataout_Click(object sender, RoutedEventArgs e)
        {
            List<BorrowHistory > tl = (List<BorrowHistory >)this.gd_borrow.ItemsSource;
            if (tl == null || tl.Count <= 0)
                return;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            BorrowOutToExcel  uoe = new BorrowOutToExcel ();
            ExcelHelp _excelHelper = new ExcelHelp();
            saveFileDialog.Filter = "Excel (*.xls)|*.XLS";
            if ((bool)(saveFileDialog.ShowDialog()))
            {
                try
                {
                    _excelHelper.SaveToExcel(saveFileDialog.FileName, uoe.ToTable(tl));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("导出失败：" + ex.Message);
                    return;
                }
                MessageBox.Show("导出成功");
            }
        }


    }
}
