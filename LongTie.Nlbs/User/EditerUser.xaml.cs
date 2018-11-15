using LT.BLL;
using LT.BLL.Check;
using LT.BLL.User;
using LT.Comm;
using LT.DAL;
using LT.DAL.MySql;
using LT.Model;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace LongTie.Nlbs.User
{
    /// <summary>
    /// Interaction logic for EditerUser.xaml
    /// </summary>
    public partial class EditerUser
    {
      
        users _user = new users();
       public  bool _disposed;
       IDepartment Department = DataAccess.CreateDepartment();
       IUserRole Uerrole = DataAccess.CreateUserRole();
        IUser User = DataAccess.CreateUser();
        IUserToRole UserToRole=DataAccess.CreateUserToRole();
        IUserDuty UserDuty = DataAccess.CreateUserDuty();
       
        UserEditer userediter = new UserEditer();
        FilterData filterdata = new FilterData();
        ReadUserCard ruc = null;
        bool isadd = true;
        string backcard = "";
       // System.Timers.Timer aTimer = null;
        private delegate void TimerDispatcherDelegate();
        public EditerUser(ReadUserCard r)
        {
            InitializeComponent();
            userediter.BindDepartment(this.cbox_department);
            userediter.BindRole(this.cbox_role);
            userediter.BindDuty(this .cb_duty);
            isadd = true;
          
            ruc = r;
            ruc.HandDataBack += new ReadUserCard.DeleteDataBack(BackCardID);
            //aTimer = new System.Timers.Timer(1000);
            //aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            //aTimer.Interval = 10;
            //aTimer.Enabled = true;
        }
        //void OnTimedEvent(object serder, EventArgs e)
        //{
        //    //this.Dispatcher.Invoke(DispatcherPriority.Normal,
        //    //    new TimerDispatcherDelegate(UpdateCardInfo));
        //}
        void BackCardID(object sender, EventArgs e)
        {
            ReadUserCard ruc = (ReadUserCard)sender;
            backcard = ruc.BackString();
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.ApplicationIdle, new Action(UpdateCardInfo));
        }
        void UpdateCardInfo()
        {
            try
            {               
              
                    filterdata.CardId = backcard;
                    filterdata.resetCard();
                string cardid = filterdata.getcardid();
                if (cardid == "")
                    return;             
                filterdata.resetid("");
                this.tb_cardid.Text = cardid;
            }
            catch { }

        }

        void ShowUser(users user)
        {
            tbox_UserName.Text = user.username;
            tbox_EmpId.Text = user.empID;
            LT.Model.UserToRoleModel.usertorole usertorolemodel = UserToRole.selectbyuserid(user.guid).FirstOrDefault();
            if (usertorolemodel != null)
            {
                for (int i = 0; i < cbox_role.Items.Count; i++)
                {
                    role r = cbox_role.Items[i] as role;
                    if (r.guid == usertorolemodel.role)
                    {
                        cbox_role.SelectedItem = r;
                        break;
                    }
                }
            }

            for (int j = 0; j < cbox_department.Items.Count; j++)
            {
                department d = cbox_department.Items[j] as department;
                if (d.guid == user.department)
                {
                    cbox_department.SelectedItem = d;
                    break;
                }
            }

            for (int k = 0; k < cb_duty.Items.Count; k++)
            {

                duties d = cb_duty.Items[k] as duties;
                if (d.guid == user.duties)
                {
                    cb_duty.SelectedItem = d;
                    break;
                }
            }
            tb_cardid.Text = user.cardID;
            tb_pwd.Text = user.password;
            tb_phone.Text = user.phoneNumber;
            this.dp_jointime.Text = user.joinDate.ToString ();
        }

        users  GetUser(users user)
        {
            user.username = tbox_UserName.Text.Trim();
            user.empID = tbox_EmpId.Text.Trim();
            user.cardID = tb_cardid.Text.Trim();
            user.is_staff = true;
            if (user.password != tb_pwd.Text.Trim())
            {
                user.password =MD5Encrypt .GetMD5 (tb_pwd.Text.Trim());
            }
           
            user.phoneNumber = tb_phone.Text.Trim();
            user.joinDate =Convert.ToDateTime ( Convert .ToDateTime (this.dp_jointime .Text .Trim ()).ToString ("yyyy-MM-dd HH:mm:ss"));
            if (cb_duty.SelectedIndex >= 0)
            {
                user.duties = cb_duty.SelectedItem == null ? "" : (cb_duty.SelectedItem as duties).guid.ToString();
            }
            user.department = (cbox_department.SelectedItem as department).guid;
         
            user.is_superuser = false;
            return user;
        }

        bool IsEmpty()
        {
            if (
                string.IsNullOrEmpty(this.tbox_UserName.Text.Trim())||string.IsNullOrEmpty(this.tb_pwd.Text.Trim())||
                string.IsNullOrEmpty(this.tbox_EmpId.Text.Trim()) || string.IsNullOrEmpty(this.tb_cardid .Text.Trim())||
               (this.cb_duty.SelectedIndex <0)||(this.cbox_department .SelectedIndex <0)||(this.cbox_role .SelectedIndex <0)
                )
            {
                MessageAlert.Alert("*为必填信息！");
                return true;
            }

            try
            {
                Convert.ToDateTime(this.dp_jointime.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch
            {
                MessageAlert.Alert("请填写入职时间！");
                return true ;
            }

                return false;
        }

        /// <summary>
        /// 判断员工信息是否存在
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        users  IsExit(users user)
        {           
            return  User.Select(user.cardID);
          
        }
        LT.Model.UserToRoleModel.usertorole IsUserToRoleExit(users user)
        {
            LT.Model.UserToRoleModel.usertorole utr= null;
            List <LT.Model.UserToRoleModel.usertorole> utrlist=UserToRole.selectbyuserid(user.guid);
            if (utrlist.Count > 0)
             return utrlist.First();
            return utr;
        }

        /// <summary>
        /// 判断重复
        /// </summary>
        /// <param name="user"></param>
        /// <param name="count">添加是count=0,修改时count=1</param>
        /// <returns></returns>
        bool IsRepeat(users user,int count=0)
        {
            List<users> ul = User.SelectByName(this.tbox_UserName.Text.Trim());
            if (ul!=null&&ul.Count > count)
            {
                MessageBox.Show("该名字已存在！\n请重新输入");
                return true;
            }
            if (User.SelectByCode(user.empID)!=null&&User.SelectByCode(user.empID).Count > count)
            {
                MessageBox.Show("该员工编号已经存在！\n请重新输入编号");
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断跟新是否重复
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        bool IsUpdataRepeat(users user)
        {
            List<users> ul = User.SelectByName(this.tbox_UserName .Text .Trim ());
            if (ul != null)
            { 
            foreach (users u in ul)
            {
                if (u != null && u.guid != user.guid)
                {
                    MessageBox.Show("该名字已存在！\n请重新输入");
                    return true;
                }
            }
            }
            ul = User.SelectByCode(user.empID);
            if (ul != null)
            {
                foreach (users u in ul)
                {
                    if (u != null && u.guid != user.guid)
                    {
                        MessageBox.Show("该员工编号已经存在！\n请重新输入编号");
                        return true;
                    }
                }
            }

            ul = User.SelectByContion(new Dictionary<string, string>{{"cardID",user.cardID}});
            if (ul != null)
            {
                foreach (users u in ul)
                {
                    if (u != null && u.guid != user.guid)
                    {
                        MessageBox.Show("该员工卡号已经存在！\n请重新输入编号");
                        return true;
                    }
                }
            }
            return false;
        }

        string   SaveUser(users user)
        {
            return User.addreturnid(user);
        }

        bool SaveUserToRole(LT.Model.UserToRoleModel.usertorole usertorole)
        {
            return UserToRole.add(usertorole);
        }

        bool UpdateUser(users user)
        {
            return User.Update(user);
        }
        bool UpDateUsertoRol(LT.Model.UserToRoleModel.usertorole usertorole)
        {

            return UserToRole.update(usertorole);
        }

        void SetEmpty()
        {
            _user = null;
            isadd = true;
            this.tb_cardid.Clear();
            this.tb_phone.Clear();
            this.tb_pwd.Text ="123456";
            this.tbox_EmpId.Clear();
            this.tbox_UserName.Clear();
            this.cb_duty.SelectedIndex = -1;
            this.cbox_department.SelectedIndex = -1;
            this.cbox_role.SelectedIndex =-1;
            this.dp_jointime.Text = DateTime.Now.ToShortDateString();
        }

        private void button_submit_Click(object sender, RoutedEventArgs e)
        {
            if (IsEmpty())
            {
                return;
            }

            try
            {
                if (isadd)
                {
                    users u = new users();
                    u = GetUser(u);
                    u.department = Department.SelectByGuid(u.department).id.ToString();
                    u.duties = UserDuty.SelectByGuid(u.duties).id.ToString();
                    role r = this.cbox_role.SelectedItem as role;
                    if (IsExit(u) != null)
                    {
                        if (MessageAlert.Alter("信息已经存在！\n是否跟新"))
                        {
                            u = GetUser(IsExit(u));
                            if (IsUpdataRepeat(u))
                                return;
                            UpdateUser(u);
                            if (IsUserToRoleExit(u) == null)
                            {
                                SaveUserToRole(new LT.Model.UserToRoleModel.usertorole() { role = r.id.ToString(), user = u.id.ToString() });
                            }
                            else
                            {
                                UpDateUsertoRol(new LT.Model.UserToRoleModel.usertorole() { role = r.guid, user = u.guid });
                            }
                        }
                        else
                        { return; }
                    }
                    else
                    {
                        u.guid = Guid.NewGuid().ToString();
                        if (IsRepeat(u, 0))
                            return;
                        string id = SaveUser(u);                       
                        if (id != "-1")
                        {
                            SaveUserToRole(new LT.Model.UserToRoleModel.usertorole() { role = r.id.ToString(), user = id });
                        }
                    }

                }
                else
                {
                    users u = new users();
                        u=_user;
                    u = GetUser(u);
                    role r = this.cbox_role.SelectedItem as role;
                    if (IsUpdataRepeat(u))
                        return;
                    UpdateUser(u);
                    UpDateUsertoRol(new LT.Model.UserToRoleModel.usertorole() { role = r.guid, user = u.guid });

                }
                MessageAlert.Alert("操作成功！");
                SetEmpty();
                userediter.BindUserModel(this.dataGrid1);
            }
            catch 
            { 
                MessageAlert.Alert("添加失败！出现异常请联系管理员"); 
            }

        }


  

      
        private void editButtonClick(object sender, RoutedEventArgs e)
        {
            int i_index = dataGrid1.SelectedIndex;
            if (i_index >= 0)
            {
               _user = dataGrid1.SelectedItem as users;
               ShowUser(_user);
               isadd = false;
            }
        }

        #region

        //private void showuer()
        //{
        //    tbox_UserName.Text = _user.username;
        //    tbox_EmpId.Text = _user.empID;
        //    LT.Model.UserToRoleModel.usertorole usertorolemodel = UserToRole.selectbyuserid(_user.guid).FirstOrDefault();
        //    if (usertorolemodel != null)
        //    {
        //        for (int i = 0; i < cbox_role.Items.Count; i++)
        //        {
        //            role r = cbox_role.Items[i] as role;
        //            if (r.guid == usertorolemodel.role)
        //            {
        //                cbox_role.SelectedItem = r;
        //                break;
        //            }
        //        }
        //    }

        //    for (int j = 0; j < cbox_department.Items.Count; j++)
        //    {
        //        department d = cbox_department.Items[j] as department;
        //        if (d.guid == _user.department)
        //        {
        //            cbox_department.SelectedItem = d;
        //            break;
        //        }
        //    }

        //    for (int k = 0; k < cb_duty.Items.Count; k++)
        //    {

        //        duties d = cb_duty.Items[k] as duties;
        //        if (d.guid == _user.duties)
        //        {
        //            cb_duty.SelectedItem = d;
        //            break;
        //        }
        //    }
        //    tb_cardid.Text = _user.cardID;
        //    tb_pwd.Text = _user.password;
        //    tb_phone.Text = _user.phoneNumber;
        //}
        //private void getuser() {
        //    _user.username = tbox_UserName.Text.Trim();
        //    _user.empID = tbox_EmpId.Text.Trim();
        //    _user.cardID = tb_cardid.Text.Trim();
        //    _user.is_staff = true;
        //    _user.password = tb_pwd.Text.Trim();
        //    _user.phoneNumber = tb_phone.Text.Trim();
        //    _user.joinDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
        //    if (cb_duty.SelectedIndex >= 0)
        //    {
        //        _user.duties = cb_duty.SelectedItem == null ? "" : (cb_duty.SelectedItem as duties).guid.ToString();
        //    }
        //    _user.department = (cbox_department.SelectedItem as department).guid;
        //    _user.is_superuser = false; 
        //}
        //private bool  save()
        //{
        //    try {

        //        if (cb_duty.SelectedIndex >= 0)
        //        {
        //            _user.duties = cb_duty.SelectedItem == null ? "" : (cb_duty.SelectedItem as duties).id.ToString();
        //        }
        //        _user.department = (cbox_department.SelectedItem as department).id.ToString ();
        //     _user.guid = Guid.NewGuid().ToString();      
                    
        //     string id= User.addreturnid (_user );
        //      if (id != "-1")
        //      if (!UserToRole.add(new LT.Model.UserToRoleModel.usertorole() { role = (cbox_role.SelectedItem as role).id.ToString(), user = id }))
        //      {
        //          User.Delete(_user);
        //          return false;
        //      }
        //  return true;
        //    }
        //    catch { return false; }
         
        //}
        //private bool updata() 
        //{                        
        //  List <LT.Model.UserToRoleModel.usertorole> ur = UserToRole.selectbyuserid(_user.guid);
        //  if (ur.Count < 0)
        //  {
        //      UserToRole.add(new LT.Model.UserToRoleModel.usertorole() { user = _user.id.ToString(), role = (cbox_role.SelectedItem as role).id.ToString() });
        //  }
        //  else 
        //  {
        //      if (UserToRole.update(new LT.Model.UserToRoleModel.usertorole() { user = _user.guid, role = (cbox_role.SelectedItem as role).guid }))
        //      {
        //          return User.Update(_user );
        //      }
        //  }
        //  return false;
      

        //}
        //private void button_submit_Click(object sender, RoutedEventArgs e)
        //{
           
        //    if (cbox_role.SelectedItem == null || cbox_department.SelectedItem == null)
        //    { MessageBox.Show("请选择角色信息或部门信息"); return; }
        //     getuser();
        //    if (isadd)
        //    {
        //        List <users > u = User.SelectByCBcode(_user.cardID ,_user.empID);
        //        if (u != null && u.Count > 0) 
        //        {
        //            if (updata())
        //            {
        //                MessageAlert.Alert("保存成功！");
        //                return;
        //            }
        //        }
        //        if (isexit(_user))
        //        {
        //            if (save())
        //            {
        //                MessageAlert.Alert("保存成功！");
        //                isadd = true;
        //                _user = new users();
        //                showuer();
        //                userediter.BindUserModel(this.dataGrid1);
        //                return;
        //            }
        //        }
                              
        //    }
        //    else {
        //        if (updata())
        //        {

        //            MessageAlert.Alert("保存成功！");
        //            isadd = true;
        //            _user = new users();
        //            showuer();
        //            userediter.BindUserModel(dataGrid1 );
        //            return;
        //        }
        //        else {
        //            isadd = true;
        //            _user = new users();
        //            showuer();
                 
        //        }
        //    }
        //    MessageBox.Show("操作失败！");

        //}

        //private bool isexit(users user) {
        //    if (user == null)
        //        return false;
            
        //        if (User.SelectByName(user.username).Count > 0)                 
        //        {
        //            MessageBox.Show("该名字已经存在,不能重复保存！\n请重新输入名字");
        //            return false;
        //        }
        //        if (User.Select(user.cardID) != null) { MessageBox.Show("员工卡号已经存在，不能重复"); return false; }
        //        if (User.SelectByCode(user.empID).Count > 0)
        //        {
        //            MessageBox.Show("该员工编号已经存在,不能重复！\n请重新输入编号");
        //            return false;
        //        }
        //        return true;


        //}
        #endregion
        private void bt_reset_Click(object sender, RoutedEventArgs e)
        {
            SetEmpty();
        }

        private void delbuttonclick(object sender, RoutedEventArgs e)
        {
            if (this.dataGrid1.SelectedIndex < 0)
            {
                return;
            }


            UserModel um = this.dataGrid1.SelectedItem as UserModel;
            if (um == null || um.guid == null)
                return;
            List<LT.Model.UserToRoleModel.usertorole> ut = new List<UserToRoleModel.usertorole>();
            ut = UserToRole.selectbyroleid(um.roleID, um.guid);
            if (ut.Count > 0)
            {
                if (!MessageAlert.Alter("是否删除该人员信息"))
                    return;
                if (UserToRole.delete(ut.FirstOrDefault ()))
                {
                    userediter.BindUserModel(dataGrid1);
                    MessageAlert.Alert("删除成功！");
                }
            }
         
        }

        private void cbox_department_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbox_department.SelectedIndex >= 0)
            {
                department d = this.cbox_department.SelectedItem as department;
                if (d != null)
                {
                    userediter.BindDuty(this.cb_duty,d.guid);
                }
            }
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            userediter.BindUserModel(this.dataGrid1);
           // dataGrid1.ItemsSource = null;
           // GetUser gu = new GetUser();
           // List<UserModel> um = new List<UserModel>();
           // um = gu.GetUserModel();
           //// dg.ItemsSource = um;
           // dataGrid1.DataContext = um;   
        }

        
    }
}
