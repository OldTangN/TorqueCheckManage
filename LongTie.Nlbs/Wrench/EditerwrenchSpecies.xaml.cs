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
    /// Interaction logic for EditerwrenchSpecies.xaml
    /// </summary>
    public partial class EditerwrenchSpecies 
    {
        IWrenchSpecies WrenchSpecies = DataAccess.CreateWrenchSpecies();
        bool isadd = true;
        wrenchspecies _ws = null;

        public EditerwrenchSpecies()
        {
            InitializeComponent();
            getspecies();
            getparentspecies();
        }
        void getparentspecies() {
            List<wrenchspecies> ws = new List<wrenchspecies>();
            ws = WrenchSpecies.select();
            ws.Insert(0,new wrenchspecies());
            this.cb_spparent.ItemsSource = ws;
            this.cb_spparent.DisplayMemberPath = "speciesName";
            this.cb_spparent.SelectedValuePath = "guid";
        }
        void getspecies()
        {
            this.dataGrid1.ItemsSource = null;
            List<wrenchspecies> wsl = new List<wrenchspecies>();
            wsl = WrenchSpecies.select();
            this.dataGrid1.ItemsSource = wsl;
        }

        void  showwrenchspecies(wrenchspecies wrenchspe)
        {
            if (wrenchspe == null) { return; }
            bool isfind = false;
            foreach (var s in cb_spparent.Items) {
                wrenchspecies wp= (wrenchspecies)s;
                if (wp != null&&wp.guid !=null) {
                    if (wp.guid == wrenchspe.parentSpecies)
                    {
                        cb_spparent.SelectedItem = wp;
                        isfind = true;
                        break;
                    }
                }
            }
            if (!isfind)
                cb_spparent.SelectedIndex = -1;
            this.tbox_spcode.Text  = wrenchspe.speciesCode;
            this.tbox_spname.Text  = wrenchspe.speciesName;
            //this.tbox_common .Text =wrenchspe .sp
        }


        bool IsEmpty()
        {
            if (this.tbox_spname.Text.Trim() == "")
            { MessageAlert.Warning("种类名称不能为空！"); return true ; }
            if (this.tbox_spcode.Text.Trim() == "")
            {
                MessageAlert.Warning("种类编号不能为空");
                return true;
            }
            return false;
        }
        bool IsExit(wrenchspecies w)
        {
            List<wrenchspecies> wss = WrenchSpecies.selectbyname(this.tbox_spname.Text.Trim());
            if (wss!=null&&wss.Count > 0)
            {
                return true;
            }         
            return false;
        }
        bool IsUpdataRepeat(wrenchspecies w)
        {
            List<wrenchspecies> wss = WrenchSpecies.selectbyname(this.tbox_spname.Text.Trim());
            foreach (wrenchspecies ws in wss)
            {
                if (ws != null && ws.guid != w.guid)
                {
                    MessageAlert.Alert("该种类名称已经存在！");
                    return true;
                }
            }
            return false;
        }
        wrenchspecies  getwrenchspecies(wrenchspecies wrenchspcies)
        {
             wrenchspcies.speciesCode =tbox_spcode .Text .Trim ();
            wrenchspcies .speciesName =tbox_spname .Text .Trim ();
            if (cb_spparent.SelectedIndex > 0)
                wrenchspcies.parentSpecies = cb_spparent.SelectedValue.ToString();
            else
            {
                wrenchspcies.parentSpecies = "";
            }
           return wrenchspcies;
        }

        bool  Add(wrenchspecies w) 
        {
            try {
              
                w.guid = Guid.NewGuid().ToString();
               return  WrenchSpecies.add(w);
            }
            catch { return false; }
  
        }

        bool updata(wrenchspecies w ) {
            try {
                
                    return WrenchSpecies.updata(w);             
            }
            catch { return false; }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (IsEmpty())
                return;
            try { 
            if (isadd) 
            {
                wrenchspecies w = new wrenchspecies();
                w = getwrenchspecies(w);
                if (IsExit(w))
                    return;
              if (Add(w))
              {
                  MessageAlert.Alert("保存成功！");  
              }
              else
              {
                  MessageAlert.Error("保存失败！"); 
                  return; 
              }
            }
            else
            {
                wrenchspecies w = new wrenchspecies();
                w = _ws;
                w = getwrenchspecies(w);
                if (IsUpdataRepeat(w))
                    return;
                if (updata(w))
                {
                    MessageAlert.Alert("更新成功！"); 
                } 
                else
                {
                    MessageAlert.Error("保存失败！"); 
                    return;
                }
            }
            getspecies();
            getparentspecies();
            clear();
            }
            catch { MessageAlert.Error("保存失败！"); }
        }

        private void bt_reset_Click(object sender, RoutedEventArgs e)
        {
            clear();
        }
        void clear() {
            this.tbox_spname.Clear();
            this.tbox_spcode.Clear();
            this.cb_spparent.SelectedIndex = -1;
            _ws = null;
            isadd = true;
        }


        private void editButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.dataGrid1.SelectedIndex < 0)
                return;
            _ws = dataGrid1.SelectedItem as wrenchspecies;
            showwrenchspecies(_ws);
            isadd = false;
        }

        private void delButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.dataGrid1.SelectedIndex >= 0)
            {
                wrenchspecies wp = this.dataGrid1.SelectedItem as wrenchspecies;
                if (wp != null)
                {
                    if (WrenchSpecies.Del(wp))
                    {
                        MessageAlert.Alert("记录删除成功！");
                    }
                    else
                    {
                        MessageAlert.Alert("该条记录不能删除！");
                    }
                }
                getspecies();
            }
        }
    }
}
