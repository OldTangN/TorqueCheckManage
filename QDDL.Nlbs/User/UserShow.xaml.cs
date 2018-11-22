using QDDL.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
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
    /// Interaction logic for UserShow.xaml
    /// </summary>
    public partial class UserShow : Window
    {
       // private static  UserShow usershow=null;
        userinfo _userinfo = new userinfo();
        //public static UserShow GetUserShow(userinfo users)
        //{
           
        //      return usershow ??( usershow = new UserShow(users));
    
        //}
        public   UserShow(userinfo u)
        {
            InitializeComponent();
            _userinfo = u;
            Usershow(_userinfo);
        }
        void Usershow(userinfo u)
        {
            if (u == null || u.user== null)
                return;
            this.cardid.Text = u.user.cardID;
            this.username.Text= u.user.username;
            this.eid.Text = u.user.empID;
            if(u.duty !=null )
            this.duty.Text =u.duty .dutiesName;
            if(u.department !=null)
            this.department.Text = u.department.departmentName;
            this.telphone.Text = u.user.phoneNumber;
            if(u.role !=null)
            this.role.Text = u.role.roleName;
            this.time.Text = u.user.joinDate.ToString().Replace ('T',' ');


        }

        private void bt_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        //protected override void OnClosing(CancelEventArgs e)
        //{
        //    Hide();
        //    e.Cancel = true;
        //} 
    }
}
