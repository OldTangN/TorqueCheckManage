using QDDL.Comm;
using QDDL.DAL;
using QDDL.DAL.MySql;
using QDDL.Model;
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

namespace QDDL.Nlbs.Wrench
{
    /// <summary>
    /// Interaction logic for WrenchRepair.xaml
    /// </summary>
    public partial class WrenchRepair
    {
        IWrench Wrench = DataAccess.CreateWrench();
        IWrenchSpecies WrenchSpecies = DataAccess.CreateWrenchSpecies();
        List<ToolModel> _toolmodellist = new List<ToolModel>();
        List<WrenchRepairBind> wrenchRepairList = new List<WrenchRepairBind>();
        public WrenchRepair()
        {
            InitializeComponent();
            databind(getspecies ());
        }

        List<wrenchspecies> getspecies()
        {
            return WrenchSpecies.select();
        }
        void databind(List<wrenchspecies> wl)
        {
            this.cb_species.ItemsSource = null;
            wl.Insert(0,new wrenchspecies());
            this.cb_species.ItemsSource = wl;
            this.cb_species.DisplayMemberPath = "speciesName";
            this.cb_species.SelectedValuePath = "guid";       
        }
        void GetBycontion(Dictionary<string, string> contion)
        {       
            List<wrench> wrenchs = Wrench.selectByContion(contion);
            if (wrenchs==null||wrenchs.Count < 0)
            {
                MessageAlert.Alert("没有该工具信息！");
                return;
            }
            foreach (wrench w in wrenchs)
            {              
                if (wrenchRepairList.FindIndex(p=>p.wrenchBarCode ==w.wrenchBarCode)<0)
                {
                    ToolModel e = GetToolModel(w);
                    wrenchRepairList.Add
                        (
                        new WrenchRepairBind() 
                        {
                        wrenchCode=e.wrenchCode,
                        wrenchBarCode =e.wrenchBarCode,
                        wrenchParentName=e.speciesName,
                        isCheck=true,
                        lastRepair =e.lastrepair,
                        days=e.cycletime.ToString ("f0"),
                        isNeedCheck=e.isallowcheck,
                        guid=e.guid
                        }
                        );
                   // _toolmodellist.Add(e);
                }
            }
            return;      
        }
        ToolModel GetToolModel(wrench w)
        {
            wrenchspecies ws = WrenchSpecies.selectByGuid(w.species);
              return new ToolModel()
            {
                wrenchBarCode = w.wrenchBarCode,
                wrenchCode = w.wrenchCode,
                speciesName = ws.speciesName,
                lastrepair = w.lastrepair,
                cycletime =Convert .ToDecimal (w.cycletime.ToString ("f0")),
                isallowcheck = w.isallowcheck,
                guid = w.guid
            };
        }

        void dgbind(List<WrenchRepairBind> lw)
        {

            this.dt_showdata.ItemsSource = null;
            this.dt_showdata.ItemsSource = lw;
        }


