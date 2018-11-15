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
        public UserloginState(userinfo userinfos)
        {
            InitializeComponent();
            show(userinfos);
        }

        public void show(userinfo ui) 
        {
            if (ui == null || ui.role == null || ui.user == null)
            { return; }
            lb_sataus.Content = "当前登录状态：已登陆";
            lb_username.Content = ui.user.username;
            lb_userdepartment.Content = ui.department.departmentName;
            lb_userrole.Content = ui.role.roleName;
        }
    }
}
