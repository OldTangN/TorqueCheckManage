using LongTie.Nlbs.User;
using LongTie.Nlbs.Wrench;
using LT.BLL;
using LT.BLL.Borrow;
using LT.BLL.Check;
using LT.BLL.Wrench;
using LT.Comm;
using LT.DAL;
using LT.DAL.MySql;
using LT.Model;
using LT.Model.BllModel;
using System;
using System.Collections.Generic;
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

namespace LongTie.Nlbs.Borrow
{
    /// <summary>
    /// WrenchBorrow.xaml 的交互逻辑
    /// </summary>
    public partial class WrenchBorrow 
    {
        IWrench Wrench = DataAccess.CreateWrench();
        IBorrow Borrow = new MySqlBorrow();
        ReadUserCard ruc = null;
   //     System.Timers.Timer aTimer = null;
        FilterData filterdata = new FilterData();
        GetUser getuser = new GetUser();
        userinfo _userinfo = new userinfo();
        userinfo borrowuser = null;
        string backcard = "";
        List<BorrowWrench> borrowwrenchlist = new List<BorrowWrench>();
        private delegate void TimerDispatcherDelegate();
        public WrenchBorrow(ReadUserCard r, userinfo u)
        {
            InitializeComponent();
            ruc = r;
            _userinfo = u;
            //aTimer = new System.Timers.Timer(1000);
            //aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            //aTimer.Interval = 10;
            //aTimer.Enabled = true;
            ruc.HandDataBack += new ReadUserCard.DeleteDataBack(BackCardID);
        }
        void BackCardID(object sender, EventArgs e)
        {
            ReadUserCard ruc = (ReadUserCard)sender;
            backcard = ruc.BackString();
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.ApplicationIdle, new Action(UpdateCardInfo));
        }

