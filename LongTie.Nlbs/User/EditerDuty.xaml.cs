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
    /// Interaction logic for EditerDuty.xaml
    /// </summary>
    public partial class EditerDuty 
    {
        IUserDuty UserDuty = DataAccess.CreateUserDuty();
        IDepartment Department = DataAccess.CreateDepartment();
        ShowDutyModel _duty = new ShowDutyModel();
        bool isadd = true ;
        public EditerDuty()
        {
            InitializeComponent();
            DataBind(GetDutyModel());
            DepartmentBind();
        }

        void DepartmentBind()
        {
            List<department> dpl = Department.SelectByFlag();
            this.cb_department.ItemsSource = null;
            this.cb_department.ItemsSource = dpl;
            this.cb_department.DisplayMemberPath = "departmentName";
            this.cb_department.SelectedValuePath = "id";
        }
        List<duties> Getduty()
        {
            return UserDuty.Select();
        }
        List<ShowDutyModel> GetDutyModel()
        {
            List<ShowDutyModel> dl = new List<ShowDutyModel>();
            List<duties> ds = Getduty();
            if(ds==null||ds.Count<=0)
                return null;
            foreach (duties d in ds)
            {
                ShowDutyModel sdm = new ShowDutyModel();
                sdm.department = d.department;
                sdm.dutiesName = d.dutiesName;
                sdm.guid = d.guid;
                sdm.id = d.id;
                sdm.departmentName = Department.SelectByGuid(d.department).departmentName;
                dl.Add(sdm);
            }
            return dl;
        }
        void DataBind(List<ShowDutyModel> dl)
        {
            this.dt_showdata.ItemsSource = null;
            this.dt_showdata.ItemsSource = dl;
        }
        void showdata(ShowDutyModel d) 
        {
            foreach (department dp in cb_department.ItemsSource as List<department>)
            {
                if (dp!=null&&(dp.guid == d.department))
                {
                    this.cb_department.SelectedItem = dp;
                }
            }
            this.tb_dutyname.Text = d.dutiesName;
            this.tb_comm.Text = d.comment;
        }
        duties GetShowduty(duties d)
        {
            d.department =(this.cb_department .SelectedItem as department).guid;
            d.dutiesName = this.tb_dutyname.Text.Trim();
            d.comment = this.tb_comm.Text.Trim();
            return d;
        }
        void clear() 
        {
            this.tb_dutyname.Clear();
            _duty = null;
            isadd = true;
       }
        bool IsExit()
        {
            List<duties> duty = UserDuty.SelectByName(this.tb_dutyname.Text.Trim ());
            if (duty != null &&duty.Count > 0)
            {
                MessageAlert.Alert("该职位名称已经存在！");
                return true; 
            }
            return false;
        }
        bool IsRepeat(duties d)
        {
            List<duties> duty = UserDuty.SelectByName(this.tb_dutyname.Text.Trim());
            foreach (duties dy in duty)
            {
                if (dy != null && dy.guid != d.guid)
                {
                    MessageAlert.Alert("该职位名称已经存在！");
                    return true; 
                }
            }

            return false;
        }
        bool save(duties d)
        {
            //List<duties> dl = UserDuty.SelectByName(d.dutiesName);
            //List<department> dpl = Department.Select();
            //if (dpl.Count < 0) 
            //{
            //    MessageAlert.Warning("当前系统还没有任何部门信息请先添加部门!");
            //    return false;
            //}
            //if (dl.Count > 0) { return false; }
            //d.department = dpl.First().id.ToString ();
            return UserDuty.Add(d);
        }

        bool Validate()
        {
            if (cb_department.SelectedIndex < 0)
            {
                MessageAlert.Warning("请选择所属部门！");
                return false;
            }

            if (this.tb_dutyname.Text.Trim() == "")
            {
                MessageAlert.Warning("请填写信息！");
                return false ;
            }
            return true;
        }

        private void bt_editer_Click(object sender, RoutedEventArgs e)
        {
            if (this.dt_showdata.SelectedIndex < 0)
                return;
            _duty = this.dt_showdata.SelectedItem as ShowDutyModel;
            showdata(_duty);
            isadd = false;
        }

       
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate())
                return;
            try
            {
                if (isadd)
                {
                    duties d = new duties();
                    d.dutiesName = this.tb_dutyname.Text.Trim();
                    d.comment = this.tb_comm.Text.Trim();
                    d.guid = Guid.NewGuid().ToString();
                    d.department = this.cb_department.SelectedValue.ToString ();
                    //if (IsExit())
                    //    return;
                    if (save(d))
                    {
                        MessageAlert.Alert("保存成功！");
                        clear();
                        DataBind(GetDutyModel());
                        return;
                    }
                }
                else
                {
                    duties duty = new duties();
                    duty.department = _duty.department;
                    duty.dutiesName = _duty.dutiesName;
                    duty.comment = _duty.comment;
                    duty.guid = _duty.guid;
                    duty.id = _duty.id;
                    duty = GetShowduty(duty);
                    //if (IsRepeat(duty))
                    //    return;
                    if (UserDuty.Update(GetShowduty(duty)))
                    {
                        MessageAlert.Alert("保存成功！");
                        clear();
                        DataBind(GetDutyModel());
                        return;
                    }
                }
                MessageAlert.Warning("保存失败！");
            }
            catch { MessageAlert.Alert("保存失败！"); }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            clear();

        }

        private void bt_del_Click(object sender, RoutedEventArgs e)
        {
            ShowDutyModel d = this.dt_showdata.SelectedItem as ShowDutyModel;
            if (d != null)
            {

                if (UserDuty.Del(new duties() { id = d.id }))
                {
                    MessageAlert.Alert("删除成功！");
                }
                else
                {
                    MessageAlert.Alert("该记录不能删除！");
                }
                DataBind(GetDutyModel());
            }
          
        }
    }
}
