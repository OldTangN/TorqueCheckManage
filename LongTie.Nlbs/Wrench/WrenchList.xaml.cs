using LT.BLL;
using LT.BLL.Wrench;
using LT.Comm;
using LT.DAL;
using LT.DAL.MySql;
using LT.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LongTie.Nlbs.Wrench
{
    /// <summary>
    /// Interaction logic for WrenchList.xaml
    /// </summary>
    public partial class WrenchList 
    {
        List<wrench> _wrenchlist = new List<wrench>();
        IWrench Wrench = DataAccess.CreateWrench();
        IWrenchStatus WrenchStatus = DataAccess.CreateWrenchStatus();
        IWrenchSpecies WrenchSpecies = DataAccess.CreateWrenchSpecies();
        MainWindow _mw = new MainWindow();
        public WrenchList()
        {
            InitializeComponent();           
            getstatuslist();
            //List<ToolModel> toolmodellist = Gettoolmodel(Wrench.select());
            //if (toolmodellist != null && toolmodellist.Count > 0)
            //    getwrenchlist(toolmodellist);
        }

        void getstatuslist()
        {
            List<wrenchstatus> status = WrenchStatus.selectAll();
            wrenchstatus w = new wrenchstatus();
            if(status!=null)
            status.Insert(0,w);
            this.cb_status.ItemsSource = status;
            this .cb_status .DisplayMemberPath ="statusName";
            this .cb_status.SelectedValuePath ="guid";
        }
        private void getwrenchlist(List<ToolModel> toolmodellist)
        {
            dataGrid1.ItemsSource = toolmodellist;
        }
        private List<ToolModel> Gettoolmodel(List<wrench> wrenchlist)
        {
            List<ToolModel> toolmodellist = new List<ToolModel>();
            try{
                if (wrenchlist != null && wrenchlist.Count > 0)
                {
                    int count = 0;

                    foreach (wrench w in wrenchlist)
                    {
                        count++;
                        wrenchspecies ws = WrenchSpecies.selectByGuid(w.species);
                        wrenchstatus wss = WrenchStatus.selectByguid(w.status);
                        toolmodellist.Add(new ToolModel()
                        {
                            comment = w.comment,
                            createDate = w.createDate,
                            factory = w.factory,
                            guid = w.guid,
                            id = count,
                            IP = w.IP,
                            port = w.port,
                            rangeMax = Convert.ToDecimal(w.rangeMax.ToString("f1")),
                            rangeMin = Convert.ToDecimal(w.rangeMin.ToString("f1")),
                            targetvalue = Convert.ToDecimal(w.targetvalue.ToString("f1")),
                            targetvalue1 = Convert.ToDecimal(w.targetvalue1.ToString("f1")),
                            targetvalue2 = Convert.ToDecimal(w.targetvalue2.ToString("f1")),
                            unit = w.unit,
                            species = w.species,
                            speciesName = ws.speciesName,
                            status = w.status,
                            statusDM = wss.statusDM,
                            statusName = wss.statusName,
                            lastrepair = w.lastrepair,
                            cycletime =Convert .ToDecimal ( w.cycletime.ToString ("f0")),
                            wrenchBarCode = w.wrenchBarCode,
                            wrenchCode = w.wrenchCode
                        });
                    }
                }
                else 
                {
                    MessageAlert.Alert("没有工具信息！");
                }
            }
            catch {}
           return  toolmodellist;
        
        }
        #region
        //private void btn_add_Click(object sender, RoutedEventArgs e)
        //{
        //    //editerWrench ew = editerWrench.GetAddediterwrench();
        //    //ew.Owner = _mw;
        //    //if ((bool)ew.ShowDialog())
        //    //{
        //    //    getwrenchlist(Gettoolmodel(Wrench.select()));
        //    //}
        //}

        //private void Btn_Edit_Click(object sender, RoutedEventArgs e)
        //{
        //    //if (dataGrid1.SelectedItem == null) { MessageBox.Show("请选择要修改的行！"); return; }
        //    //editerWrench ew = editerWrench.GetUpdateediterwrench(dataGrid1 .SelectedItem  as wrench );
        //    //ew.Owner = _mw;
        //    //if ((bool)ew.ShowDialog()) {
        //    //    getwrenchlist(Gettoolmodel(Wrench.select()));
        //    //}
        //}

        //private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        //{
        //    if (dataGrid1.SelectedItem == null) { MessageBox.Show("请选择要删除的行！"); return; }
        //    wrench w = dataGrid1.SelectedItem as wrench;
        //    wrenchstatus ws=WrenchStatus.selectByStatusDM ("0");
        //    w.status = ws.guid;
        //    if (Wrench.updata(w))
        //    { MessageBox.Show("删除成功！"); }
        //    else { MessageBox.Show("删除失败！"); }
        //    getwrenchlist(Gettoolmodel(Wrench.select()));

        //}
        #endregion


        private void dataGrid1_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

        }

        private void bt_search_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                if (!string.IsNullOrEmpty(this.tb_wrenchcode.Text.Trim()))
                    dict.Add("wrenchCode", this.tb_wrenchcode.Text.Trim());
                if (!string.IsNullOrEmpty(this.tb_wrenchbarcode.Text.Trim()))
                    dict.Add("wrenchBarCode", this.tb_wrenchbarcode.Text.Trim());
                if ((cb_status.SelectedItem as wrenchstatus) != null&&(cb_status.SelectedItem as wrenchstatus).guid != null)
                    dict.Add("status_id", (cb_status.SelectedItem as wrenchstatus).guid);
                if (dict.Count > 0)
                    getwrenchlist(Gettoolmodel(Wrench.selectByContion(dict).OrderBy (p=>p.createDate).ToList ()));
                else
                {
                    getwrenchlist(Gettoolmodel(Wrench.select().OrderBy (p=>p.createDate).ToList()));
                }
            }
            catch 
            {
                dataGrid1.ItemsSource = null;
            }
        }

        private void infoout_Click(object sender, RoutedEventArgs e)
        {
            List<ToolModel> tl = (List<ToolModel>)this.dataGrid1.ItemsSource;
            if (tl == null || tl.Count <= 0)
                return;
            WrenchExcelOut weo = new WrenchExcelOut();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            ExcelHelp _excelHelper = new ExcelHelp();
            saveFileDialog.Filter = "Excel (*.XLS)|*.xls";
            if ((bool)(saveFileDialog.ShowDialog()))
            {
                try
                {

                    _excelHelper.SaveToExcel(saveFileDialog.FileName, weo.ToTable(tl));
                    MessageBox.Show("导出成功");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("导出失败！");
                    return;
                }
             
            }
        }
        private void tb_wrenchcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.bt_search_Click(sender, e);
            }
        }


        private void tb_wrenchbarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.bt_search_Click(sender, e);
            }
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.bt_search_Click(sender, e);
            }
            if (e.Key == Key.B)
            {
                MessageAlert.Alert("BBB");
            }
        }
    }
}