        private void bt_submit_Click(object sender, RoutedEventArgs e)
        {
            if (this.tb_cycletime.Text.Trim() == "")
            {
                MessageAlert.Warning("请填写维护周期！");
                return;
            }

            //if (this.sp_SelectDate.SelectedDate == null)
            //{
            //    MessageAlert.Warning("请选择维护时间！");
            //    return;
            //}
            decimal day = 0;
            try
            {
                 day = Convert.ToDecimal(this.tb_cycletime.Text.Trim());
                //TimeSpan ts = (this.sp_SelectDate.SelectedDate == null ? Convert.ToDateTime (DateTime.Now.ToShortDateString()): Convert.ToDateTime(this.sp_SelectDate.SelectedDate)).Subtract(Convert.ToDateTime ( DateTime.Now.ToShortDateString()));
                //day = Convert.ToDecimal(ts.Days);
                if (day < 0)
                {
                    MessageAlert.Warning("请输入到期维护时间有误！"); return;
                }

            }
            catch { MessageAlert.Warning("请输入正确的数字！"); return; }

            if (day >= 0)
            {
                //List<ToolModel> lm = new List<ToolModel>();
                //lm = _toolmodellist;
                if (updata(this.rb_yes.IsChecked == true ? true : false, day, wrenchRepairList))
                {
                    MessageAlert.Alert("保存成功！");
                }
                else
                {
                    MessageAlert.Alert("保存失败！");
                    return;
                }
            }
            else
            {
                MessageAlert.Alert("维护周期必须为正数！");
                return;
            }
            List<WrenchRepairBind> wrbl = new List<WrenchRepairBind>();

            foreach (WrenchRepairBind wrb in wrenchRepairList)
            {
                wrench w = Wrench.selectByBarcode(wrb.wrenchBarCode);
                if (w != null && w.guid != null)
                {
                    //wrbl.Add(GetToolModel(w));
                    ToolModel em = GetToolModel(w);
                    wrbl.Add
                        (
                        new WrenchRepairBind()
                        {
                            wrenchCode = em.wrenchCode,
                            wrenchBarCode = em.wrenchBarCode,
                            wrenchParentName = em.speciesName,
                            isCheck = true,
                            lastRepair = em.lastrepair,
                            days = em.cycletime.ToString("f0"),
                            isNeedCheck = em.isallowcheck,
                            guid = em.guid
                        }
                        );
                }
            }
            wrenchRepairList.Clear();
            wrenchRepairList = wrbl;

            //List<ToolModel> tms = new List<ToolModel>();
            //foreach (ToolModel tm in _toolmodellist)
            //{
            //    wrench w = Wrench.selectByBarcode(tm.wrenchBarCode);
            //    if (w != null && w.guid != null)
            //    {
            //       tms.Add (GetToolModel(w));
            //    }
            //}
            //_toolmodellist.Clear();

            //_toolmodellist = tms;
            dgbind(wrenchRepairList);

        }
        private void bt_clear_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < wrenchRepairList.Count(); i++)
            {
                if (wrenchRepairList[i].isCheck)
                {
                    wrenchRepairList.RemoveAt(i);
                    i = 0;
                }
            }               
            dgbind(wrenchRepairList);
           
        }

        bool updata(bool isallow,decimal days, List<WrenchRepairBind>tl)
        {
           // decimal days = 0;
          
            foreach (WrenchRepairBind m in tl)
            {
                if (m != null && m.guid != null)
                {
                    wrench s = Wrench.selectByguid(m.guid);
                    if (s != null)
                    {
                        
                        //TimeSpan ts = (this.sp_SelectDate.SelectedDate == null ? Convert.ToDateTime(DateTime.Now.ToShortDateString()) : Convert.ToDateTime(this.sp_SelectDate.SelectedDate)).Subtract(Convert.ToDateTime(s.lastrepair.ToString ("yyyy-MM-dd")));
                        //days = Convert.ToDecimal(ts.Days);
                        s.cycletime = days;
                        s.isallowcheck = isallow;
                        Wrench.updata(s);
                    }
                
                }
            
            }
            return true;
        }
        void SetRadioButton(bool setvalue=true)
        {
            this.rb_yes.IsChecked  = setvalue;
            this.rb_not.IsChecked  = !setvalue;
        }
        void  updata() 
        {
            try
            {
                for (int i = 0; i <=dt_showdata.Items.Count; i++)
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
                                //if (tm == null)
                                //    continue;
                               // _toolmodellist.Remove(tm);
                                int index = _toolmodellist.FindIndex(p => p.wrenchBarCode == tm.wrenchBarCode);
                                _toolmodellist.RemoveAt(index);
                            }                         
                        }
                    }
                }
               // dgbind(_toolmodellist);
             
            }
            catch 
            {
                return ; 
            }
        }

        private void bt_search_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Dictionary<string, string> dt = new Dictionary<string, string>();
                if (cb_species.SelectedItem != null)
                {
                    dt.Add("species_id", (this.cb_species.SelectedItem as wrenchspecies).guid);
                }
                if (this.tb_wrenchbarcode.Text.Trim() != "")
                {
                    dt.Add("wrenchBarCode", this.tb_wrenchbarcode.Text.Trim());
                }
                GetBycontion(dt);
                dgbind(wrenchRepairList);
                //if (wrenchRepairList == null || wrenchRepairList.Count <= 0)
                //{
                //    MessageAlert.Alert("扳手信息不存在！");
                //}
            }
            catch(Exception ex)
            {
                throw ex;
            }
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
                      //  objChk.IsChecked = cb_selelctall.IsChecked;
                    }
                    else
                    {
                        Console.WriteLine("ss");
                    }
                }
                else
                {
                    Console.WriteLine("sss");
                }
            }

        }
        private void rb_not_Click(object sender, RoutedEventArgs e)
        {
            SetRadioButton(false);
        }

        private void rb_yes_Click(object sender, RoutedEventArgs e)
        {
            SetRadioButton(true );
        }
        private void tb_wrenchbarcodeKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                    this.bt_search_Click(this, e);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private void chekall_Click(object sender, RoutedEventArgs e)
        {
            uncheckall.IsChecked = false;
            chekall.IsChecked = true;
            foreach (WrenchRepairBind wrb in wrenchRepairList)
            {
                wrb.isCheck = true;
            }

            dgbind(wrenchRepairList);
        }

        private void uncheckall_Click(object sender, RoutedEventArgs e)
        {
            chekall.IsChecked = false;
            uncheckall.IsChecked = true;
            foreach (WrenchRepairBind wrb in wrenchRepairList)
            {
                wrb.isCheck = false;
            }

            dgbind(wrenchRepairList);
        }
    }

    public class WrenchRepairBind
    {
        public string wrenchCode { get; set; }
        public string wrenchBarCode { get; set; }
        public string wrenchParentName { get; set; }
        public DateTime lastRepair { get; set; }
        public string days { get; set; }
        public bool isNeedCheck { get; set; }
        public bool isCheck { get; set; }
        public string guid { get; set; }


    }
}
