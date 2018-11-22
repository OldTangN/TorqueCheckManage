using LongTie.Nlbs.Check;
using LongTie.Nlbs.Common;
using LongTie.Nlbs.Notify;
using LT.BLL;
using LT.BLL.ICCard;
using LT.Comm;
using LT.DAL;
using LT.Model;
using LT.Model.BllModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace LongTie.Nlbs
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public ICardHelper ruc = null;
        public Thread thead3;
        public Login()
        {
            InitializeComponent();
        }
        public UserLogin userlogin = null;
        public bool success = false;
        string backcard = "";
        private void Grid_Load(object sender, RoutedEventArgs e)
        {

            try
            {
                string PortName = OperationConfig.GetValue("cardcom");
                if (OperationConfig.GetValue("CardSort") == "USB")
                {
                    ruc = new UsbICCard(PortName);
                }
                else
                {
                    ruc = new ComICCard(PortName);
                }
                if (ruc.IsOpen())
                {
                    thead3 = new Thread(ruc.Read);
                    thead3.Start();
                    ruc.HandDataBack -= BackCardID;
                    ruc.HandDataBack += BackCardID;
                }
                else
                {
                    idcardError.Visibility = Visibility.Visible;
                }
            }
            catch
            {
                idcardError.Visibility = Visibility.Visible;
            }
        }

        private void BackCardID(object sender, CardEventArgs e)
        {
            backcard = e.data;
            Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(UpdateCardInfo));
        }
        void UpdateCardInfo()
        {
            try
            {
                this.Cursor = Cursors.Wait;
                if (OperationConfig.GetValue("CardSort") == "USB")
                {
                    Getuserinfo(backcard);
                }
                //else
                //{
                //    filterdata.CardId = backcard;
                //    filterdata.resetCard();
                //    string cardid = filterdata.getcardid();
                //    if (cardid == "")
                //        return;
                //    Getuserinfo(cardid);
                //    filterdata.resetid("");
                //}


                //this.tb_wrenchbarcode.Focus();
                //showjuser(_juser);
                //showzuser(_zuer);
                //this.Cursor = Cursors.Arrow;
            }
            catch { }

        }


        void Getuserinfo(string cardid)
        {
            userlogin = new UserLogin(null, null, cardid);

            if ((userlogin.emplogin() == 1))
            {
                SystData.userInfo = userlogin._userinfo;
                Main main = new Main(ruc);
                Application.Current.MainWindow = main;          
                main.Show();
                this.Close();
            }
            else
            {
                MessageAlert.Alert("登录名或密码错误!\n   登录失败！");
                // _m._userinfo = null;
                return;
            }

        }
        private void BtLogin_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(this.tb_name.Text.Trim()) || String.IsNullOrEmpty(this.tb_password.Password.Trim()))
            {
                MessageAlert.Alert("用户名或密码不能为空！");
                return;
            }
            userlogin = new UserLogin(this.tb_name.Text.Trim(), this.tb_password.Password.Trim(), null);
      
            //if ((this.tb_name.Text.Trim() == "LongTie.com" && this.tb_password.Password.Trim() == "!@#$%^&*()"))
            //{
            //    showall();
            //    return;
            //}

            if ((userlogin.emplogin() == 1))
            {
                SystData.userInfo = userlogin._userinfo;
                Main main = new Main(ruc);
                Application.Current.MainWindow = main;
                this.Close();
                main.Show();
            }
            else
            {
                MessageAlert.Alert("登录名或密码错误!\n   登录失败！");
                // _m._userinfo = null;
                return;
            }
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
            //  _m.Close();
            Environment.Exit(0);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            ruc.HandDataBack -= BackCardID;
        }
    }
}