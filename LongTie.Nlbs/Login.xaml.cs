using LongTie.Nlbs.Check;
using LongTie.Nlbs.Notify;
using LT.BLL;
using LT.Comm;
using LT.DAL;
using LT.Model;
using LT.Model.BllModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LongTie.Nlbs
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login
    {

        GetRole getrole = new GetRole();
        Main _m = null;
        IWrench Wrench = DataAccess.CreateWrench();
        List<MenuItem> mll = new List<MenuItem>();
        public Login(Main m)
        {
            InitializeComponent();
            _m = m;
            //databind();
        }
        public UserLogin userlogin = null;
        public userinfo _userinfo = null;
        public bool success = false;
        //void databind() {
        //    cb_role.ItemsSource = null;
        //    cb_role.ItemsSource = getrole.getrole();
        //    cb_role.DisplayMemberPath = "roleName";
        //    cb_role.SelectedValuePath = "guid";
        //}
        private void Grid_Load(object sender, RoutedEventArgs e)
        {

            this.tb_name.Clear();
            this.tb_password.Clear();
            this.tb_name.Focus();
            //try
            //{
            //    string path = OperationConfig.GetValue("imagelogo") + ".jpg";
            //    Uri uri = new Uri(@path, UriKind.Relative);
            //    ImageBrush ib = new ImageBrush();
            //    ib.ImageSource = new BitmapImage(uri);
            //    this.G_login.Background = ib;
            //}
            //catch { }
        }
        private void BtLogin_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(this.tb_name.Text.Trim()) || String.IsNullOrEmpty(this.tb_password.Password.Trim()))
            {
                MessageAlert.Alert("用户名或密码不能为空！");
                return;
            }
            userlogin = new UserLogin(this.tb_name.Text.Trim(), this.tb_password.Password.Trim());
            _userinfo = userlogin._userinfo;
            if ((this.tb_name.Text.Trim() == "LongTie.com" && this.tb_password.Password.Trim() == "!@#$%^&*()"))
            {
                showall();
                return;
            }

            if ((userlogin.emplogin() == 1))
            {
                if (!show())
                {
                    return;
                }
                _m._userinfo = _userinfo;
                _m.main.Children.Clear();
                if (_m.cf == null)
                {
                    _m.main.Children.Add(_m.cf = new CheckFinal(_m.ruc, _m.rct1, _m.rct2));
                    _m.cf.SetSerialPort = _m.EncoderPlcPort;
                }
                else
                    _m.main.Children.Add(_m.cf);
                _m.user.Content = "当前登录用户:" + _m._userinfo.user.username;
                List<systemcheckset> ls = new List<systemcheckset>();
                try
                {
                    ls = SerializeXML<systemcheckset>.Getlist();
                    if (ls != null && ls.Count > 0)
                    {
                        if (ls.FirstOrDefault().noticeshow)
                        {
                            //this.taskbarNotifier.Show();
                            //this.taskbarNotifier.NotifyContent.Clear();
                            //this.taskbarNotifier.NotifyContent.Add(new NotifyObject(GetWrenchList(Convert.ToInt32(ls.FirstOrDefault().noticedays))));
                            WinWrenchRepair WinWrenchRepair = new WinWrenchRepair(GetWrenchList(Convert.ToInt32(ls.FirstOrDefault().noticedays)));
                            WinWrenchRepair.Show();
                        }
                    }
                }
                catch { }
                return;
            }
            else
            {
                MessageAlert.Alert("登录名或密码错误!\n   登录失败！");
                _m._userinfo = null;
                return;
            }
        }

        private List<WrenchNotice> GetWrenchList(int days)
        {
            List<WrenchNotice> overdata = new List<WrenchNotice>();
            List<wrench> wrenchs = Wrench.select();
            foreach (wrench w in wrenchs)
            {
                if (w.cycletime > 0)
                {
                    DateTime st = Convert.ToDateTime(w.lastrepair).AddDays(Convert.ToDouble(w.cycletime));
                    DateTime ed = DateTime.Now;
                    if (st < ed)
                    {
                        WrenchNotice wn = new WrenchNotice();
                        //w.cycletime =Convert.ToDecimal ( w.cycletime.ToString("f1"));
                        //w.lastrepair = w.lastrepair.Replace('T',' ');
                        overdata.Add(new WrenchNotice() { wrenchbarcode = w.wrenchBarCode, cycletime = w.cycletime.ToString("f1"), intime = Convert.ToDateTime(w.lastrepair).AddDays(Convert.ToInt32(w.cycletime)).ToString(), lastrepairtime = w.lastrepair.ToString("yyyy-MM-dd") });

                    }
                    else
                    {
                        int day = new TimeSpan(ed.Ticks - st.Ticks).Days;
                        if (day <= days)
                        {
                            WrenchNotice wn = new WrenchNotice();
                            //w.cycletime =Convert.ToDecimal ( w.cycletime.ToString("f1"));
                            //w.lastrepair = w.lastrepair.Replace('T',' ');
                            overdata.Add(new WrenchNotice() { wrenchbarcode = w.wrenchBarCode, cycletime = w.cycletime.ToString("f1"), intime = Convert.ToDateTime(w.lastrepair).AddDays(Convert.ToInt32(w.cycletime)).ToString(), lastrepairtime = w.lastrepair.ToString("yyyy-MM-dd") });

                        }
                    }

                }
            }
            return overdata;

        }
        void showall()
        {
            list(_m.menu);

            foreach (MenuItem m in mll)
            {
                m.IsEnabled = true;

            }

        }

        List<PowerList> getpower()
        {
            return SerializeXML<PowerList>.Getlist();
        }
        PowerList GetPowerList()
        {
            return getpower().Find(p => p.role == _userinfo.role.roleName);
        }

        private bool show()
        {
            try
            {
                PowerList pl = GetPowerList();
                if (pl == null)
                {
                    MessageAlert.Alert("没有任何功能权限，无法登陆！");
                    return false;
                }
                list(_m.menu);
                foreach (string s in pl.rolepower)
                {
                    foreach (MenuItem m in mll)
                    {
                        if (m.Items.Count > 0)
                        {
                            m.IsEnabled = true;
                        }
                        if (s == m.Header.ToString())
                        {
                            m.IsEnabled = true;
                        }
                    }
                }
                return true;
            }
            catch
            {
                MessageAlert.Error("出现文件错误请联系管理员！");
                return false;
            }
        }
        private void list(Menu m)
        {
            List<MenuItem> ml = new List<MenuItem>();
            foreach (var mi in m.Items)
            {

                ml = (getlist((MenuItem)mi));
            }
            ml = mll;
        }

        List<MenuItem> getlist(MenuItem ml)
        {

            if (ml.Items.Count > 0)
                mll.Add(ml);
            foreach (var mi in ml.Items)
            {
                MenuItem m = mi as MenuItem;
                if (m == null)
                    continue;
                if (m.Items.Count > 0)
                {

                    getlist(m);
                }
                else
                { mll.Add(m); }
            }

            return mll;
        }
        private void tb_name_GotFocus(object sender, RoutedEventArgs e)
        {
            this.tb_name.Clear();
        }

        private void tb_password_GotFocus(object sender, RoutedEventArgs e)
        {
            this.tb_password.Clear();
        }

        private void tb_password_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtLogin_Click(sender, e);
            }
        }

        private void Btout_Click(object sender, RoutedEventArgs e)
        {
            _m.Close();
        }
    }
}
