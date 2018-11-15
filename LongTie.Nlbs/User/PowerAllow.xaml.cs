using LT.BLL;
using LT.Comm;
using LT.Model;
using LT.Model.BllModel;
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

namespace LongTie.Nlbs.User
{
    /// <summary>
    /// Interaction logic for PowerAllow.xaml
    /// </summary>
    public partial class PowerAllow 
    {
        List<MenuItem> mll = new List<MenuItem>();
        List<string> tempstring = new List<string>();
        List<PowerList> _powerlist = new List<PowerList>();
        GetRole getRole = new GetRole();
        public PowerAllow(Main m)
        {
            InitializeComponent();
            list(m.menu);
            databind(mll);
            datarolebind(getRole.getrole());
          _powerlist = Getxml();
        }

         List<PowerList> Getxml() {
             List<PowerList> templ = new List<PowerList>();
            try { 
          templ= SerializeXML<PowerList>.Getlist();
          return templ;
            }
            catch { return templ; }
        }

        void datarolebind(List <role> rs) {
            this.cb_role.ItemsSource = null;
            this.cb_role.ItemsSource =rs;
            this.cb_role.DisplayMemberPath = "roleName";
            this.cb_role.SelectedValuePath = "roleName";
        }
       void  databind( List<MenuItem> lm){
           this.lb_function.ItemsSource = null;
           this.lb_function.ItemsSource = lm;
           this.lb_function.DisplayMemberPath = "Header";
           this.lb_function.SelectedValuePath = "Name";
        }

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

            foreach (var  mi in ml.Items)
            {
                MenuItem m = mi as MenuItem;
                    if(m==null)
                        continue ;

                if (m.Items.Count > 0)
                {
                    getlist(m);
                }
                else { mll.Add(m); }
            }

            return mll;
        }

        private void bt_add_Click(object sender, RoutedEventArgs e)
        {
            if (this.lb_function.SelectedIndex < 0)
                return;
            if(cb_role .SelectedIndex <0){
            MessageAlert .Alert ("请选择要分配的角色");
               return ;
            }
            MenuItem mt = this.lb_function.SelectedItem as MenuItem;
        
            try
            {
                if (lv_havefunctio.Items.Count > 0) {              
                    if (tempstring.FindIndex(p =>p==mt.Header.ToString ()) >= 0) { return; }
                }
                tempstring.Add(mt.Header.ToString());
                lv_havefunctio.ItemsSource = null;
                lv_havefunctio.ItemsSource = tempstring;
            }
            catch { }
        }

        private void cb_role_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.cb_role.SelectedIndex < 0||cb_role.SelectedItem ==null)
                return;
            string temp =( this.cb_role.SelectedItem as role).roleName ;
            try
            {
                _powerlist = Getxml();

                PowerList pl = _powerlist.Find(p => p.role == temp);
                this.lv_havefunctio.ItemsSource = null;
                tempstring .Clear ();
                if (pl != null) { 
                this.lv_havefunctio.ItemsSource = pl.rolepower;
                tempstring = pl.rolepower;
                }
            }
            catch { }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string tempstr = (cb_role.SelectedItem as role).roleName;
                if (_powerlist.FindIndex(p => p.role == tempstr) >= 0)
                {
                    _powerlist.RemoveAt(_powerlist.FindIndex(p => p.role == tempstr));
                }
                PowerList pl = new PowerList() { role = tempstr, rolepower = (List<string>)lv_havefunctio.ItemsSource };
                _powerlist.Add(pl);
                SerializeXML<PowerList>.SaveList(_powerlist);
                _powerlist = SerializeXML<PowerList>.Getlist();
                MessageAlert.Alert("保存完成！");
            }
            catch { MessageAlert.Alert("保存失败！"); }
        }

        private void bt_del_Click(object sender, RoutedEventArgs e)
        {
            if (this.lv_havefunctio .SelectedIndex < 0)
                return;
            if (lv_havefunctio.Items.Count > 0)
            { 
                string tempstr=this.lv_havefunctio .SelectedItem as string ;
                if ((cb_role.SelectedItem as role )!=null&&(cb_role.SelectedItem as role).roleName== "管理员")
                {
                    if (MessageAlert.Alter("是否删除管理员的该功能"))
                        tempstring.RemoveAt(tempstring.FindIndex(p => p == tempstr));
                }
                else
                {
                    tempstring.RemoveAt(tempstring.FindIndex(p => p == tempstr));
                }
          
                lv_havefunctio.ItemsSource = null;
                lv_havefunctio.ItemsSource = tempstring;
            }
        }
    }
}
