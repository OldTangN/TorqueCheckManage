using QDDL.Comm;
using QDDL.DAL;
using QDDL.DAL.MySql;
using QDDL.DAL.Service;
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

namespace QDDL.Nlbs.User
{
    /// <summary>
    /// Interaction logic for EditerDepartment.xaml
    /// </summary>
    public partial class EditerDepartment 
    {
        IDepartment _department = DataAccess.CreateDepartment();
        List<QDDL.Model.department> _departmentlist = new List<QDDL.Model.department>();
        department _depart=new department() ;
        bool isadd = true  ;


        //public   EditerDepartment(department depart)
        //{
        //    InitializeComponent();
        //    _depart = depart;
        //    getparentdeplist();
        //    showdepart();
        //    MessageAlert.Alert("未修改！");
       
        //}
    public  EditerDepartment()
        {
            InitializeComponent();
            getparentdeplist();                      
        }
        private void getparentdeplist()
        {
            try
            {
                this.dataGrid1.ItemsSource = _department.Select();
                //_departmentlist = _department.Select();
               
                //_departmentlist.Insert(0, new department());
                //this.cb_parentdep.ItemsSource = _departmentlist;
                //this.cb_parentdep.SelectedIndex = -1;
                //this.cb_parentdep.DisplayMemberPath = "departmentName";
                //this.cb_parentdep.SelectedValuePath = "id";
            }
            catch { }
        }

        private void showdepart() {
            try
            {
                for (int i = 0; i < cb_parentdep.Items.Count; i++)
                {
                    department  r = cb_parentdep.Items[i] as department ;
                    if (r != null && r.guid != null) {
                        if (r.guid == _depart.parentDepartment)
                        {
                            cb_parentdep.SelectedItem = r;
                            break;
                        }
                    }
                 
                }
                this.tbox_depart.Text = _depart.departmentName;
            }
            catch { }
        }


        private bool  addDepartment(department dp)
        {
            try
            {
                dp.departmentName = this.tbox_depart.Text.Trim();
                department d = cb_parentdep.SelectedItem as department;
                if (d==null||d.guid == null)
                    dp.parentDepartment = "";
                else
                    dp.parentDepartment = d.guid;
                dp.guid = Guid.NewGuid().ToString();
                return _department.Add(dp);
            }
            catch { return false; }
        }

        private bool saveas(department d)
        {
            try
            {
                d.departmentName = this.tbox_depart.Text.Trim();
                department dt = cb_parentdep.SelectedItem as department;
                if (dt== null || dt.guid == null)
                    d.parentDepartment = "";
                else
                    d.parentDepartment = d.guid;
                return _department.Update (d);
            }
            catch { return false; }
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbox_depart.Text.Trim()))
            { MessageBox.Show("请填写部门名称"); return; }
            if (isadd)
            {
                department d = new department();
                if (addDepartment(d))
                {
                    MessageBox.Show("添加成功！");
                    this.dataGrid1.ItemsSource = null;
                    this.cb_parentdep.ItemsSource = null;
                    getparentdeplist();
                    clear();
                    return;
                }


            }
            else
            {
                department d = new department();
                    d=_depart;
                if (saveas(d))
                {
                    MessageBox.Show("修改成功！");
                    isadd = true;
                    this.dataGrid1.ItemsSource = null;
                    this.cb_parentdep.ItemsSource = null;
                    getparentdeplist();
                    clear();
                    return;
          
                }
            }
            MessageAlert.Alert("保存失败！");
  
         
           
        }

        private void editButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.dataGrid1.SelectedIndex < 0)
                return;
            _depart = this.dataGrid1.SelectedItem as department ;
            showdepart();
            isadd = false;
        }

        void clear() {
            _depart = null;
            this.tbox_depart.Clear();
            this.cb_parentdep.SelectedIndex =0;
            isadd = true;
           
        }
        private void bt_reset_Click(object sender, RoutedEventArgs e)
        {
            clear();
        }
    }
}
