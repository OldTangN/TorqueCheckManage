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

namespace LongTie.Nlbs.User
{
    /// <summary>
    /// winDepartment.xaml 的交互逻辑
    /// </summary>
    public partial class winDepartment : Window
    {
        IDepartment Department = DataAccess.CreateDepartment();
        bool _isadd = true;
        int _parent=0;

        int _departmentid = 0;
        public winDepartment(int parentid,int depmentid,bool isadd=false )
        {
            InitializeComponent();
            _isadd = isadd;
            _parent = parentid;
            _departmentid = depmentid;
            if (!isadd)
            {
                show(_departmentid);
            }
        }

        void show(int id)
        {
          department dl = Department.SelectById(id.ToString ());
          this.tb_comm.Text = dl.comment;
          this.tb_name.Text = dl.departmentName;
        }
        bool  Add(department d)
        {
          return   Department.Add(d);
        }

        bool Update(department d)
        {
            return Department.Update(d);
        }

        bool IsEmpty()
        {
            if (string.IsNullOrEmpty(this.tb_name.Text.Trim()))
            {
                MessageAlert.Alert("部门名称不能为空！");
                return true ;
            }
            return false;
        }
        bool IsExist()
        {
            List<department> dl = Department.Select(this.tb_name .Text .Trim ());
            if (dl == null || dl.Count <= 0)
                return false;          
            MessageAlert.Alert("部门已经存在！不能添加");
            return true;
        }
        bool IsRepeat(department d)
        {
            List<department> dl = Department.Select(this.tb_name.Text.Trim());
            foreach (department dp in dl)
            {
                if (d.guid == dp.guid)
                {
                    MessageAlert.Alert("部门重复！不能添加");
                    return true;
                }
            }
            return false;
        }

        private void bt_submin_Click(object sender, RoutedEventArgs e)
        {
            if (IsEmpty())
                return;
            if (_isadd)
            {
                department d = new department();
                d.parentDepartment = Department.SelectById(_parent.ToString()) == null ? "" : Department.SelectById(_parent.ToString()).guid;
                d.delDepartment = false;
                d.comment = this.tb_comm.Text.Trim();
                d.departmentName = this.tb_name.Text.Trim();
                d.guid = Guid.NewGuid().ToString();
                if (Add(d))
                {
                    MessageAlert.Alert ("添加成功！");
                    this.Close();
                    return;
                }
                MessageAlert.Alert("添加失败！");         
            }
            else
            {
                department dl = new department();
                dl= Department.SelectById(_departmentid.ToString());                      
                dl.parentDepartment = Department.SelectById(_parent.ToString()) == null ? "" : Department.SelectById(_parent.ToString()).guid;
               dl.delDepartment = false;
               dl.comment = this.tb_comm.Text.Trim();
               dl.departmentName = this.tb_name.Text.Trim();
               if (Update(dl))
               {
                   MessageAlert.Alert("修改成功！");
                   this.Close();
                   return;
               }
               MessageAlert.Alert("修改失败！");
            }
        }
        private void bt_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.tb_name.Clear();
            this.tb_comm.Clear();
            this.Close();
        }
    }
}
