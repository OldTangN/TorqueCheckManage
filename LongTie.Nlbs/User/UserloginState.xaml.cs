using LongTie.Nlbs.Common;
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
    /// Interaction logic for UserloginState.xaml
    /// </summary>
    public partial class UserloginState 
    {
        public UserloginState()
        {
            InitializeComponent();
            show();
        }

        public void show() 
        {
            if (SystData.userInfo== null || SystData.userInfo.role == null || SystData.userInfo.user == null)
            { return; }
            lb_sataus.Content = "当前登录状态：已登陆";
            lb_username.Content = SystData.userInfo.user.username;
            lb_userdepartment.Content = SystData.userInfo.department.departmentName;
            lb_userrole.Content = SystData.userInfo.role.roleName;
        }
    }
}