        void OnTimedEvent(object serder, EventArgs e)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal,
                new TimerDispatcherDelegate(UpdateCardInfo));
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
                this.cardid.Text = cardid;            
                borrowuser  = getuser.getuserinfo(getuser.getusers(cardid));
                if (borrowuser != null && borrowuser.user != null)
                {
                    this.username.Text = borrowuser.user.username;
                    this.telphone.Text = borrowuser.user.phoneNumber;
                    this.cb_user.IsEnabled = true;
                }
                else
                {
                    this.username.Text ="";
                    this.telphone.Text ="";
                }
            }
            catch { }
        }

        private void cb_user_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_user.SelectedIndex == 1)
            {
                UserShow us = new UserShow(borrowuser);
                us.ShowDialog();
            }
            if (cb_user.SelectedIndex == 2)
            {
                WrenchBorrowHistory wbh = new WrenchBorrowHistory();
                List<BorrowHistory> bhl = wbh.GetByUser(Borrow .SelectByUser ( _userinfo.user.guid));
                UserBorrow ub = new UserBorrow(bhl);
                ub.ShowDialog();
            }
            this.cb_user.SelectedIndex = 0;
        }

        private void cb_wrench_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (cb_wrench.SelectedIndex == 1)
            {
                if (!string.IsNullOrEmpty(this.wrenchbarcode.Text.Trim()))
                {
                    GetWrenchInfo gwi = new GetWrenchInfo();
                    wrenchinfo wi = gwi.GetWrenchinfo(this.wrenchbarcode.Text.Trim());
                    if (wi == null)
                    {
                        MessageAlert.Alert("没有任何详细信息！");
                    }
                    else
                    {
                        WrenchShow us = new WrenchShow(wi);
                        us.ShowDialog();
                    }
               
                }              
            }
            if (cb_wrench.SelectedIndex == 2)
            {
                if (string.IsNullOrEmpty(this.wrenchbarcode.Text.Trim()))
                {
                    return;
                }
                WrenchBorrowHistory wbh = new WrenchBorrowHistory();
                wrench w = Wrench.selectByBarcode(this.wrenchbarcode.Text.Trim());
                if (w == null)
                    return;
                List<BorrowHistory> bhl = wbh.GetByUser(Borrow.SelectByWrench(w.guid));
                UserBorrow ub = new UserBorrow(bhl);
                ub.ShowDialog();
            }
            this.cb_wrench.SelectedIndex = 0;
        }

        private void bt_card_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.cardid.Text.Trim()))
            {
                MessageAlert.Alert("请填写卡号！");
                return;
            }
            borrowuser = getuser.getuserinfo(getuser.getusers(this.cardid.Text.Trim()));
            if (borrowuser != null &&borrowuser.user != null)
            {
                this.username.Text = borrowuser.user.username;
                this.telphone.Text = borrowuser.user.phoneNumber;
                cb_user.IsEnabled = true;
            }

        }

        private void bt_wrenchbarcode_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(this.wrenchbarcode.Text.Trim()))
            {
                MessageAlert.Alert("请填写扳手编号");
                return;
            }
            GetWrench(this.wrenchbarcode.Text.Trim());
        }

        void GetWrench(string wrenchbarcode)
        {
            if (borrowuser == null)
            {
                MessageAlert.Alert("没有借用人信息");
                return;
            }
            if(borrowwrenchlist .FindIndex (p=>p.wrenchbarcode ==wrenchbarcode)>=0)
            {
                MessageAlert.Alert("不能重复添加！");
                return;
            }
          
            wrench w = Wrench.selectByBarcode(this.wrenchbarcode.Text.Trim());
            if(w!=null)
            borrowwrenchlist.Add(new BorrowWrench()
            {
               wrenchbarcode =w.wrenchBarCode ,
               wrenchcode =w.wrenchCode ,
               factory =w.factory ,
               borrowdate =DateTime .Now .ToString ("yyyy-MM-dd HH:mm:ss"),
               userguid =borrowuser.user .guid ,
               username =borrowuser .user .username ,
               operatorguid =_userinfo .user .guid ,
               options =_userinfo .user .username ,
               wrenchid =w.id.ToString ()
            
              
            });
            ShowWrench(w);
            DgBind(borrowwrenchlist); 
        }
        void DgBind(List<BorrowWrench> bl)
        {
            this.gd_borrow.ItemsSource = null;
            this.gd_borrow.ItemsSource = bl;
            this.count.Text = bl.Count.ToString();
        }

        void ShowWrench(wrench e)
        {
            if (e == null)
                return;
            IWrenchStatus WrenchStatus =DataAccess.CreateWrenchStatus();
            wrenchstatus w = WrenchStatus.selectByguid(e.status);
            if(w!=null)
            this.wrenchstatus.Text = w.statusName;
            this.wrenchcode.Text = e.wrenchCode;
            this.cb_wrench.IsEnabled = true;
        }

        private void bt_del_Click(object sender, RoutedEventArgs e)
        {
            if (this.gd_borrow.SelectedIndex < 0)
                return;
            BorrowWrench bw = this.gd_borrow.SelectedItem as BorrowWrench;
            if (bw == null || bw.wrenchbarcode == null)
                return;
            borrowwrenchlist.RemoveAt(borrowwrenchlist.FindIndex(p => p.wrenchbarcode == bw.wrenchbarcode));
            DgBind(borrowwrenchlist);
        }

        private void wrenchbarcode_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.wrenchbarcode.SelectAll();
                this.bt_wrenchbarcode_Click(this, e);
            }
        }

        private void bt_submit_Click(object sender, RoutedEventArgs e)
        {
            if (SaveDate())
            {
                MessageAlert.Alert("借出成功！");
                Clear();
            }
        }
        void Clear()
        {
            borrowwrenchlist.Clear();
            DgBind(borrowwrenchlist );
            borrowuser = null;
            this.username.Text = "";
            this.telphone.Text = "";
            this.wrenchstatus.Text ="";
            this.wrenchcode.Text = "";
            this.wrenchbarcode.Clear();
            this.cardid.Clear();
            cb_user.IsEnabled = false;
            this.cb_wrench.IsEnabled = false;
        }

        bool SaveDate()
        {
            if (borrowwrenchlist == null || borrowwrenchlist.Count <= 0)
            {
                MessageAlert.Alert("没有数据");
                return false;
            }
            try
            {
                foreach (BorrowWrench b in borrowwrenchlist)
                {
                    borrow bw = new borrow()
                    {
                        WrenchID = b.wrenchid,
                        borrowUser = b.userguid,
                        borrowDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        borrowOperator = b.operatorguid,
                        guid = Guid.NewGuid().ToString(),
                        is_return=false
                    };
                    Borrow.add(bw);
                }
            }
            catch { return false; }
            return true ;
        }

        private void cardid_GotFocus(object sender, RoutedEventArgs e)
        {
              Clear();
        }

        private void wrenchbarcode_GotFocus(object sender, RoutedEventArgs e)
        {
            this.wrenchstatus.Text = "";
            this.wrenchcode.Text = "";
            this.cb_wrench.IsEnabled = false;
        }


    }
}
