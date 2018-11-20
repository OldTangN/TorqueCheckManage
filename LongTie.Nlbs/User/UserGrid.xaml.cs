using LT.BLL;
using LT.BLL.Check;
using LT.BLL.ICCard;
using LT.BLL.User;
using LT.Comm;
using LT.DAL;
using LT.DAL.MySql;
using LT.Model;
using Manager;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace LongTie.Nlbs.User
{
    /// <summary>
    /// Interaction logic for UserGrid.xaml
    /// </summary>
    public partial class UserGrid : Grid
    {

        IUser _User = DataAccess.CreateUser();
        IUserRole _UserRole = DataAccess.CreateUserRole();
        IDepartment _Department = DataAccess.CreateDepartment();
        IUserToRole UserToRole = DataAccess.CreateUserToRole();

        FilterData filterdata = new FilterData();
        ICardHelper ruc = null;
        string backcard = "";
        private delegate void TimerDispatcherDelegate();

        public UserGrid(ICardHelper r)
        {
            InitializeComponent();

            ruc = r;

            ruc.HandDataBack += BackCardID;
            //aTimer = new System.Timers.Timer(1000);
            //aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            //aTimer.Interval = 10;
            //aTimer.Enabled = true;
        }
        void BackCardID(object sender, CardEventArgs e)
        {
            //ReadUserCard ruc = (ReadUserCard)sender;
            //backcard = ruc.BackString();
            backcard = e.data;
            Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(UpdateCardInfo));
        }

        //void OnTimedEvent(object serder, EventArgs e)
        //{
        //    //this.Dispatcher.Invoke(DispatcherPriority.Normal,
        //    //    new TimerDispatcherDelegate(UpdateCardInfo));
        //}

        void UpdateCardInfo()
        {
            try
            {
                if (OperationConfig.GetValue("CardSort") == "USB")
                {
                    this.tb_cardid.Text = backcard;
                }
                else
                {
                    filterdata.CardId = backcard;
                    filterdata.resetCard();


                    string cardid = filterdata.getcardid();
                    if (cardid == "")
                        return;
                    filterdata.resetid("");
                    this.tb_cardid.Text = cardid;
                }
            }
            catch { }

        }

        private List<UserModel> getuerlist(List<users> userlist)
        {
            List<UserModel> _usermodellist = new List<UserModel>();
            try
            {
                if (userlist != null && userlist.Count > 0)
                {
                    int count = 0;
                    foreach (users u in userlist)
                    {
                        if (u != null)
                        {
                            count++;
                            UserModel um = new UserModel();
                            um.id = count;
                            um.username = u.username;
                            um.empID = u.empID;
                            um.cardID = u.cardID;
                            um.is_staff = u.is_staff;
                            um.password = u.password;
                            um.phoneNumber = u.phoneNumber;
                            um.comment = u.comment;
                            um.guid = u.guid;
                            // um.joinDate = u.joinDate.ToString ().Replace('T', ' ')+"1";
                            um.joinDate = u.joinDate;
                            List<LT.Model.UserToRoleModel.usertorole> ul = new List<UserToRoleModel.usertorole>();
                            role r = new role();
                            ul = UserToRole.selectbyuserid(u.guid);
                            if (ul.Count > 0)
                            {
                                r = _UserRole.SelectByGuid(ul.FirstOrDefault().role);

                                um.roleName = r.roleName;
                                um.roleID = r.guid;
                            }
                            um.departName = _Department.SelectByGuid(u.department).departmentName;
                            um.department = u.department;

                            _usermodellist.Add(um);
                        }
                    }
                }
                else
                {
                    MessageAlert.Alert("所查信息不存在！");
                }
                return _usermodellist;
            }
            catch (Exception ex) { return _usermodellist; }
        }
        private void BindingUserModel(List<UserModel> _usermodellist)
        {
            // getuerlist();
            dataGrid1.ItemsSource = _usermodellist;
        }
        #region
        //添加用户
        //private void button3_Click(object sender, RoutedEventArgs e)
        //{
        //    EditerUser eu = new EditerUser();

        //    //eu.Show();
        //}

        //private void button2_Click(object sender, RoutedEventArgs e)
        //{
        //    winRole er = new winRole();
        //    er.Owner =_wm;
        //    er.Show();
        //}
        ////部门管理
        //private void button1_Click(object sender, RoutedEventArgs e)
        //{
        //    winDepartment  ed = new winDepartment ();
        //    ed.Show();
        //}
        //角色管理
        //private void editButtonClick(object sender, RoutedEventArgs e)
        //{
        //    int i_index = dataGrid1.SelectedIndex;
        //    if (i_index >= 0)
        //    {

        //       EditerUser edit = new EditerUser(dataGrid1.SelectedItem as users );
        //       // edit.ShowDialog();

        //        dataGrid1.SelectedIndex = i_index;
        //    }
        //}

        //private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        //{
        //    if (dataGrid1.SelectedItem == null)
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        int i_delete_index = dataGrid1.SelectedIndex;
        //        UserModel tmp = dataGrid1.SelectedItem as UserModel;

        //        if (MessageBox.Show("确认删除该用户信息？", "删除提示", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
        //        {
        //            return;
        //        }
        //        //如果当前删除 配置为  不彻底删除 
        //        tmp.is_staff = false ;
        //       if( _User .Update (tmp as users))
        //            {
        //                MessageBox.Show("删除用户信息成功", "提示信息");
        //            }
        //            else
        //            {
        //                MessageBox.Show("删除用户信息失败", "提示信息");
        //            }

        //        if (i_delete_index > 0)
        //        {
        //            BindingUserModel(getuerlist(_User.Select()));
        //        }
        //    } 
        //}
        #endregion
        private void bt_search_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Dictionary<string, string> contion = new Dictionary<string, string>();
                if (!string.IsNullOrEmpty(this.tb_name.Text.Trim()))
                    contion.Add("username", this.tb_name.Text.Trim());
                if (!string.IsNullOrEmpty(this.tb_cardid.Text.Trim()))
                    contion.Add("cardID", this.tb_cardid.Text.Trim());
                if (!string.IsNullOrEmpty(this.tb_empid.Text.Trim()))
                    contion.Add("empID", this.tb_empid.Text.Trim());
                if (contion.Count <= 0)
                {
                    GetUser gu = new GetUser();
                    BindingUserModel(gu.GetUserModel());
                    return;
                }
                BindingUserModel(getuerlist(_User.SelectByContion(contion)));

            }
            catch { }
        }

        private void tb_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.bt_search_Click(sender, e);
            }
        }

        private void tb_cardid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.bt_search_Click(sender, e);
            }
        }
        private void tb_empid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.bt_search_Click(sender, e);
            }
        }

        private void bt_out_Click(object sender, RoutedEventArgs e)
        {
            List<UserModel> tl = (List<UserModel>)this.dataGrid1.ItemsSource;
            if (tl == null || tl.Count <= 0)
                return;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            UserOutExcel uoe = new UserOutExcel();
            ExcelHelp _excelHelper = new ExcelHelp();
            saveFileDialog.Filter = "Excel (*.XLS)|*.xls";
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
