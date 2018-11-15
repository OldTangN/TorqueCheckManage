using LT.Comm;
using LT.DAL;
using LT.DAL.MySql;
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
    /// Interaction logic for ModifyPwd.xaml
    /// </summary>
    public partial class ModifyPwd 
    {
        Main _m=null;
        IUser User = DataAccess.CreateUser();
        public ModifyPwd(Main m)
        {
            InitializeComponent();
            _m = m;
        }
        private void BtLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.pb_name.Password)||string.IsNullOrEmpty (this .pb_password.Password )||string .IsNullOrEmpty (this.pb_qpwd.Password ))
            { MessageAlert.Warning("请输入必要信息！ * 为必填信息"); return; }

            if (!_m.l._userinfo . user.password.Equals(MD5Encrypt .GetMD5 ( this.pb_name.Password)))
            {
                MessageAlert.Warning("输入密码和登录密码不匹配！");
                return;
            }
            if (!this.pb_password.Password .Equals(this.pb_qpwd.Password)) {
                MessageAlert.Warning("新密码和确认密码不一致！");
                return;
            }
            try
            {
                if (_m != null)
                {
                    _m.l._userinfo .user.password =MD5Encrypt .GetMD5 (this.pb_qpwd.Password);
                    if (User.Update(_m.l._userinfo.user))
                    {
                        MessageAlert.Alert("密码更新成功！");
                    }
                    else
                    {
                        MessageAlert.Alert("密码更新失败！");
                    }
                }
            }
            catch(Exception ex) { MessageAlert .Alert (ex.ToString ());}
        }
    }
}
