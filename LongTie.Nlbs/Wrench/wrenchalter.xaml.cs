using LT.BLL.Wrench;
using LT.Comm;
using LT.DAL;
using LT.DAL.MySql;
using LT.Model;
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
using System.Windows.Shapes;

namespace LongTie.Nlbs.Wrench
{
    /// <summary>
    /// Interaction logic for wrenchalter.xaml
    /// </summary>
    public partial class wrenchalter 
    {
        WrenchComBind wc = new WrenchComBind();
        List<ToolModel> toollist = new List<ToolModel>();
        IWrench Wrench = DataAccess.CreateWrench();
        IWrenchStatus WrenchStatus = DataAccess.CreateWrenchStatus();
        public wrenchalter()
        {
            InitializeComponent();
            wc.AlterBind(this.cb_status);
        }

        ToolModel GetToolMode(wrench w) 
        {
            ToolModel tm =null;
            wrenchstatus ws = WrenchStatus.selectByguid(w.status);
            if (ws == null || ws.guid == null)
                return tm;
         tm= new ToolModel()
            {
                wrenchBarCode = w.wrenchBarCode,
                wrenchCode = w.wrenchCode,
                comment = w.comment,
                createDate = w.createDate,
                cycletime = Convert .ToDecimal ( w.cycletime.ToString ("f0")),
                factory = w.factory,
                guid = w.guid,
                id = w.id,
                IP = w.IP,
                isallowcheck = w.isallowcheck,
                lastrepair = w.lastrepair,
                port = w.port,
                rangeMax = w.rangeMax,
                rangeMin = w.rangeMin,
                species = w.species,
                status = w.status,
                statusName = ws.statusName,
                targetvalue = w.targetvalue,
                unit = w.unit
            };
         return tm;
        }

        void BindGrid(string wrenchcodebar) 
        {
            wrench w = Wrench.selectByBarcode(this.tb_wrenchbarcode.Text.Trim());
            if (w == null || w.guid == null)
            {
                MessageAlert.Alert("没有工具信息！");
                return;
            }
            ToolModel tm = GetToolMode(w);
            if(tm!=null)
             if (toollist.FindIndex(p => p.wrenchBarCode == tm.wrenchBarCode)<0)
             {
                    toollist.Add(tm);
             }
           
            this.dt_showdata.ItemsSource = null;
            this.dt_showdata.ItemsSource = toollist;
        }
        void BindGrid(List<ToolModel> tl) 
        {
            List<ToolModel> tempt = new List<ToolModel>();
            foreach (ToolModel tm in tl)
            {
                wrench w = Wrench.selectByBarcode(tm.wrenchBarCode);
                ToolModel t= GetToolMode(w);
                if (t != null)
                { tempt.Add(t); }
            }
            this.dt_showdata.ItemsSource = null;
            this.dt_showdata.ItemsSource = tempt;
            tl.Clear();
        } 
        private void bt_search_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.tb_wrenchbarcode.Text.Trim()))
            { return; }

            BindGrid(this.tb_wrenchbarcode .Text .Trim ());

        }


        private void bt_submit_Click(object sender, RoutedEventArgs e)
        {
            if (cb_status.SelectedIndex < 0)
            { MessageAlert.Alert("请选择要修改的状态！"); return; }
            if (ischeck()<=0)
            {
                MessageAlert.Warning("请选择要修该的内容！");
                return; 
            }
            wrenchstatus w=this.cb_status .SelectedItem as wrenchstatus ;
            if(w==null||w.guid ==null)
                return ;
            if (updata(w.statusDM))
            {
                MessageAlert.Alert("修改成功！");

            }     
            BindGrid(toollist);

        }


        int  ischeck()
        {
            int count = 0;
            try
            {
                for (int i = 0; i < dt_showdata.Items.Count; i++)
                {
                    var cntr = dt_showdata.ItemContainerGenerator.ContainerFromIndex(i);
                    DataGridRow ObjROw = (DataGridRow)cntr;
                    if (ObjROw != null)
                    {
                        FrameworkElement objElement = dt_showdata.Columns[0].GetCellContent(ObjROw);
                        if (objElement != null)
                        {

                            System.Windows.Controls.CheckBox objChk = (System.Windows.Controls.CheckBox)objElement;
                            if (objChk.IsChecked == true)
                            {
                                count++;
                            }


                        }
                    }
                }
                return count;
            }
            catch { return 0; }
        }

        bool updata(string type)
        {
            try
            {
                for (int i = 0; i < dt_showdata.Items.Count; i++)
                {
                    var cntr = dt_showdata.ItemContainerGenerator.ContainerFromIndex(i);
                    DataGridRow ObjROw = (DataGridRow)cntr;
                    if (ObjROw != null)
                    {
                        FrameworkElement objElement = dt_showdata.Columns[0].GetCellContent(ObjROw);
                        if (objElement != null)
                        {

                            System.Windows.Controls.CheckBox objChk = (System.Windows.Controls.CheckBox)objElement;
                            if (objChk.IsChecked == true)
                            {
                                ToolModel tm = ObjROw.Item as ToolModel;
                                if (tm == null||tm.guid ==null )
                                    continue;
                                wrench s = new wrench();
                                if(type =="001")
                                {
                                    s = Wrench.selectByguid(tm.guid);
                                    wrenchstatus ws = WrenchStatus.selectByStatusDM(type);
                                    s.status = ws.guid;
                                    s.lastrepair = DateTime.Now;
                                }
                                if (type == "002")
                                {
                                    s = Wrench.selectByguid(tm.guid);
                                    wrenchstatus ws = WrenchStatus.selectByStatusDM(type);
                                    s.status = ws.guid;
                                }
                              
                                Wrench.updata(s);
                            }


                        }
                    }
                }
                return true;
            }
            catch { return false; }
        }

        private void bt_clear_Click(object sender, RoutedEventArgs e)
        {
            toollist.Clear();
            this.dt_showdata.ItemsSource = null;
        }
        private void cb_selelctall_Click(object sender, RoutedEventArgs e)
        {

            for (int i = 0; i < this.dt_showdata.Items.Count; i++)
            {
                var cntr = dt_showdata.ItemContainerGenerator.ContainerFromIndex(i);
                DataGridRow ObjROw = (DataGridRow)cntr;
                if (ObjROw != null)
                {
                    FrameworkElement objElement = dt_showdata.Columns[0].GetCellContent(ObjROw);
                    if (objElement != null)
                    {

                        System.Windows.Controls.CheckBox objChk = (System.Windows.Controls.CheckBox)objElement;
                        objChk.IsChecked = cb_selelctall.IsChecked;
                    }
                }
            }
        }

        private void tb_wrenchbarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key ==Key.Enter)
            this.bt_search_Click(this,e);
        }
    }
}
